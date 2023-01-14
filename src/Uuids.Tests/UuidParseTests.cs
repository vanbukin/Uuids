using System;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class UuidParseTests
{
    private const string? NullString = null;

    [Test]
    public void ParseNullStringShouldThrows()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
#nullable disable
            Uuid _ = Uuid.Parse(NullString!);
#nullable restore
        });
    }

    [Test]
    public void ParseEmptyStringShouldThrows()
    {
        Assert.Throws<FormatException>(() =>
        {
            Uuid _ = Uuid.Parse(string.Empty);
        });
    }

    [Test]
    public void ParseEmptySpanShouldThrows()
    {
        Assert.Throws<FormatException>(() =>
        {
            Uuid _ = Uuid.Parse(new ReadOnlySpan<char>(Array.Empty<char>()));
        });
    }

    #region ParseN

    [Test]
    public unsafe void ParseCorrectNString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                string nString = correctNString.UuidString;
                byte[] expectedBytes = correctNString.Bytes;

                Uuid parsedUuid = Uuid.Parse(nString);

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
    public unsafe void ParseCorrectNSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                ReadOnlySpan<char> nSpan = new(correctNString.UuidString.ToCharArray());
                byte[] expectedBytes = correctNString.Bytes;

                Uuid parsedUuid = Uuid.Parse(nSpan);

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
    public void ParseNIncorrectLargeString()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeNString in UuidTestData.LargeNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeNString);
                });
            }
        });
    }

    [Test]
    public void ParseNIncorrectLargeSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeNString in UuidTestData.LargeNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeNSpan = new(largeNString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeNSpan);
                });
            }
        });
    }

    [Test]
    public void ParseNIncorrectSmallString()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallNString in UuidTestData.SmallNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallNString);
                });
            }
        });
    }

    [Test]
    public void ParseNIncorrectSmallSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallNString in UuidTestData.SmallNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallNSpan = new(smallNString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallNSpan);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectNString()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenNString in UuidTestData.BrokenNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenNString);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectNSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenNString in UuidTestData.BrokenNStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenNSpan = new(brokenNString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenNSpan);
                });
            }
        });
    }

    #endregion

    #region ParseD

    [Test]
    public unsafe void ParseCorrectDString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                string dString = correctDString.UuidString;
                byte[] expectedBytes = correctDString.Bytes;

                Uuid parsedUuid = Uuid.Parse(dString);

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
    public unsafe void ParseCorrectDSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                ReadOnlySpan<char> dSpan = new(correctDString.UuidString.ToCharArray());
                byte[] expectedBytes = correctDString.Bytes;

                Uuid parsedUuid = Uuid.Parse(dSpan);

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
    public void ParseDIncorrectLargeString()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeDString in UuidTestData.LargeDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeDString);
                });
            }
        });
    }

    [Test]
    public void ParseDIncorrectLargeSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeDString in UuidTestData.LargeDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeDSpan = new(largeDString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeDSpan);
                });
            }
        });
    }

    [Test]
    public void ParseDIncorrectSmallString()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallDString in UuidTestData.SmallDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallDString);
                });
            }
        });
    }

    [Test]
    public void ParseDIncorrectSmallSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallDString in UuidTestData.SmallDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallDSpan = new(smallDString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallDSpan);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectDString()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenDString in UuidTestData.BrokenDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenDString);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectDSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenDString in UuidTestData.BrokenDStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenDSpan = new(brokenDString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenDSpan);
                });
            }
        });
    }

    #endregion

    #region ParseB

    [Test]
    public unsafe void ParseCorrectBString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                string bString = correctBString.UuidString;
                byte[] expectedBytes = correctBString.Bytes;

                Uuid parsedUuid = Uuid.Parse(bString);

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
    public unsafe void ParseCorrectBSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                ReadOnlySpan<char> bSpan = new(correctBString.UuidString.ToCharArray());
                byte[] expectedBytes = correctBString.Bytes;

                Uuid parsedUuid = Uuid.Parse(bSpan);

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
    public void ParseBIncorrectLargeString()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeBString in UuidTestData.LargeBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeBString);
                });
            }
        });
    }

    [Test]
    public void ParseBIncorrectLargeSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeBString in UuidTestData.LargeBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeBSpan = new(largeBString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeBSpan);
                });
            }
        });
    }

    [Test]
    public void ParseBIncorrectSmallString()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallBString in UuidTestData.SmallBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallBString);
                });
            }
        });
    }

    [Test]
    public void ParseBIncorrectSmallSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallBString in UuidTestData.SmallBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallBSpan = new(smallBString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallBSpan);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectBString()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenBString in UuidTestData.BrokenBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenBString);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectBSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenBString in UuidTestData.BrokenBStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenBSpan = new(brokenBString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenBSpan);
                });
            }
        });
    }

    #endregion

    #region ParseP

    [Test]
    public unsafe void ParseCorrectPString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                string pString = correctPString.UuidString;
                byte[] expectedBytes = correctPString.Bytes;

                Uuid parsedUuid = Uuid.Parse(pString);

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
    public unsafe void ParseCorrectPSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                ReadOnlySpan<char> pSpan = new(correctPString.UuidString.ToCharArray());
                byte[] expectedBytes = correctPString.Bytes;

                Uuid parsedUuid = Uuid.Parse(pSpan);

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
    public void ParsePIncorrectLargeString()
    {
        Assert.Multiple(() =>
        {
            foreach (string largePString in UuidTestData.LargePStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largePString);
                });
            }
        });
    }

    [Test]
    public void ParsePIncorrectLargeSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string largePString in UuidTestData.LargePStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largePSpan = new(largePString.ToCharArray());
                    Uuid _ = Uuid.Parse(largePSpan);
                });
            }
        });
    }

    [Test]
    public void ParsePIncorrectSmallString()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallPString in UuidTestData.SmallPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallPString);
                });
            }
        });
    }

    [Test]
    public void ParsePIncorrectSmallSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallPString in UuidTestData.SmallPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallPSpan = new(smallPString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallPSpan);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectPString()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenPString in UuidTestData.BrokenPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenPString);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectPSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenPString in UuidTestData.BrokenPStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> brokenPSpan = new(brokenPString.ToCharArray());
                    Uuid _ = Uuid.Parse(brokenPSpan);
                });
            }
        });
    }

    #endregion

    #region ParseX

    [Test]
    public unsafe void ParseCorrectXString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                string xString = correctXString.UuidString;
                byte[] expectedBytes = correctXString.Bytes;

                Uuid parsedUuid = Uuid.Parse(xString);

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
    public unsafe void ParseCorrectXSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                ReadOnlySpan<char> xSpan = new(correctXString.UuidString.ToCharArray());
                byte[] expectedBytes = correctXString.Bytes;

                Uuid parsedUuid = Uuid.Parse(xSpan);

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
    public void ParseXIncorrectLargeString()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeXString in UuidTestData.LargeXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(largeXString);
                });
            }
        });
    }

    [Test]
    public void ParseXIncorrectLargeSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string largeXString in UuidTestData.LargeXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> largeXSpan = new(largeXString.ToCharArray());
                    Uuid _ = Uuid.Parse(largeXSpan);
                });
            }
        });
    }

    [Test]
    public void ParseXIncorrectSmallString()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallXString in UuidTestData.SmallXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(smallXString);
                });
            }
        });
    }

    [Test]
    public void ParseXIncorrectSmallSpan()
    {
        Assert.Multiple(() =>
        {
            foreach (string smallXString in UuidTestData.SmallXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> smallXSpan = new(smallXString.ToCharArray());
                    Uuid _ = Uuid.Parse(smallXSpan);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectXString()
    {
        Assert.Multiple(() =>
        {
            foreach (string brokenXString in UuidTestData.BrokenXStrings)
            {
                Assert.Throws<FormatException>(() =>
                {
                    Uuid _ = Uuid.Parse(brokenXString);
                });
            }
        });
    }

    [Test]
    public void ParseIncorrectXSpan()
    {
        foreach (string brokenXString in UuidTestData.BrokenXStrings)
        {
            Assert.Throws<FormatException>(() =>
            {
                ReadOnlySpan<char> brokenXSpan = new(brokenXString.ToCharArray());
                Uuid _ = Uuid.Parse(brokenXSpan);
            });
        }
    }

    #endregion
}
