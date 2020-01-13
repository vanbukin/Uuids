# Uuids
Uuid structure like `System.Guid`, but with big-endian string representation

## How to open
  * `git clone git@github.com:vanbukin/Uuids.git`
  * `cd ./Uuids/Uuids.CoreLib && dotnet publish -c Dubug && dotnet publish -c Release` (no IDE support for .ilproj)
  * open `./Uuids/Uuids.sln` with IDE and unload `Uuids.CoreLib` project
  * have fun

## Benchmarks
```
BenchmarkDotNet=v0.12.0, OS=macOS 10.15.2 (19C57) [Darwin 19.2.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
Server=True
```
`ctor(byte[])`

| Method            | Mean          | Error     | StdDev    | Gen 0  | Gen 1 | Gen 2| Allocated|
| ----------------- |:-------------:| ---------:|----------:|-------:|------:|-----:|---------:|
| guid_CtorByteArray| 6.939 ns      | 0.0831 ns | 0.0777 ns | -      | -     | -    | -        |
| uuid_CtorByteArray| 1.759 ns      | 0.0213 ns | 0.0200 ns | -      | -     | -    | -        |

`.ToByteArray()`

| Method            | Mean          | Error     | StdDev    | Gen 0  | Gen 1 | Gen 2| Allocated|
| ----------------- |:-------------:| ---------:|----------:|-------:|------:|-----:|---------:|
| guid_ToByteArray  | 6.993 ns      | 0.2260 ns | 0.2320 ns | 0.0085 | -     | -    | 40 B     |
| uuid_ToByteArray  | 6.751 ns      | 0.1750 ns | 0.1637 ns | 0.0085 | -     | -    | 40 B     |

`.ToString(format)`

| Method            | Mean          | Error     | StdDev    | Gen 0  | Gen 1 | Gen 2| Allocated|
| ----------------- |:-------------:| ---------:|----------:|-------:|------:|-----:|---------:|
| guid_ToString_N   | 47.1792 ns    | 0.3581 ns | 0.3350 ns | 0.0187 | -     | -    | 88 B     |
| uuid_ToString_N   | 17.8747 ns    | 0.2416 ns | 0.2260 ns | 0.0187 | -     | -    | 88 B     |
| guid_ToString_D   | 47.10 ns      | 0.227 ns  | 0.212 ns  | 0.0204 | -     | -    | 96 B     |
| uuid_ToString_D   | 27.15 ns      | 0.176 ns  | 0.156 ns  | 0.0204 | -     | -    | 96 B     |
| guid_ToString_B   | 46.58 ns      | 0.397 ns  | 0.352 ns  | 0.0221 | -     | -    | 104 B    |
| uuid_ToString_B   | 27.71 ns      | 0.209 ns  | 0.185 ns  | 0.0221 | -     | -    | 104 B    |
| guid_ToString_P   | 47.02 ns      | 0.232 ns  | 0.205 ns  | 0.0221 | -     | -    | 104 B    |
| uuid_ToString_P   | 27.64 ns      | 0.251 ns  | 0.235 ns  | 0.0221 | -     | -    | 104 B    |
| guid_ToString_X   | 55.53 ns      | 0.980 ns  | 0.917 ns  | 0.0340 | -     | -    | 160 B    |
| uuid_ToString_X   | 35.16 ns      | 0.598 ns  | 0.530 ns  | 0.0340 | -     | -    | 160 B    |

`.TryParse(format)`

| Method            | Mean          | Error     | StdDev    | Gen 0  | Gen 1 | Gen 2| Allocated|
| ----------------- |:-------------:| ---------:|----------:|-------:|------:|-----:|---------:|
| guid_TryParse_N   | 104.40 ns     | 1.796 ns  | 1.680 ns  | -      | -     | -    | -        |
| uuid_TryParse_N   | 43.89 ns      | 0.867 ns  | 0.811 ns  | -      | -     | -    | -        |
| guid_TryParse_D   | 80.62 ns      | 0.923 ns  | 0.863 ns  | -      | -     | -    | -        |
| uuid_TryParse_D   | 35.87 ns      | 0.701 ns  | 1.171 ns  | -      | -     | -    | -        |
| guid_TryParse_B   | 83.92 ns      | 1.257 ns  | 1.176 ns  | -      | -     | -    | -        |
| uuid_TryParse_B   | 33.67 ns      | 0.651 ns  | 0.775 ns  | -      | -     | -    | -        |
| guid_TryParse_P   | 79.17 ns      | 1.524 ns  | 1.565 ns  | -      | -     | -    | -        |
| uuid_TryParse_P   | 25.81 ns      | 0.507 ns  | 0.497 ns  | -      | -     | -    | -        |
| guid_TryParse_X   | 284.95 ns     | 2.798 ns  | 2.617 ns  | -      | -     | -    | -        |
| uuid_TryParse_X   | 43.72 ns      | 0.472 ns  | 0.442 ns  | -      | -     | -    | -        |

`Generation`

| Method            | Mean          | Error     | StdDev    | Gen 0  | Gen 1 | Gen 2| Allocated|
| ----------------- |:-------------:| ---------:|----------:|-------:|------:|-----:|---------:|
| guid              | 271.4 ns      | 2.93 ns   | 2.74 ns   | -      | -     | -    | -        |
| uuid              | 308.6 ns      | 1.26 ns   | 1.12 ns   | -      | -     | -    | -        |
