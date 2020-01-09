using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Uuids
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "InvertIf")]
    [SuppressMessage("ReSharper", "RedundantIfElseBlock")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    [SuppressMessage("ReSharper", "MergeSequentialChecks")]
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    [SuppressMessage("ReSharper", "CommentTypo")]
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public unsafe partial struct Uuid : IFormattable, IComparable, IComparable<Uuid>, IEquatable<Uuid>
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

//...
        private const ushort MaximalChar = 103;

        private static readonly uint* TableToHex;
        private static readonly byte* TableFromHexToBytes;

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

        [FieldOffset(0)] private ulong _ulong0;
        [FieldOffset(8)] private ulong _ulong8;

        [FieldOffset(0)] private int _int0;
        [FieldOffset(4)] private int _int4;
        [FieldOffset(8)] private int _int8;
        [FieldOffset(12)] private int _int12;

        public Uuid(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 16)
                throw new ArgumentException("Byte array for Uuid must be exactly 16 bytes long.", nameof(bytes));
            this = Unsafe.ReadUnaligned<Uuid>(ref MemoryMarshal.GetReference(new ReadOnlySpan<byte>(bytes)));
        }

        public Uuid(byte* bytes)
        {
            this = Unsafe.ReadUnaligned<Uuid>(bytes);
        }

        public Uuid(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != 16)
                throw new ArgumentException("Byte array for Uuid must be exactly 16 bytes long.", nameof(bytes));
            this = Unsafe.ReadUnaligned<Uuid>(ref MemoryMarshal.GetReference(bytes));
        }

        public byte[] ToByteArray()
        {
            var result = new byte[16];
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(new Span<byte>(result)), this);
            return result;
        }

        public bool TryWriteBytes(Span<byte> destination)
        {
            if (Unsafe.SizeOf<Uuid>() > (uint) destination.Length)
                return false;
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(object? value)
        {
            if (value == null) return 1;
            if (!(value is Uuid)) throw new ArgumentException("Object must be of type Uuid.", nameof(value));
            var other = (Uuid) value;
            if (other._byte0 != _byte0)
                return _byte0 < other._byte0 ? -1 : 1;
            if (other._byte1 != _byte1)
                return _byte1 < other._byte1 ? -1 : 1;
            if (other._byte2 != _byte2)
                return _byte2 < other._byte2 ? -1 : 1;
            if (other._byte3 != _byte3)
                return _byte3 < other._byte3 ? -1 : 1;
            if (other._byte4 != _byte4)
                return _byte4 < other._byte4 ? -1 : 1;
            if (other._byte5 != _byte5)
                return _byte5 < other._byte5 ? -1 : 1;
            if (other._byte6 != _byte6)
                return _byte6 < other._byte6 ? -1 : 1;
            if (other._byte7 != _byte7)
                return _byte7 < other._byte7 ? -1 : 1;
            if (other._byte8 != _byte8)
                return _byte8 < other._byte8 ? -1 : 1;
            if (other._byte9 != _byte9)
                return _byte9 < other._byte9 ? -1 : 1;
            if (other._byte10 != _byte10)
                return _byte10 < other._byte10 ? -1 : 1;
            if (other._byte11 != _byte11)
                return _byte11 < other._byte11 ? -1 : 1;
            if (other._byte12 != _byte12)
                return _byte12 < other._byte12 ? -1 : 1;
            if (other._byte13 != _byte13)
                return _byte13 < other._byte13 ? -1 : 1;
            if (other._byte14 != _byte14)
                return _byte14 < other._byte14 ? -1 : 1;
            if (other._byte15 != _byte15)
                return _byte15 < other._byte15 ? -1 : 1;
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(Uuid other)
        {
            if (other._byte0 != _byte0)
                return _byte0 < other._byte0 ? -1 : 1;
            if (other._byte1 != _byte1)
                return _byte1 < other._byte1 ? -1 : 1;
            if (other._byte2 != _byte2)
                return _byte2 < other._byte2 ? -1 : 1;
            if (other._byte3 != _byte3)
                return _byte3 < other._byte3 ? -1 : 1;
            if (other._byte4 != _byte4)
                return _byte4 < other._byte4 ? -1 : 1;
            if (other._byte5 != _byte5)
                return _byte5 < other._byte5 ? -1 : 1;
            if (other._byte6 != _byte6)
                return _byte6 < other._byte6 ? -1 : 1;
            if (other._byte7 != _byte7)
                return _byte7 < other._byte7 ? -1 : 1;
            if (other._byte8 != _byte8)
                return _byte8 < other._byte8 ? -1 : 1;
            if (other._byte9 != _byte9)
                return _byte9 < other._byte9 ? -1 : 1;
            if (other._byte10 != _byte10)
                return _byte10 < other._byte10 ? -1 : 1;
            if (other._byte11 != _byte11)
                return _byte11 < other._byte11 ? -1 : 1;
            if (other._byte12 != _byte12)
                return _byte12 < other._byte12 ? -1 : 1;
            if (other._byte13 != _byte13)
                return _byte13 < other._byte13 ? -1 : 1;
            if (other._byte14 != _byte14)
                return _byte14 < other._byte14 ? -1 : 1;
            if (other._byte15 != _byte15)
                return _byte15 < other._byte15 ? -1 : 1;
            return 0;
        }

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

        public override int GetHashCode()
        {
            return _int0 ^ _int4 ^ _int8 ^ _int12;
        }

        public static bool operator ==(Uuid left, Uuid right)
        {
            return left._ulong0 == right._ulong0 && left._ulong8 == right._ulong8;
        }

        public static bool operator !=(Uuid left, Uuid right)
        {
            return left._ulong0 != right._ulong0 || left._ulong8 != right._ulong8;
        }

        public override string ToString()
        {
            return ToString("D", null);
        }

        public string ToString(string? format)
        {
            return ToString(format, null);
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            if (string.IsNullOrEmpty(format)) format = "D";

            if (format.Length != 1)
                throw new FormatException(
                    "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");

            switch (format[0])
            {
                case 'D':
                case 'd':
                {
                    var uuidString = CoreLib.Internal.FastAllocateString(36);
                    fixed (char* uuidChars = uuidString)
                    {
                        FormatD(uuidChars);
                    }

                    return uuidString;
                }
                case 'N':
                case 'n':
                {
                    var uuidString = CoreLib.Internal.FastAllocateString(32);
                    fixed (char* uuidChars = uuidString)
                    {
                        FormatN(uuidChars);
                    }

                    return uuidString;
                }
                case 'B':
                case 'b':
                {
                    var uuidString = CoreLib.Internal.FastAllocateString(38);
                    fixed (char* uuidChars = uuidString)
                    {
                        FormatB(uuidChars);
                    }

                    return uuidString;
                }
                case 'P':
                case 'p':
                {
                    var uuidString = CoreLib.Internal.FastAllocateString(38);
                    fixed (char* uuidChars = uuidString)
                    {
                        FormatP(uuidChars);
                    }

                    return uuidString;
                }
                case 'X':
                case 'x':
                {
                    var uuidString = CoreLib.Internal.FastAllocateString(68);
                    fixed (char* uuidChars = uuidString)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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

        private static Vector256<byte> ShuffleMask = Vector256.Create(
            255, 0, 255, 2, 255, 4, 255, 6, 255, 8, 255, 10, 255, 12, 255, 14,
            255, 0, 255, 2, 255, 4, 255, 6, 255, 8, 255, 10, 255, 12, 255, 14);


        private static Vector256<byte> AsciiTable = Vector256.Create(
            (byte) 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102,
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102);
        // '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private void FormatN(char* dest)
        {
            // dddddddddddddddddddddddddddddddd
            if (Avx2.IsSupported)
            {
                fixed (Uuid* thisPtr = &this)
                {
                    var uuidVector = Avx2.ConvertToVector256Int16(Sse3.LoadDquVector128((byte*) thisPtr));
                    var hi = Avx2.ShiftRightLogical(uuidVector, 4).AsByte();
                    var lo = Avx2.Shuffle(uuidVector.AsByte(), ShuffleMask);
                    var asciiBytes = Avx2.Shuffle(AsciiTable, Avx2.And(Avx2.Or(hi, lo), Vector256.Create((byte) 0x0F)));
                    Avx.Store((short*) dest, Avx2.ConvertToVector256Int16(asciiBytes.GetLower()));
                    Avx.Store((((short*) dest) + 16), Avx2.ConvertToVector256Int16(asciiBytes.GetUpper()));
                }
            }
            else
            {
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
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private void FormatX(char* dest)
        {
            // {0xdddddddd,0xdddd,0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
            var destUints = (uint*) dest;
            var uintDestAsChars = (char**) &destUints;
            dest[0] = '{';
            dest[11] = dest[18] = dest[31] = dest[36] = dest[41] = dest[46] = dest[51] = dest[56] = dest[61] = ',';
            destUints[6] = destUints[16] = destUints[21] = destUints[26] = destUints[31] = ZeroX; // 0x
            destUints[7] = TableToHex[_byte4];
            destUints[8] = TableToHex[_byte5];
            destUints[17] = TableToHex[_byte9];
            destUints[22] = TableToHex[_byte11];
            destUints[27] = TableToHex[_byte13];
            destUints[32] = TableToHex[_byte15];
            destUints[33] = CloseBraces; // }}
            *uintDestAsChars += 1;
            destUints[0] = destUints[9] = destUints[13] = destUints[18] = destUints[23] = destUints[28] = ZeroX; // 0x
            destUints[1] = TableToHex[_byte0];
            destUints[2] = TableToHex[_byte1];
            destUints[3] = TableToHex[_byte2];
            destUints[4] = TableToHex[_byte3];
            destUints[10] = TableToHex[_byte6];
            destUints[11] = TableToHex[_byte7];
            destUints[12] = CommaBrace; // ,{
            destUints[14] = TableToHex[_byte8];
            destUints[19] = TableToHex[_byte10];
            destUints[24] = TableToHex[_byte12];
            destUints[29] = TableToHex[_byte14];
        }

        public Uuid(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            var result = new Uuid();
            var resultPtr = (byte*) &result;
            fixed (char* uuidStringPtr = input)
            {
                ParseWithExceptions(input, uuidStringPtr, resultPtr);
            }

            this = result;
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
            if (input.IsEmpty)
                throw new FormatException("Unrecognized Uuid format.");
            var result = new Uuid();
            var resultPtr = (byte*) &result;
            fixed (char* uuidStringPtr = input)
            {
                ParseWithExceptions(input, uuidStringPtr, resultPtr);
            }

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
            if (input.IsEmpty)
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

        public static Uuid ParseExact(string input, string format)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (format == null)
                throw new ArgumentNullException(nameof(input));
            var result = new Uuid();
            var resultPtr = (byte*) &result;
            switch ((char) (format[0] | 0x20))
            {
                case 'd':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsD(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'n':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsN(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'b':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsB(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'p':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsP(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'x':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsP(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                default:
                {
                    throw new FormatException(
                        "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
                }
            }
        }

        public static Uuid ParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format)
        {
            if (input.IsEmpty)
                throw new FormatException("Unrecognized Uuid format.");
            if (format.Length != 1)
                throw new FormatException(
                    "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
            var result = new Uuid();
            var resultPtr = (byte*) &result;
            switch ((char) (format[0] | 0x20))
            {
                case 'd':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsD(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'n':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsN(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'b':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsB(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'p':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsP(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                case 'x':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        ParseWithExceptionsX(input, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
                default:
                {
                    throw new FormatException(
                        "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
                }
            }
        }

        public static bool TryParseExact(string? input, string? format, out Uuid output)
        {
            if (input == null || format?.Length != 1)
            {
                output = default;
                return false;
            }

            var result = new Uuid();
            var resultPtr = (byte*) &result;
            var parsed = false;

            switch ((char) (format[0] | 0x20))
            {
                case 'd':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsD(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'n':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsN(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'b':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsB(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'p':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsP(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'x':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsX(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            }

            if (parsed)
            {
                output = result;
                return true;
            }

            output = default;
            return false;
        }

        public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out Uuid output)
        {
            if (format.Length != 1)
            {
                output = default;
                return false;
            }

            var result = new Uuid();
            var resultPtr = (byte*) &result;
            var parsed = false;

            switch ((char) (format[0] | 0x20))
            {
                case 'd':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsD(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'n':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsN(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'b':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsB(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'p':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsP(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
                case 'x':
                {
                    fixed (char* uuidStringPtr = input)
                    {
                        parsed = ParseWithoutExceptionsX(input, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            }

            if (parsed)
            {
                output = result;
                return true;
            }

            output = default;
            return false;
        }

        private static bool ParseWithoutExceptions(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length == 0)
                return false;
            switch (uuidString[0])
            {
                case '(':
                {
                    return ParseWithoutExceptionsP(uuidString, uuidStringPtr, resultPtr);
                }
                case '{':
                {
                    return uuidString.Contains('-')
                        ? ParseWithoutExceptionsB(uuidString, uuidStringPtr, resultPtr)
                        : ParseWithoutExceptionsX(uuidString, uuidStringPtr, resultPtr);
                }
                default:
                {
                    return uuidString.Contains('-')
                        ? ParseWithoutExceptionsD(uuidString, uuidStringPtr, resultPtr)
                        : ParseWithoutExceptionsN(uuidString, uuidStringPtr, resultPtr);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool ParseWithoutExceptionsD(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 36u)
                return false;

            if (uuidStringPtr[8] != '-'
                || uuidStringPtr[13] != '-'
                || uuidStringPtr[18] != '-'
                || uuidStringPtr[23] != '-')
                return false;

            return TryParsePtrD(uuidStringPtr, resultPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool ParseWithoutExceptionsN(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            return (uint) uuidString.Length == 32u && TryParsePtrN(uuidStringPtr, resultPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool ParseWithoutExceptionsB(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 38u)
                return false;
            if (uuidStringPtr[37] != '}'
                || uuidStringPtr[9] != '-'
                || uuidStringPtr[14] != '-'
                || uuidStringPtr[19] != '-'
                || uuidStringPtr[24] != '-')
                return false;

            return TryParsePtrPorB(uuidStringPtr, resultPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool ParseWithoutExceptionsP(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 38u)
                return false;
            if (uuidStringPtr[37] != ')'
                || uuidStringPtr[9] != '-'
                || uuidStringPtr[14] != '-'
                || uuidStringPtr[19] != '-'
                || uuidStringPtr[24] != '-')
                return false;

            return TryParsePtrPorB(uuidStringPtr, resultPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool ParseWithoutExceptionsX(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
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
                return false;

            return TryParsePtrX(uuidStringPtr, resultPtr);
        }

        private static void ParseWithExceptions(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length == 0)
                throw new FormatException("Unrecognized Uuid format.");

            switch (uuidString[0])
            {
                case '(':
                {
                    ParseWithExceptionsP(uuidString, uuidStringPtr, resultPtr);
                    break;
                }
                case '{':
                {
                    if (uuidString.Contains('-'))
                    {
                        ParseWithExceptionsB(uuidString, uuidStringPtr, resultPtr);
                        break;
                    }

                    ParseWithExceptionsX(uuidString, uuidStringPtr, resultPtr);
                    break;
                }
                default:
                {
                    if (uuidString.Contains('-'))
                    {
                        ParseWithExceptionsD(uuidString, uuidStringPtr, resultPtr);
                        break;
                    }

                    ParseWithExceptionsN(uuidString, uuidStringPtr, resultPtr);
                    break;
                }
            }
        }

        private static void ParseWithExceptionsD(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 36u)
                throw new FormatException(
                    "Uuid should contain 32 digits with 4 dashes xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.");

            if (uuidStringPtr[8] != '-' || uuidStringPtr[13] != '-' || uuidStringPtr[18] != '-' ||
                uuidStringPtr[23] != '-')
                throw new FormatException("Dashes are in the wrong position for Uuid parsing.");

            if (!TryParsePtrD(uuidStringPtr, resultPtr))
                throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }

        private static void ParseWithExceptionsN(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 32u)
                throw new FormatException(
                    "Uuid should contain only 32 digits xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.");

            if (!TryParsePtrN(uuidStringPtr, resultPtr))
                throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }

        private static void ParseWithExceptionsB(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 38u || uuidString[37] != '}')
                throw new FormatException(
                    "Uuid should contain 32 digits with 4 dashes {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}.");

            if (uuidStringPtr[9] != '-' || uuidStringPtr[14] != '-' || uuidStringPtr[19] != '-' ||
                uuidStringPtr[24] != '-')
                throw new FormatException("Dashes are in the wrong position for Uuid parsing.");

            if (!TryParsePtrPorB(uuidStringPtr, resultPtr))
                throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }

        private static void ParseWithExceptionsP(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 38u || uuidString[37] != ')')
                throw new FormatException(
                    "Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            if (uuidStringPtr[9] != '-' || uuidStringPtr[14] != '-' || uuidStringPtr[19] != '-' ||
                uuidStringPtr[24] != '-')
                throw new FormatException("Dashes are in the wrong position for Uuid parsing.");

            if (!TryParsePtrPorB(uuidStringPtr, resultPtr))
                throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }

        private static void ParseWithExceptionsX(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
        {
            if ((uint) uuidString.Length != 68u || uuidString[0] != '{' || uuidString[66] != '}')
                throw new FormatException(
                    "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");

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
                throw new FormatException(
                    "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");

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
                throw new FormatException("Expected 0x prefix.");

            if (uuidStringPtr[67] != '}') throw new FormatException("Could not find the ending brace.");

            if (!TryParsePtrX(uuidStringPtr, resultPtr))
                throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
                                                            resultPtr[11] =
                                                                (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                            // 12 byte
                                                            if (value[28] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[28]]) != 0xFF
                                                                && value[29] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[29]]) !=
                                                                0xFF)
                                                            {
                                                                resultPtr[12] =
                                                                    (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                // 13 byte
                                                                if (value[30] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[30]]) !=
                                                                    0xFF
                                                                    && value[31] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[31]]) !=
                                                                    0xFF)
                                                                {
                                                                    resultPtr[13] =
                                                                        (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                    // 14 byte
                                                                    if (value[32] < MaximalChar
                                                                        && (hexByteHi =
                                                                            TableFromHexToBytes[value[32]]) != 0xFF
                                                                        && value[33] < MaximalChar
                                                                        && (hexByteLow =
                                                                            TableFromHexToBytes[value[33]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] =
                                                                            (byte) ((byte) (hexByteHi << 4) | hexByteLow
                                                                            );
                                                                        // 15 byte
                                                                        if (value[34] < MaximalChar
                                                                            && (hexByteHi =
                                                                                TableFromHexToBytes[value[34]]) != 0xFF
                                                                            && value[35] < MaximalChar
                                                                            && (hexByteLow =
                                                                                TableFromHexToBytes[value[35]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] =
                                                                                (byte) ((byte) (hexByteHi << 4) |
                                                                                        hexByteLow);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
                                                            resultPtr[11] =
                                                                (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                            // 12 byte
                                                            if (value[24] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[24]]) != 0xFF
                                                                && value[25] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[25]]) !=
                                                                0xFF)
                                                            {
                                                                resultPtr[12] =
                                                                    (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                // 13 byte
                                                                if (value[26] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[26]]) !=
                                                                    0xFF
                                                                    && value[27] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[27]]) !=
                                                                    0xFF)
                                                                {
                                                                    resultPtr[13] =
                                                                        (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                    // 14 byte
                                                                    if (value[28] < MaximalChar
                                                                        && (hexByteHi =
                                                                            TableFromHexToBytes[value[28]]) != 0xFF
                                                                        && value[29] < MaximalChar
                                                                        && (hexByteLow =
                                                                            TableFromHexToBytes[value[29]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] =
                                                                            (byte) ((byte) (hexByteHi << 4) | hexByteLow
                                                                            );
                                                                        // 15 byte
                                                                        if (value[30] < MaximalChar
                                                                            && (hexByteHi =
                                                                                TableFromHexToBytes[value[30]]) != 0xFF
                                                                            && value[31] < MaximalChar
                                                                            && (hexByteLow =
                                                                                TableFromHexToBytes[value[31]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] =
                                                                                (byte) ((byte) (hexByteHi << 4) |
                                                                                        hexByteLow);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
                                                            resultPtr[11] =
                                                                (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                            // 12 byte
                                                            if (value[29] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[29]]) != 0xFF
                                                                && value[30] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[30]]) !=
                                                                0xFF)
                                                            {
                                                                resultPtr[12] =
                                                                    (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                // 13 byte
                                                                if (value[31] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[31]]) !=
                                                                    0xFF
                                                                    && value[32] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[32]]) !=
                                                                    0xFF)
                                                                {
                                                                    resultPtr[13] =
                                                                        (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                    // 14 byte
                                                                    if (value[33] < MaximalChar
                                                                        && (hexByteHi =
                                                                            TableFromHexToBytes[value[33]]) != 0xFF
                                                                        && value[34] < MaximalChar
                                                                        && (hexByteLow =
                                                                            TableFromHexToBytes[value[34]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] =
                                                                            (byte) ((byte) (hexByteHi << 4) | hexByteLow
                                                                            );
                                                                        // 15 byte
                                                                        if (value[35] < MaximalChar
                                                                            && (hexByteHi =
                                                                                TableFromHexToBytes[value[35]]) != 0xFF
                                                                            && value[36] < MaximalChar
                                                                            && (hexByteLow =
                                                                                TableFromHexToBytes[value[36]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] =
                                                                                (byte) ((byte) (hexByteHi << 4) |
                                                                                        hexByteLow);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
                                                            resultPtr[11] =
                                                                (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                            // value[46] == ','
                                                            // value[47] == '0'
                                                            // value[48] == 'x'

                                                            // 12 byte
                                                            if (value[49] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[49]]) != 0xFF
                                                                && value[50] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[50]]) !=
                                                                0xFF)
                                                            {
                                                                resultPtr[12] =
                                                                    (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                // value[51] == ','
                                                                // value[52] == '0'
                                                                // value[53] == 'x'

                                                                // 13 byte
                                                                if (value[54] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[54]]) !=
                                                                    0xFF
                                                                    && value[55] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[55]]) !=
                                                                    0xFF)
                                                                {
                                                                    resultPtr[13] =
                                                                        (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                    // value[56] == ','
                                                                    // value[57] == '0'
                                                                    // value[58] == 'x'

                                                                    // 14 byte
                                                                    if (value[59] < MaximalChar
                                                                        && (hexByteHi =
                                                                            TableFromHexToBytes[value[59]]) != 0xFF
                                                                        && value[60] < MaximalChar
                                                                        && (hexByteLow =
                                                                            TableFromHexToBytes[value[60]]) != 0xFF)
                                                                    {
                                                                        resultPtr[14] =
                                                                            (byte) ((byte) (hexByteHi << 4) | hexByteLow
                                                                            );

                                                                        // value[61] == ','
                                                                        // value[62] == '0'
                                                                        // value[63] == 'x'

                                                                        // 15 byte
                                                                        if (value[64] < MaximalChar
                                                                            && (hexByteHi =
                                                                                TableFromHexToBytes[value[64]]) != 0xFF
                                                                            && value[65] < MaximalChar
                                                                            && (hexByteLow =
                                                                                TableFromHexToBytes[value[65]]) != 0xFF)
                                                                        {
                                                                            resultPtr[15] =
                                                                                (byte) ((byte) (hexByteHi << 4) |
                                                                                        hexByteLow);
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
    }
}