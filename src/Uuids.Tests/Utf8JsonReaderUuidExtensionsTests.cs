using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests;

public class Utf8JsonReaderUuidExtensionsTests
{
    private const int MaxExpansionFactorWhileEscaping = 6;
    private const int MaximumFormatUuidLength = 68;
    private const int MaximumEscapedUuidLength = MaxExpansionFactorWhileEscaping * MaximumFormatUuidLength;

    [Test]
    public void GetUuidTokenTypeStringButNotUuidThrowsFormatException()
    {
        FormatException? ex = Assert.Throws<FormatException>(() =>
        {
            byte[] data = Encoding.UTF8.GetBytes("{ \"value\": \"hsadgfhygsdaf\" }");
            Utf8JsonReader reader = new(data);
            reader.Read(); // StartObject
            reader.Read(); // "value"
            reader.Read(); // "hsadgfhygsdaf"
            reader.GetUuid();
        });
        Assert.AreEqual("System.Text.Json.Rethrowable", ex?.Source);
    }

    [Test]
    public void TryGetUuidTokenTypeStartObjectThrowsInvalidOperationException()
    {
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(() =>
        {
            byte[] data = Encoding.UTF8.GetBytes("{ \"value\": 42 }");
            Utf8JsonReader reader = new(data);
            reader.Read();
            reader.TryGetUuid(out _);
        });
        Assert.AreEqual("System.Text.Json.Rethrowable", ex?.Source);
    }

    [Test]
    public void TryGetUuidTooLongInputStringWithValueSequenceNotParsed()
    {
        StringBuilder longUnescapedJsonStringBuilder = new(MaximumEscapedUuidLength + 1);
        for (int i = 0; i < MaximumEscapedUuidLength + 1; i++)
        {
            longUnescapedJsonStringBuilder.Append('0');
        }

        string longUnescapedJsonString = longUnescapedJsonStringBuilder.ToString();
        string json = "{\"value\":\"" + longUnescapedJsonString + "\"}";
        byte[] utf8JsonBytes = Encoding.UTF8.GetBytes(json);
        (MemorySegment head, MemorySegment tail) = SplitByteArrayIntoSegments(utf8JsonBytes, 13);
        ReadOnlySequence<byte> sequence = new(head, 0, tail, tail.Memory.Length);
        Utf8JsonReader reader = new(sequence);
        reader.Read(); // StartObject
        reader.Read(); // "value"
        reader.Read(); // UuidTooLongEscapedValue

        Assert.False(reader.TryGetUuid(out Uuid actualUuid));
        Assert.AreEqual(Uuid.Empty, actualUuid);
    }

