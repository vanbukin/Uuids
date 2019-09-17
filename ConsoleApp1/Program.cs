using System;
using System.Buffers.Binary;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    public static unsafe class Program
    {
        private static readonly byte* TableFromHexToBytes;

        static Program()
        {
            TableFromHexToBytes = (byte*) Marshal.AllocHGlobal(256).ToPointer();
            for (var i = 0; i < 256; i++)
            {
                TableFromHexToBytes[i] = (char) i switch
                {
                    '0' => (byte) 0x0,
                    '1' => (byte) 0x1,
                    '2' => (byte) 0x2,
                    '3' => (byte) 0x3,
                    '4' => (byte) 0x4,
                    '5' => (byte) 0x5,
                    '6' => (byte) 0x6,
                    '7' => (byte) 0x7,
                    '8' => (byte) 0x8,
                    '9' => (byte) 0x9,
                    'a' => (byte) 0xa,
                    'A' => (byte) 0xa,
                    'b' => (byte) 0xb,
                    'B' => (byte) 0xb,
                    'c' => (byte) 0xc,
                    'C' => (byte) 0xc,
                    'd' => (byte) 0xd,
                    'D' => (byte) 0xd,
                    'e' => (byte) 0xe,
                    'E' => (byte) 0xe,
                    'f' => (byte) 0xf,
                    'F' => (byte) 0xf,
                    _ => byte.MaxValue
                };
            }
        }


        private static bool TryParseHexToUint32(ReadOnlySpan<char> value, out uint result)
        {
            result = 0u;
            if (value.IsEmpty || (uint) value.Length != 8)
            {
                return false;
            }

            var parsedData = 0u;
            for (var i = 0; i < 4; i++)
            {
                byte hexByteHi;
                byte hexByteLow;
                if ((hexByteHi = TableFromHexToBytes[value[i * 2]]) != 0xFF
                    && (hexByteLow = TableFromHexToBytes[value[i * 2 + 1]]) != 0xFF)
                {
                    var hexByte = (uint) ((hexByteHi << 4) | hexByteLow) << (i * 8);
                    parsedData |= hexByte;
                }
                else
                {
                    return false;
                }
            }

            result = parsedData;
            return true;
        }

        private static unsafe byte[] GetNumberBytes(uint number)
        {
            var bytesOnStack = stackalloc byte[4];
            ((uint*) bytesOnStack)[0] = number;
            var result = new byte[4];
            result[0] = bytesOnStack[0];
            result[1] = bytesOnStack[1];
            result[2] = bytesOnStack[2];
            result[3] = bytesOnStack[3];
            return result;
        }

        private static void GenerateGarbage()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                var str = i.ToString();
            }
        }

        private static void Do(ReadOnlySpan<char> chars)
        {
            fixed (char* ptr = &chars.GetPinnableReference())
            {
                DoPtr(ptr, chars.Length);
            }
        }

        private static void DoPtr(char* ptr, int length)
        {
            GenerateGarbage();
            var spanExternal = new Span<char>(ptr, length);
            for (var i = 0; i < length; i++)
            {
                if (i % 10 == 0)
                {
                    GenerateGarbage();
                    var spanBefore = new Span<char>(ptr, length);
                    GC.Collect(2, GCCollectionMode.Forced, true, true);
                    var spanAfter = new Span<char>(ptr, length);
                    var xx = 0;
                }
            }

            var tt = 0;
        }

        public static unsafe void Main(string[] args)
        {
            // var uuid1 = new Uuid.Uuid("杦cb41752c36e11e99cb52a2be2dbcce4");
            //var uuid = Uuid.Uuid.Parse("4cb41752c36e11e99cb52a2be2dbcce4");
//            GenerateGarbage();
//            var str = "4cb41752c36e11e99cb52a2be2dbcce4";
//            GenerateGarbage();
//            Do(str.AsSpan());
            var span = "4cb41752c36e11e99cb52a2be2dbcce4".AsSpan();
            var parsedOld = Uuid.Uuid.TryParse("4cb41752c36e11e99cb52a2be2dbcce4", out var oldUuid);
            var tt = 0;

//            var byte0 = (byte) '0';
//            var byte9 = (byte) '9';
//            var byteA = (byte) 'A';
//            var byteF = (byte) 'F';
//            var bytea = (byte) 'a';
//            var bytef = (byte) 'f';
//            var HexToChar2 = new byte[256];
//            for (var i = 0; i < 256; i++)
//            {
//                HexToChar2[i] = i >= '0' && i <= '9'
//                    ? (byte) (i - 48)
//                    : i >= 'A' && i <= 'F'
//                        ? (byte) (i - 55)
//                        : i >= 'a' && i <= 'f'
//                            ? (byte) (i - 87)
//                            : byte.MaxValue;
//            }
//
//            var byteA1 = 0xa0;
//            var byteA2 = 0x0a;
//            var byteA3 = 0xaa;
//            var ttdd = 0;
//            var uuid = new Uuid.Uuid("4cb41752c36e11e99cb52a2be2dbcce4");


//            var nullString = "00000001";
//            var parseHex1 = uint.TryParse(nullString, NumberStyles.AllowHexSpecifier, null, out var result1);
//            var parseHex2 = TryParseHexToUint32(nullString, out var result2);

            //var parseHex2 = uint.TryParse(nullString, NumberStyles.AllowHexSpecifier | NumberStyles.AllowLeadingWhite, null, out var result2);
//            var lookupTable = Uuid.CharToHexLookup;
//            var tableLen = lookupTable.Length;


//            var stringN = "4cb41752c36e11e99cb52a2be2dbcce4";
//            var stringNSlice = stringN.AsSpan().Slice(0, 8);
//            var stringNSliceParsed = uint.TryParse(stringNSlice, NumberStyles.AllowHexSpecifier, null,
//                out var stringNSliceParsedUint);
//
//            var littleEndianBytes = stackalloc byte[4];
//            ((uint*) littleEndianBytes)[0] = stringNSliceParsedUint;
//            var littleEndianBytesSpan = new Span<byte>(littleEndianBytes, 4);
//
//            var bigEndianOrderedUint = BinaryPrimitives.ReverseEndianness(stringNSliceParsedUint);
//            var bigEndianBytes = stackalloc byte[4];
//            ((uint*) bigEndianBytes)[0] = bigEndianOrderedUint;
//            var bigEndianBytesSpan = new Span<byte>(bigEndianBytes, 4);
//            var stringToParse = new string[]
//            {
////                null as string,
////                string.Empty,
////                "4cb41752c36e11e99cb52a2be2dbcce4",
////                "000000000",
////                "00000000",
//                "1a0000a1",
//                "00000001",
//                "4cb41752"
//            };
//            var parsedResults = stringToParse.Select(x => Parse(x.AsSpan())).ToArray();
            var end = 0;
        }

        private static unsafe ParseResult Parse(ReadOnlySpan<char> chars)
        {
            var stringToParse = new string(chars);
            var uintBigEndianResult = 0u;
            var uintBigEndianBytes = new byte[4];
            var uintWasParsed = uint.TryParse(stringToParse, NumberStyles.AllowHexSpecifier, null, out var uintResult);
            if (uintWasParsed)
            {
                uintBigEndianResult = BinaryPrimitives.ReverseEndianness(uintResult);
                fixed (byte* uintBigEndianBytesPtr = uintBigEndianBytes)
                {
                    ((uint*) uintBigEndianBytesPtr)[0] = uintBigEndianResult;
                }
            }

            var customResultBytes = new byte[4];
            var customWasParsed = TryParseHexToUint32(chars, out var customResult);
            if (customWasParsed)
            {
                fixed (byte* customResultBytesPtr = customResultBytes)
                {
                    ((uint*) customResultBytesPtr)[0] = customResult;
                }
            }

            return new ParseResult(
                stringToParse,
                uintWasParsed,
                uintResult,
                uintBigEndianResult,
                uintBigEndianBytes,
                customWasParsed,
                customResult,
                customResultBytes);
        }

        private class ParseResult
        {
            public ParseResult(
                string stringToParse,
                bool uintWasParsed,
                uint uintResult,
                uint uintBigEndianResult,
                byte[] uintBigEndianBytes,
                bool customWasParsed,
                uint customResult,
                byte[] customResultBytes)
            {
                StringToParse = stringToParse;
                UintWasParsed = uintWasParsed;
                UintResult = uintResult;
                UintBigEndianResult = uintBigEndianResult;
                UintBigEndianBytes = uintBigEndianBytes;
                CustomWasParsed = customWasParsed;
                CustomResult = customResult;
                CustomResultBytes = customResultBytes;
            }

            public string StringToParse { get; }
            public bool UintWasParsed { get; }
            public uint UintResult { get; }
            public uint UintBigEndianResult { get; }
            public byte[] UintBigEndianBytes { get; }
            public bool CustomWasParsed { get; }
            public uint CustomResult { get; }
            public byte[] CustomResultBytes { get; }
        }
    }

