using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Uuids.Benchmarks
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "RedundantArrayCreationExpression")]
    public unsafe class UuidTryParseBenchmarks
    {
        private string[] _sometimesBrokenRandomUuidsP_1_000_000 = new string[] { };
        private string[] _sometimesBrokenRandomUuidsB_1_000_000 = new string[] { };
        private string[] _sometimesBrokenRandomUuidsX_1_000_000 = new string[] { };
        private string[] _sometimesBrokenRandomUuidsD_1_000_000 = new string[] { };
        private string[] _sometimesBrokenRandomUuidsN_1_000_000 = new string[] { };

        [GlobalSetup]
        public void Setup()
        {
            _sometimesBrokenRandomUuidsP_1_000_000 = GenerateSometimesBrokenGuidsPStringsArray(1_000_000);
            _sometimesBrokenRandomUuidsB_1_000_000 = GenerateSometimesBrokenGuidsBStringsArray(1_000_000);
            _sometimesBrokenRandomUuidsX_1_000_000 = GenerateSometimesBrokenGuidsXStringsArray(1_000_000);
            _sometimesBrokenRandomUuidsD_1_000_000 = GenerateSometimesBrokenGuidsDStringsArray(1_000_000);
            _sometimesBrokenRandomUuidsN_1_000_000 = GenerateSometimesBrokenGuidsNStringsArray(1_000_000);
        }

        // P
        [Benchmark]
        public void uuid_TryParseP_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsP_1_000_000)
            {
                var _ = Uuid.TryParse(brokenString, out var _);
            }
        }

        [Benchmark]
        public void guid_TryParseP_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsP_1_000_000)
            {
                var _ = Guid.TryParse(brokenString, out var _);
            }
        }

        // B
        [Benchmark]
        public void uuid_TryParseB_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsB_1_000_000)
            {
                var _ = Uuid.TryParse(brokenString, out var _);
            }
        }

        [Benchmark]
        public void guid_TryParseB_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsB_1_000_000)
            {
                var _ = Guid.TryParse(brokenString, out var _);
            }
        }

        // X
        [Benchmark]
        public void uuid_TryParseX_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsX_1_000_000)
            {
                var _ = Uuid.TryParse(brokenString, out var _);
            }
        }

        [Benchmark]
        public void guid_TryParseX_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsX_1_000_000)
            {
                var _ = Guid.TryParse(brokenString, out var _);
            }
        }

        // D
        [Benchmark]
        public void uuid_TryParseD_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsD_1_000_000)
            {
                var _ = Uuid.TryParse(brokenString, out var _);
            }
        }

        [Benchmark]
        public void guid_TryParseD_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsD_1_000_000)
            {
                var _ = Guid.TryParse(brokenString, out var _);
            }
        }

        // N
        [Benchmark]
        public void uuid_TryParseN_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsN_1_000_000)
            {
                var _ = Uuid.TryParse(brokenString, out var _);
            }
        }

        [Benchmark]
        public void guid_TryParseN_1kk()
        {
            foreach (var brokenString in _sometimesBrokenRandomUuidsN_1_000_000)
            {
                var _ = Guid.TryParse(brokenString, out var _);
            }
        }

        public static string[] GenerateSometimesBrokenGuidsPStringsArray(int count)
        {
            // (dddddddd-dddd-dddd-dddd-dddddddddddd)
            var nStrings = GenerateSometimesBrokenGuidsNStringsArray(count);
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = string.Create(38, nStrings[i], (span, stringN) =>
                {
                    span[0] = '(';
                    span[9] = span[14] = span[19] = span[24] = '-';
                    span[1] = stringN[0];
                    span[2] = stringN[1];
                    span[3] = stringN[2];
                    span[4] = stringN[3];
                    span[5] = stringN[4];
                    span[6] = stringN[5];
                    span[7] = stringN[6];
                    span[8] = stringN[7];

                    span[10] = stringN[8];
                    span[11] = stringN[9];
                    span[12] = stringN[10];
                    span[13] = stringN[11];

                    span[15] = stringN[12];
                    span[16] = stringN[13];
                    span[17] = stringN[14];
                    span[18] = stringN[15];

                    span[20] = stringN[16];
                    span[21] = stringN[17];
                    span[22] = stringN[18];
                    span[23] = stringN[19];

                    span[25] = stringN[20];
                    span[26] = stringN[21];
                    span[27] = stringN[22];
                    span[28] = stringN[23];
                    span[29] = stringN[24];
                    span[30] = stringN[25];
                    span[31] = stringN[26];
                    span[32] = stringN[27];
                    span[33] = stringN[28];
                    span[34] = stringN[29];
                    span[35] = stringN[30];
                    span[36] = stringN[31];
                    span[37] = ')';
                });
            }

            return result;
        }

        public static string[] GenerateSometimesBrokenGuidsBStringsArray(int count)
        {
            // {dddddddd-dddd-dddd-dddd-dddddddddddd}
            var nStrings = GenerateSometimesBrokenGuidsNStringsArray(count);
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = string.Create(38, nStrings[i], (span, stringN) =>
                {
                    span[0] = '{';
                    span[9] = span[14] = span[19] = span[24] = '-';
                    span[1] = stringN[0];
                    span[2] = stringN[1];
                    span[3] = stringN[2];
                    span[4] = stringN[3];
                    span[5] = stringN[4];
                    span[6] = stringN[5];
                    span[7] = stringN[6];
                    span[8] = stringN[7];

                    span[10] = stringN[8];
                    span[11] = stringN[9];
                    span[12] = stringN[10];
                    span[13] = stringN[11];

                    span[15] = stringN[12];
                    span[16] = stringN[13];
                    span[17] = stringN[14];
                    span[18] = stringN[15];

                    span[20] = stringN[16];
                    span[21] = stringN[17];
                    span[22] = stringN[18];
                    span[23] = stringN[19];

                    span[25] = stringN[20];
                    span[26] = stringN[21];
                    span[27] = stringN[22];
                    span[28] = stringN[23];
                    span[29] = stringN[24];
                    span[30] = stringN[25];
                    span[31] = stringN[26];
                    span[32] = stringN[27];
                    span[33] = stringN[28];
                    span[34] = stringN[29];
                    span[35] = stringN[30];
                    span[36] = stringN[31];
                    span[37] = '}';
                });
            }

            return result;
        }

        public static string[] GenerateSometimesBrokenGuidsXStringsArray(int count)
        {
            // {0xdddddddd,0xdddd,0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
            var nStrings = GenerateSometimesBrokenGuidsNStringsArray(count);
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = string.Create(68, nStrings[i], (span, stringN) =>
                {
                    span[0] = span[26] = '{';
                    span[1] = '0';
                    span[2] = 'x';
                    span[3] = stringN[0];
                    span[4] = stringN[1];
                    span[5] = stringN[2];
                    span[6] = stringN[3];
                    span[7] = stringN[4];
                    span[8] = stringN[5];
                    span[9] = stringN[6];
                    span[10] = stringN[7];
                    span[11] = ',';
                    span[12] = '0';
                    span[13] = 'x';
                    span[14] = stringN[8];
                    span[15] = stringN[9];
                    span[16] = stringN[10];
                    span[17] = stringN[11];
                    span[18] = ',';
                    span[19] = '0';
                    span[20] = 'x';
                    span[21] = stringN[12];
                    span[22] = stringN[13];
                    span[23] = stringN[14];
                    span[24] = stringN[15];
                    span[25] = ',';
                    span[26] = '{';
                    span[27] = '0';
                    span[28] = 'x';
                    span[29] = stringN[16];
                    span[30] = stringN[17];
                    span[31] = ',';
                    span[32] = '0';
                    span[33] = 'x';
                    span[34] = stringN[18];
                    span[35] = stringN[19];
                    span[36] = ',';
                    span[37] = '0';
                    span[38] = 'x';
                    span[39] = stringN[20];
                    span[40] = stringN[21];
                    span[41] = ',';
                    span[42] = '0';
                    span[43] = 'x';
                    span[44] = stringN[22];
                    span[45] = stringN[23];
                    span[46] = ',';
                    span[47] = '0';
                    span[48] = 'x';
                    span[49] = stringN[24];
                    span[50] = stringN[25];
                    span[51] = ',';
                    span[52] = '0';
                    span[53] = 'x';
                    span[54] = stringN[26];
                    span[55] = stringN[27];
                    span[56] = ',';
                    span[57] = '0';
                    span[58] = 'x';
                    span[59] = stringN[28];
                    span[60] = stringN[29];
                    span[61] = ',';
                    span[62] = '0';
                    span[63] = 'x';
                    span[64] = stringN[30];
                    span[65] = stringN[31];
                    span[66] = '}';
                    span[67] = '}';
                });
            }

            return result;
        }

        public static string[] GenerateSometimesBrokenGuidsDStringsArray(int count)
        {
            // dddddddd-dddd-dddd-dddd-dddddddddddd
            var nStrings = GenerateSometimesBrokenGuidsNStringsArray(count);
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = string.Create(36, nStrings[i], (span, stringN) =>
                {
                    span[8] = span[13] = span[18] = span[23] = '-';
                    span[0] = stringN[0];
                    span[1] = stringN[1];
                    span[2] = stringN[2];
                    span[3] = stringN[3];
                    span[4] = stringN[4];
                    span[5] = stringN[5];
                    span[6] = stringN[6];
                    span[7] = stringN[7];

                    span[9] = stringN[8];
                    span[10] = stringN[9];
                    span[11] = stringN[10];
                    span[12] = stringN[11];

                    span[14] = stringN[12];
                    span[15] = stringN[13];
                    span[16] = stringN[14];
                    span[17] = stringN[15];

                    span[19] = stringN[16];
                    span[20] = stringN[17];
                    span[21] = stringN[18];
                    span[22] = stringN[19];

                    span[24] = stringN[20];
                    span[25] = stringN[21];
                    span[26] = stringN[22];
                    span[27] = stringN[23];
                    span[28] = stringN[24];
                    span[29] = stringN[25];
                    span[30] = stringN[26];
                    span[31] = stringN[27];
                    span[32] = stringN[28];
                    span[33] = stringN[29];
                    span[34] = stringN[30];
                    span[35] = stringN[31];
                });
            }

            return result;
        }

        public static string[] GenerateSometimesBrokenGuidsNStringsArray(int count)
        {
            var random = new Random();
            var uuidIntegers = stackalloc int[4];
            var charToBreakPtr = stackalloc char[1];
            var charBytesPtr = (byte*) charToBreakPtr;
            var result = new string[count];
            var breakUpperByteOnCharArray = new bool[32];
            for (int i = 0; i < breakUpperByteOnCharArray.Length; i++)
            {
                breakUpperByteOnCharArray[i] = false;
            }

            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < 4; j++)
                    uuidIntegers[j] = random.Next();

                var bytesOfUuid = new ReadOnlySpan<byte>(uuidIntegers, 16).ToArray();
                var nString = GetStringN(bytesOfUuid);
                var spanOfString = MemoryMarshal.CreateSpan(
                    ref MemoryMarshal.GetReference(nString.AsSpan()),
                    nString.Length);

                var brokenCharIndex = i % 32;
                if (brokenCharIndex != 0)
                {
                    var shouldBreakUpperByte = breakUpperByteOnCharArray[brokenCharIndex];
                    breakUpperByteOnCharArray[brokenCharIndex] = !shouldBreakUpperByte;
                    charToBreakPtr[0] = nString[brokenCharIndex];
                    if (shouldBreakUpperByte)
                    {
                        charBytesPtr[0] = 110;
                    }
                    else
                    {
                        charBytesPtr[1] = 110;
                    }

                    spanOfString[brokenCharIndex] = charToBreakPtr[0];
                }

                result[i] = nString;
            }

            return result;
        }

        private static string GetStringN(byte[] bytesOfUuid)
        {
            if (bytesOfUuid?.Length != 16)
                throw new ArgumentException("Array should contain 16 bytes", nameof(bytesOfUuid));
            return BitConverter
                .ToString(bytesOfUuid)
                .Replace("-", string.Empty)
                .ToLowerInvariant();
        }
    }
}