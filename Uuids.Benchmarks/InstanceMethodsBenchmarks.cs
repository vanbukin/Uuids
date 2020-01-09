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
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class InstanceMethodsBenchmarks
    {
        static InstanceMethodsBenchmarks()
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

        // ToByteArray
        [Benchmark]
        [BenchmarkCategory("ToByteArray")]
        [ArgumentsSource(nameof(GuidArgs))]
        public byte[] guid_ToByteArray(Guid guid)
        {
            return guid.ToByteArray();
        }

        [Benchmark]
        [BenchmarkCategory("ToByteArray")]
        [ArgumentsSource(nameof(UuidArgs))]
        public byte[] uuid_ToByteArray(Uuid uuid)
        {
            return uuid.ToByteArray();
        }

        // TryWriteBytes
        [Benchmark(OperationsPerInvoke = 16)]
        [BenchmarkCategory("TryWriteBytes")]
        [ArgumentsSource(nameof(UuidArgs))]
        public void uuid_TryWriteBytes(Uuid uuid)
        {
            Span<byte> buffer = stackalloc byte[16];
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);

            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);

            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);

            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
            uuid.TryWriteBytes(buffer);
        }

        [Benchmark(OperationsPerInvoke = 16)]
        [BenchmarkCategory("TryWriteBytes")]
        [ArgumentsSource(nameof(GuidArgs))]
        public void guid_TryWriteBytes(Guid guid)
        {
            Span<byte> buffer = stackalloc byte[16];
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);

            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);

            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);

            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
            guid.TryWriteBytes(buffer);
        }
    }
}