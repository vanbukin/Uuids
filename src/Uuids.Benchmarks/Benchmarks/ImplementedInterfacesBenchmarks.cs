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
public class ImplementedInterfacesBenchmarks
{
    private static readonly Guid _guidValue;
    private static readonly Guid _guidValueSame;
    private static readonly Guid _guidValueDifferent;
    private static readonly Uuid _uuidValue;
    private static readonly Uuid _uuidValueSame;
    private static readonly Uuid _uuidValueDifferent;

    static ImplementedInterfacesBenchmarks()
    {
        string guidString = Guid.NewGuid().ToString("N");
        _guidValue = new(guidString);
        _guidValueSame = new(guidString);
        _uuidValue = new(guidString);
        _uuidValueSame = new(guidString);

        string differentGuidString = Guid.NewGuid().ToString("N");
        _guidValueDifferent = new(differentGuidString);
        _uuidValueDifferent = new(differentGuidString);
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

    public IEnumerable<object[]> GuidDifferentValues()
    {
        yield return new object[]
        {
            _guidValue,
            _guidValueDifferent
        };
    }

    public IEnumerable<object[]> UuidDifferentValues()
    {
        yield return new object[]
        {
            _uuidValue,
            _uuidValueDifferent
        };
    }

    // IEquatable<T>.Equals with same value
    [Benchmark]
    [BenchmarkCategory("IEquatable_T_Equals_same")]
    [ArgumentsSource(nameof(GuidSameValues))]
    public bool guid_EqualsSame(Guid guidValue, Guid sameValue)
    {
        return guidValue.Equals(sameValue);
    }

    [Benchmark]
    [BenchmarkCategory("IEquatable_T_Equals_same")]
    [ArgumentsSource(nameof(UuidSameValues))]
    public bool uuid_EqualsSame(Uuid uuidValue, Uuid sameValue)
    {
        return uuidValue.Equals(sameValue);
    }

    // IEquatable<T>.Equals with different value
    [Benchmark]
    [BenchmarkCategory("IEquatable_T_Equals_different")]
    [ArgumentsSource(nameof(GuidDifferentValues))]
    public bool guid_EqualsDifferent(Guid guidValue, Guid differentValue)
    {
        return guidValue.Equals(differentValue);
    }

    [Benchmark]
    [BenchmarkCategory("IEquatable_T_Equals_different")]
    [ArgumentsSource(nameof(UuidDifferentValues))]
    public bool uuid_EqualsDifferent(Uuid uuidValue, Uuid differentValue)
    {
        return uuidValue.Equals(differentValue);
    }

    // IComparable.CompareTo with same value
    [Benchmark]
    [BenchmarkCategory("IComparable_CompareTo_same")]
    [ArgumentsSource(nameof(UuidSameValues))]
    public int uuid_CompareToSameValueObject(Uuid uuidValue, object sameValue)
    {
        return uuidValue.CompareTo(sameValue);
    }

    [Benchmark]
    [BenchmarkCategory("IComparable_CompareTo_same")]
    [ArgumentsSource(nameof(GuidSameValues))]
    public int guid_CompareToSameValueObject(Guid guidValue, object sameValue)
    {
        return guidValue.CompareTo(sameValue);
    }

    // IComparable.CompareTo with different value
    [Benchmark]
    [BenchmarkCategory("IComparable_CompareTo_different")]
    [ArgumentsSource(nameof(UuidDifferentValues))]
    public int uuid_CompareToDifferentValueObject(Uuid uuidValue, object differentValue)
    {
        return uuidValue.CompareTo(differentValue);
    }

    [Benchmark]
    [BenchmarkCategory("IComparable_CompareTo_different")]
    [ArgumentsSource(nameof(GuidDifferentValues))]
    public int guid_CompareToDifferentValueObject(Guid guidValue, object differentValue)
    {
        return guidValue.CompareTo(differentValue);
    }

    // IComparable<T>.CompareTo with same value
    [Benchmark]
    [BenchmarkCategory("IComparable_T_CompareTo_same")]
    [ArgumentsSource(nameof(UuidSameValues))]
    public int uuid_CompareTo_T_SameValue(Uuid uuidValue, Uuid sameValue)
    {
        return uuidValue.CompareTo(sameValue);
    }

    [Benchmark]
    [BenchmarkCategory("IComparable_T_CompareTo_same")]
    [ArgumentsSource(nameof(GuidSameValues))]
    public int guid_CompareTo_T_SameValue(Guid guidValue, Guid sameValue)
    {
        return guidValue.CompareTo(sameValue);
    }

    // IComparable<T>.CompareTo with different value
    [Benchmark]
    [BenchmarkCategory("IComparable _T_CompareTo_different")]
    [ArgumentsSource(nameof(UuidDifferentValues))]
    public int uuid_CompareTo_T_DifferentValue(Uuid uuidValue, Uuid differentValue)
    {
        return uuidValue.CompareTo(differentValue);
    }

    [Benchmark]
    [BenchmarkCategory("IComparable _T_CompareTo_different")]
    [ArgumentsSource(nameof(GuidDifferentValues))]
    public int guid_CompareTo_T_DifferentValue(Guid guidValue, Guid differentValue)
    {
        return guidValue.CompareTo(differentValue);
    }
#nullable disable
    // IComparable.CompareTo with null
    [Benchmark]
    [BenchmarkCategory("IComparable_CompareTo_null")]
    [ArgumentsSource(nameof(UuidArgs))]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public int uuid_CompareToNull(Uuid uuidValue)
    {
        return uuidValue.CompareTo((object) null);
    }

    [Benchmark]
    [BenchmarkCategory("IComparable_CompareTo_null")]
    [ArgumentsSource(nameof(GuidArgs))]
    [SuppressMessage("ReSharper", "RedundantCast")]
    public int guid_CompareToNull(Guid guidValue)
    {
        return guidValue.CompareTo((object) null);
    }
#nullable restore
}
