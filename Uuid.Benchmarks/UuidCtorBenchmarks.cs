using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Uuid.Benchmarks
{
    public unsafe class UuidCtorBenchmarks
    {
        private static readonly byte* UuidsBytesPtr = SetupUuidsBytesPtr();
        private static readonly byte[][] UuidsBytes = SetupUuidsBytes(UuidsBytesPtr);
        private static readonly string[] UuidsNStrings = Utils.GenerateRandomUuidNStringsArray(Count);

        private const int Count = 1_000_000;

        [GlobalSetup]
        public void Setup()
        {
        }

        private static byte* SetupUuidsBytesPtr()
        {
            var resultPtr = (byte*) Marshal.AllocHGlobal(sizeof(Guid) * Count);
            if (resultPtr == null)
                throw new Exception($"{nameof(UuidsBytesPtr)} could not be null!");
            var guidPtr = stackalloc Guid[1];
            for (long i = 0; i < Count; i++)
            {
                guidPtr[0] = Guid.NewGuid();
                var guidBytesPtr = (byte*) guidPtr;
                for (long j = 0; j < sizeof(Guid); j++)
                {
                    var byteIndex = i * sizeof(Guid) + j;
                    resultPtr[byteIndex] = guidBytesPtr[j];
                }
            }

            return resultPtr;
        }

        private static byte[][] SetupUuidsBytes(byte* bytesPtr)
        {
            var result = new byte[Count][];
            for (var i = 0; i < Count; i++)
            {
                result[i] = new byte[sizeof(Guid)];
                for (long j = 0; j < sizeof(Guid); j++)
                {
                    result[i][j] = bytesPtr[i * sizeof(Guid) + j];
                }
            }

            return result;
        }

        [Benchmark]
        public void uuid_CtorPtr()
        {
            for (var i = 0; i < Count; i++)
            {
                var ptr = UuidsBytesPtr + i * sizeof(Uuid);
                var _ = new Uuid(ptr);
            }
        }

        [Benchmark]
        public void uuid_CtorByteArray()
        {
            for (var i = 0; i < Count; i++)
            {
                var _ = new Uuid(UuidsBytes[i]);
            }
        }

        [Benchmark]
        public void guid_CtorByteArray()
        {
            for (var i = 0; i < Count; i++)
            {
                var _ = new Guid(UuidsBytes[i]);
            }
        }

        [Benchmark]
        public void uuid_CtorSpan()
        {
            for (var i = 0; i < Count; i++)
            {
                var ptr = UuidsBytesPtr + i * sizeof(Uuid);
                var readOnlySpan = new ReadOnlySpan<byte>(ptr, sizeof(Uuid));
                var _ = new Uuid(readOnlySpan);
            }
        }

        [Benchmark]
        public void guid_CtorSpan()
        {
            for (var i = 0; i < Count; i++)
            {
                var ptr = UuidsBytesPtr + i * sizeof(Guid);
                var readOnlySpan = new ReadOnlySpan<byte>(ptr, sizeof(Guid));
                var _ = new Guid(readOnlySpan);
            }
        }

        [Benchmark]
        public void uuid_CtorStringN()
        {
            for (var i = 0; i < Count; i++)
            {
                var _ = new Uuid(UuidsNStrings[i]);
            }
        }

        [Benchmark]
        public void guid_CtorStringN()
        {
            for (var i = 0; i < Count; i++)
            {
                var _ = new Guid(UuidsNStrings[i]);
            }
        }
    }
}