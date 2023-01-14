using System;
using Newtonsoft.Json;

namespace Uuids.Tests.Data.Models;

public class UuidStringWithBytes
{
    public UuidStringWithBytes(string uuidString, byte[] bytes)
    {
        UuidString = uuidString ?? throw new ArgumentNullException(nameof(uuidString));
        Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
    }

    public string UuidString { get; }

    public byte[] Bytes { get; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
