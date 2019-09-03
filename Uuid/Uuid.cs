using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Uuid
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public readonly unsafe struct Uuid : IFormattable, IComparable, IComparable<Uuid>, IEquatable<Uuid>
    {
        static Uuid()
        {
            TableToHex = (uint*) Marshal.AllocHGlobal(sizeof(uint) * 256).ToPointer();
            for (var i = 0; i < 256; i++)
            {
                var chars = Convert.ToString(i, 16).PadLeft(2, '0');
                TableToHex[i] = ((uint) chars[1] << 16) | chars[0];
            }

#nullable disable
            // ReSharper disable once PossibleNullReferenceException
            FastAllocateString = (Func<int, string>) typeof(string)
                .GetMethod(nameof(FastAllocateString), BindingFlags.Static | BindingFlags.NonPublic)
                .CreateDelegate(typeof(Func<int, string>));
#nullable restore
        }

        private static readonly uint* TableToHex;
        private static readonly Func<int, string> FastAllocateString;

        // ReSharper disable once RedundantDefaultMemberInitializer
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly Uuid Empty = new Uuid();


        [FieldOffset(0)] private readonly byte _byte0;
        [FieldOffset(1)] private readonly byte _byte1;
        [FieldOffset(2)] private readonly byte _byte2;
        [FieldOffset(3)] private readonly byte _byte3;
        [FieldOffset(4)] private readonly byte _byte4;
        [FieldOffset(5)] private readonly byte _byte5;
        [FieldOffset(6)] private readonly byte _byte6;
        [FieldOffset(7)] private readonly byte _byte7;
        [FieldOffset(8)] private readonly byte _byte8;
        [FieldOffset(9)] private readonly byte _byte9;
        [FieldOffset(10)] private readonly byte _byte10;
        [FieldOffset(11)] private readonly byte _byte11;
        [FieldOffset(12)] private readonly byte _byte12;
        [FieldOffset(13)] private readonly byte _byte13;
        [FieldOffset(14)] private readonly byte _byte14;
        [FieldOffset(15)] private readonly byte _byte15;

        [FieldOffset(0)] private readonly ulong _ulong0;
        [FieldOffset(8)] private readonly ulong _ulong1;

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
            this = Unsafe.Read<Uuid>(bytes);
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
            fixed (byte* bytePtr = result)
            {
                var ulongPtr = (ulong*) bytePtr;
                ulongPtr[0] = _ulong0;
                ulongPtr[1] = _ulong1;
            }

            return result;
        }

        public bool TryWriteBytes(Span<byte> destination)
        {
            if ((uint) destination.Length < 16)
                return false;
            fixed (byte* bytePtr = &destination.GetPinnableReference())
            {
                var ulongPtr = (ulong*) bytePtr;
                ulongPtr[0] = _ulong0;
                ulongPtr[1] = _ulong1;
            }

            return true;
        }

        public int CompareTo(object? value)
        {
            if (value == null) return 1;

            if (!(value is Uuid)) throw new ArgumentException("Object must be of type Uuid.", nameof(value));

            var other = (Uuid) value;

            if (other._ulong0 != _ulong0) return _ulong0 < other._ulong0 ? -1 : 1;

            if (other._ulong1 != _ulong1) return _ulong1 < other._ulong1 ? -1 : 1;

            return 0;
        }

        public int CompareTo(Uuid other)
        {
            if (other._ulong0 != _ulong0) return _ulong0 < other._ulong0 ? -1 : 1;

            if (other._ulong1 != _ulong1) return _ulong1 < other._ulong1 ? -1 : 1;

            return 0;
        }

        public bool Equals(Uuid other)
        {
            return _ulong0 == other._ulong0 && _ulong1 == other._ulong1;
        }

        [SuppressMessage("ReSharper", "RedundantAssignment")]
        [SuppressMessage("ReSharper", "RedundantIfElseBlock")]
        [SuppressMessage("ReSharper", "MergeSequentialChecks")]
        public override bool Equals(object? obj)
        {
            Uuid other;
            // Check that o is a Uuid first
            if (obj == null || !(obj is Uuid))
                return false;
            else
                other = (Uuid) obj;

            return _ulong0 == other._ulong0 && _ulong1 == other._ulong1;
        }

        [SuppressMessage("ReSharper", "RedundantCast")]
        [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
        public override int GetHashCode()
        {
            var xorULongs = _ulong0 ^ _ulong1;
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
                    var guidString = FastAllocateString(36);
                    fixed (char* guidChars = &guidString.GetPinnableReference())
                    {
                        FormatD(guidChars);
                    }

                    return guidString;
                }
                case 'N':
                case 'n':
                {
                    var guidString = FastAllocateString(32);
                    fixed (char* guidChars = &guidString.GetPinnableReference())
                    {
                        FormatN(guidChars);
                    }

                    return guidString;
                }
                case 'B':
                case 'b':
                {
                    var guidString = FastAllocateString(38);
                    fixed (char* guidChars = &guidString.GetPinnableReference())
                    {
                        FormatB(guidChars);
                    }

                    return guidString;
                }
                case 'P':
                case 'p':
                {
                    var guidString = FastAllocateString(38);
                    fixed (char* guidChars = &guidString.GetPinnableReference())
                    {
                        FormatP(guidChars);
                    }

                    return guidString;
                }
                case 'X':
                case 'x':
                {
                    var guidString = FastAllocateString(68);
                    fixed (char* guidChars = &guidString.GetPinnableReference())
                    {
                        FormatX(guidChars);
                    }

                    return guidString;
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
    }
}