    [Test]
    public void TryGetUuidCorrectUnescapedStringNoValueSequence()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidBytesWithUtf8Bytes correctUtf8String in Utf8JsonTestData.CorrectUtf8UnescapedStrings)
            {
                Uuid expectedUuid = new(correctUtf8String.UuidBytes);
                string json = "{\"value\":\"" + correctUtf8String.Utf8String + "\"}";
                byte[] utf8JsonBytes = Encoding.UTF8.GetBytes(json);
                Utf8JsonReader reader = new(utf8JsonBytes);
                reader.Read(); // StartObject
                reader.Read(); // "value"
                reader.Read(); // UuidEscapedValue

                Assert.True(reader.TryGetUuid(out Uuid actualUuid));
                Assert.AreEqual(expectedUuid, actualUuid);
            }
        });
    }

    [Test]
    public void TryGetUuidCorrectEscapedStringNoValueSequence()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidBytesWithUtf8Bytes correctUtf8EscapedString in Utf8JsonTestData.CorrectUtf8EscapedStrings)
            {
                Uuid expectedUuid = new(correctUtf8EscapedString.UuidBytes);
                string json = "{\"value\":\"" + correctUtf8EscapedString.Utf8String + "\"}";
                byte[] utf8JsonBytes = Encoding.UTF8.GetBytes(json);
                Utf8JsonReader reader = new(utf8JsonBytes);
                reader.Read(); // StartObject
                reader.Read(); // "value"
                reader.Read(); // UuidEscapedValue

                Assert.True(reader.TryGetUuid(out Uuid actualUuid));
                Assert.AreEqual(expectedUuid, actualUuid);
            }
        });
    }

    [Test]
    public void TryGetUuidCorrectUnescapedStringWithValueSequence()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidBytesWithUtf8Bytes correctUtf8String in Utf8JsonTestData.CorrectUtf8UnescapedStrings)
            {
                Uuid expectedUuid = new(correctUtf8String.UuidBytes);
                string json = "{\"value\":\"" + correctUtf8String.Utf8String + "\"}";
                byte[] utf8JsonBytes = Encoding.UTF8.GetBytes(json);
                (MemorySegment head, MemorySegment tail) = SplitByteArrayIntoSegments(utf8JsonBytes, 13);
                ReadOnlySequence<byte> sequence = new(head, 0, tail, tail.Memory.Length);
                Utf8JsonReader reader = new(sequence);
                reader.Read(); // StartObject
                reader.Read(); // "value"
                reader.Read(); // UuidEscapedValue

                Assert.True(reader.TryGetUuid(out Uuid actualUuid));
                Assert.AreEqual(expectedUuid, actualUuid);
            }
        });
    }

    [Test]
    public void TryGetUuidCorrectEscapedStringWithValueSequence()
    {
        Assert.Multiple(() =>
        {
            foreach (UuidBytesWithUtf8Bytes correctUtf8EscapedString in Utf8JsonTestData.CorrectUtf8EscapedStrings)
            {
                Uuid expectedUuid = new(correctUtf8EscapedString.UuidBytes);
                string json = "{\"value\":\"" + correctUtf8EscapedString.Utf8String + "\"}";
                byte[] utf8JsonBytes = Encoding.UTF8.GetBytes(json);
                (MemorySegment head, MemorySegment tail) = SplitByteArrayIntoSegments(utf8JsonBytes, 13);
                ReadOnlySequence<byte> sequence = new(head, 0, tail, tail.Memory.Length);
                Utf8JsonReader reader = new(sequence);
                reader.Read(); // StartObject
                reader.Read(); // "value"
                reader.Read(); // UuidEscapedValue

                Assert.True(reader.TryGetUuid(out Uuid actualUuid));
                Assert.AreEqual(expectedUuid, actualUuid);
            }
        });
    }

    private static (MemorySegment head, MemorySegment tail) SplitByteArrayIntoSegments(byte[] bytes, int splitBy)
    {
        int parts = bytes.Length / splitBy;
        if (parts * splitBy < bytes.Length)
        {
            parts += 1;
        }

        MemorySegment head = new(new());
        MemorySegment tail = head;
        for (int i = 0; i < parts; i++)
        {
            int offset = i * splitBy;
            int memoryLength = offset + splitBy > bytes.Length
                ? splitBy - (offset + splitBy - bytes.Length)
                : splitBy;

            ReadOnlyMemory<byte> memory = new(bytes, offset, memoryLength);
            tail = tail.Append(memory);
        }

        return (head, tail);
    }

    [Test]
    public void TryGetUuidTooLongEscapedStringAfterUnescapeNotParsed()
    {
        string escapedString = Utf8JsonTestData.ToUtf8EscapedString(new("f91c971cf7ab404e9a24546b133533dd"), "N");
        int charsToAppend = MaximumEscapedUuidLength - escapedString.Length;
        StringBuilder longUnescapedJsonStringBuilder = new(escapedString, MaximumEscapedUuidLength);
        for (int i = 0; i < charsToAppend; i++)
        {
            longUnescapedJsonStringBuilder.Append('0');
        }

        string longUnescapedJsonString = longUnescapedJsonStringBuilder.ToString();
        string json = "{\"value\":\"" + longUnescapedJsonString + "\"}";
        byte[] utf8JsonBytes = Encoding.UTF8.GetBytes(json);
        Utf8JsonReader reader = new(utf8JsonBytes);
        reader.Read(); // StartObject
        reader.Read(); // "value"
        reader.Read(); // UuidTooLongEscapedValue

        Assert.False(reader.TryGetUuid(out Uuid actualUuid));
        Assert.AreEqual(Uuid.Empty, actualUuid);
    }

    [Test]
    public void TryGetUuidTooLongInputStringNotParsed()
    {
        StringBuilder longUnescapedJsonStringBuilder = new(MaximumEscapedUuidLength + 1);
        for (int i = 0; i < MaximumEscapedUuidLength + 1; i++)
        {
            longUnescapedJsonStringBuilder.Append('0');
        }

        string longUnescapedJsonString = longUnescapedJsonStringBuilder.ToString();
        string json = "{\"value\":\"" + longUnescapedJsonString + "\"}";
        byte[] utf8JsonBytes = Encoding.UTF8.GetBytes(json);
        Utf8JsonReader reader = new(utf8JsonBytes);
        reader.Read(); // StartObject
        reader.Read(); // "value"
        reader.Read(); // UuidTooLongEscapedValue

        Assert.False(reader.TryGetUuid(out Uuid actualUuid));
        Assert.AreEqual(Uuid.Empty, actualUuid);
    }

    internal sealed class MemorySegment : ReadOnlySequenceSegment<byte>
    {
        public MemorySegment(ReadOnlyMemory<byte> memory)
        {
            Memory = memory;
        }

        public MemorySegment Append(ReadOnlyMemory<byte> memory)
        {
            MemorySegment segment = new(memory)
            {
                RunningIndex = RunningIndex + Memory.Length
            };
            Next = segment;
            return segment;
        }
    }
}
