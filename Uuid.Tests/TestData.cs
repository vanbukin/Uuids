using System;
using System.Diagnostics.CodeAnalysis;

namespace Uuid.Tests
{
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public static class TestData
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static object[] CorrectUuidBytesArrays { get; } =
        {
            new object[] {new byte[] {10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160}},
            new object[] {new byte[] {163, 167, 252, 114, 206, 122, 17, 233, 128, 237, 0, 13, 58, 17, 37, 233}},
            new object[] {new byte[] {241, 186, 230, 119, 206, 55, 78, 240, 175, 188, 141, 114, 36, 63, 217, 193}},
            new object[] {new byte[] {230, 35, 75, 5, 129, 19, 99, 68, 152, 188, 145, 109, 120, 166, 14, 235}},
            new object[] {new byte[] {255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255}}
        };

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static object[] CorrectUuidD { get; } =
        {
            new object[] {"8ebd8563-8c94-d04b-6a9a-72ace1cf398b"},
            new object[] {"6385bd8e-948c-4bd0-9a6a-ac728b39cfe1"},
            new object[] {"2bbd5967-b4c3-41d7-975e-40e19916c50a"},
            new object[] {"9b9870b4-336f-44e9-a94d-90b5576b73ed"},
            new object[] {"6ee5c06f-d993-49b1-832f-5a313ff59d36"},
            new object[] {"c5b81b2f-d5e5-4bde-bc8d-bd248cf08bad"},
            new object[] {"ff16f891-06a7-41ca-a0d8-8fb87a497abc"},
            new object[] {"17734475-959f-4a2b-93ab-01e8f05e03bf"},
            new object[] {"106ed2da-7b72-4f8e-9f51-59359c077095"},
            new object[] {"1dc5979d-1e45-4d67-8f89-f896a02dfb47"},
            new object[] {"2a4f1fcf-a2fe-4eed-847d-f41ad93e35d5"}
        };

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static object[] CorrectUuidB { get; } =
        {
            new object[] {"{8ebd8563-8c94-d04b-6a9a-72ace1cf398b}"},
            new object[] {"{6385bd8e-948c-4bd0-9a6a-ac728b39cfe1}"},
            new object[] {"{2bbd5967-b4c3-41d7-975e-40e19916c50a}"},
            new object[] {"{9b9870b4-336f-44e9-a94d-90b5576b73ed}"},
            new object[] {"{6ee5c06f-d993-49b1-832f-5a313ff59d36}"},
            new object[] {"{c5b81b2f-d5e5-4bde-bc8d-bd248cf08bad}"},
            new object[] {"{ff16f891-06a7-41ca-a0d8-8fb87a497abc}"},
            new object[] {"{17734475-959f-4a2b-93ab-01e8f05e03bf}"},
            new object[] {"{106ed2da-7b72-4f8e-9f51-59359c077095}"},
            new object[] {"{1dc5979d-1e45-4d67-8f89-f896a02dfb47}"},
            new object[] {"{2a4f1fcf-a2fe-4eed-847d-f41ad93e35d5}"}
        };

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static object[] CorrectUuidP { get; } =
        {
            new object[] {"(8ebd8563-8c94-d04b-6a9a-72ace1cf398b)"},
            new object[] {"(6385bd8e-948c-4bd0-9a6a-ac728b39cfe1)"},
            new object[] {"(2bbd5967-b4c3-41d7-975e-40e19916c50a)"},
            new object[] {"(9b9870b4-336f-44e9-a94d-90b5576b73ed)"},
            new object[] {"(6ee5c06f-d993-49b1-832f-5a313ff59d36)"},
            new object[] {"(c5b81b2f-d5e5-4bde-bc8d-bd248cf08bad)"},
            new object[] {"(ff16f891-06a7-41ca-a0d8-8fb87a497abc)"},
            new object[] {"(17734475-959f-4a2b-93ab-01e8f05e03bf)"},
            new object[] {"(106ed2da-7b72-4f8e-9f51-59359c077095)"},
            new object[] {"(1dc5979d-1e45-4d67-8f89-f896a02dfb47)"},
            new object[] {"(2a4f1fcf-a2fe-4eed-847d-f41ad93e35d5)"}
        };

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static object[] CorrectUuidX { get; } =
        {
            new object[] {"{0x8ebd8563,0x8c94,0xd04b,{0x6a,0x9a,0x72,0xac,0xe1,0xcf,0x39,0x8b}}"},
            new object[] {"{0x6385bd8e,0x948c,0x4bd0,{0x9a,0x6a,0xac,0x72,0x8b,0x39,0xcf,0xe1}}"},
            new object[] {"{0x2bbd5967,0xb4c3,0x41d7,{0x97,0x5e,0x40,0xe1,0x99,0x16,0xc5,0x0a}}"},
            new object[] {"{0x9b9870b4,0x336f,0x44e9,{0xa9,0x4d,0x90,0xb5,0x57,0x6b,0x73,0xed}}"},
            new object[] {"{0x6ee5c06f,0xd993,0x49b1,{0x83,0x2f,0x5a,0x31,0x3f,0xf5,0x9d,0x36}}"},
            new object[] {"{0xc5b81b2f,0xd5e5,0x4bde,{0xbc,0x8d,0xbd,0x24,0x8c,0xf0,0x8b,0xad}}"},
            new object[] {"{0xff16f891,0x06a7,0x41ca,{0xa0,0xd8,0x8f,0xb8,0x7a,0x49,0x7a,0xbc}}"},
            new object[] {"{0x17734475,0x959f,0x4a2b,{0x93,0xab,0x01,0xe8,0xf0,0x5e,0x03,0xbf}}"},
            new object[] {"{0x106ed2da,0x7b72,0x4f8e,{0x9f,0x51,0x59,0x35,0x9c,0x07,0x70,0x95}}"},
            new object[] {"{0x1dc5979d,0x1e45,0x4d67,{0x8f,0x89,0xf8,0x96,0xa0,0x2d,0xfb,0x47}}"},
            new object[] {"{0x2a4f1fcf,0xa2fe,0x4eed,{0x84,0x7d,0xf4,0x1a,0xd9,0x3e,0x35,0xd5}}"}
        };

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static object[] CorrectCompareToArraysAndResult { get; } =
        {
            new object[]
            {
                new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                0
            },
            new object[]
            {
                new byte[] {255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255},
                new byte[] {255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255},
                0
            },
            new object[]
            {
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // uuidBytes       1-1
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // compareToBytes  1-1
                0
            },
            new object[]
            {
                new byte[] {2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // uuidBytes       2-1
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // compareToBytes  1-1
                1
            },
            new object[]
            {
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0}, // uuidBytes       1-2
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // compareToBytes  1-1
                1
            },
            new object[]
            {
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // uuidBytes       1-1
                new byte[] {2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // compareToBytes  2-1
                -1
            },
            new object[]
            {
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0}, // uuidBytes       1-1
                new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0}, // compareToBytes  1-2
                -1
            }
        };

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static object[] IncorrectUuidBytesArraysAndExceptionTypes { get; } =
        {
            new object[]
            {
                new byte[] {10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150}, // 15 bytes
                typeof(ArgumentException)
            },
            new object[]
            {
                new byte[] {163, 167, 252, 114, 206, 122, 17, 233, 128, 237, 0, 13, 58, 17, 37, 233, 255}, // 17 bytes
                typeof(ArgumentException)
            }
        };

        public static object[] CorrectUuidX1 { get; } =
        {
            new object[] {"{0x8ebd8563,0x8c94,0xd04b,{0x6a,0x9a,0x72,0xac,0xe1,0xcf,0x39,0x8b}}"},
            new object[] {"{0x6385bd8e,0x948c,0x4bd0,{0x9a,0x6a,0xac,0x72,0x8b,0x39,0xcf,0xe1}}"},
            new object[] {"{0x2bbd5967,0xb4c3,0x41d7,{0x97,0x5e,0x40,0xe1,0x99,0x16,0xc5,0x0a}}"},
            new object[] {"{0x9b9870b4,0x336f,0x44e9,{0xa9,0x4d,0x90,0xb5,0x57,0x6b,0x73,0xed}}"},
            new object[] {"{0x6ee5c06f,0xd993,0x49b1,{0x83,0x2f,0x5a,0x31,0x3f,0xf5,0x9d,0x36}}"},
            new object[] {"{0xc5b81b2f,0xd5e5,0x4bde,{0xbc,0x8d,0xbd,0x24,0x8c,0xf0,0x8b,0xad}}"},
            new object[] {"{0xff16f891,0x06a7,0x41ca,{0xa0,0xd8,0x8f,0xb8,0x7a,0x49,0x7a,0xbc}}"},
            new object[] {"{0x17734475,0x959f,0x4a2b,{0x93,0xab,0x01,0xe8,0xf0,0x5e,0x03,0xbf}}"},
            new object[] {"{0x106ed2da,0x7b72,0x4f8e,{0x9f,0x51,0x59,0x35,0x9c,0x07,0x70,0x95}}"},
            new object[] {"{0x1dc5979d,0x1e45,0x4d67,{0x8f,0x89,0xf8,0x96,0xa0,0x2d,0xfb,0x47}}"},
            new object[] {"{0x2a4f1fcf,0xa2fe,0x4eed,{0x84,0x7d,0xf4,0x1a,0xd9,0x3e,0x35,0xd5}}"}
        };
    }
}