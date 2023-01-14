using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Uuids.Benchmarks.Benchmarks;

[GcServer(true)]
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class OverridesBenchmarks
{
    private static readonly Guid _guidValue;
    private static readonly Guid _guidValueSame;
    private static readonly Uuid _uuidValue;
    private static readonly Uuid _uuidValueSame;

    static OverridesBenchmarks()
    {
        string guidString = Guid.NewGuid().ToString("N");
        _guidValue = new(guidString);
        _guidValueSame = new(guidString);
        _uuidValue = new(guidString);
        _uuidValueSame = new(guidString);
    }

    public IEnumerable<Guid> GuidArgs()
    {
        yield return _guidValue;
    }

    public IEnumerable<Uuid> UuidArgs()
    {
        yield return _uuidValue;
    }

    public IEnumerable<object[]> GuidSameValues()
    {
        yield return new object[]
        {
            _guidValue,
            _guidValueSame
        };
    }

    public IEnumerable<object[]> UuidSameValues()
    {
        yield return new object[]
        {
            _uuidValue,
            _uuidValueSame
        };
    }

    public IEnumerable<object[]> GuidDifferentTypesValues()
    {
        yield return new[]
        {
            _guidValue,
            new object()
        };
    }

    public IEnumerable<object[]> UuidDifferentTypesValues()
    {
        yield return new[]
        {
            _uuidValue,
            new object()
        };
    }

    // GetHashCode
    [Benchmark]
    [BenchmarkCategory("GetHashCode")]
    [ArgumentsSource(nameof(UuidArgs))]
    public int uuid_GetHashCode(Uuid uuidValue)
    {
        return uuidValue.GetHashCode();
    }

    [Benchmark]
    [BenchmarkCategory("GetHashCode")]
    [ArgumentsSource(nameof(GuidArgs))]
    public int guid_GetHashCode(Guid guidValue)
    {
        return guidValue.GetHashCode();
    }

    // Equals with same value object
    [Benchmark]
    [BenchmarkCategory("Equals_same_value_object")]
    [ArgumentsSource(nameof(UuidSameValues))]
    public bool uuid_EqualsWithSameValueObject(Uuid uuidValue, object sameValue)
    {
        return uuidValue.Equals(sameValue);
    }

    [Benchmark]
    [BenchmarkCategory("Equals_same_value_object")]
    [ArgumentsSource(nameof(GuidSameValues))]
    public bool guid_EqualsWithSameValueObject(Guid guidValue, object sameValue)
    {
        return guidValue.Equals(sameValue);
    }

    // Equals with other type
    [Benchmark]
    [BenchmarkCategory("Equals_different_types_values")]
    [ArgumentsSource(nameof(UuidDifferentTypesValues))]
    public bool uuid_EqualsDifferentTypesValues(Uuid uuidValue, object differentTypeValue)
    {
        return uuidValue.Equals(differentTypeValue);
    }

    [Benchmark]
    [BenchmarkCategory("Equals_different_types_values")]
    [ArgumentsSource(nameof(GuidDifferentTypesValues))]
    public bool guid_EqualsDifferentTypesValues(Guid guidValue, object differentTypeValue)
    {
        return guidValue.Equals(differentTypeValue);
    }

    // Equals with null
    [Benchmark]
    [BenchmarkCategory("Equals_null")]
    [ArgumentsSource(nameof(UuidArgs))]
    [SuppressMessage("ReSharper", "RedundantCast")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public bool uuid_EqualsWithNull(Uuid uuidValue)
    {
        return uuidValue.Equals((object?) null);
    }

    [Benchmark]
    [BenchmarkCategory("Equals_null")]
    [ArgumentsSource(nameof(GuidArgs))]
    [SuppressMessage("ReSharper", "RedundantCast")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public bool guid_EqualsWithNull(Guid guidValue)
    {
        return guidValue.Equals((object?) null);
    }
}
