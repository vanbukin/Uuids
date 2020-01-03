using System;
using BenchmarkDotNet.Attributes;

namespace Uuids.Benchmarks
{
    [MemoryDiagnoser]
    public class UuidGeneratorBenchmarks
    {
        [Benchmark]
        public void guid_NewGuid()
        {
            var _ = Guid.NewGuid();
        }

        [Benchmark]
        public void uuid_NewUuid()
        {
            var _ = Uuid.NewTimeBased();
        }
    }
}