using System;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Utils;

namespace Uuids.Tests;

public class UuidToStringTests
{
    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToString(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);

        string actualString = uuid.ToString();

        Assert.AreEqual(expectedString, actualString);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringNullFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);

        string actualString = uuid.ToString(null);

        Assert.AreEqual(expectedString, actualString);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringEmptyFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);

        string actualString = uuid.ToString(string.Empty);

        Assert.AreEqual(expectedString, actualString);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringIncorrectFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);

        Assert.Throws<FormatException>(() =>
        {
            string _ = uuid.ToString("Ъ");
        });
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringTooLongFormat(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);

        Assert.Throws<FormatException>(() =>
        {
            string _ = uuid.ToString("NN");
        });
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringN(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringN(correctBytes);

        string actualString = uuid.ToString("N");

        Assert.AreEqual(expectedString, actualString);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringD(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringD(correctBytes);

        string actualString = uuid.ToString("D");

        Assert.AreEqual(expectedString, actualString);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringB(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringB(correctBytes);

        string actualString = uuid.ToString("B");

        Assert.AreEqual(expectedString, actualString);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringP(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringP(correctBytes);

        string actualString = uuid.ToString("P");

        Assert.AreEqual(expectedString, actualString);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToStringX(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        string expectedString = UuidTestsUtils.GetStringX(correctBytes);

        string actualString = uuid.ToString("X");

        Assert.AreEqual(expectedString, actualString);
    }
}
