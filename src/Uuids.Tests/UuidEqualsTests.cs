using NUnit.Framework;
using Uuids.Tests.Data;

namespace Uuids.Tests;

public class UuidEqualsTests
{
    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void EqualsWithObjectNullReturnFalse(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);

        bool isEquals = uuid.Equals(null);

        Assert.False(isEquals);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void EqualsWithObjectOtherTypeReturnFalse(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);
        object objectWithAnotherType = 42;

        bool isEquals = uuid.Equals(objectWithAnotherType);

        Assert.False(isEquals);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectEqualsToBytesAndResult))]
    public void EqualsWithObjectUuid(
        byte[] correctBytes,
        byte[] correctEqualsBytes,
        bool expectedResult)
    {
        Uuid uuid = new(correctBytes);
        object objectUuid = new Uuid(correctEqualsBytes);

        bool isEquals = uuid.Equals(objectUuid);

        Assert.AreEqual(expectedResult, isEquals);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectEqualsToBytesAndResult))]
    public void EqualsWithOtherUuid(
        byte[] correctBytes,
        byte[] correctEqualsBytes,
        bool expectedResult)
    {
        Uuid uuid = new(correctBytes);
        Uuid otherUuid = new(correctEqualsBytes);

        bool isEquals = uuid.Equals(otherUuid);

        Assert.AreEqual(expectedResult, isEquals);
    }
}
