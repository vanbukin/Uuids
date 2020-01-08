using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Uuids.Benchmarks
{
    [GcServer(true)]
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ImplementedInterfacesBenchmarks
    {
        static ImplementedInterfacesBenchmarks()
        {
            var guidString = Guid.NewGuid().ToString("N");
            _guid = new Guid(guidString);
            _guidSame = new Guid(guidString);
            _uuid = new Uuid(guidString);
            _uuidSame = new Uuid(guidString);

            var differentGuidString = Guid.NewGuid().ToString("N");
            _guidDifferent = new Guid(differentGuidString);
            _uuidDifferent = new Uuid(differentGuidString);
        }
        
        private static readonly Guid _guid;
        private static readonly Guid _guidSame;
        private static readonly Guid _guidDifferent;
        private static readonly Uuid _uuid;
        private static readonly Uuid _uuidSame;
        private static readonly Uuid _uuidDifferent;

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

        public IEnumerable<object[]> GuidDifferentValues()
        {
            yield return new object[] {_guid, _guidDifferent};
        }

        public IEnumerable<object[]> UuidDifferentValues()
        {
            yield return new object[] {_uuid, _uuidDifferent};
        }

        // IEquatable<T>.Equals with same value
        [Benchmark]
        [BenchmarkCategory("IEquatable_T_Equals_same")]
        [ArgumentsSource(nameof(GuidSameValues))]
        public bool guid_EqualsSame(Guid guid, Guid sameValue)
        {
            return guid.Equals(sameValue);
        }

        [Benchmark]
        [BenchmarkCategory("IEquatable_T_Equals_same")]
        [ArgumentsSource(nameof(UuidSameValues))]
        public bool uuid_EqualsSame(Uuid uuid, Uuid sameValue)
        {
            return uuid.Equals(sameValue);
        }

        // IEquatable<T>.Equals with different value
        [Benchmark]
        [BenchmarkCategory("IEquatable_T_Equals_different")]
        [ArgumentsSource(nameof(GuidDifferentValues))]
        public bool guid_EqualsDifferent(Guid guid, Guid differentValue)
        {
            return guid.Equals(differentValue);
        }

        [Benchmark]
        [BenchmarkCategory("IEquatable_T_Equals_different")]
        [ArgumentsSource(nameof(UuidDifferentValues))]
        public bool uuid_EqualsDifferent(Uuid uuid, Uuid differentValue)
        {
            return uuid.Equals(differentValue);
        }

        // IComparable.CompareTo with same value
        [Benchmark]
        [BenchmarkCategory("IComparable_CompareTo_same")]
        [ArgumentsSource(nameof(UuidSameValues))]
        public int uuid_CompareToSameValueObject(Uuid uuid, object sameValue)
        {
            return uuid.CompareTo(sameValue);
        }

        [Benchmark]
        [BenchmarkCategory("IComparable_CompareTo_same")]
        [ArgumentsSource(nameof(GuidSameValues))]
        public int guid_CompareToSameValueObject(Guid guid, object sameValue)
        {
            return guid.CompareTo(sameValue);
        }
#nullable disable
        // IComparable.CompareTo with null
        [Benchmark]
        [BenchmarkCategory("IComparable_CompareTo_null")]
        [ArgumentsSource(nameof(UuidArgs))]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public int uuid_CompareToNull(Uuid uuid)
        {
            return uuid.CompareTo((object) null);
        }

        [Benchmark]
        [BenchmarkCategory("IComparable_CompareTo_null")]
        [ArgumentsSource(nameof(GuidArgs))]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public int guid_CompareToNull(Guid guid)
        {
            return guid.CompareTo((object) null);
        }
#nullable restore
        // IComparable.CompareTo with different value
        [Benchmark]
        [BenchmarkCategory("IComparable_CompareTo_different")]
        [ArgumentsSource(nameof(UuidDifferentValues))]
        public int uuid_CompareToDifferentValueObject(Uuid uuid, object differentValue)
        {
            return uuid.CompareTo(differentValue);
        }

        [Benchmark]
        [BenchmarkCategory("IComparable_CompareTo_different")]
        [ArgumentsSource(nameof(GuidDifferentValues))]
        public int guid_CompareToDifferentValueObject(Guid guid, object differentValue)
        {
            return guid.CompareTo(differentValue);
        }

        // IComparable<T>.CompareTo with same value
        [Benchmark]
        [BenchmarkCategory("IComparable_T_CompareTo_same")]
        [ArgumentsSource(nameof(UuidSameValues))]
        public int uuid_CompareTo_T_SameValue(Uuid uuid, Uuid sameValue)
        {
            return uuid.CompareTo(sameValue);
        }

        [Benchmark]
        [BenchmarkCategory("IComparable_T_CompareTo_same")]
        [ArgumentsSource(nameof(GuidSameValues))]
        public int guid_CompareTo_T_SameValue(Guid guid, Guid sameValue)
        {
            return guid.CompareTo(sameValue);
        }

        // IComparable<T>.CompareTo with different value
        [Benchmark]
        [BenchmarkCategory("IComparable _T_CompareTo_different")]
        [ArgumentsSource(nameof(UuidDifferentValues))]
        public int uuid_CompareTo_T_DifferentValue(Uuid uuid, Uuid differentValue)
        {
            return uuid.CompareTo(differentValue);
        }

        [Benchmark]
        [BenchmarkCategory("IComparable _T_CompareTo_different")]
        [ArgumentsSource(nameof(GuidDifferentValues))]
        public int guid_CompareTo_T_DifferentValue(Guid guid, Guid differentValue)
        {
            return guid.CompareTo(differentValue);
        }
    }
}