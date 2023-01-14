# Uuids

The main goal is [Uuid](./src/Uuids/Uuid.cs) implementation according to the [RFC4122](https://tools.ietf.org/html/rfc4122).

.NET provides [System.Guid](https://docs.microsoft.com/en-us/dotnet/api/system.guid) struct which is special case of the RFC4122 implementation. System.Guid has [little-endian layout](https://github.com/dotnet/runtime/blob/v7.0.0/src/libraries/System.Private.CoreLib/src/System/Guid.cs#L30-L32) for the first 8 bytes (int32, int16, int16).

Project goal is to provide Uuid fully compliant with RFC4122 (big-endian layout) and preserve System.Guid-like behaviour. Also project contains generators to create different Uuid variants. Currently supported variants:

- Time-based (like [Uuid v1](https://tools.ietf.org/html/rfc4122#section-4.1.3)).

  ```csharp
    var uuid = Uuid.NewTimeBased();
  ```

- Time-based, optimized for MySQL.

  ```csharp
    var uuid = Uuid.NewMySqlOptimized();
  ```

  Equals `UUID_TO_BIN(UUID(), 1)` from [MySQL 8.0](https://dev.mysql.com/doc/refman/8.0/en/miscellaneous-functions.html#function_uuid-to-bin)

## Benchmarks

```
BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-JULLCB : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
```

`ctor(byte[])`

| Method             |      Mean |     Error |    StdDev | Allocated |
|--------------------|----------:|----------:|----------:|----------:|
| guid_CtorByteArray | 0.5098 ns | 0.0029 ns | 0.0027 ns |         - |
| uuid_CtorByteArray | 1.1099 ns | 0.0035 ns | 0.0033 ns |         - |

`.ToByteArray()`

| Method           |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------|----------:|----------:|----------:|-------:|----------:|
| guid_ToByteArray | 3.3055 ns | 0.0775 ns | 0.0647 ns | 0.0001 |      40 B |
| uuid_ToByteArray | 3.3236 ns | 0.0989 ns | 0.0926 ns | 0.0001 |      40 B |

`.ToString(format)`

| Method          | Categories |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|-----------------|------------|----------:|----------:|----------:|-------:|----------:|
| guid_ToString_B | ToString_B | 16.390 ns | 0.2053 ns | 0.1920 ns | 0.0002 |     104 B |
| uuid_ToString_B | ToString_B | 15.250 ns | 0.2593 ns | 0.2426 ns | 0.0002 |     104 B |
| guid_ToString_D | ToString_D | 15.223 ns | 0.2475 ns | 0.2315 ns | 0.0001 |      96 B |
| uuid_ToString_D | ToString_D | 12.045 ns | 0.1027 ns | 0.0961 ns | 0.0002 |      96 B |
| guid_ToString_N | ToString_N | 15.945 ns | 0.2685 ns | 0.2511 ns | 0.0001 |      88 B |
| uuid_ToString_N | ToString_N |  7.685 ns | 0.1344 ns | 0.1257 ns | 0.0002 |      88 B |
| guid_ToString_P | ToString_P | 17.264 ns | 0.1461 ns | 0.1367 ns | 0.0002 |     104 B |
| uuid_ToString_P | ToString_P | 14.610 ns | 0.2126 ns | 0.1989 ns | 0.0002 |     104 B |
| guid_ToString_X | ToString_X | 22.444 ns | 0.1768 ns | 0.1653 ns | 0.0003 |     160 B |
| uuid_ToString_X | ToString_X | 16.465 ns | 0.2559 ns | 0.2394 ns | 0.0003 |     160 B |

`.TryParse(format)`

| Method          | Categories |      Mean |     Error |    StdDev |    Median | Allocated |
|-----------------|------------|----------:|----------:|----------:|----------:|----------:|
| guid_TryParse_B | TryParseB  | 21.042 ns | 0.0404 ns | 0.0358 ns | 21.044 ns |         - |
| uuid_TryParse_B | TryParseB  | 10.999 ns | 0.0211 ns | 0.0197 ns | 10.998 ns |         - |
| guid_TryParse_D | TryParseD  | 26.706 ns | 0.0352 ns | 0.0312 ns | 26.703 ns |         - |
| uuid_TryParse_D | TryParseD  | 10.758 ns | 0.0189 ns | 0.0177 ns | 10.758 ns |         - |
| guid_TryParse_N | TryParseN  | 24.615 ns | 0.0403 ns | 0.0377 ns | 24.617 ns |         - |
| uuid_TryParse_N | TryParseN  | 10.024 ns | 0.0236 ns | 0.0221 ns | 10.024 ns |         - |
| guid_TryParse_P | TryParseP  | 27.417 ns | 0.0421 ns | 0.0394 ns | 27.427 ns |         - |
| uuid_TryParse_P | TryParseP  |  7.652 ns | 0.0204 ns | 0.0191 ns |  7.656 ns |         - |
| guid_TryParse_X | TryParseX  | 82.407 ns | 1.6383 ns | 2.3497 ns | 83.912 ns |         - |
| uuid_TryParse_X | TryParseX  | 14.150 ns | 0.0225 ns | 0.0199 ns | 14.154 ns |         - |

`Generation`

| Method                 |   Mean   |    Error |   StdDev | Allocated |
|------------------------|:--------:|---------:|---------:|----------:|
| guid_New               | 28.99 ns | 0.201 ns | 0.188 ns |         - |
| uuid_NewTimeBased      | 58.61 ns | 0.581 ns | 0.544 ns |         - |
| uuid_NewMySqlOptimized | 59.18 ns | 0.499 ns | 0.467 ns |         - |
