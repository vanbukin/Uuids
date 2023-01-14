using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class UuidParseWithFormatProviderTests
{
    private const string? NullString = null;

    [SuppressMessage("ReSharper", "RedundantCast")]
    [SuppressMessage("Design", "CA1024:Use properties where appropriate")]
    public static IEnumerable GetFormatProviders()
    {
        yield return (IFormatProvider?) CultureInfo.InvariantCulture;
        yield return (IFormatProvider?) new CultureInfo("en-US");
        yield return (IFormatProvider?) null!;
    }

    [Test]
    public void ParseNullStringShouldThrows([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
#nullable disable
            Uuid _ = Uuid.Parse(NullString!, formatProvider);
#nullable restore
        });
    }

    [Test]
    public void ParseEmptyStringShouldThrows([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Throws<FormatException>(() =>
        {
            Uuid _ = Uuid.Parse(string.Empty, formatProvider);
        });
    }

    [Test]
    public void ParseEmptySpanShouldThrows([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Throws<FormatException>(() =>
        {
            Uuid _ = Uuid.Parse(new ReadOnlySpan<char>(Array.Empty<char>()), formatProvider);
        });
    }

    #region ParseN

    [Test]
    public unsafe void ParseCorrectNString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                string nString = correctNString.UuidString;
                byte[] expectedBytes = correctNString.Bytes;

                Uuid parsedUuid = Uuid.Parse(nString, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public unsafe void ParseCorrectNSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                ReadOnlySpan<char> nSpan = new(correctNString.UuidString.ToCharArray());
                byte[] expectedBytes = correctNString.Bytes;

                Uuid parsedUuid = Uuid.Parse(nSpan, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public void ParseNIncorrectLargeString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeNString in UuidTestData.LargeNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeNString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseNIncorrectLargeSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeNString in UuidTestData.LargeNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeNSpan = new(largeNString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeNSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseNIncorrectSmallString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallNString in UuidTestData.SmallNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallNString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseNIncorrectSmallSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallNString in UuidTestData.SmallNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallNSpan = new(smallNString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallNSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectNString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenNString in UuidTestData.BrokenNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenNString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectNSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenNString in UuidTestData.BrokenNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenNSpan = new(brokenNString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenNSpan, formatProvider);
                });
            }
        });
    }

    #endregion

    #region ParseD

    [Test]
    public unsafe void ParseCorrectDString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                string dString = correctDString.UuidString;
                byte[] expectedBytes = correctDString.Bytes;

                Uuid parsedUuid = Uuid.Parse(dString, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public unsafe void ParseCorrectDSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                ReadOnlySpan<char> dSpan = new(correctDString.UuidString.ToCharArray());
                byte[] expectedBytes = correctDString.Bytes;

                Uuid parsedUuid = Uuid.Parse(dSpan, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public void ParseDIncorrectLargeString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeDString in UuidTestData.LargeDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeDString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseDIncorrectLargeSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeDString in UuidTestData.LargeDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeDSpan = new(largeDString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeDSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseDIncorrectSmallString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallDString in UuidTestData.SmallDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallDString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseDIncorrectSmallSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallDString in UuidTestData.SmallDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallDSpan = new(smallDString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallDSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectDString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenDString in UuidTestData.BrokenDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenDString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectDSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenDString in UuidTestData.BrokenDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenDSpan = new(brokenDString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenDSpan, formatProvider);
                });
            }
        });
    }

    #endregion

    #region ParseB

    [Test]
    public unsafe void ParseCorrectBString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                string bString = correctBString.UuidString;
                byte[] expectedBytes = correctBString.Bytes;

                Uuid parsedUuid = Uuid.Parse(bString, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public unsafe void ParseCorrectBSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                ReadOnlySpan<char> bSpan = new(correctBString.UuidString.ToCharArray());
                byte[] expectedBytes = correctBString.Bytes;

                Uuid parsedUuid = Uuid.Parse(bSpan, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public void ParseBIncorrectLargeString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeBString in UuidTestData.LargeBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeBString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseBIncorrectLargeSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeBString in UuidTestData.LargeBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeBSpan = new(largeBString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeBSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseBIncorrectSmallString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallBString in UuidTestData.SmallBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallBString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseBIncorrectSmallSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallBString in UuidTestData.SmallBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallBSpan = new(smallBString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallBSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectBString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenBString in UuidTestData.BrokenBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenBString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectBSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenBString in UuidTestData.BrokenBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenBSpan = new(brokenBString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenBSpan, formatProvider);
                });
            }
        });
    }

    #endregion

    #region ParseP

    [Test]
    public unsafe void ParseCorrectPString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                string pString = correctPString.UuidString;
                byte[] expectedBytes = correctPString.Bytes;

                Uuid parsedUuid = Uuid.Parse(pString, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public unsafe void ParseCorrectPSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                ReadOnlySpan<char> pSpan = new(correctPString.UuidString.ToCharArray());
                byte[] expectedBytes = correctPString.Bytes;

                Uuid parsedUuid = Uuid.Parse(pSpan, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public void ParsePIncorrectLargeString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largePString in UuidTestData.LargePStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largePString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParsePIncorrectLargeSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largePString in UuidTestData.LargePStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largePSpan = new(largePString.ToCharArray());
                    Uuid _ = Uuid.Parse(largePSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParsePIncorrectSmallString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallPString in UuidTestData.SmallPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallPString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParsePIncorrectSmallSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallPString in UuidTestData.SmallPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallPSpan = new(smallPString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallPSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectPString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenPString in UuidTestData.BrokenPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenPString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectPSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenPString in UuidTestData.BrokenPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenPSpan = new(brokenPString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenPSpan, formatProvider);
                });
            }
        });
    }

    #endregion

    #region ParseX

    [Test]
    public unsafe void ParseCorrectXString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                string xString = correctXString.UuidString;
                byte[] expectedBytes = correctXString.Bytes;

                Uuid parsedUuid = Uuid.Parse(xString, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public unsafe void ParseCorrectXSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                ReadOnlySpan<char> xSpan = new(correctXString.UuidString.ToCharArray());
                byte[] expectedBytes = correctXString.Bytes;

                Uuid parsedUuid = Uuid.Parse(xSpan, formatProvider);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    [Test]
    public void ParseXIncorrectLargeString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeXString in UuidTestData.LargeXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeXString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseXIncorrectLargeSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string largeXString in UuidTestData.LargeXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeXSpan = new(largeXString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeXSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseXIncorrectSmallString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallXString in UuidTestData.SmallXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallXString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseXIncorrectSmallSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string smallXString in UuidTestData.SmallXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallXSpan = new(smallXString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallXSpan, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectXString([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenXString in UuidTestData.BrokenXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenXString, formatProvider);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectXSpan([ValueSource(nameof(GetFormatProviders))] IFormatProvider formatProvider)
    {
        foreach (string brokenXString in UuidTestData.BrokenXStrings)
        {
            Assert.Throws<FormatException>(() =>
            {
                ReadOnlySpan<char> brokenXSpan = new(brokenXString.ToCharArray());
                Uuid _ = Uuid.Parse(brokenXSpan, formatProvider);
            });
        }
    }

    #endregion
}
