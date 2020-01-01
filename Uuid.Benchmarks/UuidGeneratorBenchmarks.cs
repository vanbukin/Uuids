using System;
using BenchmarkDotNet.Attributes;

namespace Uuid.Benchmarks
{
    public class UuidGeneratorBenchmarks
    {
        [GlobalSetup]
        public void Setup()
        {
            for (var i = 0; i < 1_000_000; i++)
            {
                var _ = Guid.NewGuid();
            }

            for (var i = 0; i < 1_000_000; i++)
            {
                var _ = Uuid.NewUuid();
            }
        }

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