//    [StructLayout(LayoutKind.Explicit, Pack = 1)]
//    public unsafe struct Uuid
//    {
//        static Uuid()
//        {
//            TableToHex = (uint*) Marshal.AllocHGlobal(sizeof(uint) * 256).ToPointer();
//            for (var i = 0; i < 256; i++)
//            {
//                var chars = Convert.ToString(i, 16).PadLeft(2, '0');
//                TableToHex[i] = ((uint) chars[1] << 16) | chars[0];
//            }
//
//#nullable disable
//            // ReSharper disable once PossibleNullReferenceException
//            FastAllocateString = (Func<int, string>) typeof(string)
//                .GetMethod(nameof(FastAllocateString), BindingFlags.Static | BindingFlags.NonPublic)
//                .CreateDelegate(typeof(Func<int, string>));
//#nullable restore
//        }
//
//        public static ReadOnlySpan<byte> CharToHexLookup => new byte[]
//        {
//            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 15
//            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 31
//            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 47
//            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 63
//            0xFF, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 79
//            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 95
//            0xFF, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf // 103
//        };
//
//        private static readonly uint* TableToHex;
//        private static readonly Func<int, string> FastAllocateString;
//
//        // ReSharper disable once RedundantDefaultMemberInitializer
//        // ReSharper disable once MemberCanBePrivate.Global
//        public static readonly Uuid Empty = new Uuid();
//
//
//        [FieldOffset(0)] private byte _byte0;
//        [FieldOffset(1)] private byte _byte1;
//        [FieldOffset(2)] private byte _byte2;
//        [FieldOffset(3)] private byte _byte3;
//        [FieldOffset(4)] private byte _byte4;
//        [FieldOffset(5)] private byte _byte5;
//        [FieldOffset(6)] private byte _byte6;
//        [FieldOffset(7)] private byte _byte7;
//        [FieldOffset(8)] private byte _byte8;
//        [FieldOffset(9)] private byte _byte9;
//        [FieldOffset(10)] private byte _byte10;
//        [FieldOffset(11)] private byte _byte11;
//        [FieldOffset(12)] private byte _byte12;
//        [FieldOffset(13)] private byte _byte13;
//        [FieldOffset(14)] private byte _byte14;
//        [FieldOffset(15)] private byte _byte15;
//
//        [FieldOffset(0)] internal ulong _ulong0;
//        [FieldOffset(8)] internal ulong _ulong8;
//
//        [FieldOffset(0)] internal uint _uint0;
//        [FieldOffset(4)] internal uint _uint4;
//        [FieldOffset(8)] internal uint _uint8;
//        [FieldOffset(12)] internal uint _uint12;
//
//        [FieldOffset(0)] internal ushort _ushort0;
//        [FieldOffset(2)] internal ushort _ushort2;
//        [FieldOffset(4)] internal ushort _ushort4;
//        [FieldOffset(6)] internal ushort _ushort6;
//        [FieldOffset(8)] internal ushort _ushort8;
//        [FieldOffset(10)] internal ushort _ushort10;
//        [FieldOffset(12)] internal ushort _ushort12;
//        [FieldOffset(14)] internal ushort _ushort14;
//
//        private enum UuidParseThrowStyle : byte
//        {
//            None = 0,
//            All = 1,
//            AllButOverflow = 2
//        }
//
//        [StructLayout(LayoutKind.Explicit, Pack = 1)]
//        private ref struct UuidResult
//        {
//            [FieldOffset(0)] internal Uuid _parsedUuid;
//            [FieldOffset(16)] private readonly UuidParseThrowStyle _throwStyle;
//
//            internal UuidResult(UuidParseThrowStyle canThrow) : this()
//            {
//                _throwStyle = canThrow;
//            }
//
//            internal void SetFailure(bool overflow, string failureMessage)
//            {
//                if (_throwStyle == UuidParseThrowStyle.None) return;
//
//                if (overflow)
//                {
//                    if (_throwStyle == UuidParseThrowStyle.All) throw new OverflowException(failureMessage);
//
//                    throw new FormatException("Unrecognized Uuid format.");
//                }
//
//                throw new FormatException(failureMessage);
//            }
//        }
//
//        private static bool TryParseExactN(ReadOnlySpan<char> uuidString, ref UuidResult result)
//        {
//            // e.g. "d85b1407351d4694939203acc5870eb1"
//
//            if ((uint) uuidString.Length != 32)
//            {
//                result.SetFailure(false, "Uuid should contain only 32 digits xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.");
//                return false;
//            }
//
//            ref var parsedUuid = ref result._parsedUuid;
//
//            if (uint.TryParse(uuidString.Slice(0, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmpRaw0)
//                && uint.TryParse(uuidString.Slice(8, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmpRaw8)
//                && uint.TryParse(uuidString.Slice(16, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmpRaw16)
//                && uint.TryParse(uuidString.Slice(24, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmpRaw24)
//            )
//            {
//                parsedUuid._uint0 = BinaryPrimitives.ReverseEndianness(uintTmpRaw0);
//                parsedUuid._uint4 = BinaryPrimitives.ReverseEndianness(uintTmpRaw8);
//                parsedUuid._uint8 = BinaryPrimitives.ReverseEndianness(uintTmpRaw16);
//                parsedUuid._uint12 = BinaryPrimitives.ReverseEndianness(uintTmpRaw24);
//                return true;
//            }
//
//            result.SetFailure(false, "Uuid string should only contain hexadecimal characters.");
//            return false;
//        }
//    }
}