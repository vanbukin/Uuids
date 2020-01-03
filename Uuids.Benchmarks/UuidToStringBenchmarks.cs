using System;
using BenchmarkDotNet.Attributes;

namespace Uuids.Benchmarks
{
    [MemoryDiagnoser]
    public unsafe class UuidToStringBenchmarks
    {
        private Uuid _uuid;
        private Guid _guid;

        [GlobalSetup]
        public void Setup()
        {
            _guid = Guid.NewGuid();
            var uuidBytesPtr = stackalloc byte[sizeof(Uuid)];
            *(Guid*) uuidBytesPtr = _guid;
            _uuid = new Uuid(uuidBytesPtr);
        }


        [Benchmark]
        public string uuid_ToString()
        {
            return _uuid.ToString();
        }

        [Benchmark]
        public string guid_ToString()
        {
            return _guid.ToString();
        }

        [Benchmark]
        public string uuid_ToString_D()
        {
            return _uuid.ToString("D");
        }

        [Benchmark]
        public string guid_ToString_D()
        {
            return _guid.ToString("D");
        }

        [Benchmark]
        public string uuid_ToString_N()
        {
            return _uuid.ToString("N");
        }

        [Benchmark]
        public string guid_ToString_N()
        {
            return _guid.ToString("N");
        }

        [Benchmark]
        public string uuid_ToString_B()
        {
            return _uuid.ToString("B");
        }

        [Benchmark]
        public string guid_ToString_B()
        {
            return _guid.ToString("B");
        }

        [Benchmark]
        public string uuid_ToString_P()
        {
            return _uuid.ToString("P");
        }

        [Benchmark]
        public string guid_ToString_P()
        {
            return _guid.ToString("P");
        }

        [Benchmark]
        public string uuid_ToString_X()
        {
            return _uuid.ToString("X");
        }

        [Benchmark]
        public string guid_ToString_X()
        {
            return _guid.ToString("X");
        }
    }
}