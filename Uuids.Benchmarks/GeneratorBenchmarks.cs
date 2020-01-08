using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Uuids.Benchmarks
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GeneratorBenchmarks
    {
        [Benchmark]
        public Guid guid_New()
        {
            return Guid.NewGuid();
        }

        [Benchmark]
        public Uuid uuid_New()
        {
            return Uuid.NewTimeBased();
        }
    }
}