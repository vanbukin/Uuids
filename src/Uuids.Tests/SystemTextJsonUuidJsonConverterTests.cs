using System.IO;
using System.Text;
using System.Text.Json;
using NUnit.Framework;

namespace Uuids.Tests;

public class SystemTextJsonUuidJsonConverterTests
{
    [Test]
    public void ReadCorrect()
    {
        Uuid expectedUuid = new("d0bec403-3323-44df-9dd4-4456121ab00b");
        byte[] data = Encoding.UTF8.GetBytes($"\"{expectedUuid.ToString("N")}\"");
        Utf8JsonReader reader = new(data);
        reader.Read();
        SystemTextJsonUuidJsonConverter converter = new();

#pragma warning disable 8625
        Uuid actualUuid = converter.Read(ref reader, typeof(Uuid), null);
#pragma warning restore 8625

        Assert.AreEqual(expectedUuid, actualUuid);
    }

    [Test]
    public void WriteCorrect()
    {
        string expectedValue = "\"edbe2e116ead4ee7848eaef7bc2ae2d6\"";
        Uuid uuid = new(expectedValue.Trim('"'));
        using MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);
        SystemTextJsonUuidJsonConverter converter = new();

#pragma warning disable 8625
        converter.Write(writer, uuid, null);
#pragma warning restore 8625

        writer.Flush();
        string actualValue = Encoding.UTF8.GetString(stream.ToArray());
        Assert.AreEqual(expectedValue, actualValue);
    }
}
