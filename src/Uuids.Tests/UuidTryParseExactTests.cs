using System;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class UuidTryParseExactTests
{
    private const string? NullString = null;

    [Test]
    public void TryParseExactNullStringCorrectFormatShouldFalse()
    {
        foreach (string format in UuidFormats.All)
        {
            bool parsed = Uuid.TryParseExact(NullString, format, out Uuid uuid);
            Assert.Multiple(() =>
            {
                Assert.False(parsed);
                Assert.AreEqual(Uuid.Empty, uuid);
            });
        }
    }

    [Test]
    public void TryParseExactCorrectStringNullFormatShouldFalse()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
#pragma warning disable 8625
                bool parsed = Uuid.TryParseExact(correctNString.UuidString, NullString, out Uuid uuid);
#pragma warning restore 8625
                Assert.False(parsed);
                Assert.AreEqual(Uuid.Empty, uuid);
            }
        });
    }

    [Test]
    public void TryParseExactCorrectStringIncorrectFormatShouldFalse()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                bool parsed = Uuid.TryParseExact(correctNString.UuidString, "Ъ", out Uuid uuid);
                Assert.False(parsed);
                Assert.AreEqual(Uuid.Empty, uuid);
            }
        });
    }

    [Test]
    public void TryParseExactEmptyStringCorrectFormatShouldFalse()
    {
        Assert.Multiple(() =>
        {
            foreach (string format in UuidFormats.All)
            {
                bool parsed = Uuid.TryParseExact(string.Empty, format, out Uuid uuid);
                Assert.False(parsed);
                Assert.AreEqual(Uuid.Empty, uuid);
            }
        });
    }

    [Test]
    public void TryParseExactEmptySpanCorrectFormatShouldFalse()
    {
        Assert.Multiple(() =>
        {
            foreach (string format in UuidFormats.All)
            {
                ReadOnlySpan<char> stringSpan = new(Array.Empty<char>());
                ReadOnlySpan<char> formatSpan = new(format.ToCharArray());
                bool parsed = Uuid.TryParseExact(stringSpan, formatSpan, out Uuid uuid);
                Assert.False(parsed);
                Assert.AreEqual(Uuid.Empty, uuid);
            }
        });
    }

    [Test]
    public void TryParseExactCorrectSpanEmptyFormatShouldFalse()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                ReadOnlySpan<char> stringSpan = new(correctNString.UuidString.ToCharArray());
                ReadOnlySpan<char> formatSpan = new(Array.Empty<char>());
                bool parsed = Uuid.TryParseExact(stringSpan, formatSpan, out Uuid uuid);
                Assert.False(parsed);
                Assert.AreEqual(Uuid.Empty, uuid);
            }
        });
    }

    [Test]
    public void TryParseExactCorrectSpanIncorrectFormatShouldFalse()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                ReadOnlySpan<char> stringSpan = new(correctNString.UuidString.ToCharArray());
                ReadOnlySpan<char> formatSpan = new(new[] { 'Ъ' });
                bool parsed = Uuid.TryParseExact(stringSpan, formatSpan, out Uuid uuid);
                Assert.False(parsed);
                Assert.AreEqual(Uuid.Empty, uuid);
            }
        });
    }

    #region TryParseExactN

    [Test]
    public void TryParseExactCorrectNCorrectFormat()
    {
        TryParseExactCorrectStringCorrectFormat(
            UuidTestData.CorrectNStrings,
            UuidFormats.N);
    }

    [Test]
    public void ParseExactCorrectNIncorrectFormat()
    {
        TryParseExactCorrectStringIncorrectFormat(
            UuidTestData.CorrectNStrings,
            UuidFormats.AllExceptN);
    }

    [Test]
    public void ParseExactIncorrectNCorrectFormat()
    {
        TryParseExactIncorrectStringCorrectFormat(
            UuidTestData.BrokenNStrings,
            UuidFormats.N);
    }

    [Test]
    public void ParseExactNotNStringCorrectFormat()
    {
        TryParseExactOtherFormatStringCorrectFormat(
            UuidTestData.CorrectDStrings,
            UuidFormats.N);
    }

    #endregion

    #region TryParseExactD

    [Test]
    public void TryParseExactCorrectDCorrectFormat()
    {
        TryParseExactCorrectStringCorrectFormat(
            UuidTestData.CorrectDStrings,
            UuidFormats.D);
    }

    [Test]
    public void ParseExactCorrectDIncorrectFormat()
    {
        TryParseExactCorrectStringIncorrectFormat(
            UuidTestData.CorrectDStrings,
            UuidFormats.AllExceptD);
    }

    [Test]
    public void ParseExactIncorrectDCorrectFormat()
    {
        TryParseExactIncorrectStringCorrectFormat(
            UuidTestData.BrokenDStrings,
            UuidFormats.D);
    }

    [Test]
    public void ParseExactNotDStringCorrectFormat()
    {
        TryParseExactOtherFormatStringCorrectFormat(
            UuidTestData.CorrectNStrings,
            UuidFormats.D);
    }

    #endregion

    #region TryParseExactB

    [Test]
    public void TryParseExactCorrectBCorrectFormat()
    {
        TryParseExactCorrectStringCorrectFormat(
            UuidTestData.CorrectBStrings,
            UuidFormats.B);
    }

    [Test]
    public void ParseExactCorrectBIncorrectFormat()
    {
        TryParseExactCorrectStringIncorrectFormat(
            UuidTestData.CorrectBStrings,
            UuidFormats.AllExceptB);
    }

    [Test]
    public void ParseExactIncorrectBCorrectFormat()
    {
        TryParseExactIncorrectStringCorrectFormat(
            UuidTestData.BrokenBStrings,
            UuidFormats.B);
    }

    [Test]
    public void ParseExactNotBStringCorrectFormat()
    {
        TryParseExactOtherFormatStringCorrectFormat(
            UuidTestData.CorrectNStrings,
            UuidFormats.B);
    }

    #endregion

    #region TryParseExactP

    [Test]
    public void TryParseExactCorrectPCorrectFormat()
    {
        TryParseExactCorrectStringCorrectFormat(
            UuidTestData.CorrectPStrings,
            UuidFormats.P);
    }

    [Test]
    public void ParseExactCorrectPIncorrectFormat()
    {
        TryParseExactCorrectStringIncorrectFormat(
            UuidTestData.CorrectPStrings,
            UuidFormats.AllExceptP);
    }

    [Test]
    public void ParseExactIncorrectPCorrectFormat()
    {
        TryParseExactIncorrectStringCorrectFormat(
            UuidTestData.BrokenPStrings,
            UuidFormats.P);
    }

    [Test]
    public void ParseExactNotPStringCorrectFormat()
    {
        TryParseExactOtherFormatStringCorrectFormat(
            UuidTestData.CorrectNStrings,
            UuidFormats.P);
    }

    #endregion

    #region TryParseExactX

    [Test]
    public void TryParseExactCorrectXCorrectFormat()
    {
        TryParseExactCorrectStringCorrectFormat(
            UuidTestData.CorrectXStrings,
            UuidFormats.X);
    }

    [Test]
    public void ParseExactCorrectXIncorrectFormat()
    {
        TryParseExactCorrectStringIncorrectFormat(
            UuidTestData.CorrectXStrings,
            UuidFormats.AllExceptX);
    }

    [Test]
    public void ParseExactIncorrectXCorrectFormat()
    {
        TryParseExactIncorrectStringCorrectFormat(
            UuidTestData.BrokenXStrings,
            UuidFormats.X);
    }

    [Test]
    public void ParseExactNotXStringCorrectFormat()
    {
        TryParseExactOtherFormatStringCorrectFormat(
            UuidTestData.CorrectNStrings,
            UuidFormats.X);
    }

    #endregion


    #region Helpers

    private unsafe void TryParseExactCorrectStringCorrectFormat(
        UuidStringWithBytes[] correctStrings,
        string[] correctFormats)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctString in correctStrings)
            {
                foreach (string format in correctFormats)
                {
                    bool isParsedFromString = Uuid.TryParseExact(
                        correctString.UuidString,
                        format,
                        out Uuid parsedUuidFromString);
                    bool isParsedBoolFromSpan = Uuid.TryParseExact(
                        new(correctString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(format.ToCharArray()),
                        out Uuid parsedUuidFromSpan);

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidFromString;
                        *(Uuid*) pinnedSpan = parsedUuidFromSpan;
                    }

                    Assert.True(isParsedFromString);
                    Assert.True(isParsedBoolFromSpan);
                    Assert.AreEqual(correctString.Bytes, actualBytesString);
                    Assert.AreEqual(correctString.Bytes, actualBytesSpan);
                }
            }
        });
    }

    private static readonly byte[] ExpectedEmptyUuidBytes =
    {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0
    };

    private unsafe void TryParseExactCorrectStringIncorrectFormat(
        UuidStringWithBytes[] correctStrings,
        string[] incorrectFormats)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctString in correctStrings)
            {
                foreach (string incorrectFormat in incorrectFormats)
                {
                    bool isParsedFromString = Uuid.TryParseExact(
                        correctString.UuidString,
                        incorrectFormat,
                        out Uuid parsedUuidFromString);
                    bool isParsedBoolFromSpan = Uuid.TryParseExact(
                        new(correctString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(incorrectFormat.ToCharArray()),
                        out Uuid parsedUuidFromSpan);

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidFromString;
                        *(Uuid*) pinnedSpan = parsedUuidFromSpan;
                    }

                    Assert.False(isParsedFromString);
                    Assert.False(isParsedBoolFromSpan);
                    Assert.AreEqual(ExpectedEmptyUuidBytes, actualBytesString);
                    Assert.AreEqual(ExpectedEmptyUuidBytes, actualBytesSpan);
                }
            }
        });
    }

    private unsafe void TryParseExactIncorrectStringCorrectFormat(
        string[] brokenStrings,
        string[] correctFormats)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenString in brokenStrings)
            {
                foreach (string correctFormat in correctFormats)
                {
                    bool isParsedFromString = Uuid.TryParseExact(
                        brokenString,
                        correctFormat,
                        out Uuid parsedUuidFromString);
                    bool isParsedBoolFromSpan = Uuid.TryParseExact(
                        new(brokenString.ToCharArray()),
                        new ReadOnlySpan<char>(correctFormat.ToCharArray()),
                        out Uuid parsedUuidFromSpan);

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidFromString;
                        *(Uuid*) pinnedSpan = parsedUuidFromSpan;
                    }

                    Assert.False(isParsedFromString);
                    Assert.False(isParsedBoolFromSpan);
                    Assert.AreEqual(ExpectedEmptyUuidBytes, actualBytesString);
                    Assert.AreEqual(ExpectedEmptyUuidBytes, actualBytesSpan);
                }
            }
        });
    }

    private unsafe void TryParseExactOtherFormatStringCorrectFormat(
        UuidStringWithBytes[] otherFormatStrings,
        string[] correctFormats)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes otherFormatString in otherFormatStrings)
            {
                foreach (string correctFormat in correctFormats)
                {
                    bool isParsedFromString = Uuid.TryParseExact(
                        otherFormatString.UuidString,
                        correctFormat,
                        out Uuid parsedUuidFromString);
                    bool isParsedBoolFromSpan = Uuid.TryParseExact(
                        new(otherFormatString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(correctFormat.ToCharArray()),
                        out Uuid parsedUuidFromSpan);

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidFromString;
                        *(Uuid*) pinnedSpan = parsedUuidFromSpan;
                    }

                    Assert.False(isParsedFromString);
                    Assert.False(isParsedBoolFromSpan);
                    Assert.AreEqual(ExpectedEmptyUuidBytes, actualBytesString);
                    Assert.AreEqual(ExpectedEmptyUuidBytes, actualBytesSpan);
                }
            }
        });
    }

    #endregion
}
