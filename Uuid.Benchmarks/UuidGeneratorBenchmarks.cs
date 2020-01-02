using System;
using BenchmarkDotNet.Attributes;

namespace Uuid.Benchmarks
{
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
            var _ = Uuid.NewUuid();
        }
    }
}