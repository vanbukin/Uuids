using System;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Uuids.Tests.Utils.Hex;

public class HexUtilsTests
{
    #region IsHexString

    [TestCaseSource(typeof(HexUtilsTestsData), nameof(HexUtilsTestsData.ValidHexStrings))]
    public void IsHexStringCorrect(string validHexString)
    {
        Assert.True(HexUtils.IsHexString(validHexString));
    }

    [Test]
    public void IsHexStringReturnFalseWhenNull()
    {
        Assert.False(HexUtils.IsHexString(null));
    }

    [Test]
    public void IsHexStringReturnFalseWhenOddStringLength()
    {
        Assert.False(HexUtils.IsHexString("F"));
    }

    [TestCaseSource(typeof(HexUtilsTestsData), nameof(HexUtilsTestsData.BrokenHexStrings))]
    public void IsHexStringReturnFalseWithBrokenStrings(string brokenHexString)
    {
        Assert.False(HexUtils.IsHexString(brokenHexString));
    }

    #endregion

    #region GetBytes

    [TestCaseSource(typeof(HexUtilsTestsData), nameof(HexUtilsTestsData.ValidHexStrings))]
    public void CanGetBytesCorrect(string validHexString)
    {
        ArgumentNullException.ThrowIfNull(validHexString);
        byte[] expected = HexStringToByteArrayNaive(validHexString);
        byte[]? actual = HexUtils.GetBytes(validHexString);
        Assert.Multiple(() =>
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            Assert.True(expected!.Length == actual!.Length);
            Assert.True(expected.SequenceEqual(actual));
        });
    }

    [Test]
    public void CanGetBytesReturnNullWhenNull()
    {
        Assert.Null(HexUtils.GetBytes(null!));
    }

    [Test]
    public void CanGetBytesReturnNullWhenOddStringLength()
    {
        Assert.Null(HexUtils.GetBytes("fff"));
    }

    [TestCaseSource(typeof(HexUtilsTestsData), nameof(HexUtilsTestsData.BrokenHexStrings))]
    public void CanGetBytesReturnNullWithBrokenStrings(string brokenHexString)
    {
        Assert.Null(HexUtils.GetBytes(brokenHexString));
    }

    #endregion

    #region GetString

    [TestCaseSource(typeof(HexUtilsTestsData), nameof(HexUtilsTestsData.ValidByteArrays))]
    public void CanGetStringCorrect(byte[] bytesToHex)
    {
        ArgumentNullException.ThrowIfNull(bytesToHex);
        string expected = ByteArrayToHexStringNaive(bytesToHex);
        string? actual = HexUtils.GetString(bytesToHex);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void CanGetStringReturnNullWhenArrayIsNull()
    {
        Assert.Null(HexUtils.GetString(null!));
    }

    [Test]
    public void CanGetStringReturnEmptyStringWhenArrayIsEmpty()
    {
        Assert.AreEqual(string.Empty, HexUtils.GetString(Array.Empty<byte>()));
    }

    #endregion

    #region Utils

    private static byte[] HexStringToByteArrayNaive(string hex)
    {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    private static string ByteArrayToHexStringNaive(byte[] bytes)
    {
        StringBuilder builder = new(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            builder.AppendFormat("{0:x2}", b);
        }

        return builder.ToString();
    }

    #endregion
}
