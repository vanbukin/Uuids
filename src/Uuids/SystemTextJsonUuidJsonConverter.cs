using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Uuids;

/// <summary>
///     System.Text.Json converter for <see cref="Uuid" />.
/// </summary>
public class SystemTextJsonUuidJsonConverter : JsonConverter<Uuid>
{
    /// <inheritdoc />
    public override Uuid Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.GetUuid();
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        Uuid value,
        JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        // Always will be well-formatted, cuz we allocate exact buffer for output format
        Span<char> outputBuffer = stackalloc char[32];
        value.TryFormat(outputBuffer, out _, "N", null);
        writer.WriteStringValue(outputBuffer);
    }
}
