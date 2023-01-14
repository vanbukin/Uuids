using NUnit.Framework;
using Uuids.Tests.Data;

namespace Uuids.Tests;

public class UuidToByteArrayTests
{
    [TestCaseSource(typeof(UuidTestData), nameof(UuidTestData.CorrectUuidBytesArrays))]
    public void ToByteArray(byte[] correctBytes)
    {
        Uuid uuid = new(correctBytes);

        byte[] uuidBytes = uuid.ToByteArray();

        Assert.AreEqual(correctBytes, uuidBytes);
    }
}
