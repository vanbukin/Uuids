using System;
using System.Collections.Generic;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class UuidParseExactTests
{
    private const string? NullString = null;

    [Test]
    public void ParseExactNullStringCorrectFormatShouldThrows()
    {
        Assert.Multiple(() =>
        {
            foreach (string format in UuidFormats.All)
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
#pragma warning disable 8625
                    Uuid _ = Uuid.ParseExact(NullString, format);
#pragma warning restore 8625
                });
            }
        });
    }

    [Test]
    public void ParseExactCorrectStringNullFormatShouldThrows()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
#pragma warning disable 8625
                    Uuid _ = Uuid.ParseExact(correctNString.UuidString, NullString);
#pragma warning restore 8625
                });
            }
        });
    }

    [Test]
    public void ParseExactCorrectStringIncorrectFormatShouldThrows()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    // ReSharper disable once RedundantCast
                    Uuid _ = Uuid.ParseExact(correctNString.UuidString, "Ъ");
                });
            }
        });
    }

    [Test]
    public void ParseExactEmptyStringCorrectFormatShouldThrows()
    {
        Assert.Multiple(() =>
        {
            foreach (string format in UuidFormats.All)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.ParseExact(string.Empty, format);
                });
            }
        });
    }

    [Test]
    public void ParseExactEmptySpanCorrectFormatShouldThrows()
    {
        Assert.Multiple(() =>
        {
            foreach (string format in UuidFormats.All)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> formatSpan = new(format.ToCharArray());
                    Uuid _ = Uuid.ParseExact(new(Array.Empty<char>()), formatSpan);
                });
            }
        });
    }

    [Test]
    public void ParseExactCorrectSpanEmptyFormatShouldThrows()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> nStringSpan = new(correctNString.UuidString.ToCharArray());
                    ReadOnlySpan<char> formatSpan = new(Array.Empty<char>());
                    // ReSharper disable once RedundantCast
                    Uuid _ = Uuid.ParseExact(nStringSpan, formatSpan);
                });
            }
        });
    }

    [Test]
    public void ParseExactCorrectSpanIncorrectFormatShouldThrows()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> nStringSpan = new(correctNString.UuidString.ToCharArray());
                    ReadOnlySpan<char> formatSpan = new(new[] { 'Ъ' });
                    // ReSharper disable once RedundantCast
                    Uuid _ = Uuid.ParseExact(nStringSpan, formatSpan);
                });
            }
        });
    }

    #region ParseExactN

    [Test]
    public unsafe void ParseExactCorrectNCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                List<byte[]> results = new();
                foreach (string format in UuidFormats.N)
                {
                    Uuid parsedUuidString = Uuid.ParseExact(correctNString.UuidString, format);
                    Uuid parsedUuidSpan = Uuid.ParseExact(
                        new(correctNString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(format.ToCharArray()));

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidString;
                        *(Uuid*) pinnedSpan = parsedUuidSpan;
                    }

                    results.Add(actualBytesString);
                    results.Add(actualBytesSpan);
                }

                foreach (byte[] result in results)
                {
                    Assert.AreEqual(correctNString.Bytes, result);
                }
            }
        });
    }

    [Test]
    public void ParseExactCorrectNIncorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                foreach (string format in UuidFormats.AllExceptN)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctNString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctNString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactIncorrectNCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenNString in UuidTestData.BrokenNStrings)
            {
                foreach (string format in UuidFormats.N)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(brokenNString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(brokenNString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactNotNStringCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                foreach (string format in UuidFormats.N)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctDString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctDString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    #endregion

    #region ParseExactD

    [Test]
    public unsafe void ParseExactCorrectDCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                List<byte[]> results = new();
                foreach (string format in UuidFormats.D)
                {
                    Uuid parsedUuidString = Uuid.ParseExact(correctDString.UuidString, format);
                    Uuid parsedUuidSpan = Uuid.ParseExact(
                        new(correctDString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(format.ToCharArray()));

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidString;
                        *(Uuid*) pinnedSpan = parsedUuidSpan;
                    }

                    results.Add(actualBytesString);
                    results.Add(actualBytesSpan);
                }

                foreach (byte[] result in results)
                {
                    Assert.AreEqual(correctDString.Bytes, result);
                }
            }
        });
    }

    [Test]
    public void ParseExactCorrectDIncorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                foreach (string format in UuidFormats.AllExceptD)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctDString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctDString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactIncorrectDCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenDString in UuidTestData.BrokenDStrings)
            {
                foreach (string format in UuidFormats.D)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(brokenDString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(brokenDString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactNotDStringCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                foreach (string format in UuidFormats.D)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctNString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctNString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    #endregion

    #region ParseExactB

    [Test]
    public unsafe void ParseExactCorrectBCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                List<byte[]> results = new();
                foreach (string format in UuidFormats.B)
                {
                    Uuid parsedUuidString = Uuid.ParseExact(correctBString.UuidString, format);
                    Uuid parsedUuidSpan = Uuid.ParseExact(
                        new(correctBString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(format.ToCharArray()));

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidString;
                        *(Uuid*) pinnedSpan = parsedUuidSpan;
                    }

                    results.Add(actualBytesString);
                    results.Add(actualBytesSpan);
                }

                foreach (byte[] result in results)
                {
                    Assert.AreEqual(correctBString.Bytes, result);
                }
            }
        });
    }

    [Test]
    public void ParseExactCorrectBIncorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                foreach (string format in UuidFormats.AllExceptB)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctBString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctBString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactIncorrectBCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenBString in UuidTestData.BrokenBStrings)
            {
                foreach (string format in UuidFormats.B)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(brokenBString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(brokenBString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactNotBStringCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                foreach (string format in UuidFormats.B)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctNString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctNString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    #endregion

    #region ParseExactP

    [Test]
    public unsafe void ParseExactCorrectPCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                List<byte[]> results = new();
                foreach (string format in UuidFormats.P)
                {
                    Uuid parsedUuidString = Uuid.ParseExact(correctPString.UuidString, format);
                    Uuid parsedUuidSpan = Uuid.ParseExact(
                        new(correctPString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(format.ToCharArray()));

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidString;
                        *(Uuid*) pinnedSpan = parsedUuidSpan;
                    }

                    results.Add(actualBytesString);
                    results.Add(actualBytesSpan);
                }

                foreach (byte[] result in results)
                {
                    Assert.AreEqual(correctPString.Bytes, result);
                }
            }
        });
    }

    [Test]
    public void ParseExactCorrectPIncorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                foreach (string format in UuidFormats.AllExceptP)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctPString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctPString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactIncorrectPCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenPString in UuidTestData.BrokenPStrings)
            {
                foreach (string format in UuidFormats.P)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(brokenPString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(brokenPString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactNotPStringCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                foreach (string format in UuidFormats.P)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctNString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctNString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    #endregion

    #region ParseExactX

    [Test]
    public unsafe void ParseExactCorrectXCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                List<byte[]> results = new();
                foreach (string format in UuidFormats.X)
                {
                    Uuid parsedUuidString = Uuid.ParseExact(correctXString.UuidString, format);
                    Uuid parsedUuidSpan = Uuid.ParseExact(
                        new(correctXString.UuidString.ToCharArray()),
                        new ReadOnlySpan<char>(format.ToCharArray()));

                    byte[] actualBytesString = new byte[16];
                    byte[] actualBytesSpan = new byte[16];
                    fixed (byte* pinnedString = actualBytesString, pinnedSpan = actualBytesSpan)
                    {
                        *(Uuid*) pinnedString = parsedUuidString;
                        *(Uuid*) pinnedSpan = parsedUuidSpan;
                    }

                    results.Add(actualBytesString);
                    results.Add(actualBytesSpan);
                }

                foreach (byte[] result in results)
                {
                    Assert.AreEqual(correctXString.Bytes, result);
                }
            }
        });
    }

    [Test]
    public void ParseExactCorrectXIncorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                foreach (string format in UuidFormats.AllExceptX)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctXString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctXString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactIncorrectXCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenXString in UuidTestData.BrokenXStrings)
            {
                foreach (string format in UuidFormats.X)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(brokenXString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(brokenXString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    [Test]
    public void ParseExactNotXStringCorrectFormat()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                foreach (string format in UuidFormats.X)
                {
                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(correctNString.UuidString, format);
                    });

                    Assert.Throws<FormatException>(() =>
                    {
                        Uuid _ = Uuid.ParseExact(
                            new(correctNString.UuidString.ToCharArray()),
                            new ReadOnlySpan<char>(format.ToCharArray()));
                    });
                }
            }
        });
    }

    #endregion
}
