using System;
using NUnit.Framework;
using Uuids.Tests.Data;

namespace Uuids.Tests;

public class UuidCompareToTests
{
    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectCompareToArraysAndResult))]
    public void CompareToObjectCorrect(
        byte[] correctBytes,
        byte[] correctCompareToBytes,
        int expectedResult)
    {
        Uuid uuid = new(correctBytes);
        object uuidToCompareAsObject = new Uuid(correctCompareToBytes);

        int compareResult = uuid.CompareTo(uuidToCompareAsObject);

        Assert.AreEqual(expectedResult, compareResult);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void CompareToObjectNullShouldReturn1(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);

        int compareResult = uuid.CompareTo(null);

        Assert.AreEqual(1, compareResult);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void CompareToObjectOtherTypeShouldThrows(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);

        Assert.Throws<ArgumentException>(() =>
        {
            int _ = uuid.CompareTo(1337);
        });
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectCompareToArraysAndResult))]
    public void CompareToUuidCorrect(
        byte[] correctBytes,
        byte[] correctCompareToBytes,
        int expectedResult)
    {
        Uuid uuid = new(correctBytes);
        Uuid uuidToCompareAsObject = new(correctCompareToBytes);

        int compareResult = uuid.CompareTo(uuidToCompareAsObject);

        Assert.AreEqual(expectedResult, compareResult);
    }
}
