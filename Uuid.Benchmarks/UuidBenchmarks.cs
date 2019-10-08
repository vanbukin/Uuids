using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Uuid.Benchmarks
{
    [MemoryDiagnoser]
    public unsafe class UuidBenchmarks
    {
        private readonly object _emptyObject = new object();
        private Guid _differentGuid;
        private Uuid _differentUuid;
        private Guid _guid;
        private byte[] _guidBytes;
        private string _guidStringB;
        private string _guidStringD;
        private string _guidStringN;
        private string _guidStringP;
        private string _guidStringX;
        private string[] _brokenRandomUuidsN_1_000_000;
        private string[] _randomUuidsN_1_000_000;
        private string[] _randomUuidsN_10_000;
        private string[] _randomUuidsN_100;
        private string[] _randomUuidsN_100_000;
        private string[] _randomUuidsN_1000;
        private Guid _sameGuid;
        private Uuid _sameUuid;
        private Uuid _uuid;
        private byte[] _uuidBytes;
        private byte* _uuidBytesPtr;


        [GlobalSetup]
        public void Setup()
        {
            // 4cb41752c36e11e99cb52a2be2dbcce4
            _uuidBytes = new byte[]
            {
                76, 180, 23, 82, 195, 110, 17, 233, 156, 181, 42, 43, 226, 219, 204, 228
            };
            _guidBytes = new byte[]
            {
                82, 23, 180, 76, 110, 195, 233, 17, 156, 181, 42, 43, 226, 219, 204, 228
            };
            _guidStringN = "8ebd85638c94d04b6a9a72ace1cf398b";
            _guidStringD = "8ebd8563-8c94-d04b-6a9a-72ace1cf398b";
            _guidStringP = "(8ebd8563-8c94-d04b-6a9a-72ace1cf398b)";
            _guidStringB = "{8ebd8563-8c94-d04b-6a9a-72ace1cf398b}";
            _guidStringX = "{0x8ebd8563,0x8c94,0xd04b,{0x6a,0x9a,0x72,0xac,0xe1,0xcf,0x39,0x8b}}";
            _guid = new Guid("4cb41752c36e11e99cb52a2be2dbcce4");
            _sameGuid = new Guid("4cb41752c36e11e99cb52a2be2dbcce4");
            _differentGuid = new Guid("4cb41752c36e11e99cb52a2be2dbcce5");
            _uuid = new Uuid(_uuidBytes);
            _sameUuid = new Uuid(_uuidBytes);
            _differentUuid = new Uuid(new byte[]
            {
                76, 180, 23, 82, 195, 110, 17, 233, 156, 181, 42, 43, 226, 219, 204, 229
            });
            _uuidBytesPtr = (byte*) Marshal.AllocHGlobal(16);
            // ReSharper disable once PossibleNullReferenceException
            _uuidBytesPtr[0] = 76;
            _uuidBytesPtr[1] = 180;
            _uuidBytesPtr[2] = 23;
            _uuidBytesPtr[3] = 82;
            _uuidBytesPtr[4] = 195;
            _uuidBytesPtr[5] = 110;
            _uuidBytesPtr[6] = 17;
            _uuidBytesPtr[7] = 233;
            _uuidBytesPtr[8] = 156;
            _uuidBytesPtr[9] = 181;
            _uuidBytesPtr[10] = 42;
            _uuidBytesPtr[11] = 43;
            _uuidBytesPtr[12] = 226;
            _uuidBytesPtr[13] = 219;
            _uuidBytesPtr[14] = 204;
            _uuidBytesPtr[15] = 228;
            //_randomUuidsN_100 = GenerateRandomUuidNStringsArray(100);
            //_randomUuidsN_1000 = GenerateRandomUuidNStringsArray(1000);
            //_randomUuidsN_10_000 = GenerateRandomUuidNStringsArray(10_000);
            //_randomUuidsN_100_000 = GenerateRandomUuidNStringsArray(100_000);
            _brokenRandomUuidsN_1_000_000 = GenerateSometimesBrokenGuidsNStringsArray(1_000_000);
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
                for (var j = 0; j < 4; j++) uuidIntegers[j] = random.Next();

                var bytesOfUuid = new ReadOnlySpan<byte>(uuidIntegers, 16).ToArray();
                var nString = BitConverter
                    .ToString(bytesOfUuid)
                    .Replace("-", string.Empty)
                    .ToLowerInvariant();
                var spanOfString = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(nString.AsSpan()), nString.Length);


                var brokenChar = i % 32;
                if (brokenChar != 0)
                {
                    var breakUpperByte = breakUpperByteOnCharArray[brokenChar];
                    breakUpperByteOnCharArray[brokenChar] = !breakUpperByte;
                    charToBreakPtr[0] = nString[brokenChar];
                    if (breakUpperByte)
                    {
                        charBytesPtr[0] = 110;
                    }
                    else
                    {
                        charBytesPtr[1] = 110;
                    }

                    spanOfString[brokenChar] = charToBreakPtr[0];
                }

                result[i] = nString;
            }

            return result;
        }

        public static string[] GenerateRandomUuidNStringsArray(int count)
        {
            var random = new Random();
            var uuidIntegers = stackalloc int[4];
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < 4; j++) uuidIntegers[j] = random.Next();

                var bytesOfUuid = new ReadOnlySpan<byte>(uuidIntegers, 16).ToArray();
                var nString = BitConverter
                    .ToString(bytesOfUuid)
                    .Replace("-", string.Empty)
                    .ToLowerInvariant();
                result[i] = nString;
            }

            return result;
        }


