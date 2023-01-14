using NUnit.Framework;
using Uuids.Tests.Data;

namespace Uuids.Tests;

public class UuidOperatorsTests
{
    #region ==

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectEqualsToBytesAndResult))]
    public void EqualsOperator(
        byte[] correctBytes,
        byte[] correctEqualsBytes,
        bool expectedResult)
    {
        Uuid uuid = new(correctBytes);
        Uuid otherUuid = new(correctEqualsBytes);

        bool isEquals = uuid == otherUuid;

        Assert.AreEqual(expectedResult, isEquals);
    }

    #endregion

    #region !=

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectEqualsToBytesAndResult))]
    public void NotEqualsOperator(
        byte[] correctBytes,
        byte[] correctEqualsBytes,
        bool notExpectedResult)
    {
        bool expectedResult = !notExpectedResult;
        Uuid uuid = new(correctBytes);
        Uuid otherUuid = new(correctEqualsBytes);

        bool isEquals = uuid != otherUuid;

        Assert.AreEqual(expectedResult, isEquals);
    }

    #endregion

    #region <

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.LeftLessThanRight))]
    public void LessThan_ReturnsTrue_WhenLeftLessThanRight(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left < right;
        Assert.True(isLeftLessThatRight);
    }

    [Test]
    public void LessThan_ReturnsFalse_WhenLeftEqualsToRight()
    {
        string uuidString = "a0b8e3b45fab11eda0f8378e06e839cc";
        Uuid left = new(uuidString);
        Uuid right = new(uuidString);

        bool isLeftLessThatRight = left < right;

        Assert.False(isLeftLessThatRight);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.RightLessThanLeft))]
    public void LessThan_ReturnsFalse_WhenRightLessThanLeft(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left < right;
        Assert.False(isLeftLessThatRight);
    }

    #endregion

    #region <=

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.LeftLessThanRight))]
    public void LessThanOrEqual_ReturnsTrue_WhenLeftLessThanRight(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left <= right;
        Assert.True(isLeftLessThatRight);
    }

    [Test]
    public void LessThanOrEqual_ReturnsTrue_WhenLeftEqualsToRight()
    {
        string uuidString = "a0b8e3b45fab11eda0f8378e06e839cc";
        Uuid left = new(uuidString);
        Uuid right = new(uuidString);

        bool isLeftLessThatRight = left <= right;

        Assert.True(isLeftLessThatRight);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.RightLessThanLeft))]
    public void LessThanOrEqual_ReturnsFalse_WhenRightLessThanLeft(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left <= right;
        Assert.False(isLeftLessThatRight);
    }

    #endregion

    #region >

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.RightLessThanLeft))]
    public void GreaterThan_ReturnsTrue_WhenLeftGreaterThanRight(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left > right;
        Assert.True(isLeftLessThatRight);
    }

    [Test]
    public void GreaterThan_ReturnsFalse_WhenLeftEqualsToRight()
    {
        string uuidString = "a0b8e3b45fab11eda0f8378e06e839cc";
        Uuid left = new(uuidString);
        Uuid right = new(uuidString);

        bool isLeftLessThatRight = left > right;

        Assert.False(isLeftLessThatRight);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.LeftLessThanRight))]
    public void GreaterThan_ReturnsFalse_WhenRightGreaterThanLeft(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left > right;
        Assert.False(isLeftLessThatRight);
    }

    #endregion

    #region >=

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.RightLessThanLeft))]
    public void GreaterThanOrEqual_ReturnsTrue_WhenLeftGreaterThanRight(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left >= right;
        Assert.True(isLeftLessThatRight);
    }

    [Test]
    public void GreaterThanOrEqual_ReturnsFalse_WhenLeftEqualsToRight()
    {
        string uuidString = "a0b8e3b45fab11eda0f8378e06e839cc";
        Uuid left = new(uuidString);
        Uuid right = new(uuidString);

        bool isLeftLessThatRight = left >= right;

        Assert.True(isLeftLessThatRight);
    }

    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.LeftLessThanRight))]
    public void GreaterThanOrEqual_ReturnsFalse_WhenRightGreaterThanLeft(Uuid left, Uuid right)
    {
        bool isLeftLessThatRight = left >= right;
        Assert.False(isLeftLessThatRight);
    }

    #endregion
}
