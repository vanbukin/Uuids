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
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class OverridesBenchmarks
    {
        static OverridesBenchmarks()
        {
            var guidString = Guid.NewGuid().ToString("N");
            _guid = new Guid(guidString);
            _guidSame = new Guid(guidString);
            _uuid = new Uuid(guidString);
            _uuidSame = new Uuid(guidString);
        }

        private static readonly Guid _guid;
        private static readonly Guid _guidSame;
        private static readonly Uuid _uuid;
        private static readonly Uuid _uuidSame;

        public IEnumerable<Guid> GuidArgs()
        {
            yield return _guid;
        }

        public IEnumerable<Uuid> UuidArgs()
        {
            yield return _uuid;
        }

        public IEnumerable<object[]> GuidSameValues()
        {
            yield return new object[] {_guid, _guidSame};
        }

        public IEnumerable<object[]> UuidSameValues()
        {
            yield return new object[] {_uuid, _uuidSame};
        }

        public IEnumerable<object[]> GuidDifferentTypesValues()
        {
            yield return new[] {_guid, new object()};
        }

        public IEnumerable<object[]> UuidDifferentTypesValues()
        {
            yield return new[] {_uuid, new object()};
        }

        // GetHashCode
        [Benchmark]
        [BenchmarkCategory("GetHashCode")]
        [ArgumentsSource(nameof(UuidArgs))]
        public int uuid_GetHashCode(Uuid uuid)
        {
            return uuid.GetHashCode();
        }

        [Benchmark]
        [BenchmarkCategory("GetHashCode")]
        [ArgumentsSource(nameof(GuidArgs))]
        public int guid_GetHashCode(Guid guid)
        {
            return guid.GetHashCode();
        }

#nullable disable
        // Equals with null
        [Benchmark]
        [BenchmarkCategory("Equals_null")]
        [ArgumentsSource(nameof(UuidArgs))]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public bool uuid_EqualsWithNull(Uuid uuid)
        {
            return uuid.Equals((object) null);
        }

        [Benchmark]
        [BenchmarkCategory("Equals_null")]
        [ArgumentsSource(nameof(GuidArgs))]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public bool guid_EqualsWithNull(Guid guid)
        {
            return guid.Equals((object) null);
        }
#nullable restore

        // Equals with same value object
        [Benchmark]
        [BenchmarkCategory("Equals_same_value_object")]
        [ArgumentsSource(nameof(UuidSameValues))]
        public bool uuid_EqualsWithSameValueObject(Uuid uuid, object sameValue)
        {
            return uuid.Equals(sameValue);
        }

        [Benchmark]
        [BenchmarkCategory("Equals_same_value_object")]
        [ArgumentsSource(nameof(GuidSameValues))]
        public bool guid_EqualsWithSameValueObject(Guid guid, object sameValue)
        {
            return guid.Equals(sameValue);
        }

        // Equals with other type
        [Benchmark]
        [BenchmarkCategory("Equals_different_types_values")]
        [ArgumentsSource(nameof(UuidDifferentTypesValues))]
        public bool uuid_EqualsDifferentTypesValues(Uuid uuid, object differentTypeValue)
        {
            return uuid.Equals(differentTypeValue);
        }

        [Benchmark]
        [BenchmarkCategory("Equals_different_types_values")]
        [ArgumentsSource(nameof(GuidDifferentTypesValues))]
        public bool guid_EqualsDifferentTypesValues(Guid guid, object differentTypeValue)
        {
            return guid.Equals(differentTypeValue);
        }
    }
}