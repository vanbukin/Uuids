using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace Uuid
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "InvertIf")]
    [SuppressMessage("ReSharper", "RedundantIfElseBlock")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    public unsafe struct Uuid : IFormattable, IComparable, IComparable<Uuid>, IEquatable<Uuid>
    {
        static Uuid()
        {
            TableToHex = (uint*) Marshal.AllocHGlobal(sizeof(uint) * 256).ToPointer();
            for (var i = 0; i < 256; i++)
            {
                var chars = Convert.ToString(i, 16).PadLeft(2, '0');
                TableToHex[i] = ((uint) chars[1] << 16) | chars[0];
            }

            TableFromHexToBytes = (byte*) Marshal.AllocHGlobal(103).ToPointer();
            for (var i = 0; i < 103; i++)
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

#nullable disable
            // ReSharper disable once PossibleNullReferenceException
            FastAllocateString = (Func<int, string>) typeof(string)
                .GetMethod(nameof(FastAllocateString), BindingFlags.Static | BindingFlags.NonPublic)
                .CreateDelegate(typeof(Func<int, string>));
#nullable restore
        }

        private const ushort MaximalChar = 103;

        internal static ReadOnlySpan<byte> CharToHexLookup => new byte[]
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 15
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 31
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 47
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 63
            0xFF, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 79
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 95
            0xFF, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf // 102
        };

        private static readonly uint* TableToHex;
        private static readonly byte* TableFromHexToBytes;
        private static readonly Func<int, string> FastAllocateString;

        // ReSharper disable once RedundantDefaultMemberInitializer
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly Uuid Empty = new Uuid();


        [FieldOffset(0)] private byte _byte0;
        [FieldOffset(1)] private byte _byte1;
        [FieldOffset(2)] private byte _byte2;
        [FieldOffset(3)] private byte _byte3;
        [FieldOffset(4)] private byte _byte4;
        [FieldOffset(5)] private byte _byte5;
        [FieldOffset(6)] private byte _byte6;
        [FieldOffset(7)] private byte _byte7;
        [FieldOffset(8)] private byte _byte8;
        [FieldOffset(9)] private byte _byte9;
        [FieldOffset(10)] private byte _byte10;
        [FieldOffset(11)] private byte _byte11;
        [FieldOffset(12)] private byte _byte12;
        [FieldOffset(13)] private byte _byte13;
        [FieldOffset(14)] private byte _byte14;
        [FieldOffset(15)] private byte _byte15;

        [FieldOffset(0)] internal ulong _ulong0;
        [FieldOffset(8)] internal ulong _ulong8;

        [FieldOffset(0)] internal uint _uint0;
        [FieldOffset(4)] internal uint _uint4;
        [FieldOffset(8)] internal uint _uint8;
        [FieldOffset(12)] internal uint _uint12;

        [FieldOffset(0)] internal ushort _ushort0;
        [FieldOffset(2)] internal ushort _ushort2;
        [FieldOffset(4)] internal ushort _ushort4;
        [FieldOffset(6)] internal ushort _ushort6;
        [FieldOffset(8)] internal ushort _ushort8;
        [FieldOffset(10)] internal ushort _ushort10;
        [FieldOffset(12)] internal ushort _ushort12;
        [FieldOffset(14)] internal ushort _ushort14;

        public Uuid(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if ((uint) bytes.Length != 16)
                throw new ArgumentException("Byte array for Uuid must be exactly 16 bytes long.", nameof(bytes));
            this = MemoryMarshal.Read<Uuid>(bytes);
        }

        public Uuid(byte* bytes)
        {
            this = Unsafe.ReadUnaligned<Uuid>(bytes);
        }

        public Uuid(ReadOnlySpan<byte> bytes)
        {
            if ((uint) bytes.Length != 16)
                throw new ArgumentException("Byte array for Uuid must be exactly 16 bytes long.", nameof(bytes));
            this = MemoryMarshal.Read<Uuid>(bytes);
        }

        public byte[] ToByteArray()
        {
            var result = new byte[16];
            MemoryMarshal.Write(result, ref this);
            return result;
        }

        public bool TryWriteBytes(Span<byte> destination)
        {
            return MemoryMarshal.TryWrite(destination, ref this);
        }

        public int CompareTo(object? value)
        {
            if (value == null) return 1;

            if (!(value is Uuid)) throw new ArgumentException("Object must be of type Uuid.", nameof(value));

            var other = (Uuid) value;

            if (other._ulong0 != _ulong0)
                return _ulong0 < other._ulong0 ? -1 : 1;

            if (other._ulong8 != _ulong8)
                return _ulong8 < other._ulong8 ? -1 : 1;

            return 0;
        }

        public int CompareTo(Uuid other)
        {
            if (other._ulong0 != _ulong0) return _ulong0 < other._ulong0 ? -1 : 1;

            if (other._ulong8 != _ulong8) return _ulong8 < other._ulong8 ? -1 : 1;

            return 0;
        }

        [SuppressMessage("ReSharper", "RedundantAssignment")]
        [SuppressMessage("ReSharper", "RedundantIfElseBlock")]
        [SuppressMessage("ReSharper", "MergeSequentialChecks")]
        public override bool Equals(object? obj)
        {
            Uuid other;
            if (obj == null || !(obj is Uuid))
                return false;
            else
                other = (Uuid) obj;
            return _ulong0 == other._ulong0 && _ulong8 == other._ulong8;
        }

        public bool Equals(Uuid other)
        {
            return _ulong0 == other._ulong0 && _ulong8 == other._ulong8;
        }

        [SuppressMessage("ReSharper", "RedundantCast")]
        [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            var xorULongs = _ulong0 ^ _ulong8;
            return ((int) xorULongs) ^ (int) (xorULongs >> 32);
        }

        public static bool operator ==(Uuid left, Uuid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Uuid left, Uuid right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return ToString("D", null);
        }

        [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
        public string ToString(string? format)
        {
            return ToString(format, null);
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            if (string.IsNullOrEmpty(format)) format = "D";

            // all acceptable format strings are of length 1
            if (format.Length != 1)
                throw new FormatException(
                    "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");

            switch (format[0])
            {
                case 'D':
                case 'd':
                {
                    var uuidString = FastAllocateString(36);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatD(uuidChars);
                    }

                    return uuidString;
                }
                case 'N':
                case 'n':
                {
                    var uuidString = FastAllocateString(32);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatN(uuidChars);
                    }

                    return uuidString;
                }
                case 'B':
                case 'b':
                {
                    var uuidString = FastAllocateString(38);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatB(uuidChars);
                    }

                    return uuidString;
                }
                case 'P':
                case 'p':
                {
                    var uuidString = FastAllocateString(38);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatP(uuidChars);
                    }

                    return uuidString;
                }
                case 'X':
                case 'x':
                {
                    var uuidString = FastAllocateString(68);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatX(uuidChars);
                    }

                    return uuidString;
                }
                default:
                    throw new FormatException(
                        "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
            }
        }

        private void FormatD(char* dest)
        {
            // dddddddd-dddd-dddd-dddd-dddddddddddd
            var uintDest = (uint*) dest;
            var uintDestAsChars = (char**) &uintDest;
            dest[8] = dest[13] = dest[18] = dest[23] = '-';
            uintDest[0] = TableToHex[_byte0];
            uintDest[1] = TableToHex[_byte1];
            uintDest[2] = TableToHex[_byte2];
            uintDest[3] = TableToHex[_byte3];
            uintDest[7] = TableToHex[_byte6];
            uintDest[8] = TableToHex[_byte7];
            uintDest[12] = TableToHex[_byte10];
            uintDest[13] = TableToHex[_byte11];
            uintDest[14] = TableToHex[_byte12];
            uintDest[15] = TableToHex[_byte13];
            uintDest[16] = TableToHex[_byte14];
            uintDest[17] = TableToHex[_byte15];
            *uintDestAsChars += 1;
            uintDest[4] = TableToHex[_byte4];
            uintDest[5] = TableToHex[_byte5];
            uintDest[9] = TableToHex[_byte8];
            uintDest[10] = TableToHex[_byte9];
        }

        private void FormatN(char* dest)
        {
            // dddddddddddddddddddddddddddddddd
            var destUints = (uint*) dest;
            destUints[0] = TableToHex[_byte0];
            destUints[1] = TableToHex[_byte1];
            destUints[2] = TableToHex[_byte2];
            destUints[3] = TableToHex[_byte3];
            destUints[4] = TableToHex[_byte4];
            destUints[5] = TableToHex[_byte5];
            destUints[6] = TableToHex[_byte6];
            destUints[7] = TableToHex[_byte7];
            destUints[8] = TableToHex[_byte8];
            destUints[9] = TableToHex[_byte9];
            destUints[10] = TableToHex[_byte10];
            destUints[11] = TableToHex[_byte11];
            destUints[12] = TableToHex[_byte12];
            destUints[13] = TableToHex[_byte13];
            destUints[14] = TableToHex[_byte14];
            destUints[15] = TableToHex[_byte15];
        }

        private void FormatB(char* dest)
        {
            // {dddddddd-dddd-dddd-dddd-dddddddddddd}
            var destUints = (uint*) dest;
            var destUintsAsChars = (char**) &destUints;
            dest[0] = '{';
            dest[9] = dest[14] = dest[19] = dest[24] = '-';
            dest[37] = '}';
            destUints[5] = TableToHex[_byte4];
            destUints[6] = TableToHex[_byte5];
            destUints[10] = TableToHex[_byte8];
            destUints[11] = TableToHex[_byte9];
            *destUintsAsChars += 1;
            destUints[0] = TableToHex[_byte0];
            destUints[1] = TableToHex[_byte1];
            destUints[2] = TableToHex[_byte2];
            destUints[3] = TableToHex[_byte3];
            destUints[7] = TableToHex[_byte6];
            destUints[8] = TableToHex[_byte7];
            destUints[12] = TableToHex[_byte10];
            destUints[13] = TableToHex[_byte11];
            destUints[14] = TableToHex[_byte12];
            destUints[15] = TableToHex[_byte13];
            destUints[16] = TableToHex[_byte14];
            destUints[17] = TableToHex[_byte15];
        }

        private void FormatP(char* dest)
        {
            // (dddddddd-dddd-dddd-dddd-dddddddddddd)
            var destUints = (uint*) dest;
            var destUintsAsChars = (char**) &destUints;
            dest[0] = '(';
            dest[9] = dest[14] = dest[19] = dest[24] = '-';
            dest[37] = ')';
            destUints[5] = TableToHex[_byte4];
            destUints[6] = TableToHex[_byte5];
            destUints[10] = TableToHex[_byte8];
            destUints[11] = TableToHex[_byte9];
            *destUintsAsChars += 1;
            destUints[0] = TableToHex[_byte0];
            destUints[1] = TableToHex[_byte1];
            destUints[2] = TableToHex[_byte2];
            destUints[3] = TableToHex[_byte3];
            destUints[7] = TableToHex[_byte6];
            destUints[8] = TableToHex[_byte7];
            destUints[12] = TableToHex[_byte10];
            destUints[13] = TableToHex[_byte11];
            destUints[14] = TableToHex[_byte12];
            destUints[15] = TableToHex[_byte13];
            destUints[16] = TableToHex[_byte14];
            destUints[17] = TableToHex[_byte15];
        }

        private const uint ZeroX = 7864368; // 0x
        private const uint CommaBrace = 8060972; // ,{
        private const uint CloseBraces = 8192125; // }}

        private void FormatX(char* dest)
        {
            // {0xdddddddd,0xdddd,0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
            var uintDest = (uint*) dest;
            var uintDestAsChars = (char**) &uintDest;
            dest[0] = '{';
            dest[11] = dest[18] = dest[31] = dest[36] = dest[41] = dest[46] = dest[51] = dest[56] = dest[61] = ',';
            uintDest[6] = uintDest[16] = uintDest[21] = uintDest[26] = uintDest[31] = ZeroX; // 0x
            uintDest[7] = TableToHex[_byte4];
            uintDest[8] = TableToHex[_byte5];
            uintDest[17] = TableToHex[_byte9];
            uintDest[22] = TableToHex[_byte11];
            uintDest[27] = TableToHex[_byte13];
            uintDest[32] = TableToHex[_byte15];
            uintDest[33] = CloseBraces; // }}
            *uintDestAsChars += 1;
            uintDest[0] = uintDest[9] = uintDest[13] = uintDest[18] = uintDest[23] = uintDest[28] = ZeroX; // 0x
            uintDest[1] = TableToHex[_byte0];
            uintDest[2] = TableToHex[_byte1];
            uintDest[3] = TableToHex[_byte2];
            uintDest[4] = TableToHex[_byte3];
            uintDest[10] = TableToHex[_byte6];
            uintDest[11] = TableToHex[_byte7];
            uintDest[12] = CommaBrace; // ,{
            uintDest[14] = TableToHex[_byte8];
            uintDest[19] = TableToHex[_byte10];
            uintDest[24] = TableToHex[_byte12];
            uintDest[29] = TableToHex[_byte14];
        }

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

        public Uuid(string uuidString)
        {
            if (uuidString == null)
                throw new ArgumentNullException(nameof(uuidString));
            var result = new Uuid();
            var resultPtr = (byte*) &result;
            fixed (char* uuidStringPtr = uuidString)
            {
                ParseWithExceptions(uuidString, uuidStringPtr, resultPtr);
            }

            this = result;
        }

        private static bool ParseWithoutExceptions(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length == 0)
                return false;
            switch (uuidString[0])
            {
                case '(': // P
                {
                    if ((uint) uuidString.Length != 38u)
                        return false;
                    if (uuidStringPtr[37] != ')'
                        || uuidStringPtr[9] != '-'
                        || uuidStringPtr[14] != '-'
                        || uuidStringPtr[19] != '-'
                        || uuidStringPtr[24] != '-')
                    {
                        return false;
                    }

                    return TryParsePtrPorB(uuidStringPtr, resultPtr);
                }
                case '{':
                {
                    if (uuidString.Contains('-')) // B
                    {
                        if ((uint) uuidString.Length != 38u)
                            return false;
                        if (uuidStringPtr[37] != '}'
                            || uuidStringPtr[9] != '-'
                            || uuidStringPtr[14] != '-'
                            || uuidStringPtr[19] != '-'
                            || uuidStringPtr[24] != '-')
                        {
                            return false;
                        }

                        return TryParsePtrPorB(uuidStringPtr, resultPtr);
                    }
                    else // X
                    {
                        if ((uint) uuidString.Length != 68u)
                            return false;
                        if (uuidStringPtr[0] != '{'
                            || uuidStringPtr[1] != '0'
                            || uuidStringPtr[2] != 'x'
                            || uuidStringPtr[11] != ','
                            || uuidStringPtr[12] != '0'
                            || uuidStringPtr[13] != 'x'
                            || uuidStringPtr[18] != ','
                            || uuidStringPtr[19] != '0'
                            || uuidStringPtr[20] != 'x'
                            || uuidStringPtr[25] != ','
                            || uuidStringPtr[27] != '0'
                            || uuidStringPtr[28] != 'x'
                            || uuidStringPtr[31] != ','
                            || uuidStringPtr[32] != '0'
                            || uuidStringPtr[33] != 'x'
                            || uuidStringPtr[36] != ','
                            || uuidStringPtr[37] != '0'
                            || uuidStringPtr[38] != 'x'
                            || uuidStringPtr[41] != ','
                            || uuidStringPtr[42] != '0'
                            || uuidStringPtr[43] != 'x'
                            || uuidStringPtr[46] != ','
                            || uuidStringPtr[47] != '0'
                            || uuidStringPtr[48] != 'x'
                            || uuidStringPtr[51] != ','
                            || uuidStringPtr[52] != '0'
                            || uuidStringPtr[53] != 'x'
                            || uuidStringPtr[57] != '0'
                            || uuidStringPtr[56] != ','
                            || uuidStringPtr[58] != 'x'
                            || uuidStringPtr[62] != '0'
                            || uuidStringPtr[61] != ','
                            || uuidStringPtr[63] != 'x'
                            || uuidStringPtr[66] != '}'
                            || uuidStringPtr[67] != '}')
                        {
                            return false;
                        }

                        return TryParsePtrX(uuidStringPtr, resultPtr);
                    }
                }
                default:
                {
                    if (uuidString.Contains('-')) // D
                    {
                        // e.g. "d85b1407-351d-4694-9392-03acc5870eb1"
                        if ((uint) uuidString.Length != 36u)
                            return false;

                        if (uuidStringPtr[8] != '-'
                            || uuidStringPtr[13] != '-'
                            || uuidStringPtr[18] != '-'
                            || uuidStringPtr[23] != '-')
                        {
                            return false;
                        }

                        return TryParsePtrD(uuidStringPtr, resultPtr);
                    }
                    else // N
                    {
                        return (uint) uuidString.Length == 32u && TryParsePtrN(uuidStringPtr, resultPtr);
                    }
                }
            }
        }

        private static void ParseWithExceptions(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length == 0)
                throw new FormatException("Unrecognized Uuid format.");

            switch (uuidString[0])
            {
                case '(': // P
                {
                    if ((uint) uuidString.Length != 38u || uuidString[37] != ')')
                    {
                        throw new FormatException("Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                    }

                    if (uuidStringPtr[9] != '-' || uuidStringPtr[14] != '-' || uuidStringPtr[19] != '-' || uuidStringPtr[24] != '-')
                    {
                        throw new FormatException("Dashes are in the wrong position for Uuid parsing.");
                    }

                    if (!TryParsePtrPorB(uuidStringPtr, resultPtr))
                    {
                        throw new FormatException("Uuid string should only contain hexadecimal characters.");
                    }

                    break;
                }
                case '{':
                {
                    if (uuidString.Contains('-')) // B
                    {
                        if ((uint) uuidString.Length != 38u || uuidString[37] != '}')
                        {
                            throw new FormatException("Uuid should contain 32 digits with 4 dashes {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}.");
                        }

                        if (uuidStringPtr[9] != '-' || uuidStringPtr[14] != '-' || uuidStringPtr[19] != '-' || uuidStringPtr[24] != '-')
                        {
                            throw new FormatException("Dashes are in the wrong position for Uuid parsing.");
                        }

                        if (!TryParsePtrPorB(uuidStringPtr, resultPtr))
                        {
                            throw new FormatException("Uuid string should only contain hexadecimal characters.");
                        }
                    }
                    else // X
                    {
                        if ((uint) uuidString.Length != 68u || uuidString[0] != '{' || uuidString[66] != '}')
                        {
                            throw new FormatException(
                                "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");
                        }

                        if (uuidStringPtr[11] != ','
                            || uuidStringPtr[18] != ','
                            || uuidStringPtr[25] != ','
                            || uuidStringPtr[31] != ','
                            || uuidStringPtr[36] != ','
                            || uuidStringPtr[41] != ','
                            || uuidStringPtr[46] != ','
                            || uuidStringPtr[51] != ','
                            || uuidStringPtr[56] != ','
                            || uuidStringPtr[61] != ',')
                        {
                            throw new FormatException(
                                "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
                        }

                        if (uuidStringPtr[1] != '0'
                            || uuidStringPtr[2] != 'x'
                            || uuidStringPtr[12] != '0'
                            || uuidStringPtr[13] != 'x'
                            || uuidStringPtr[19] != '0'
                            || uuidStringPtr[20] != 'x'
                            || uuidStringPtr[27] != '0'
                            || uuidStringPtr[28] != 'x'
                            || uuidStringPtr[32] != '0'
                            || uuidStringPtr[33] != 'x'
                            || uuidStringPtr[37] != '0'
                            || uuidStringPtr[38] != 'x'
                            || uuidStringPtr[42] != '0'
                            || uuidStringPtr[43] != 'x'
                            || uuidStringPtr[47] != '0'
                            || uuidStringPtr[48] != 'x'
                            || uuidStringPtr[52] != '0'
                            || uuidStringPtr[53] != 'x'
                            || uuidStringPtr[57] != '0'
                            || uuidStringPtr[58] != 'x'
                            || uuidStringPtr[62] != '0'
                            || uuidStringPtr[63] != 'x')
                        {
                            throw new FormatException("Expected 0x prefix.");
                        }

                        if (uuidStringPtr[67] != '}')
                        {
                            throw new FormatException("Could not find the ending brace.");
                        }

                        if (!TryParsePtrX(uuidStringPtr, resultPtr))
                        {
                            throw new FormatException("Uuid string should only contain hexadecimal characters.");
                        }
                    }

                    break;
                }
                default:
                {
                    if (uuidString.Contains('-')) // D
                    {
                        if ((uint) uuidString.Length != 36u)
                        {
                            throw new FormatException("Uuid should contain 32 digits with 4 dashes xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.");
                        }

                        if (uuidStringPtr[8] != '-' || uuidStringPtr[13] != '-' || uuidStringPtr[18] != '-' || uuidStringPtr[23] != '-')
                        {
                            throw new FormatException("Dashes are in the wrong position for Uuid parsing.");
                        }

                        if (!TryParsePtrD(uuidStringPtr, resultPtr))
                        {
                            throw new FormatException("Uuid string should only contain hexadecimal characters.");
                        }
                    }
                    else // N
                    {
                        if ((uint) uuidString.Length != 32u)
                        {
                            throw new FormatException(
                                "Uuid should contain only 32 digits xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.");
                        }

                        if (!TryParsePtrN(uuidStringPtr, resultPtr))
                        {
                            throw new FormatException("Uuid string should only contain hexadecimal characters.");
                        }
                    }

                    break;
                }
            }
        }

        private static bool TryParsePtrN(char* value, byte* resultPtr)
        {
            // e.g. "d85b1407351d4694939203acc5870eb1"
            byte hexByteHi;
            byte hexByteLow;
            // 0 byte
            if (value[0] < MaximalChar
                && (hexByteHi = TableFromHexToBytes[value[0]]) != 0xFF
                && value[1] < MaximalChar
                && (hexByteLow = TableFromHexToBytes[value[1]]) != 0xFF)
            {
                resultPtr[0] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                // 1 byte
                if (value[2] < MaximalChar
                    && (hexByteHi = TableFromHexToBytes[value[2]]) != 0xFF
                    && value[3] < MaximalChar
                    && (hexByteLow = TableFromHexToBytes[value[3]]) != 0xFF)
                {
                    resultPtr[1] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                    // 2 byte
                    if (value[4] < MaximalChar
                        && (hexByteHi = TableFromHexToBytes[value[4]]) != 0xFF
                        && value[5] < MaximalChar
                        && (hexByteLow = TableFromHexToBytes[value[5]]) != 0xFF)
                    {
                        resultPtr[2] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                        // 3 byte
                        if (value[6] < MaximalChar
                            && (hexByteHi = TableFromHexToBytes[value[6]]) != 0xFF
                            && value[7] < MaximalChar
                            && (hexByteLow = TableFromHexToBytes[value[7]]) != 0xFF)
                        {
                            resultPtr[3] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                            // 4 byte
                            if (value[8] < MaximalChar
                                && (hexByteHi = TableFromHexToBytes[value[8]]) != 0xFF
                                && value[9] < MaximalChar
                                && (hexByteLow = TableFromHexToBytes[value[9]]) != 0xFF)
                            {
                                resultPtr[4] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                // 5 byte
                                if (value[10] < MaximalChar
                                    && (hexByteHi = TableFromHexToBytes[value[10]]) != 0xFF
                                    && value[11] < MaximalChar
                                    && (hexByteLow = TableFromHexToBytes[value[11]]) != 0xFF)
                                {
                                    resultPtr[5] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                    // 6 byte
                                    if (value[12] < MaximalChar
                                        && (hexByteHi = TableFromHexToBytes[value[12]]) != 0xFF
                                        && value[13] < MaximalChar
                                        && (hexByteLow = TableFromHexToBytes[value[13]]) != 0xFF)
                                    {
                                        resultPtr[6] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                        // 7 byte
                                        if (value[14] < MaximalChar
                                            && (hexByteHi = TableFromHexToBytes[value[14]]) != 0xFF
                                            && value[15] < MaximalChar
                                            && (hexByteLow = TableFromHexToBytes[value[15]]) != 0xFF)
                                        {
                                            resultPtr[7] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                            // 8 byte
                                            if (value[16] < MaximalChar
                                                && (hexByteHi = TableFromHexToBytes[value[16]]) != 0xFF
                                                && value[17] < MaximalChar
                                                && (hexByteLow = TableFromHexToBytes[value[17]]) != 0xFF)
                                            {
                                                resultPtr[8] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                // 9 byte
                                                if (value[18] < MaximalChar
                                                    && (hexByteHi = TableFromHexToBytes[value[18]]) != 0xFF
                                                    && value[19] < MaximalChar
                                                    && (hexByteLow = TableFromHexToBytes[value[19]]) != 0xFF)
                                                {
                                                    resultPtr[9] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                    // 10 byte
                                                    if (value[20] < MaximalChar
                                                        && (hexByteHi = TableFromHexToBytes[value[20]]) != 0xFF
                                                        && value[21] < MaximalChar
                                                        && (hexByteLow = TableFromHexToBytes[value[21]]) != 0xFF)
                                                    {
                                                        resultPtr[10] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                        // 11 byte
                                                        if (value[22] < MaximalChar
                                                            && (hexByteHi = TableFromHexToBytes[value[22]]) != 0xFF
                                                            && value[23] < MaximalChar
                                                            && (hexByteLow = TableFromHexToBytes[value[23]]) != 0xFF)
                                                        {
                                                            resultPtr[11] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                            // 12 byte
                                                            if (value[24] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[24]]) != 0xFF
                                                                && value[25] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[25]]) != 0xFF)
                                                            {
                                                                resultPtr[12] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                // 13 byte
                                                                if (value[26] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[26]]) != 0xFF
                                                                    && value[27] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[27]]) != 0xFF)
                                                                {
                                                                    resultPtr[13] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                    // 14 byte
                                                                    if (value[28] < MaximalChar
                                                                        && (hexByteHi = TableFromHexToBytes[value[28]]) != 0xFF
                                                                        && value[29] < MaximalChar
                                                                        && (hexByteLow = TableFromHexToBytes[value[29]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                        // 15 byte
                                                                        if (value[30] < MaximalChar
                                                                            && (hexByteHi = TableFromHexToBytes[value[30]]) != 0xFF
                                                                            && value[31] < MaximalChar
                                                                            && (hexByteLow = TableFromHexToBytes[value[31]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                            return true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static bool TryParsePtrD(char* value, byte* resultPtr)
        {
            // e.g. "d85b1407-351d-4694-9392-03acc5870eb1"
            byte hexByteHi;
            byte hexByteLow;
            // 0 byte
            if (value[0] < MaximalChar
                && (hexByteHi = TableFromHexToBytes[value[0]]) != 0xFF
                && value[1] < MaximalChar
                && (hexByteLow = TableFromHexToBytes[value[1]]) != 0xFF)
            {
                resultPtr[0] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                // 1 byte
                if (value[2] < MaximalChar
                    && (hexByteHi = TableFromHexToBytes[value[2]]) != 0xFF
                    && value[3] < MaximalChar
                    && (hexByteLow = TableFromHexToBytes[value[3]]) != 0xFF)
                {
                    resultPtr[1] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                    // 2 byte
                    if (value[4] < MaximalChar
                        && (hexByteHi = TableFromHexToBytes[value[4]]) != 0xFF
                        && value[5] < MaximalChar
                        && (hexByteLow = TableFromHexToBytes[value[5]]) != 0xFF)
                    {
                        resultPtr[2] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                        // 3 byte
                        if (value[6] < MaximalChar
                            && (hexByteHi = TableFromHexToBytes[value[6]]) != 0xFF
                            && value[7] < MaximalChar
                            && (hexByteLow = TableFromHexToBytes[value[7]]) != 0xFF)
                        {
                            resultPtr[3] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                            // value[8] == '-'

                            // 4 byte
                            if (value[9] < MaximalChar
                                && (hexByteHi = TableFromHexToBytes[value[9]]) != 0xFF
                                && value[10] < MaximalChar
                                && (hexByteLow = TableFromHexToBytes[value[10]]) != 0xFF)
                            {
                                resultPtr[4] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                // 5 byte
                                if (value[11] < MaximalChar
                                    && (hexByteHi = TableFromHexToBytes[value[11]]) != 0xFF
                                    && value[12] < MaximalChar
                                    && (hexByteLow = TableFromHexToBytes[value[12]]) != 0xFF)
                                {
                                    resultPtr[5] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                    // value[13] == '-'

                                    // 6 byte
                                    if (value[14] < MaximalChar
                                        && (hexByteHi = TableFromHexToBytes[value[14]]) != 0xFF
                                        && value[15] < MaximalChar
                                        && (hexByteLow = TableFromHexToBytes[value[15]]) != 0xFF)
                                    {
                                        resultPtr[6] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                        // 7 byte
                                        if (value[16] < MaximalChar
                                            && (hexByteHi = TableFromHexToBytes[value[16]]) != 0xFF
                                            && value[17] < MaximalChar
                                            && (hexByteLow = TableFromHexToBytes[value[17]]) != 0xFF)
                                        {
                                            resultPtr[7] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                            // value[18] == '-'

                                            // 8 byte
                                            if (value[19] < MaximalChar
                                                && (hexByteHi = TableFromHexToBytes[value[19]]) != 0xFF
                                                && value[20] < MaximalChar
                                                && (hexByteLow = TableFromHexToBytes[value[20]]) != 0xFF)
                                            {
                                                resultPtr[8] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                // 9 byte
                                                if (value[21] < MaximalChar
                                                    && (hexByteHi = TableFromHexToBytes[value[21]]) != 0xFF
                                                    && value[22] < MaximalChar
                                                    && (hexByteLow = TableFromHexToBytes[value[22]]) != 0xFF)
                                                {
                                                    resultPtr[9] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                    // value[23] == '-'

                                                    // 10 byte
                                                    if (value[24] < MaximalChar
                                                        && (hexByteHi = TableFromHexToBytes[value[24]]) != 0xFF
                                                        && value[25] < MaximalChar
                                                        && (hexByteLow = TableFromHexToBytes[value[25]]) != 0xFF)
                                                    {
                                                        resultPtr[10] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                        // 11 byte
                                                        if (value[26] < MaximalChar
                                                            && (hexByteHi = TableFromHexToBytes[value[26]]) != 0xFF
                                                            && value[27] < MaximalChar
                                                            && (hexByteLow = TableFromHexToBytes[value[27]]) != 0xFF)
                                                        {
                                                            resultPtr[11] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                            // 12 byte
                                                            if (value[28] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[28]]) != 0xFF
                                                                && value[29] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[29]]) != 0xFF)
                                                            {
                                                                resultPtr[12] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                // 13 byte
                                                                if (value[30] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[30]]) != 0xFF
                                                                    && value[31] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[31]]) != 0xFF)
                                                                {
                                                                    resultPtr[13] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                    // 14 byte
                                                                    if (value[32] < MaximalChar
                                                                        && (hexByteHi = TableFromHexToBytes[value[32]]) != 0xFF
                                                                        && value[33] < MaximalChar
                                                                        && (hexByteLow = TableFromHexToBytes[value[33]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                        // 15 byte
                                                                        if (value[34] < MaximalChar
                                                                            && (hexByteHi = TableFromHexToBytes[value[34]]) != 0xFF
                                                                            && value[35] < MaximalChar
                                                                            && (hexByteLow = TableFromHexToBytes[value[35]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                            return true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static bool TryParsePtrPorB(char* value, byte* resultPtr)
        {
            // e.g. "{d85b1407-351d-4694-9392-03acc5870eb1}"
            // e.g. "(d85b1407-351d-4694-9392-03acc5870eb1)"

            byte hexByteHi;
            byte hexByteLow;
            // 0 byte
            if (value[1] < MaximalChar
                && (hexByteHi = TableFromHexToBytes[value[1]]) != 0xFF
                && value[2] < MaximalChar
                && (hexByteLow = TableFromHexToBytes[value[2]]) != 0xFF)
            {
                resultPtr[0] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                // 1 byte
                if (value[3] < MaximalChar
                    && (hexByteHi = TableFromHexToBytes[value[3]]) != 0xFF
                    && value[4] < MaximalChar
                    && (hexByteLow = TableFromHexToBytes[value[4]]) != 0xFF)
                {
                    resultPtr[1] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                    // 2 byte
                    if (value[5] < MaximalChar
                        && (hexByteHi = TableFromHexToBytes[value[5]]) != 0xFF
                        && value[6] < MaximalChar
                        && (hexByteLow = TableFromHexToBytes[value[6]]) != 0xFF)
                    {
                        resultPtr[2] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                        // 3 byte
                        if (value[7] < MaximalChar
                            && (hexByteHi = TableFromHexToBytes[value[7]]) != 0xFF
                            && value[8] < MaximalChar
                            && (hexByteLow = TableFromHexToBytes[value[8]]) != 0xFF)
                        {
                            resultPtr[3] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                            // value[9] == '-'

                            // 4 byte
                            if (value[10] < MaximalChar
                                && (hexByteHi = TableFromHexToBytes[value[10]]) != 0xFF
                                && value[11] < MaximalChar
                                && (hexByteLow = TableFromHexToBytes[value[11]]) != 0xFF)
                            {
                                resultPtr[4] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                // 5 byte
                                if (value[12] < MaximalChar
                                    && (hexByteHi = TableFromHexToBytes[value[12]]) != 0xFF
                                    && value[13] < MaximalChar
                                    && (hexByteLow = TableFromHexToBytes[value[13]]) != 0xFF)
                                {
                                    resultPtr[5] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                    // value[14] == '-'

                                    // 6 byte
                                    if (value[15] < MaximalChar
                                        && (hexByteHi = TableFromHexToBytes[value[15]]) != 0xFF
                                        && value[16] < MaximalChar
                                        && (hexByteLow = TableFromHexToBytes[value[16]]) != 0xFF)
                                    {
                                        resultPtr[6] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                        // 7 byte
                                        if (value[17] < MaximalChar
                                            && (hexByteHi = TableFromHexToBytes[value[17]]) != 0xFF
                                            && value[18] < MaximalChar
                                            && (hexByteLow = TableFromHexToBytes[value[18]]) != 0xFF)
                                        {
                                            resultPtr[7] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                            // value[19] == '-'

                                            // 8 byte
                                            if (value[20] < MaximalChar
                                                && (hexByteHi = TableFromHexToBytes[value[20]]) != 0xFF
                                                && value[21] < MaximalChar
                                                && (hexByteLow = TableFromHexToBytes[value[21]]) != 0xFF)
                                            {
                                                resultPtr[8] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                // 9 byte
                                                if (value[22] < MaximalChar
                                                    && (hexByteHi = TableFromHexToBytes[value[22]]) != 0xFF
                                                    && value[23] < MaximalChar
                                                    && (hexByteLow = TableFromHexToBytes[value[23]]) != 0xFF)
                                                {
                                                    resultPtr[9] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                    // value[24] == '-'

                                                    // 10 byte
                                                    if (value[25] < MaximalChar
                                                        && (hexByteHi = TableFromHexToBytes[value[25]]) != 0xFF
                                                        && value[26] < MaximalChar
                                                        && (hexByteLow = TableFromHexToBytes[value[26]]) != 0xFF)
                                                    {
                                                        resultPtr[10] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                        // 11 byte
                                                        if (value[27] < MaximalChar
                                                            && (hexByteHi = TableFromHexToBytes[value[27]]) != 0xFF
                                                            && value[28] < MaximalChar
                                                            && (hexByteLow = TableFromHexToBytes[value[28]]) != 0xFF)
                                                        {
                                                            resultPtr[11] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                            // 12 byte
                                                            if (value[29] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[29]]) != 0xFF
                                                                && value[30] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[30]]) != 0xFF)
                                                            {
                                                                resultPtr[12] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                // 13 byte
                                                                if (value[31] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[31]]) != 0xFF
                                                                    && value[32] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[32]]) != 0xFF)
                                                                {
                                                                    resultPtr[13] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                    // 14 byte
                                                                    if (value[33] < MaximalChar
                                                                        && (hexByteHi = TableFromHexToBytes[value[33]]) != 0xFF
                                                                        && value[34] < MaximalChar
                                                                        && (hexByteLow = TableFromHexToBytes[value[34]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                        // 15 byte
                                                                        if (value[35] < MaximalChar
                                                                            && (hexByteHi = TableFromHexToBytes[value[35]]) != 0xFF
                                                                            && value[36] < MaximalChar
                                                                            && (hexByteLow = TableFromHexToBytes[value[36]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                            return true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static bool TryParsePtrX(char* value, byte* resultPtr)
        {
            // e.g. "{0xd85b1407,0x351d,0x4694,{0x93,0x92,0x03,0xac,0xc5,0x87,0x0e,0xb1}}"

            byte hexByteHi;
            byte hexByteLow;
            // value[0] == '{'
            // value[1] == '0'
            // value[2] == 'x'
            // 0 byte
            if (value[3] < MaximalChar
                && (hexByteHi = TableFromHexToBytes[value[3]]) != 0xFF
                && value[4] < MaximalChar
                && (hexByteLow = TableFromHexToBytes[value[4]]) != 0xFF)
            {
                resultPtr[0] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                // 1 byte
                if (value[5] < MaximalChar
                    && (hexByteHi = TableFromHexToBytes[value[5]]) != 0xFF
                    && value[6] < MaximalChar
                    && (hexByteLow = TableFromHexToBytes[value[6]]) != 0xFF)
                {
                    resultPtr[1] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                    // 2 byte
                    if (value[7] < MaximalChar
                        && (hexByteHi = TableFromHexToBytes[value[7]]) != 0xFF
                        && value[8] < MaximalChar
                        && (hexByteLow = TableFromHexToBytes[value[8]]) != 0xFF)
                    {
                        resultPtr[2] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                        // 3 byte
                        if (value[9] < MaximalChar
                            && (hexByteHi = TableFromHexToBytes[value[9]]) != 0xFF
                            && value[10] < MaximalChar
                            && (hexByteLow = TableFromHexToBytes[value[10]]) != 0xFF)
                        {
                            resultPtr[3] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                            // value[11] == ','
                            // value[12] == '0'
                            // value[13] == 'x'

                            // 4 byte
                            if (value[14] < MaximalChar
                                && (hexByteHi = TableFromHexToBytes[value[14]]) != 0xFF
                                && value[15] < MaximalChar
                                && (hexByteLow = TableFromHexToBytes[value[15]]) != 0xFF)
                            {
                                resultPtr[4] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                // 5 byte
                                if (value[16] < MaximalChar
                                    && (hexByteHi = TableFromHexToBytes[value[16]]) != 0xFF
                                    && value[17] < MaximalChar
                                    && (hexByteLow = TableFromHexToBytes[value[17]]) != 0xFF)
                                {
                                    resultPtr[5] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                    // value[18] == ','
                                    // value[19] == '0'
                                    // value[20] == 'x'

                                    // 6 byte
                                    if (value[21] < MaximalChar
                                        && (hexByteHi = TableFromHexToBytes[value[21]]) != 0xFF
                                        && value[22] < MaximalChar
                                        && (hexByteLow = TableFromHexToBytes[value[22]]) != 0xFF)
                                    {
                                        resultPtr[6] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                        // 7 byte
                                        if (value[23] < MaximalChar
                                            && (hexByteHi = TableFromHexToBytes[value[23]]) != 0xFF
                                            && value[24] < MaximalChar
                                            && (hexByteLow = TableFromHexToBytes[value[24]]) != 0xFF)
                                        {
                                            resultPtr[7] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                            // value[25] == ','
                                            // value[26] == '{'
                                            // value[27] == '0'
                                            // value[28] == 'x'

                                            // 8 byte
                                            if (value[29] < MaximalChar
                                                && (hexByteHi = TableFromHexToBytes[value[29]]) != 0xFF
                                                && value[30] < MaximalChar
                                                && (hexByteLow = TableFromHexToBytes[value[30]]) != 0xFF)
                                            {
                                                resultPtr[8] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                // value[31] == ','
                                                // value[32] == '0'
                                                // value[33] == 'x'

                                                // 9 byte
                                                if (value[34] < MaximalChar
                                                    && (hexByteHi = TableFromHexToBytes[value[34]]) != 0xFF
                                                    && value[35] < MaximalChar
                                                    && (hexByteLow = TableFromHexToBytes[value[35]]) != 0xFF)
                                                {
                                                    resultPtr[9] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                    // value[36] == ','
                                                    // value[37] == '0'
                                                    // value[38] == 'x'

                                                    // 10 byte
                                                    if (value[39] < MaximalChar
                                                        && (hexByteHi = TableFromHexToBytes[value[39]]) != 0xFF
                                                        && value[40] < MaximalChar
                                                        && (hexByteLow = TableFromHexToBytes[value[40]]) != 0xFF)
                                                    {
                                                        resultPtr[10] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                        // value[41] == ','
                                                        // value[42] == '0'
                                                        // value[43] == 'x'

                                                        // 11 byte
                                                        if (value[44] < MaximalChar
                                                            && (hexByteHi = TableFromHexToBytes[value[44]]) != 0xFF
                                                            && value[45] < MaximalChar
                                                            && (hexByteLow = TableFromHexToBytes[value[45]]) != 0xFF)
                                                        {
                                                            resultPtr[11] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                            // value[46] == ','
                                                            // value[47] == '0'
                                                            // value[48] == 'x'

                                                            // 12 byte
                                                            if (value[49] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[49]]) != 0xFF
                                                                && value[50] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[50]]) != 0xFF)
                                                            {
                                                                resultPtr[12] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                // value[51] == ','
                                                                // value[52] == '0'
                                                                // value[53] == 'x'

                                                                // 13 byte
                                                                if (value[54] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[54]]) != 0xFF
                                                                    && value[55] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[55]]) != 0xFF)
                                                                {
                                                                    resultPtr[13] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                    // value[56] == ','
                                                                    // value[57] == '0'
                                                                    // value[58] == 'x'

                                                                    // 14 byte
                                                                    if (value[59] < MaximalChar
                                                                        && (hexByteHi = TableFromHexToBytes[value[59]]) != 0xFF
                                                                        && value[60] < MaximalChar
                                                                        && (hexByteLow = TableFromHexToBytes[value[60]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                        // value[61] == ','
                                                                        // value[62] == '0'
                                                                        // value[63] == 'x'

                                                                        // 15 byte
                                                                        if (value[64] < MaximalChar
                                                                            && (hexByteHi = TableFromHexToBytes[value[64]]) != 0xFF
                                                                            && value[65] < MaximalChar
                                                                            && (hexByteLow = TableFromHexToBytes[value[65]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                            return true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static Uuid Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            var result = new Uuid();
            var resultPtr = (byte*) &result;
            fixed (char* uuidStringPtr = input)
            {
                ParseWithExceptions(input, uuidStringPtr, resultPtr);
            }

            return result;
        }

        public static Uuid Parse(ReadOnlySpan<char> input)
        {
            if (input.IsEmpty || (uint) input.Length > 68)
                throw new FormatException("Unrecognized Uuid format.");
            var inputPtr = stackalloc char[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                inputPtr[i] = input[i];
            }

            var result = new Uuid();
            var resultPtr = (byte*) &result;
            var span = new ReadOnlySpan<char>(inputPtr, input.Length);
            ParseWithExceptions(span, inputPtr, resultPtr);
            return result;
        }

        public static bool TryParse(string? input, out Uuid output)
        {
            if (input == null)
            {
                output = default;
                return false;
            }

            var result = new Uuid();
            var resultPtr = (byte*) &result;
            fixed (char* uuidStringPtr = input)
            {
                if (ParseWithoutExceptions(input, uuidStringPtr, resultPtr))
                {
                    output = result;
                    return true;
                }
            }

            output = default;
            return false;
        }

        public static bool TryParse(ReadOnlySpan<char> input, out Uuid output)
        {
            if (input.IsEmpty || (uint) input.Length > 68)
            {
                output = default;
                return false;
            }

            var inputPtr = stackalloc char[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                inputPtr[i] = input[i];
            }

            var result = new Uuid();
            var resultPtr = (byte*) &result;
            var span = new ReadOnlySpan<char>(inputPtr, input.Length);
            if (ParseWithoutExceptions(span, inputPtr, resultPtr))
            {
                output = result;
                return true;
            }

            output = default;
            return false;
        }

//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------
//        public static bool TryParse(string? input, out Uuid result)
//        {
//            if (input == null)
//            {
//                result = default;
//                return false;
//            }
//
//            return TryParse((ReadOnlySpan<char>) input, out result);
//        }
//
//        public static bool TryParse(ReadOnlySpan<char> input, out Uuid result)
//        {
//            var parseResult = new UuidResult(UuidParseThrowStyle.None);
//            if (TryParseUuid(input, ref parseResult))
//            {
//                result = parseResult._parsedUuid;
//                return true;
//            }
//            else
//            {
//                result = default;
//                return false;
//            }
//        }
//
//        public static Uuid ParseExact(string input, string format)
//        {
//            return ParseExact(
//                input != null ? (ReadOnlySpan<char>) input : throw new ArgumentNullException(nameof(input)),
//                format != null ? (ReadOnlySpan<char>) format : throw new ArgumentNullException(nameof(format)));
//        }
//
//        public static Uuid ParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format)
//        {
//            if (format.Length != 1)
//                // all acceptable format strings are of length 1
//                throw new FormatException(
//                    "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
//
//            input = input.Trim();
//
//            var result = new UuidResult(UuidParseThrowStyle.AllButOverflow);
//            // ReSharper disable once UnusedVariable
//            var success = (char) (format[0] | 0x20) switch
//            {
//                'd' => TryParseExactD(input, ref result),
//                'n' => TryParseExactN(input, ref result),
//                'b' => TryParseExactB(input, ref result),
//                'p' => TryParseExactP(input, ref result),
//                'x' => TryParseExactX(input, ref result),
//                _ => throw new FormatException(
//                    "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\"."),
//            };
//            return Unsafe.Read<Uuid>(&result._parsedUuid);
//        }
//
//        public static bool TryParseExact(string? input, string? format, out Uuid result)
//        {
//            if (input == null)
//            {
//                result = default;
//                return false;
//            }
//
//            return TryParseExact((ReadOnlySpan<char>) input, format, out result);
//        }
//
//        [SuppressMessage("ReSharper", "RedundantIfElseBlock")]
//        public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out Uuid result)
//        {
//            if (format.Length != 1)
//            {
//                result = default;
//                return false;
//            }
//
//            input = input.Trim();
//
//            var parseResult = new UuidResult(UuidParseThrowStyle.None);
//            var success = (char) (format[0] | 0x20) switch
//            {
//                'd' => TryParseExactD(input, ref parseResult),
//                'n' => TryParseExactN(input, ref parseResult),
//                'b' => TryParseExactB(input, ref parseResult),
//                'p' => TryParseExactP(input, ref parseResult),
//                'x' => TryParseExactX(input, ref parseResult),
//                _ => false
//            };
//
//            if (success)
//            {
//                result = Unsafe.Read<Uuid>(&parseResult._parsedUuid);
//                return true;
//            }
//            else
//            {
//                result = default;
//                return false;
//            }
//        }


//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------

//        private static bool TryParseUuid(ReadOnlySpan<char> uuidString, ref UuidResult result)
//        {
//            uuidString = uuidString.Trim(); // Remove whitespace from beginning and end
//
//            if (uuidString.Length == 0)
//            {
//                result.SetFailure(false, "Unrecognized Uuid format.");
//                return false;
//            }
//
//            return uuidString[0] switch
//            {
//                '(' => TryParseExactP(uuidString, ref result),
//                '{' => uuidString.Contains('-')
//                    ? TryParseExactB(uuidString, ref result)
//                    : TryParseExactX(uuidString, ref result),
//                _ => uuidString.Contains('-')
//                    ? TryParseExactD(uuidString, ref result)
//                    : TryParseExactN(uuidString, ref result),
//            };
//        }
//
//        private static bool TryParseExactB(ReadOnlySpan<char> uuidString, ref UuidResult result)
//        {
//            // e.g. "{d85b1407-351d-4694-9392-03acc5870eb1}"
//
//            if ((uint) uuidString.Length != 38 || uuidString[0] != '{' || uuidString[37] != '}')
//            {
//                result.SetFailure(false,
//                    "Uuid should contain 32 digits with 4 dashes {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}.");
//                return false;
//            }
//
//            return TryParseExactD(uuidString.Slice(1, 36), ref result);
//        }
//
//        private static bool TryParseExactD(ReadOnlySpan<char> uuidString, ref UuidResult result)
//        {
//            // e.g. "d85b1407-351d-4694-9392-03acc5870eb1"
//
//            if ((uint) uuidString.Length != 36)
//            {
//                result.SetFailure(false,
//                    "Uuid should contain 32 digits with 4 dashes xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.");
//                return false;
//            }
//
//            if (uuidString[8] != '-' || uuidString[13] != '-' || uuidString[18] != '-' || uuidString[23] != '-')
//            {
//                result.SetFailure(false, "Dashes are in the wrong position for Uuid parsing.");
//                return false;
//            }
//
//            ref var parsedUuid = ref result._parsedUuid;
//
//            if (TryParseHex(uuidString.Slice(0, 8), out parsedUuid._uint0)
//                && TryParseHex(uuidString.Slice(9, 4), out parsedUuid._ushort4)
//                && TryParseHex(uuidString.Slice(14, 4), out parsedUuid._ushort6)
//                && TryParseHex(uuidString.Slice(19, 4), out parsedUuid._ushort8)
//                && TryParseHex(uuidString.Slice(24, 4), out parsedUuid._ushort10)
//                && (TryParseHexToUint32(uuidString.Slice(28, 8), out parsedUuid._uint12)))
//            {
//                return true;
//            }
//
//            result.SetFailure(false, "Uuid string should only contain hexadecimal characters.");
//            return false;
//        }
//
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
//
//            if (TryDirectParseHexToUuid(uuidString, out result._parsedUuid))
//            {
//                return true;
//            }
//
//            result.SetFailure(false, "Uuid string should only contain hexadecimal characters.");
//            return false;
//        }
//
//        private static bool TryParseExactP(ReadOnlySpan<char> uuidString, ref UuidResult result)
//        {
//            // e.g. "(d85b1407-351d-4694-9392-03acc5870eb1)"
//
//            if ((uint) uuidString.Length != 38 || uuidString[0] != '(' || uuidString[37] != ')')
//            {
//                result.SetFailure(false,
//                    "Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
//                return false;
//            }
//
//            return TryParseExactD(uuidString.Slice(1, 36), ref result);
//        }
//
//        private static bool TryParseExactX(ReadOnlySpan<char> uuidString, ref UuidResult result)
//        {
//            // e.g. "{0xd85b1407,0x351d,0x4694,{0x93,0x92,0x03,0xac,0xc5,0x87,0x0e,0xb1}}"
//
//            uuidString = EatAllWhitespace(uuidString);
//
//            // Check for leading '{'
//            if ((uint) uuidString.Length == 0 || uuidString[0] != '{')
//            {
//                result.SetFailure(
//                    false,
//                    "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");
//                return false;
//            }
//
//            // Check for '0x'
//            if (!IsHexPrefix(uuidString, 1))
//            {
//                result.SetFailure(false, "Expected 0x prefix.");
//                return false;
//            }
//
//            // Find the end of this hex number (since it is not fixed length)
//            var numStart = 3;
//            var numLen = uuidString.Slice(numStart).IndexOf(',');
//            if (numLen <= 0)
//            {
//                result.SetFailure(
//                    false,
//                    "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
//                return false;
//            }
//
//
//            var overflow = false;
//            if (!TryParseHex(uuidString.Slice(numStart, numLen), out result._parsedUuid._uint0, ref overflow) ||
//                overflow)
//            {
//                result.SetFailure(overflow, overflow
//                    ? "Value was either too large or too small for a UInt32."
//                    : "Uuid string should only contain hexadecimal characters.");
//                return false;
//            }
//
//            // Check for '0x'
//            if (!IsHexPrefix(uuidString, numStart + numLen + 1))
//            {
//                result.SetFailure(false, "Expected 0x prefix.");
//                return false;
//            }
//
//            // +3 to get by ',0x'
//            numStart = numStart + numLen + 3;
//            numLen = uuidString.Slice(numStart).IndexOf(',');
//            if (numLen <= 0)
//            {
//                result.SetFailure(false,
//                    "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
//                return false;
//            }
//
//            // Read in the number
//            if (!TryParseHex(uuidString.Slice(numStart, numLen), out result._parsedUuid._ushort4, ref overflow) ||
//                overflow)
//            {
//                result.SetFailure(overflow, overflow
//                    ? "Value was either too large or too small for a UInt16."
//                    : "Uuid string should only contain hexadecimal characters.");
//                return false;
//            }
//
//            // Check for '0x'
//            if (!IsHexPrefix(uuidString, numStart + numLen + 1))
//            {
//                result.SetFailure(false, "Expected 0x prefix.");
//                return false;
//            }
//
//            // +3 to get by ',0x'
//            numStart = numStart + numLen + 3;
//            numLen = uuidString.Slice(numStart).IndexOf(',');
//            if (numLen <= 0)
//            {
//                result.SetFailure(false,
//                    "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
//                return false;
//            }
//
//            // Read in the number
//            if (!TryParseHex(uuidString.Slice(numStart, numLen), out result._parsedUuid._ushort6, ref overflow) ||
//                overflow)
//            {
//                result.SetFailure(overflow,
//                    overflow
//                        ? "Value was either too large or too small for a UInt16."
//                        : "Uuid string should only contain hexadecimal characters.");
//                return false;
//            }
//
//            // Check for '{'
//            if ((uint) uuidString.Length <= (uint) (numStart + numLen + 1) || uuidString[numStart + numLen + 1] != '{')
//            {
//                result.SetFailure(false, "Expected {0xdddddddd, etc}.");
//                return false;
//            }
//
//            // Prepare for loop
//            numLen++;
//            for (var i = 0; i < 8; i++)
//            {
//                // Check for '0x'
//                if (!IsHexPrefix(uuidString, numStart + numLen + 1))
//                {
//                    result.SetFailure(false, "Expected 0x prefix.");
//                    return false;
//                }
//
//                // +3 to get by ',0x' or '{0x' for first case
//                numStart = numStart + numLen + 3;
//
//                // Calculate number length
//                if (i < 7) // first 7 cases
//                {
//                    numLen = uuidString.Slice(numStart).IndexOf(',');
//                    if (numLen <= 0)
//                    {
//                        result.SetFailure(false,
//                            "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
//                        return false;
//                    }
//                }
//                else // last case ends with '}', not ','
//                {
//                    numLen = uuidString.Slice(numStart).IndexOf('}');
//                    if (numLen <= 0)
//                    {
//                        result.SetFailure(false,
//                            "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");
//                        return false;
//                    }
//                }
//
//                // Read in the number
//                if (!TryParseHex(uuidString.Slice(numStart, numLen), out byte byteVal, ref overflow) || overflow)
//                {
//                    // The previous implementation had some odd inconsistencies, which are carried forward here.
//                    // The byte values in the X format are treated as integers with regards to overflow, so
//                    // a "byte" value like 0xddd in Uuid's ctor results in a FormatException but 0xddddddddd results
//                    // in OverflowException.
//                    result.SetFailure(overflow,
//                        overflow
//                            ? "Value was either too large or too small for an unsigned byte."
//                            : "Uuid string should only contain hexadecimal characters.");
//                    return false;
//                }
//
//                Unsafe.Add(ref result._parsedUuid._byte8, i) = byteVal;
//            }
//
//            // Check for last '}'
//            if (numStart + numLen + 1 >= uuidString.Length || uuidString[numStart + numLen + 1] != '}')
//            {
//                result.SetFailure(false, "Could not find the ending brace.");
//                return false;
//            }
//
//            // Check if we have extra characters at the end
//            if (numStart + numLen + 1 != uuidString.Length - 1)
//            {
//                result.SetFailure(false, "Additional non-parsable characters are at the end of the string.");
//                return false;
//            }
//
//            return true;
//        }
//
//        private static bool TryDirectParseHexToUuid(ReadOnlySpan<char> value, out Uuid result)
//        {
//            var parsedUuid = new Uuid();
//            var parsedUuidPtr = (byte*) &parsedUuid;
//
//            for (var i = 0; i < 16; i++)
//            {
//                byte hexByteHi;
//                byte hexByteLow;
//                if ((hexByteHi = TableFromHexToBytes[value[i * 2]]) != 0xFF
//                    && (hexByteLow = TableFromHexToBytes[value[i * 2 + 1]]) != 0xFF)
//                {
//                    unchecked
//                    {
//                        parsedUuidPtr[i] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
//                    }
//                }
//                else
//                {
//                    result = new Uuid();
//                    return false;
//                }
//            }
//
//            result = parsedUuid;
//            return true;
//        }
//
//        private static bool TryParseHexToUint32(ReadOnlySpan<char> value, out uint result)
//        {
//            result = 0u;
//            if (value.IsEmpty || (uint) value.Length != 8)
//            {
//                return false;
//            }
//
//            var parsedData = 0u;
//            for (var i = 0; i < 4; i++)
//            {
//                byte hexByteHi;
//                byte hexByteLow;
//                if ((hexByteHi = TableFromHexToBytes[value[i * 2]]) != 0xFF
//                    && (hexByteLow = TableFromHexToBytes[value[i * 2 + 1]]) != 0xFF)
//                {
//                    var hexByte = (uint) ((hexByteHi << 4) | hexByteLow) << (i * 8);
//                    parsedData |= hexByte;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//
//            result = parsedData;
//            return true;
//        }
//
//        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out ushort result)
//        {
//            var overflowIgnored = false;
//            return TryParseHex(uuidString, out result, ref overflowIgnored);
//        }
//
//        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out uint result)
//        {
//            var overflowIgnored = false;
//            return TryParseHex(uuidString, out result, ref overflowIgnored);
//        }
//
//        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out byte result, ref bool overflow)
//        {
//            if ((uint) uuidString.Length > 0)
//            {
//                if (uuidString[0] == '+') uuidString = uuidString.Slice(1);
//
//                if ((uint) uuidString.Length > 1 && uuidString[0] == '0' && (uuidString[1] | 0x20) == 'x')
//                    uuidString = uuidString.Slice(2);
//            }
//
//            // Skip past leading 0s.
//            var i = 0;
//            for (; i < uuidString.Length && uuidString[i] == '0'; i++)
//            {
//            }
//
//            var processedDigits = 0;
//            byte tmp = 0;
//            for (; i < uuidString.Length; i++)
//            {
//                int numValue;
//                var currentChar = uuidString[i];
//                if (currentChar >= (uint) CharToHexLookup.Length || (numValue = CharToHexLookup[currentChar]) == 0xFF)
//                {
//                    if (processedDigits > 2) overflow = true;
//                    result = 0;
//                    return false;
//                }
//
//                tmp = (byte) ((tmp * 16) + (uint) numValue);
//                processedDigits++;
//            }
//
//            if (processedDigits > 2) overflow = true;
//            result = BinaryPrimitives.ReverseEndianness(tmp);
//            return true;
//        }
//
//        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out ushort result, ref bool overflow)
//        {
//            if ((uint) uuidString.Length > 0)
//            {
//                if (uuidString[0] == '+') uuidString = uuidString.Slice(1);
//
//                if ((uint) uuidString.Length > 1 && uuidString[0] == '0' && (uuidString[1] | 0x20) == 'x')
//                    uuidString = uuidString.Slice(2);
//            }
//
//            // Skip past leading 0s.
//            var i = 0;
//            for (; i < uuidString.Length && uuidString[i] == '0'; i++)
//            {
//            }
//
//            var processedDigits = 0;
//            ushort tmp = 0;
//            for (; i < uuidString.Length; i++)
//            {
//                int numValue;
//                var currentChar = uuidString[i];
//                if (currentChar >= (uint) CharToHexLookup.Length || (numValue = CharToHexLookup[currentChar]) == 0xFF)
//                {
//                    if (processedDigits > 4) overflow = true;
//                    result = 0;
//                    return false;
//                }
//
//                tmp = (ushort) ((tmp * 16) + (uint) numValue);
//                processedDigits++;
//            }
//
//            if (processedDigits > 4) overflow = true;
//            result = BinaryPrimitives.ReverseEndianness(tmp);
//            return true;
//        }
//
//        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out uint result, ref bool overflow)
//        {
//            if ((uint) uuidString.Length > 0)
//            {
//                if (uuidString[0] == '+') uuidString = uuidString.Slice(1);
//
//                if ((uint) uuidString.Length > 1 && uuidString[0] == '0' && (uuidString[1] | 0x20) == 'x')
//                    uuidString = uuidString.Slice(2);
//            }
//
//            // Skip past leading 0s.
//            var i = 0;
//            for (; i < uuidString.Length && uuidString[i] == '0'; i++)
//            {
//            }
//
//            var processedDigits = 0;
//            uint tmp = 0;
//            for (; i < uuidString.Length; i++)
//            {
//                int numValue;
//                var currentChar = uuidString[i];
//                if (currentChar >= (uint) CharToHexLookup.Length || (numValue = CharToHexLookup[currentChar]) == 0xFF)
//                {
//                    if (processedDigits > 8) overflow = true;
//                    result = 0;
//                    return false;
//                }
//
//                tmp = (tmp * 16) + (uint) numValue;
//                processedDigits++;
//            }
//
//            if (processedDigits > 8) overflow = true;
//            result = BinaryPrimitives.ReverseEndianness(tmp);
//            return true;
//        }
//
//        private static ReadOnlySpan<char> EatAllWhitespace(ReadOnlySpan<char> str)
//        {
//            // Find the first whitespace character.  If there is none, just return the input.
//            int i;
//            for (i = 0; i < str.Length && !char.IsWhiteSpace(str[i]); i++)
//            {
//            }
//
//            if (i == str.Length) return str;
//
//            // There was at least one whitespace.  Copy over everything prior to it to a new array.
//            var chArr = new char[str.Length];
//            var newLength = 0;
//            if (i > 0)
//            {
//                newLength = i;
//                str.Slice(0, i).CopyTo(chArr);
//            }
//
//            // Loop through the remaining chars, copying over non-whitespace.
//            for (; i < str.Length; i++)
//            {
//                var c = str[i];
//                if (!char.IsWhiteSpace(c)) chArr[newLength++] = c;
//            }
//
//            // Return the string with the whitespace removed.
//            return new ReadOnlySpan<char>(chArr, 0, newLength);
//        }
//
//        private static bool IsHexPrefix(ReadOnlySpan<char> str, int i)
//        {
//            return i + 1 < str.Length &&
//                   str[i] == '0' &&
//                   (str[i + 1] | 0x20) == 'x';
//        }
    }
}