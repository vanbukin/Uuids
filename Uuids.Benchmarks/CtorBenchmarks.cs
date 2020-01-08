using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Uuids.Benchmarks
{
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
        [Benchmark(OperationsPerInvoke = 16)]
        [BenchmarkCategory("ReadOnlySpan<byte>")]
        [Arguments(new byte[] {253, 47, 238, 170, 214, 83, 143, 78, 140, 107, 139, 132, 94, 5, 145, 199})]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void guid_CtorReadOnlySpan(byte[] guidBytes)
        {
            var span = new ReadOnlySpan<byte>(guidBytes);
            new Guid(span);
            new Guid(span);
            new Guid(span);
            new Guid(span);

            new Guid(span);
            new Guid(span);
            new Guid(span);
            new Guid(span);

            new Guid(span);
            new Guid(span);
            new Guid(span);
            new Guid(span);

            new Guid(span);
            new Guid(span);
            new Guid(span);
            new Guid(span);
        }

        [Benchmark(OperationsPerInvoke = 16)]
        [BenchmarkCategory("ReadOnlySpan<byte>")]
        [Arguments(new byte[] {170, 238, 47, 253, 83, 214, 78, 143, 140, 107, 139, 132, 94, 5, 145, 199})]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void uuid_CtorReadOnlySpan(byte[] uuidBytes)
        {
            var span = new ReadOnlySpan<byte>(uuidBytes);
            new Uuid(span);
            new Uuid(span);
            new Uuid(span);
            new Uuid(span);

            new Uuid(span);
            new Uuid(span);
            new Uuid(span);
            new Uuid(span);

            new Uuid(span);
            new Uuid(span);
            new Uuid(span);
            new Uuid(span);

            new Uuid(span);
            new Uuid(span);
            new Uuid(span);
            new Uuid(span);
        }
    }
}