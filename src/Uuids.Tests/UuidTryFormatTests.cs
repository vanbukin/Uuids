using System;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Utils;

namespace Uuids.Tests;

public unsafe class UuidTryFormatTests
{
    #region TryFormat

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatNullFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);
        char* bufferPtr = stackalloc char[32];
        Span<char> spanBuffer = new(bufferPtr, 32);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten));
        Assert.AreEqual(32, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 32));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatEmptyFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);
        char* bufferPtr = stackalloc char[32];
        Span<char> spanBuffer = new(bufferPtr, 32);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, ReadOnlySpan<char>.Empty));
        Assert.AreEqual(32, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 32));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatIncorrectFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        Span<char> buffer = stackalloc char[68];
        Assert.False(uuid.TryFormat(buffer, out int charsWritten, "Ъ".AsSpan()));
        Assert.AreEqual(0, charsWritten);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatTooLongFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        Span<char> buffer = stackalloc char[68];
        Assert.False(uuid.TryFormat(buffer, out int charsWritten, "ЪЪ".AsSpan()));
        Assert.AreEqual(0, charsWritten);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatNCorrect(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);
        char* bufferPtr = stackalloc char[32];
        Span<char> spanBuffer = new(bufferPtr, 32);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'N' })));
        Assert.AreEqual(32, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 32));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatDCorrect(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringD(correctBytes);
        char* bufferPtr = stackalloc char[36];
        Span<char> spanBuffer = new(bufferPtr, 36);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'D' })));
        Assert.AreEqual(36, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 36));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatBCorrect(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringB(correctBytes);
        char* bufferPtr = stackalloc char[38];
        Span<char> spanBuffer = new(bufferPtr, 38);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'B' })));
        Assert.AreEqual(38, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 38));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatPCorrect(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringP(correctBytes);
        char* bufferPtr = stackalloc char[38];
        Span<char> spanBuffer = new(bufferPtr, 38);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'P' })));
        Assert.AreEqual(38, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 38));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatXCorrect(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringX(correctBytes);
        char* bufferPtr = stackalloc char[68];
        Span<char> spanBuffer = new(bufferPtr, 68);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'X' })));
        Assert.AreEqual(68, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 68));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void TryFormatSmallDestination(byte[] correctBytes)
    {
        Assert.Multiple(() =>
        {
            Uuid uuid = new(correctBytes);
            Span<char> buffer = stackalloc char[10];
            char[] formats =
            {
                'N',
                'n',
                'D',
                'd',
                'B',
                'b',
                'P',
                'p',
                'X',
                'x'
            };
            foreach (char format in formats)
            {
                Assert.False(uuid.TryFormat(buffer, out int charsWritten, new(new[] { format })));
                Assert.AreEqual(0, charsWritten);
            }
        });
    }

    #endregion

    #region ISpanFormattable.TryFormat

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatEmptyFormat(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);
        char* bufferPtr = stackalloc char[32];
        Span<char> spanBuffer = new(bufferPtr, 32);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, ReadOnlySpan<char>.Empty, null));
        Assert.AreEqual(32, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 32));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatIncorrectFormat(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        Span<char> buffer = stackalloc char[68];
        Assert.False(uuid.TryFormat(buffer, out int charsWritten, "Ъ".AsSpan(), null));
        Assert.AreEqual(0, charsWritten);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatTooLongFormat(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        Span<char> buffer = stackalloc char[68];
        Assert.False(uuid.TryFormat(buffer, out int charsWritten, "ЪЪ".AsSpan(), null));
        Assert.AreEqual(0, charsWritten);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatNCorrect(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);
        char* bufferPtr = stackalloc char[32];
        Span<char> spanBuffer = new(bufferPtr, 32);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'N' }), null));
        Assert.AreEqual(32, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 32));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatDCorrect(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        string expectedString = UuidTestsUtils.GetStringD(correctBytes);
        char* bufferPtr = stackalloc char[36];
        Span<char> spanBuffer = new(bufferPtr, 36);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'D' }), null));
        Assert.AreEqual(36, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 36));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatBCorrect(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        string expectedString = UuidTestsUtils.GetStringB(correctBytes);
        char* bufferPtr = stackalloc char[38];
        Span<char> spanBuffer = new(bufferPtr, 38);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'B' }), null));
        Assert.AreEqual(38, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 38));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatPCorrect(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        string expectedString = UuidTestsUtils.GetStringP(correctBytes);
        char* bufferPtr = stackalloc char[38];
        Span<char> spanBuffer = new(bufferPtr, 38);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'P' }), null));
        Assert.AreEqual(38, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 38));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatXCorrect(byte[] correctBytes)
    {
        ISpanFormattable uuid = new Uuid(correctBytes);
        string expectedString = UuidTestsUtils.GetStringX(correctBytes);
        char* bufferPtr = stackalloc char[68];
        Span<char> spanBuffer = new(bufferPtr, 68);
        Assert.True(uuid.TryFormat(spanBuffer, out int charsWritten, new(new[] { 'X' }), null));
        Assert.AreEqual(68, charsWritten);
        Assert.AreEqual(expectedString, new string(bufferPtr, 0, 68));
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void SpanFormattableTryFormatSmallDestination(byte[] correctBytes)
    {
        Assert.Multiple(() =>
        {
            ISpanFormattable uuid = new Uuid(correctBytes);
            Span<char> buffer = stackalloc char[10];
            char[] formats =
            {
                'N',
                'n',
                'D',
                'd',
                'B',
                'b',
                'P',
                'p',
                'X',
                'x'
            };
            foreach (char format in formats)
            {
                Assert.False(uuid.TryFormat(buffer, out int charsWritten, new(new[] { format }), null));
                Assert.AreEqual(0, charsWritten);
            }
        });
    }

    #endregion
}
