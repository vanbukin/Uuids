using System;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class UuidTryParseTests
{
    [Test]
    public void TryParseNullStringShouldFalse()
    {
        bool parsed = Uuid.TryParse((string?) null, out Uuid uuid);
        Assert.Multiple(() =>
        {
            Assert.False(parsed);
            Assert.AreEqual(Uuid.Empty, uuid);
        });
    }

    [Test]
    public void TryParseEmptyStringShouldFalse()
    {
        bool parsed = Uuid.TryParse(string.Empty, out Uuid uuid);
        Assert.Multiple(() =>
        {
            Assert.False(parsed);
            Assert.AreEqual(Uuid.Empty, uuid);
        });
    }

    [Test]
    public void TryParseEmptySpanShouldFalse()
    {
        bool parsed = Uuid.TryParse(new ReadOnlySpan<char>(Array.Empty<char>()), out Uuid uuid);
        Assert.Multiple(() =>
        {
            Assert.False(parsed);
            Assert.AreEqual(Uuid.Empty, uuid);
        });
    }

    #region TryParseN

    [Test]
    public void TryParseCorrectNString()
    {
        TryParseCorrectString(UuidTestData.CorrectNStrings);
    }

    [Test]
    public void TryParseCorrectNSpan()
    {
        TryParseCorrectSpan(UuidTestData.CorrectNStrings);
    }

    [Test]
    public void TryParseNIncorrectLargeString()
    {
        TryParseIncorrectString(UuidTestData.LargeNStrings);
    }

    [Test]
    public void ParseNIncorrectLargeSpan()
    {
        TryParseIncorrectSpan(UuidTestData.LargeNStrings);
    }

    [Test]
    public void ParseNIncorrectSmallString()
    {
        TryParseIncorrectString(UuidTestData.SmallNStrings);
    }

    [Test]
    public void ParseNIncorrectSmallSpan()
    {
        TryParseIncorrectSpan(UuidTestData.SmallNStrings);
    }

    [Test]
    public void ParseIncorrectNString()
    {
        TryParseIncorrectString(UuidTestData.BrokenNStrings);
    }

    [Test]
    public void ParseIncorrectNSpan()
    {
        TryParseIncorrectSpan(UuidTestData.BrokenNStrings);
    }

    #endregion

    #region TryParseD

    [Test]
    public void TryParseCorrectDString()
    {
        TryParseCorrectString(UuidTestData.CorrectDStrings);
    }

    [Test]
    public void TryParseCorrectDSpan()
    {
        TryParseCorrectSpan(UuidTestData.CorrectDStrings);
    }

    [Test]
    public void TryParseDIncorrectLargeString()
    {
        TryParseIncorrectString(UuidTestData.LargeDStrings);
    }

    [Test]
    public void ParseDIncorrectLargeSpan()
    {
        TryParseIncorrectSpan(UuidTestData.LargeDStrings);
    }

    [Test]
    public void ParseDIncorrectSmallString()
    {
        TryParseIncorrectString(UuidTestData.SmallDStrings);
    }

    [Test]
    public void ParseDIncorrectSmallSpan()
    {
        TryParseIncorrectSpan(UuidTestData.SmallDStrings);
    }

    [Test]
    public void ParseIncorrectDString()
    {
        TryParseIncorrectString(UuidTestData.BrokenDStrings);
    }

    [Test]
    public void ParseIncorrectDSpan()
    {
        TryParseIncorrectSpan(UuidTestData.BrokenDStrings);
    }

    #endregion

    #region TryParseB

    [Test]
    public void TryParseCorrectBString()
    {
        TryParseCorrectString(UuidTestData.CorrectBStrings);
    }

    [Test]
    public void TryParseCorrectBSpan()
    {
        TryParseCorrectSpan(UuidTestData.CorrectBStrings);
    }

    [Test]
    public void TryParseBIncorrectLargeString()
    {
        TryParseIncorrectString(UuidTestData.LargeBStrings);
    }

    [Test]
    public void ParseBIncorrectLargeSpan()
    {
        TryParseIncorrectSpan(UuidTestData.LargeBStrings);
    }

    [Test]
    public void ParseBIncorrectSmallString()
    {
        TryParseIncorrectString(UuidTestData.SmallBStrings);
    }

    [Test]
    public void ParseBIncorrectSmallSpan()
    {
        TryParseIncorrectSpan(UuidTestData.SmallBStrings);
    }

    [Test]
    public void ParseIncorrectBString()
    {
        TryParseIncorrectString(UuidTestData.BrokenBStrings);
    }

    [Test]
    public void ParseIncorrectBSpan()
    {
        TryParseIncorrectSpan(UuidTestData.BrokenBStrings);
    }

    #endregion

    #region TryParseP

    [Test]
    public void TryParseCorrectPString()
    {
        TryParseCorrectString(UuidTestData.CorrectPStrings);
    }

    [Test]
    public void TryParseCorrectPSpan()
    {
        TryParseCorrectSpan(UuidTestData.CorrectPStrings);
    }

    [Test]
    public void TryParsePIncorrectLargeString()
    {
        TryParseIncorrectString(UuidTestData.LargePStrings);
    }

    [Test]
    public void ParsePIncorrectLargeSpan()
    {
        TryParseIncorrectSpan(UuidTestData.LargePStrings);
    }

    [Test]
    public void ParsePIncorrectSmallString()
    {
        TryParseIncorrectString(UuidTestData.SmallPStrings);
    }

    [Test]
    public void ParsePIncorrectSmallSpan()
    {
        TryParseIncorrectSpan(UuidTestData.SmallPStrings);
    }

    [Test]
    public void ParseIncorrectPString()
    {
        TryParseIncorrectString(UuidTestData.BrokenPStrings);
    }

    [Test]
    public void ParseIncorrectPSpan()
    {
        TryParseIncorrectSpan(UuidTestData.BrokenPStrings);
    }

    #endregion

    #region TryParseX

    [Test]
    public void TryParseCorrectXString()
    {
        TryParseCorrectString(UuidTestData.CorrectXStrings);
    }

    [Test]
    public void TryParseCorrectXSpan()
    {
        TryParseCorrectSpan(UuidTestData.CorrectXStrings);
    }

    [Test]
    public void TryParseXIncorrectLargeString()
    {
        TryParseIncorrectString(UuidTestData.LargeXStrings);
    }

    [Test]
    public void ParseXIncorrectLargeSpan()
    {
        TryParseIncorrectSpan(UuidTestData.LargeXStrings);
    }

    [Test]
    public void ParseXIncorrectSmallString()
    {
        TryParseIncorrectString(UuidTestData.SmallXStrings);
    }

    [Test]
    public void ParseXIncorrectSmallSpan()
    {
        TryParseIncorrectSpan(UuidTestData.SmallXStrings);
    }

    [Test]
    public void ParseIncorrectXString()
    {
        TryParseIncorrectString(UuidTestData.BrokenXStrings);
    }

    [Test]
    public void ParseIncorrectXSpan()
    {
        TryParseIncorrectSpan(UuidTestData.BrokenXStrings);
    }

    #endregion

    #region Helpers

    private unsafe void TryParseCorrectString(UuidStringWithBytes[] correctStrings)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctString in correctStrings)
            {
                string stringToParse = correctString.UuidString;
                byte[] expectedBytes = correctString.Bytes;

                bool parsed = Uuid.TryParse(stringToParse, out Uuid uuid);

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

    private unsafe void TryParseCorrectSpan(UuidStringWithBytes[] correctStrings)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctString in correctStrings)
            {
                ReadOnlySpan<char> spanToParse = new(correctString.UuidString.ToCharArray());
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

    private static void TryParseIncorrectString(string[] incorrectLargeStrings)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeString in incorrectLargeStrings)
            {
                Assert.False(Uuid.TryParse(largeString, out _));
            }
        });
    }

    private static void TryParseIncorrectSpan(string[] incorrectLargeStrings)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeString in incorrectLargeStrings)
            {
                ReadOnlySpan<char> largeSpan = new(largeString.ToCharArray());
                Assert.False(Uuid.TryParse(largeSpan, out _));
            }
        });
    }

    #endregion
}
