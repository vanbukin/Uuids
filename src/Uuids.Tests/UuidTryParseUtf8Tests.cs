using System;
using System.Text;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class UuidTryParseUtf8Tests
{
    [Test]
    public void TryParseUtf8NullSpanShouldFalse()
    {
        bool parsed = Uuid.TryParse((ReadOnlySpan<byte>) null, out Uuid uuid);
        Assert.Multiple(() =>
        {
            Assert.False(parsed);
            Assert.AreEqual(Uuid.Empty, uuid);
        });
    }

    [Test]
    public void TryParseUtf8EmptySpanShouldFalse()
    {
        bool parsed = Uuid.TryParse(ReadOnlySpan<byte>.Empty, out Uuid uuid);
        Assert.Multiple(() =>
        {
            Assert.False(parsed);
            Assert.AreEqual(Uuid.Empty, uuid);
        });
    }

    #region TryParseN

    [Test]
    public void TryParseUtf8CorrectNSpan()
    {
        TryParseUtf8CorrectSpan(UuidTestData.CorrectNStrings);
    }

    [Test]
    public void ParseUtf8NIncorrectLargeSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.LargeNStrings);
    }

    [Test]
    public void ParseNIncorrectSmallSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.SmallNStrings);
    }

    [Test]
    public void ParseIncorrectNSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.BrokenNStrings);
    }

    #endregion

    #region TryParseD

    [Test]
    public void TryParseUtf8CorrectDSpan()
    {
        TryParseUtf8CorrectSpan(UuidTestData.CorrectDStrings);
    }

    [Test]
    public void TryParseUtf8DIncorrectLargeSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.LargeDStrings);
    }

    [Test]
    public void TryParseUtf8DIncorrectSmallSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.SmallDStrings);
    }

    [Test]
    public void TryParseUtf8IncorrectDSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.BrokenDStrings);
    }

    #endregion

    #region TryParseB

    [Test]
    public void TryParseUtf8CorrectBSpan()
    {
        TryParseUtf8CorrectSpan(UuidTestData.CorrectBStrings);
    }

    [Test]
    public void TryParseUtf8BIncorrectLargeSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.LargeBStrings);
    }

    [Test]
    public void TryParseUtf8BIncorrectSmallSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.SmallBStrings);
    }

    [Test]
    public void TryParseUtf8IncorrectBSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.BrokenBStrings);
    }

    #endregion

    #region TryParseP

    [Test]
    public void TryParseUtf8CorrectPSpan()
    {
        TryParseUtf8CorrectSpan(UuidTestData.CorrectPStrings);
    }

    [Test]
    public void TryParseUtf8PIncorrectLargeSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.LargePStrings);
    }

    [Test]
    public void TryParseUtf8PIncorrectSmallSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.SmallPStrings);
    }

    [Test]
    public void TryParseUtf8IncorrectPSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.BrokenPStrings);
    }

    #endregion

    #region TryParseX

    [Test]
    public void TryParseUtf8CorrectXSpan()
    {
        TryParseUtf8CorrectSpan(UuidTestData.CorrectXStrings);
    }

    [Test]
    public void TryParseUtf8XIncorrectLargeSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.LargeXStrings);
    }

    [Test]
    public void TryParseUtf8XIncorrectSmallSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.SmallXStrings);
    }

    [Test]
    public void TryParseUtf8IncorrectXSpan()
    {
        TryParseUtf8IncorrectSpan(UuidTestData.BrokenXStrings);
    }

    #endregion

    #region Helpers

    private unsafe void TryParseUtf8CorrectSpan(UuidStringWithBytes[] correctStrings)
    {
        Assert.Multiple(() =>
        {
            Span<byte> utf8Buffer = stackalloc byte[8192];
            foreach (UuidStringWithBytes correctString in correctStrings)
            {
                int utf8Chars = GetUtf8BytesSpanFromString(correctString.UuidString, utf8Buffer);
                Span<byte> spanToParse = utf8Buffer.Slice(0, utf8Chars);
                byte[] expectedBytes = correctString.Bytes;

                bool parsed = Uuid.TryParse(spanToParse, out Uuid uuid);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = uuid;
                }

                Assert.True(parsed);
                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    private void TryParseUtf8IncorrectSpan(string[] incorrectLargeStrings)
    {
        Assert.Multiple(() =>
        {
            Span<byte> utf8Buffer = stackalloc byte[8192];
            foreach (string largeString in incorrectLargeStrings)
            {
                int utf8Chars = GetUtf8BytesSpanFromString(largeString, utf8Buffer);
                Span<byte> spanToParse = utf8Buffer.Slice(0, utf8Chars);
                Assert.False(Uuid.TryParse(spanToParse, out _));
            }
        });
    }

    private static int GetUtf8BytesSpanFromString(string uuidString, Span<byte> result)
    {
        byte[] resultBytes = Encoding.UTF8.GetBytes(uuidString);
        if (resultBytes.Length > result.Length)
        {
            throw new ArgumentException("Utf8 bytes larger than provided buffer");
        }

        for (int i = 0; i < resultBytes.Length; i++)
        {
            result[i] = resultBytes[i];
        }

        return resultBytes.Length;
    }

    #endregion
}
