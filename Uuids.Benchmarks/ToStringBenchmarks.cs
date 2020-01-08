using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Uuids.Benchmarks
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ToStringBenchmarks
    {
        static ToStringBenchmarks()
        {
            var guidString = Guid.NewGuid().ToString("N");
            _guid = new Guid(guidString);
            _uuid = new Uuid(guidString);
        }

        private static readonly Guid _guid;
        private static readonly Uuid _uuid;

        public IEnumerable<Guid> GuidArgs()
        {
            yield return _guid;
        }

        public IEnumerable<Uuid> UuidArgs()
        {
            yield return _uuid;
        }

        // ToString("N");
        [Benchmark]
        [BenchmarkCategory("ToString_N")]
        [ArgumentsSource(nameof(GuidArgs))]
        public string guid_ToString_N(Guid guid)
        {
            return guid.ToString("N");
        }

        [Benchmark]
        [BenchmarkCategory("ToString_N")]
        [ArgumentsSource(nameof(UuidArgs))]
        public string uuid_ToString_N(Uuid uuid)
        {
            return uuid.ToString("N");
        }

        //ToString("D");
        [Benchmark]
        [BenchmarkCategory("ToString_D")]
        [ArgumentsSource(nameof(GuidArgs))]
        public string guid_ToString_D(Guid guid)
        {
            return guid.ToString("D");
        }

        [Benchmark]
        [BenchmarkCategory("ToString_D")]
        [ArgumentsSource(nameof(UuidArgs))]
        public string uuid_ToString_D(Uuid uuid)
        {
            return uuid.ToString("D");
        }

        //ToString("B");
        [Benchmark]
        [BenchmarkCategory("ToString_B")]
        [ArgumentsSource(nameof(GuidArgs))]
        public string guid_ToString_B(Guid guid)
        {
            return guid.ToString("B");
        }

        [Benchmark]
        [BenchmarkCategory("ToString_B")]
        [ArgumentsSource(nameof(UuidArgs))]
        public string uuid_ToString_B(Uuid uuid)
        {
            return uuid.ToString("B");
        }

        //ToString("P");
        [Benchmark]
        [BenchmarkCategory("ToString_P")]
        [ArgumentsSource(nameof(GuidArgs))]
        public string guid_ToString_P(Guid guid)
        {
            return guid.ToString("P");
        }

        [Benchmark]
        [BenchmarkCategory("ToString_P")]
        [ArgumentsSource(nameof(UuidArgs))]
        public string uuid_ToString_P(Uuid uuid)
        {
            return uuid.ToString("P");
        }

        //ToString("X");
        [Benchmark]
        [BenchmarkCategory("ToString_X")]
        [ArgumentsSource(nameof(GuidArgs))]
        public string guid_ToString_X(Guid guid)
        {
            return guid.ToString("X");
        }

        [Benchmark]
        [BenchmarkCategory("ToString_X")]
        [ArgumentsSource(nameof(UuidArgs))]
        public string uuid_ToString_X(Uuid uuid)
        {
            return uuid.ToString("X");
        }
    }
}