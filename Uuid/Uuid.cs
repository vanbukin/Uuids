using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

        private enum UuidParseThrowStyle : byte
        {
            None = 0,
            All = 1,
            AllButOverflow = 2
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        private ref struct ParsedUuid
        {
            [FieldOffset(0)] internal byte _byte0;
            [FieldOffset(1)] internal byte _byte1;
            [FieldOffset(2)] internal byte _byte2;
            [FieldOffset(3)] internal byte _byte3;
            [FieldOffset(4)] internal byte _byte4;
            [FieldOffset(5)] internal byte _byte5;
            [FieldOffset(6)] internal byte _byte6;
            [FieldOffset(7)] internal byte _byte7;
            [FieldOffset(8)] internal byte _byte8;
            [FieldOffset(9)] internal byte _byte9;
            [FieldOffset(10)] internal byte _byte10;
            [FieldOffset(11)] internal byte _byte11;
            [FieldOffset(12)] internal byte _byte12;
            [FieldOffset(13)] internal byte _byte13;
            [FieldOffset(14)] internal byte _byte14;
            [FieldOffset(15)] internal byte _byte15;

            [FieldOffset(0)] internal ulong _ulong0;
            [FieldOffset(8)] internal ulong _ulong8;

            [FieldOffset(0)] internal uint _uint0;
            [FieldOffset(4)] internal uint _uint4;
            [FieldOffset(8)] internal uint _uint8;
            [FieldOffset(12)] internal uint _uint12;

            [FieldOffset(0)] internal short _short0;
            [FieldOffset(2)] internal short _short2;
            [FieldOffset(4)] internal short _short4;
            [FieldOffset(6)] internal short _short6;
            [FieldOffset(8)] internal short _short8;
            [FieldOffset(10)] internal short _short10;
            [FieldOffset(12)] internal short _short12;
            [FieldOffset(14)] internal short _short14;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        private ref struct UuidResult
        {
            [FieldOffset(0)] private readonly UuidParseThrowStyle _throwStyle;
            [FieldOffset(1)] internal ParsedUuid _parsedUuid;

            internal UuidResult(UuidParseThrowStyle canThrow) : this()
            {
                _throwStyle = canThrow;
            }

            internal void SetFailure(bool overflow, string failureMessage)
            {
                if (_throwStyle == UuidParseThrowStyle.None) return;

                if (overflow)
                {
                    if (_throwStyle == UuidParseThrowStyle.All) throw new OverflowException(failureMessage);

                    throw new FormatException("Unrecognized Uuid format.");
                }

                throw new FormatException(failureMessage);
            }
        }

        public Uuid(string uuidString)
        {
            if (uuidString == null)
                throw new ArgumentNullException(nameof(uuidString));
            var result = new UuidResult(UuidParseThrowStyle.All);
            TryParseUuid(uuidString, ref result);
            this = Unsafe.Read<Uuid>(&result._parsedUuid);
        }

        public static Uuid Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            var result = new UuidResult(UuidParseThrowStyle.AllButOverflow);
            TryParseUuid((ReadOnlySpan<char>) input, ref result);
            return Unsafe.Read<Uuid>(&result._parsedUuid);
        }

        public static Uuid Parse(ReadOnlySpan<char> input)
        {
            var result = new UuidResult(UuidParseThrowStyle.AllButOverflow);
            TryParseUuid(input, ref result);
            return Unsafe.Read<Uuid>(&result._parsedUuid);
        }

        public static bool TryParse(string? input, out Uuid result)
        {
            if (input == null)
            {
                result = default;
                return false;
            }

            return TryParse((ReadOnlySpan<char>) input, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> input, out Uuid result)
        {
            var parseResult = new UuidResult(UuidParseThrowStyle.None);
            if (TryParseUuid(input, ref parseResult))
            {
                result = Unsafe.Read<Uuid>(&parseResult._parsedUuid);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public static Uuid ParseExact(string input, string format)
        {
            return ParseExact(
                input != null ? (ReadOnlySpan<char>) input : throw new ArgumentNullException(nameof(input)),
                format != null ? (ReadOnlySpan<char>) format : throw new ArgumentNullException(nameof(format)));
        }

        public static Uuid ParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format)
        {
            if (format.Length != 1)
                // all acceptable format strings are of length 1
                throw new FormatException(
                    "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");

            input = input.Trim();

            var result = new UuidResult(UuidParseThrowStyle.AllButOverflow);
            // ReSharper disable once UnusedVariable
            var success = (char) (format[0] | 0x20) switch
            {
                'd' => TryParseExactD(input, ref result),
                'n' => TryParseExactN(input, ref result),
                'b' => TryParseExactB(input, ref result),
                'p' => TryParseExactP(input, ref result),
                'x' => TryParseExactX(input, ref result),
                _ => throw new FormatException(
                    "Format string can be only \"D\", \"d\", \"N\", \"n\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\"."),
            };
            return Unsafe.Read<Uuid>(&result._parsedUuid);
        }

        public static bool TryParseExact(string? input, string? format, out Uuid result)
        {
            if (input == null)
            {
                result = default;
                return false;
            }

            return TryParseExact((ReadOnlySpan<char>) input, format, out result);
        }

        [SuppressMessage("ReSharper", "RedundantIfElseBlock")]
        public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out Uuid result)
        {
            if (format.Length != 1)
            {
                result = default;
                return false;
            }

            input = input.Trim();

            var parseResult = new UuidResult(UuidParseThrowStyle.None);
            var success = false;
            switch ((char) (format[0] | 0x20))
            {
                case 'd':
                    success = TryParseExactD(input, ref parseResult);
                    break;

                case 'n':
                    success = TryParseExactN(input, ref parseResult);
                    break;

                case 'b':
                    success = TryParseExactB(input, ref parseResult);
                    break;

                case 'p':
                    success = TryParseExactP(input, ref parseResult);
                    break;

                case 'x':
                    success = TryParseExactX(input, ref parseResult);
                    break;
            }

            if (success)
            {
                result = Unsafe.Read<Uuid>(&parseResult._parsedUuid);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        private static bool TryParseUuid(ReadOnlySpan<char> uuidString, ref UuidResult result)
        {
            uuidString = uuidString.Trim(); // Remove whitespace from beginning and end

            if (uuidString.Length == 0)
            {
                result.SetFailure(false, "Unrecognized Uuid format.");
                return false;
            }

            return uuidString[0] switch
            {
                '(' => TryParseExactP(uuidString, ref result),
                '{' => uuidString.Contains('-')
                    ? TryParseExactB(uuidString, ref result)
                    : TryParseExactX(uuidString, ref result),
                _ => uuidString.Contains('-')
                    ? TryParseExactD(uuidString, ref result)
                    : TryParseExactN(uuidString, ref result),
            };
        }

        private static bool TryParseExactB(ReadOnlySpan<char> uuidString, ref UuidResult result)
        {
            // e.g. "{d85b1407-351d-4694-9392-03acc5870eb1}"

            if ((uint) uuidString.Length != 38 || uuidString[0] != '{' || uuidString[37] != '}')
            {
                result.SetFailure(false,
                    "Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                return false;
            }

            return TryParseExactD(uuidString.Slice(1, 36), ref result);
        }

        private static bool TryParseExactD(ReadOnlySpan<char> uuidString, ref UuidResult result)
        {
            // e.g. "d85b1407-351d-4694-9392-03acc5870eb1"

            if ((uint) uuidString.Length != 36)
            {
                result.SetFailure(false,
                    "Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                return false;
            }

            if (uuidString[8] != '-' || uuidString[13] != '-' || uuidString[18] != '-' || uuidString[23] != '-')
            {
                result.SetFailure(false, "Dashes are in the wrong position for Uuid parsing.");
                return false;
            }

            ref var parsedUuid = ref result._parsedUuid;

            if (TryParseHex(uuidString.Slice(0, 8), out var uintTmp0)
                && TryParseHex(uuidString.Slice(9, 4), out var uintTmp9)
                && TryParseHex(uuidString.Slice(14, 4), out var uintTmp14)
                && TryParseHex(uuidString.Slice(19, 4), out var uintTmp19)
                && TryParseHex(uuidString.Slice(24, 4), out var uintTmp24)
                && uint.TryParse(uuidString.Slice(28, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmp28))
            {
                parsedUuid._uint0 = uintTmp0;
                parsedUuid._short4 = (short) uintTmp9;
                parsedUuid._short6 = (short) uintTmp14;
                parsedUuid._short8 = (short) uintTmp19;
                parsedUuid._short10 = (short) uintTmp24;
                parsedUuid._uint12 = uintTmp28;
                return true;
            }

            result.SetFailure(false, "Uuid string should only contain hexadecimal characters.");
            return false;
        }

        private static bool TryParseExactN(ReadOnlySpan<char> uuidString, ref UuidResult result)
        {
            // e.g. "d85b1407351d4694939203acc5870eb1"

            if ((uint) uuidString.Length != 32)
            {
                result.SetFailure(false, "Uuid should contain only 32 digits (xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx).");
                return false;
            }

            ref var parsedUuid = ref result._parsedUuid;

            if (uint.TryParse(uuidString.Slice(0, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmp0)
                && uint.TryParse(uuidString.Slice(8, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmp8)
                && uint.TryParse(uuidString.Slice(16, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmp16)
                && uint.TryParse(uuidString.Slice(24, 8), NumberStyles.AllowHexSpecifier, null, out var uintTmp24)
            )
            {
                parsedUuid._uint0 = uintTmp0;
                parsedUuid._uint4 = uintTmp8;
                parsedUuid._uint8 = uintTmp16;
                parsedUuid._uint12 = uintTmp24;
                return true;
            }

            result.SetFailure(false, "Uuid string should only contain hexadecimal characters.");
            return false;
        }

        private static bool TryParseExactP(ReadOnlySpan<char> uuidString, ref UuidResult result)
        {
            // e.g. "(d85b1407-351d-4694-9392-03acc5870eb1)"

            if ((uint) uuidString.Length != 38 || uuidString[0] != '(' || uuidString[37] != ')')
            {
                result.SetFailure(false,
                    "Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                return false;
            }

            return TryParseExactD(uuidString.Slice(1, 36), ref result);
        }

        private static bool TryParseExactX(ReadOnlySpan<char> uuidString, ref UuidResult result)
        {
            // e.g. "{0xd85b1407,0x351d,0x4694,{0x93,0x92,0x03,0xac,0xc5,0x87,0x0e,0xb1}}"

            uuidString = EatAllWhitespace(uuidString);

            // Check for leading '{'
            if ((uint) uuidString.Length == 0 || uuidString[0] != '{')
            {
                result.SetFailure(
                    false,
                    "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");
                return false;
            }

            // Check for '0x'
            if (!IsHexPrefix(uuidString, 1))
            {
                result.SetFailure(false, "Expected 0x prefix.");
                return false;
            }

            // Find the end of this hex number (since it is not fixed length)
            var numStart = 3;
            var numLen = uuidString.Slice(numStart).IndexOf(',');
            if (numLen <= 0)
            {
                result.SetFailure(
                    false,
                    "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
                return false;
            }


            var overflow = false;
            if (!TryParseHex(uuidString.Slice(numStart, numLen), out result._parsedUuid._uint0, ref overflow) ||
                overflow)
            {
                result.SetFailure(overflow, overflow
                    ? "Value was either too large or too small for a UInt32."
                    : "Uuid string should only contain hexadecimal characters.");
                return false;
            }

            // Check for '0x'
            if (!IsHexPrefix(uuidString, numStart + numLen + 1))
            {
                result.SetFailure(false, "Expected 0x prefix.");
                return false;
            }

            // +3 to get by ',0x'
            numStart = numStart + numLen + 3;
            numLen = uuidString.Slice(numStart).IndexOf(',');
            if (numLen <= 0)
            {
                result.SetFailure(false,
                    "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
                return false;
            }

            // Read in the number
            if (!TryParseHex(uuidString.Slice(numStart, numLen), out result._parsedUuid._short4, ref overflow) ||
                overflow)
            {
                result.SetFailure(overflow, overflow
                    ? "Value was either too large or too small for a UInt32."
                    : "Uuid string should only contain hexadecimal characters.");
                return false;
            }

            // Check for '0x'
            if (!IsHexPrefix(uuidString, numStart + numLen + 1))
            {
                result.SetFailure(false, "Expected 0x prefix.");
                return false;
            }

            // +3 to get by ',0x'
            numStart = numStart + numLen + 3;
            numLen = uuidString.Slice(numStart).IndexOf(',');
            if (numLen <= 0)
            {
                result.SetFailure(false,
                    "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
                return false;
            }

            // Read in the number
            if (!TryParseHex(uuidString.Slice(numStart, numLen), out result._parsedUuid._short6, ref overflow) ||
                overflow)
            {
                result.SetFailure(overflow,
                    overflow
                        ? "Value was either too large or too small for a UInt32."
                        : "Uuid string should only contain hexadecimal characters.");
                return false;
            }

            // Check for '{'
            if ((uint) uuidString.Length <= (uint) (numStart + numLen + 1) || uuidString[numStart + numLen + 1] != '{')
            {
                result.SetFailure(false, "Expected {0xdddddddd, etc}.");
                return false;
            }

            // Prepare for loop
            numLen++;
            for (var i = 0; i < 8; i++)
            {
                // Check for '0x'
                if (!IsHexPrefix(uuidString, numStart + numLen + 1))
                {
                    result.SetFailure(false, "Expected 0x prefix.");
                    return false;
                }

                // +3 to get by ',0x' or '{0x' for first case
                numStart = numStart + numLen + 3;

                // Calculate number length
                if (i < 7) // first 7 cases
                {
                    numLen = uuidString.Slice(numStart).IndexOf(',');
                    if (numLen <= 0)
                    {
                        result.SetFailure(false,
                            "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
                        return false;
                    }
                }
                else // last case ends with '}', not ','
                {
                    numLen = uuidString.Slice(numStart).IndexOf('}');
                    if (numLen <= 0)
                    {
                        result.SetFailure(false,
                            "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");
                        return false;
                    }
                }

                // Read in the number
                uint byteVal;
                if (!TryParseHex(uuidString.Slice(numStart, numLen), out byteVal, ref overflow) || overflow ||
                    byteVal > byte.MaxValue)
                {
                    // The previous implementation had some odd inconsistencies, which are carried forward here.
                    // The byte values in the X format are treated as integers with regards to overflow, so
                    // a "byte" value like 0xddd in Uuid's ctor results in a FormatException but 0xddddddddd results
                    // in OverflowException.
                    result.SetFailure(overflow,
                        overflow
                            ? "Value was either too large or too small for a UInt32."
                            : byteVal > byte.MaxValue
                                ? "Value was either too large or too small for an unsigned byte."
                                : "Uuid string should only contain hexadecimal characters.");
                    return false;
                }

                Unsafe.Add(ref result._parsedUuid._byte8, i) = (byte) byteVal;
            }

            // Check for last '}'
            if (numStart + numLen + 1 >= uuidString.Length || uuidString[numStart + numLen + 1] != '}')
            {
                result.SetFailure(false, "Could not find the ending brace.");
                return false;
            }

            // Check if we have extra characters at the end
            if (numStart + numLen + 1 != uuidString.Length - 1)
            {
                result.SetFailure(false, "Additional non-parsable characters are at the end of the string.");
                return false;
            }

            return true;
        }

        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out short result, ref bool overflow)
        {
            var success = TryParseHex(uuidString, out uint tmp, ref overflow);
            result = (short) tmp;
            return success;
        }

        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out uint result)
        {
            var overflowIgnored = false;
            return TryParseHex(uuidString, out result, ref overflowIgnored);
        }

        private static bool TryParseHex(ReadOnlySpan<char> uuidString, out uint result, ref bool overflow)
        {
            if ((uint) uuidString.Length > 0)
            {
                if (uuidString[0] == '+') uuidString = uuidString.Slice(1);

                if ((uint) uuidString.Length > 1 && uuidString[0] == '0' && (uuidString[1] | 0x20) == 'x')
                    uuidString = uuidString.Slice(2);
            }

            // Skip past leading 0s.
            var i = 0;
            for (; i < uuidString.Length && uuidString[i] == '0'; i++)
            {
            }

            var processedDigits = 0;
            uint tmp = 0;
            for (; i < uuidString.Length; i++)
            {
                int numValue;
                var c = uuidString[i];
                if (c >= (uint) CharToHexLookup.Length || (numValue = CharToHexLookup[c]) == 0xFF)
                {
                    if (processedDigits > 8) overflow = true;
                    result = 0;
                    return false;
                }

                tmp = (tmp * 16) + (uint) numValue;
                processedDigits++;
            }

            if (processedDigits > 8) overflow = true;
            result = tmp;
            return true;
        }

        private static ReadOnlySpan<char> EatAllWhitespace(ReadOnlySpan<char> str)
        {
            // Find the first whitespace character.  If there is none, just return the input.
            int i;
            for (i = 0; i < str.Length && !char.IsWhiteSpace(str[i]); i++)
            {
            }

            if (i == str.Length) return str;

            // There was at least one whitespace.  Copy over everything prior to it to a new array.
            var chArr = new char[str.Length];
            var newLength = 0;
            if (i > 0)
            {
                newLength = i;
                str.Slice(0, i).CopyTo(chArr);
            }

            // Loop through the remaining chars, copying over non-whitespace.
            for (; i < str.Length; i++)
            {
                var c = str[i];
                if (!char.IsWhiteSpace(c)) chArr[newLength++] = c;
            }

            // Return the string with the whitespace removed.
            return new ReadOnlySpan<char>(chArr, 0, newLength);
        }

        private static bool IsHexPrefix(ReadOnlySpan<char> str, int i)
        {
            return i + 1 < str.Length &&
                   str[i] == '0' &&
                   (str[i + 1] | 0x20) == 'x';
        }
    }
}