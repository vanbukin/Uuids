using System;

namespace Uuids.Tests.Data.Models;

public class UuidBytesWithUtf8Bytes
{
    public UuidBytesWithUtf8Bytes(byte[] uuidBytes, string utf8String)
    {
        UuidBytes = uuidBytes ?? throw new ArgumentNullException(nameof(uuidBytes));
        Utf8String = utf8String ?? throw new ArgumentNullException(nameof(utf8String));
    }

    public byte[] UuidBytes { get; }

    public string Utf8String { get; }
}
