using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Uuids;

/// <summary>
///     Extension methods for <see cref="Utf8JsonReader" />, that used to work with <see cref="Uuid" /> values.
/// </summary>
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public static class Utf8JsonReaderUuidExtensions
{
    // https://github.com/dotnet/runtime/blob/v7.0.2/src/libraries/System.Text.Json/src/System/Text/Json/ThrowHelper.cs#L14
    private const string ExceptionSourceValueToRethrowAsJsonException = "System.Text.Json.Rethrowable";

    /// <summary>
    ///     Parses the current JSON token value from the source as a <see cref="Uuid" />. Returns the value if the entire UTF-8 encoded token value
    ///     can be successfully parsed to a <see cref="Uuid" /> value. Throws exceptions otherwise.
    /// </summary>
    /// <param name="reader">Instance of <see cref="Utf8JsonReader" />.</param>
    /// <returns></returns>
    /// <exception cref="FormatException">Thrown if the JSON token value is of an unsupported format for a <see cref="Uuid" />.</exception>
    public static Uuid GetUuid(this ref Utf8JsonReader reader)
    {
        if (!reader.TryGetUuid(out Uuid value))
        {
            throw new FormatException("The JSON value is not in a supported Uuid format.")
            {
                Source = ExceptionSourceValueToRethrowAsJsonException
            };
        }

        return value;
    }

    /// <summary>
    ///     Parses the current JSON token value from the source as a <see cref="Uuid" />. Returns <see langword="true" /> if the entire UTF-8
    ///     encoded token value can be successfully parsed to a <see cref="Uuid" /> value. Returns <see langword="false" /> otherwise.
    /// </summary>
    /// <param name="reader">Instance of <see cref="Utf8JsonReader" />.</param>
    /// <param name="value">Output <see cref="Uuid" /> value.</param>
    /// <returns></returns>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static bool TryGetUuid(this ref Utf8JsonReader reader, out Uuid value)
    {
        string? possibleUuidString = reader.GetString();
        if (Uuid.TryParse(possibleUuidString, out value))
        {
            return true;
        }

        value = default;
        return false;
    }
}
