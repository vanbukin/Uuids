using System;
using BenchmarkDotNet.Attributes;

namespace Uuids.Benchmarks
{
    public unsafe class UuidCommonBenchmarks
    {
        private readonly object _emptyObject = new object();
        private Uuid _uuid;
        private Uuid _sameUuid;
        private Uuid _differentUuid;
        private Guid _guid;
        private Guid _sameGuid;
        private Guid _differentGuid;

        [GlobalSetup]
        public void Setup()
        {
            _guid = Guid.NewGuid();
            var uuidBytesPtr = stackalloc byte[sizeof(Uuid)];
            *(Guid*) uuidBytesPtr = _guid;
            _uuid = new Uuid(uuidBytesPtr);

            _sameGuid = new Guid(_guid.ToByteArray());
            _sameUuid = new Uuid(_uuid.ToByteArray());

            var differentGuidBytes = _sameGuid.ToByteArray();
            unchecked
            {
                differentGuidBytes[0]++;
            }

            _differentGuid = new Guid(differentGuidBytes);
            var differentUuidBytesPtr = stackalloc byte[sizeof(Uuid)];
            *(Guid*) differentUuidBytesPtr = _differentGuid;
            _differentUuid = new Uuid(differentUuidBytesPtr);
        }

        [Benchmark]
        public int uuid_GetHashCode()
        {
            return _uuid.GetHashCode();
        }

        [Benchmark]
        public int guid_GetHashCode()
        {
            return _guid.GetHashCode();
        }

        [Benchmark]
        public byte[] uuid_ToByteArray()
        {
            return _uuid.ToByteArray();
        }

        [Benchmark]
        public byte[] guid_ToByteArray()
        {
            return _guid.ToByteArray();
        }

        [Benchmark]
        public bool uuid_EqualsWithNull()
        {
            return _uuid.Equals(null);
        }

        [Benchmark]
        public bool guid_EqualsWithNull()
        {
            return _guid.Equals(null);
        }

        [Benchmark]
        public bool uuid_EqualsWithSame()
        {
            return _uuid.Equals(_sameUuid);
        }

        [Benchmark]
        public bool guid_EqualsWithSame()
        {
            return _guid.Equals(_sameGuid);
        }

        [Benchmark]
        public bool uuid_EqualsWithNotSame()
        {
            return _uuid.Equals(_differentUuid);
        }

        [Benchmark]
        public bool guid_EqualsWithNotSame()
        {
            return _guid.Equals(_differentGuid);
        }

        [Benchmark]
        public bool uuid_EqualsWithAnotherType()
        {
            return _uuid.Equals(_emptyObject);
        }

        [Benchmark]
        public bool guid_EqualsWithAnotherType()
        {
            return _guid.Equals(_emptyObject);
        }

        [Benchmark]
        public int uuid_CompareToSame()
        {
            return _uuid.CompareTo(_sameUuid);
        }

        [Benchmark]
        public int guid_CompareToSame()
        {
            return _guid.CompareTo(_sameGuid);
        }

        [Benchmark]
        public int uuid_CompareToDifferent()
        {
            return _uuid.CompareTo(_differentUuid);
        }

        [Benchmark]
        public int guid_CompareToDifferent()
        {
            return _guid.CompareTo(_differentGuid);
        }

        [Benchmark]
        public int uuid_CompareToNull()
        {
            return _uuid.CompareTo(null);
        }

        [Benchmark]
        public int guid_CompareToNull()
        {
            return _guid.CompareTo(null);
        }

        [Benchmark]
        public bool uuid_TryWriteBytes()
        {
            Span<byte> buffer = stackalloc byte[16];
            return _uuid.TryWriteBytes(buffer);
        }

        [Benchmark]
        public bool guid_TryWriteBytes()
        {
            Span<byte> buffer = stackalloc byte[16];
            return _guid.TryWriteBytes(buffer);
        }
    }
}