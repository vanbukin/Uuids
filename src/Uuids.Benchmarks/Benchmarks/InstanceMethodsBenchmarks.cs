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
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class InstanceMethodsBenchmarks
{
    private static readonly Guid _guidValue;
    private static readonly Uuid _uuidValue;

    static InstanceMethodsBenchmarks()
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

    // ToByteArray
    [Benchmark]
    [BenchmarkCategory("ToByteArray")]
    [ArgumentsSource(nameof(GuidArgs))]
    public byte[] guid_ToByteArray(Guid guidValue)
    {
        return guidValue.ToByteArray();
    }

    [Benchmark]
    [BenchmarkCategory("ToByteArray")]
    [ArgumentsSource(nameof(UuidArgs))]
    public byte[] uuid_ToByteArray(Uuid uuidValue)
    {
        return uuidValue.ToByteArray();
    }

    // TryWriteBytes
    [Benchmark(OperationsPerInvoke = 16)]
    [BenchmarkCategory("TryWriteBytes")]
    [ArgumentsSource(nameof(UuidArgs))]
    public void uuid_TryWriteBytes(Uuid uuidValue)
    {
        Span<byte> buffer = stackalloc byte[16];
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);

        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);

        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);

        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
        uuidValue.TryWriteBytes(buffer);
    }

    [Benchmark(OperationsPerInvoke = 16)]
    [BenchmarkCategory("TryWriteBytes")]
    [ArgumentsSource(nameof(GuidArgs))]
    public void guid_TryWriteBytes(Guid guidValue)
    {
        Span<byte> buffer = stackalloc byte[16];
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);

        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);

        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);

        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
        guidValue.TryWriteBytes(buffer);
    }
}
