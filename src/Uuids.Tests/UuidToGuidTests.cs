using System;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;
using Uuids.Tests.Utils;

namespace Uuids.Tests;

public class UuidToGuidTests
{
    [Test]
    public void ToGuidByteLayout()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes nString in UuidTestData.CorrectNStrings)
            {
                Uuid uuid = new(nString.Bytes);
                Guid guid = uuid.ToGuidByteLayout();

                byte[] expectedBytes = uuid.ToByteArray();
                byte[] actualBytes = guid.ToByteArray();
                string? expectedGuidString = HexUtils.GetString(new[]
                {
                    nString.Bytes[3],
                    nString.Bytes[2],
                    nString.Bytes[1],
                    nString.Bytes[0],
                    nString.Bytes[5],
                    nString.Bytes[4],
                    nString.Bytes[7],
                    nString.Bytes[6],
                    nString.Bytes[8],
                    nString.Bytes[9],
                    nString.Bytes[10],
                    nString.Bytes[11],
                    nString.Bytes[12],
                    nString.Bytes[13],
                    nString.Bytes[14],
                    nString.Bytes[15]
                });
                string actualGuidString = guid.ToString("N");

                Assert.AreEqual(expectedBytes, actualBytes);
                Assert.True(string.Equals(expectedGuidString, actualGuidString, StringComparison.Ordinal));
            }
        });
    }

    [Test]
    public void ToGuidStringLayout()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidStringWithBytes nString in UuidTestData.CorrectNStrings)
            {
                Uuid uuid = new(nString.Bytes);
                Guid guid = uuid.ToGuidStringLayout();

                byte[] expectedBytes =
                {
                    nString.Bytes[3],
                    nString.Bytes[2],
                    nString.Bytes[1],
                    nString.Bytes[0],
                    nString.Bytes[5],
                    nString.Bytes[4],
                    nString.Bytes[7],
                    nString.Bytes[6],
                    nString.Bytes[8],
                    nString.Bytes[9],
                    nString.Bytes[10],
                    nString.Bytes[11],
                    nString.Bytes[12],
                    nString.Bytes[13],
                    nString.Bytes[14],
                    nString.Bytes[15]
                };
                byte[] actualBytes = guid.ToByteArray();
                string expectedGuidString = uuid.ToString("N");
                string actualGuidString = guid.ToString("N");

                Assert.AreEqual(expectedBytes, actualBytes);
                Assert.True(string.Equals(expectedGuidString, actualGuidString, StringComparison.Ordinal));
            }
        });
    }
}