//        [Benchmark]
//        public Uuid uuid_CtorPtr()
//        {
//            return new Uuid(_uuidBytesPtr);
//        }
//
//        [Benchmark]
//        public Uuid uuid_CtorByteArray()
//        {
//            return new Uuid(_uuidBytes);
//        }
//
//        [Benchmark]
//        public Guid guid_CtorByteArray()
//        {
//            return new Guid(_guidBytes);
//        }
//
//        [Benchmark]
//        public Uuid uuid_CtorSpan()
//        {
//            var span = new ReadOnlySpan<byte>(_uuidBytesPtr, 16);
//            return new Uuid(span);
//        }
//
//        [Benchmark]
//        public Guid guid_CtorSpan()
//        {
//            var span = new ReadOnlySpan<byte>(_uuidBytesPtr, 16);
//            return new Guid(span);
//        }
//
//        [Benchmark]
//        public string uuid_ToString()
//        {
//            return _uuid.ToString();
//        }
//
//        [Benchmark]
//        public string guid_ToString()
//        {
//            return _guid.ToString();
//        }
//
//        [Benchmark]
//        public string uuid_ToString_D()
//        {
//            return _uuid.ToString("D");
//        }
//
//        [Benchmark]
//        public string guid_ToString_D()
//        {
//            return _guid.ToString("D");
//        }
//
//        [Benchmark]
//        public string uuid_ToString_N()
//        {
//            return _uuid.ToString("N");
//        }
//
//        [Benchmark]
//        public string guid_ToString_N()
//        {
//            return _guid.ToString("N");
//        }
//
//        [Benchmark]
//        public string uuid_ToString_B()
//        {
//            return _uuid.ToString("B");
//        }
//
//        [Benchmark]
//        public string guid_ToString_B()
//        {
//            return _guid.ToString("B");
//        }
//
//        [Benchmark]
//        public string uuid_ToString_P()
//        {
//            return _uuid.ToString("P");
//        }
//
//        [Benchmark]
//        public string guid_ToString_P()
//        {
//            return _guid.ToString("P");
//        }
//
//        [Benchmark]
//        public string uuid_ToString_X()
//        {
//            return _uuid.ToString("X");
//        }
//
//        [Benchmark]
//        public string guid_ToString_X()
//        {
//            return _guid.ToString("X");
//        }
//
//        [Benchmark]
//        public int uuid_GetHashCode()
//        {
//            return _uuid.GetHashCode();
//        }
//
//        [Benchmark]
//        public int guid_GetHashCode()
//        {
//            return _guid.GetHashCode();
//        }
//
//        [Benchmark]
//        public byte[] uuid_ToByteArray()
//        {
//            return _uuid.ToByteArray();
//        }
//
//        [Benchmark]
//        public byte[] guid_ToByteArray()
//        {
//            return _guid.ToByteArray();
//        }
//
//        [Benchmark]
//        public bool uuid_EqualsWithNull()
//        {
//            return _uuid.Equals(null);
//        }
//
//        [Benchmark]
//        public bool guid_EqualsWithNull()
//        {
//            return _guid.Equals(null);
//        }
//
//        [Benchmark]
//        public bool uuid_EqualsWithSame()
//        {
//            return _uuid.Equals(_sameUuid);
//        }
//
//        [Benchmark]
//        public bool guid_EqualsWithSame()
//        {
//            return _guid.Equals(_sameGuid);
//        }
//
//        [Benchmark]
//        public bool uuid_EqualsWithNotSame()
//        {
//            return _uuid.Equals(_differentUuid);
//        }
//
//        [Benchmark]
//        public bool guid_EqualsWithNotSame()
//        {
//            return _guid.Equals(_differentGuid);
//        }
//
//        [Benchmark]
//        public bool uuid_EqualsWithAnotherType()
//        {
//            return _uuid.Equals(_emptyObject);
//        }
//
//        [Benchmark]
//        public bool guid_EqualsWithAnotherType()
//        {
//            return _guid.Equals(_emptyObject);
//        }
//
//        [Benchmark]
//        public int uuid_CompareToSame()
//        {
//            return _uuid.CompareTo(_sameUuid);
//        }
//
//        [Benchmark]
//        public int guid_CompareToSame()
//        {
//            return _guid.CompareTo(_sameGuid);
//        }
//
//        [Benchmark]
//        public int uuid_CompareToDifferent()
//        {
//            return _uuid.CompareTo(_differentUuid);
//        }
//
//        [Benchmark]
//        public int guid_CompareToDifferent()
//        {
//            return _guid.CompareTo(_differentGuid);
//        }
//
//        [Benchmark]
//        public int uuid_CompareToNull()
//        {
//            return _uuid.CompareTo(null);
//        }
//
//        [Benchmark]
//        public int guid_CompareToNull()
//        {
//            return _guid.CompareTo(null);
//        }
//
//        [Benchmark]
//        public bool uuid_TryWriteBytes()
//        {
//            Span<byte> buffer = stackalloc byte[16];
//            return _uuid.TryWriteBytes(buffer);
//        }
//
//        [Benchmark]
//        public bool guid_TryWriteBytes()
//        {
//            Span<byte> buffer = stackalloc byte[16];
//            return _guid.TryWriteBytes(buffer);
//        }
//
//        [Benchmark]
//        public Uuid uuid_CtorStringN()
//        {
//            return new Uuid(_guidStringN);
//        }
//
//
//        [Benchmark]
//        public Guid guid_CtorStringN()
//        {
//            return new Guid(_guidStringN);
//        }
//
//        [Benchmark]
//        public Uuid uuid_CtorStringD()
//        {
//            return new Uuid(_guidStringD);
//        }
//
//
//        [Benchmark]
//        public Guid guid_CtorStringD()
//        {
//            return new Guid(_guidStringD);
//        }
//
//        [Benchmark]
//        public Uuid uuid_CtorStringP()
//        {
//            return new Uuid(_guidStringP);
//        }
//
//
//        [Benchmark]
//        public Guid guid_CtorStringP()
//        {
//            return new Guid(_guidStringP);
//        }
//
//        [Benchmark]
//        public Uuid uuid_CtorStringB()
//        {
//            return new Uuid(_guidStringB);
//        }
//
//
//        [Benchmark]
//        public Guid guid_CtorStringB()
//        {
//            return new Guid(_guidStringB);
//        }
//
//        [Benchmark]
//        public Uuid uuid_CtorStringX()
//        {
//            return new Uuid(_guidStringX);
//        }
//
//
//        [Benchmark]
//        public Guid guid_CtorStringX()
//        {
//            return new Guid(_guidStringX);
//        }
//
//        private const byte OLD_FLAG = 0;
//        private const sbyte NEW_FLAG = 0;
//
//        [Benchmark]
//        public void uuid_TryParseN_1kk_Old()
//        {
//            for (var i = 0; i < _brokenRandomUuidsN_1_000_000.Length; i++)
//            {
//                var _ = Uuid.TryParse(_brokenRandomUuidsN_1_000_000[i], out var _);
//            }
//        }
//
//        [Benchmark]
//        public void uuid_TryParseN_1kk_New()
//        {
//            for (var i = 0; i < _brokenRandomUuidsN_1_000_000.Length; i++)
//            {
//                var _ = Uuid.TryParseNew(_brokenRandomUuidsN_1_000_000[i], out var _);
//            }
//        }


        [Benchmark]
        public string uuid_ToString_N()
        {
            return _uuid.ToString("N");
        }
        
        [Benchmark]
        public string uuid_ToString_N_AVX()
        {
            return _uuid.ToString("M");
        }
    }
}