using System;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class UuidCtorTests
{
    #region Bytes

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public unsafe void CtorFromByteArrayCorrectBytes(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);

        byte[] uuidBytes = new byte[16];
        fixed (byte* pinnedUuidArray = uuidBytes)
        {
            *(Uuid*) pinnedUuidArray = uuid;
        }

        Assert.AreEqual(correctBytes, uuidBytes);
    }

    [Test]
    public void CtorFromByteArrayNullShouldThrows()
    {
#nullable disable
        Assert.Throws<ArgumentNullException>(() =>
        {
            Uuid _ = new((byte[]) null);
        });
#nullable restore
    }

    [Test]
    public void CtorFromByteArrayNot16BytesShouldThrows()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            Uuid _ = new(new byte[]
            {
                1,
                2,
                3
            });
        });
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public unsafe void CtorFromPtrCorrectData(byte[] correctBytes)
    {
        ArgumentNullException.ThrowIfNull(correctBytes);
        byte* bytePtr = stackalloc byte[correctBytes.Length];
        for (int i = 0; i < correctBytes.Length; i++)
        {
            bytePtr[i] = correctBytes[i];
        }

        Uuid uuid = new(bytePtr);

        byte[] uuidBytes = new byte[16];
        fixed (byte* pinnedUuidArray = uuidBytes)
        {
            *(Uuid*) pinnedUuidArray = uuid;
        }

        Assert.AreEqual(correctBytes, uuidBytes);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public unsafe void CtorFromReadOnlySpanCorrectBytes(byte[] correctBytes)
    {
        ReadOnlySpan<byte> span = new(correctBytes);
        Uuid uuid = new(span);

        byte[] uuidBytes = new byte[16];
        fixed (byte* pinnedUuidArray = uuidBytes)
        {
            *(Uuid*) pinnedUuidArray = uuid;
        }

        Assert.AreEqual(correctBytes, uuidBytes);
    }

    [Test]
    public void CtorFromReadOnlySpanNot16BytesShouldThrows()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            ReadOnlySpan<byte> span = new(new byte[]
            {
                1,
                2,
                3
            });
            Uuid _ = new(span);
        });
    }

    #endregion

    #region Chars_Strings

    [Test]
    public void CtorFromStringNullShouldThrows()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
#nullable disable
            // ReSharper disable once RedundantCast
            Uuid _ = new((string) null);
#nullable restore
        });
    }

    [Test]
    public void CtorFromStringEmptyShouldThrows()
    {
        Assert.Throws<FormatException>(() =>
        {
            Uuid _ = new(string.Empty);
        });
    }

    [Test]
    public void CtorFromCharSpanEmptyShouldThrows()
    {
        Assert.Throws<FormatException>(() =>
        {
            Uuid _ = new(new ReadOnlySpan<char>(Array.Empty<char>()));
        });
    }

    #region N

    public unsafe void CtorFromStringCorrectNString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                string nString = correctNString.UuidString;
                byte[] expectedBytes = correctNString.Bytes;

                Uuid parsedUuid = new(nString);

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
    public unsafe void CtorFromCharSpanCorrectN()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctNString in UuidTestData.CorrectNStrings)
            {
                ReadOnlySpan<char> nSpan = new(correctNString.UuidString.ToCharArray());
                byte[] expectedBytes = correctNString.Bytes;

                Uuid parsedUuid = new(nSpan);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    #endregion

    #region D

    [Test]
    public unsafe void CtorFromStringCorrectDString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                string dString = correctDString.UuidString;
                byte[] expectedBytes = correctDString.Bytes;

                Uuid parsedUuid = new(dString);

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
    public unsafe void CtorFromCharSpanCorrectD()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctDString in UuidTestData.CorrectDStrings)
            {
                ReadOnlySpan<char> dSpan = new(correctDString.UuidString.ToCharArray());
                byte[] expectedBytes = correctDString.Bytes;

                Uuid parsedUuid = new(dSpan);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    #endregion

    #region B

    [Test]
    public unsafe void CtorFromStringCorrectBString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                string bString = correctBString.UuidString;
                byte[] expectedBytes = correctBString.Bytes;

                Uuid parsedUuid = new(bString);

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
    public unsafe void CtorFromCharSpanCorrectB()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctBString in UuidTestData.CorrectBStrings)
            {
                ReadOnlySpan<char> bSpan = new(correctBString.UuidString.ToCharArray());
                byte[] expectedBytes = correctBString.Bytes;

                Uuid parsedUuid = new(bSpan);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    #endregion

    #region P

    [Test]
    public unsafe void CtorFromStringCorrectPString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                string pString = correctPString.UuidString;
                byte[] expectedBytes = correctPString.Bytes;

                Uuid parsedUuid = new(pString);

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
    public unsafe void CtorFromCharSpanCorrectP()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctPString in UuidTestData.CorrectPStrings)
            {
                ReadOnlySpan<char> pSpan = new(correctPString.UuidString.ToCharArray());
                byte[] expectedBytes = correctPString.Bytes;

                Uuid parsedUuid = new(pSpan);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    #endregion

    #region X

    [Test]
    public unsafe void CtorFromStringCorrectXString()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                string xString = correctXString.UuidString;
                byte[] expectedBytes = correctXString.Bytes;

                Uuid parsedUuid = new(xString);

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
    public unsafe void CtorFromCharSpanCorrectX()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes correctXString in UuidTestData.CorrectXStrings)
            {
                ReadOnlySpan<char> xSpan = new(correctXString.UuidString.ToCharArray());
                byte[] expectedBytes = correctXString.Bytes;

                Uuid parsedUuid = new(xSpan);

                byte[] actualBytes = new byte[16];
                fixed (byte* pinnedActualBytes = actualBytes)
                {
                    *(Uuid*) pinnedActualBytes = parsedUuid;
                }

                Assert.AreEqual(expectedBytes, actualBytes);
            }
        });
    }

    #endregion

    #endregion
}
