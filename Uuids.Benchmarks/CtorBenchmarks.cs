using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Uuids.Benchmarks
{
    [GcServer(true)]
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class CtorBenchmarks
    {
        // byte[]
        [Benchmark]
        [BenchmarkCategory("byte[]")]
        [Arguments(new byte[] {253, 47, 238, 170, 214, 83, 143, 78, 140, 107, 139, 132, 94, 5, 145, 199})]
        public Guid guid_CtorByteArray(byte[] guidBytes)
        {
            return new Guid(guidBytes);
        }

        [Benchmark]
        [BenchmarkCategory("byte[]")]
        [Arguments(new byte[] {170, 238, 47, 253, 83, 214, 78, 143, 140, 107, 139, 132, 94, 5, 145, 199})]
        public Uuid uuid_CtorByteArray(byte[] uuidBytes)
        {
            return new Uuid(uuidBytes);
        }

        // ReadOnlySpan<byte>
        [Benchmark]
        [BenchmarkCategory("ReadOnlySpan<byte>")]
        [Arguments(new byte[] {253, 47, 238, 170, 214, 83, 143, 78, 140, 107, 139, 132, 94, 5, 145, 199})]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public Guid guid_CtorReadOnlySpan(ReadOnlySpan<byte> span)
        {
            return new Guid(span);
        }

        [Benchmark]
        [BenchmarkCategory("ReadOnlySpan<byte>")]
        [Arguments(new byte[] {170, 238, 47, 253, 83, 214, 78, 143, 140, 107, 139, 132, 94, 5, 145, 199})]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public Uuid uuid_CtorReadOnlySpan(ReadOnlySpan<byte> span)
        {
            return new Uuid(span);
        }
    }
}