using System;
using NUnit.Framework;
using Uuids.Tests.Data;

namespace Uuids.Tests;

public class UuidTryWriteBytesTests
{
    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public unsafe void ToByteArrayExactOutputSize(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        byte* buffer = stackalloc byte[16];
        Span<byte> output = new(buffer, 16);

        bool wasWritten = uuid.TryWriteBytes(output);

        byte[] outputBytes = output.ToArray();

        Assert.True(wasWritten);
        Assert.AreEqual(correctBytes, outputBytes);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public unsafe void ToByteArrayLargeOutputSize(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        byte* buffer = stackalloc byte[512];
        Span<byte> output = new(buffer, 512);

        bool wasWritten = uuid.TryWriteBytes(output);

        byte[] outputBytes = output.Slice(0, 16).ToArray();

        Assert.True(wasWritten);
        Assert.AreEqual(correctBytes, outputBytes);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public unsafe void ToByteArraySmallOutputSize(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        byte* buffer = stackalloc byte[4];
        Span<byte> output = new(buffer, 4);

        bool wasWritten = uuid.TryWriteBytes(output);

        byte[] outputBytes = output.ToArray();

        Assert.False(wasWritten);
        Assert.AreNotEqual(correctBytes, outputBytes);
    }
}
