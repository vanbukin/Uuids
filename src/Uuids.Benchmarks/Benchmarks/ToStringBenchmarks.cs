using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Uuids.Benchmarks.Benchmarks;

[GcServer(true)]
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ToStringBenchmarks
{
    private static readonly Guid _guidValue;
    private static readonly Uuid _uuidValue;

    static ToStringBenchmarks()
    {
        string guidString = Guid.NewGuid().ToString("N");
        _guidValue = new(guidString);
        _uuidValue = new(guidString);
    }

    public IEnumerable<Guid> GuidArgs()
    {
        yield return _guidValue;
    }

    public IEnumerable<Uuid> UuidArgs()
    {
        yield return _uuidValue;
    }

    // ToString("N");
    [Benchmark]
    [BenchmarkCategory("ToString_N")]
    [ArgumentsSource(nameof(GuidArgs))]
    public string guid_ToString_N(Guid guidValue)
    {
        return guidValue.ToString("N");
    }

    [Benchmark]
    [BenchmarkCategory("ToString_N")]
    [ArgumentsSource(nameof(UuidArgs))]
    public string uuid_ToString_N(Uuid uuidValue)
    {
        return uuidValue.ToString("N");
    }

    //ToString("D");
    [Benchmark]
    [BenchmarkCategory("ToString_D")]
    [ArgumentsSource(nameof(GuidArgs))]
    public string guid_ToString_D(Guid guidValue)
    {
        return guidValue.ToString("D");
    }

    [Benchmark]
    [BenchmarkCategory("ToString_D")]
    [ArgumentsSource(nameof(UuidArgs))]
    public string uuid_ToString_D(Uuid uuidValue)
    {
        return uuidValue.ToString("D");
    }

    //ToString("B");
    [Benchmark]
    [BenchmarkCategory("ToString_B")]
    [ArgumentsSource(nameof(GuidArgs))]
    public string guid_ToString_B(Guid guidValue)
    {
        return guidValue.ToString("B");
    }

    [Benchmark]
    [BenchmarkCategory("ToString_B")]
    [ArgumentsSource(nameof(UuidArgs))]
    public string uuid_ToString_B(Uuid uuidValue)
    {
        return uuidValue.ToString("B");
    }

    //ToString("P");
    [Benchmark]
    [BenchmarkCategory("ToString_P")]
    [ArgumentsSource(nameof(GuidArgs))]
    public string guid_ToString_P(Guid guidValue)
    {
        return guidValue.ToString("P");
    }

    [Benchmark]
    [BenchmarkCategory("ToString_P")]
    [ArgumentsSource(nameof(UuidArgs))]
    public string uuid_ToString_P(Uuid uuidValue)
    {
        return uuidValue.ToString("P");
    }

    //ToString("X");
    [Benchmark]
    [BenchmarkCategory("ToString_X")]
    [ArgumentsSource(nameof(GuidArgs))]
    public string guid_ToString_X(Guid guidValue)
    {
        return guidValue.ToString("X");
    }

    [Benchmark]
    [BenchmarkCategory("ToString_X")]
    [ArgumentsSource(nameof(UuidArgs))]
    public string uuid_ToString_X(Uuid uuidValue)
    {
        return uuidValue.ToString("X");
    }
}
