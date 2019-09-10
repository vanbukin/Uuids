using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NUnit.Framework;

namespace Uuid.Tests
{
    [SuppressMessage("ReSharper", "HeapView.ClosureAllocation")]
    [SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    public class UuidLikeGuidBehavior
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

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public unsafe void Ctor_From_ByteArray_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                var guidArray = new byte[16];
                fixed (byte* pinnedGuidArray = guidArray)
                {
                    *(Guid*) pinnedGuidArray = guid;
                }

                Assert.AreEqual(guidArray, uuidArray);
            });
        }

        [TestCaseSource(nameof(IncorrectUuidBytesArraysAndExceptionTypes))]
        public void Ctor_From_ByteArray_Incorrect_SameAsGuid(byte[] incorrectBytes, Type exceptionType)
        {
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(incorrectBytes);
                Assert.IsNotNull(exceptionType);
                Assert.IsTrue(typeof(Exception).IsAssignableFrom(exceptionType));
                Assert.Throws(exceptionType, () =>
                {
                    var _ = new Guid(incorrectBytes);
                });
                Assert.Throws(exceptionType, () =>
                {
                    var _ = new Uuid(incorrectBytes);
                });
            });
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void Ctor_From_ByteArray_Null_SameAsGuid()
        {
            byte[] bytes = null;

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var _ = new Guid(bytes);
                });
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var _ = new Uuid(bytes);
                });
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public unsafe void Ctor_From_ReadOnlySpan_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var readOnlySpan = new ReadOnlySpan<byte>(correctBytes);

                var uuid = new Uuid(readOnlySpan);
                var guid = new Guid(readOnlySpan);

                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                var guidArray = new byte[16];
                fixed (byte* pinnedGuidArray = guidArray)
                {
                    *(Guid*) pinnedGuidArray = guid;
                }

                Assert.AreEqual(guidArray, uuidArray);
            });
        }

        [TestCaseSource(nameof(IncorrectUuidBytesArraysAndExceptionTypes))]
        public void Ctor_From_ReadOnlySpan_Incorrect_SameAsGuid(byte[] incorrectBytes, Type exceptionType)
        {
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(incorrectBytes);
                Assert.IsNotNull(exceptionType);
                Assert.IsTrue(typeof(Exception).IsAssignableFrom(exceptionType));
                Assert.Throws(exceptionType, () =>
                {
                    var readOnlySpan = new ReadOnlySpan<byte>(incorrectBytes);
                    var _ = new Guid(readOnlySpan);
                });
                Assert.Throws(exceptionType, () =>
                {
                    var readOnlySpan = new ReadOnlySpan<byte>(incorrectBytes);
                    var _ = new Uuid(readOnlySpan);
                });
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void ToByteArray_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                var uuidArray = uuid.ToByteArray();
                var guidArray = guid.ToByteArray();

                Assert.AreEqual(guidArray, uuidArray);
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void TryWriteBytes_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                var uuidArray = new byte[16];
                var uuidSpan = new Span<byte>(uuidArray);
                var uuidWriteOk = uuid.TryWriteBytes(uuidSpan);

                var guidArray = new byte[16];
                var guidSpan = new Span<byte>(guidArray);
                var guidWriteOk = guid.TryWriteBytes(guidSpan);

                Assert.IsTrue(uuidWriteOk);
                Assert.IsTrue(guidWriteOk);
                Assert.AreEqual(guidArray, uuidArray);
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void TryWriteBytes_IncorrectSpan_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                var uuidArray = new byte[15];
                var uuidSpan = new Span<byte>(uuidArray);
                var uuidWriteOk = uuid.TryWriteBytes(uuidSpan);

                var guidArray = new byte[15];
                var guidSpan = new Span<byte>(guidArray);
                var guidWriteOk = guid.TryWriteBytes(guidSpan);

                Assert.IsFalse(uuidWriteOk);
                Assert.IsFalse(guidWriteOk);
                Assert.AreEqual(guidArray, uuidArray);
            });
        }

        [TestCaseSource(nameof(CorrectCompareToArraysAndResult))]
        public void CompareTo_Object_SameAsGuid(
            byte[] correctBytes,
            byte[] correctCompareToBytes,
            int expectedResult)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);
                Assert.AreEqual(16, correctCompareToBytes.Length);
                Assert.True(expectedResult == -1 || expectedResult == 0 || expectedResult == 1);

                var uuid = new Uuid(correctBytes);
                var compareToUuid = new Uuid(correctCompareToBytes);
                var compareToUuidObject = (object) compareToUuid;
                var guid = new Guid(correctBytes);
                var compareToGuid = new Guid(correctCompareToBytes);
                var compareToGuidObject = (object) compareToGuid;

                var uuidCompareToResult = uuid.CompareTo(compareToUuidObject);
                var guidCompareToResult = guid.CompareTo(compareToGuidObject);

                Assert.AreEqual(guidCompareToResult, uuidCompareToResult);
                Assert.AreEqual(expectedResult, guidCompareToResult);
                Assert.AreEqual(expectedResult, uuidCompareToResult);
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void CompareTo_Object_Null_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                Assert.AreEqual(1, guid.CompareTo(null));
                Assert.AreEqual(1, uuid.CompareTo(null));
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public void CompareTo_Object_AnotherType_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                Assert.Throws<ArgumentException>(() => guid.CompareTo(42));
                Assert.Throws<ArgumentException>(() => uuid.CompareTo(42));
            });
        }

        [TestCaseSource(nameof(CorrectCompareToArraysAndResult))]
        public void CompareTo_SameType_SameAsGuid(
            byte[] correctBytes,
            byte[] correctCompareToBytes,
            int expectedResult)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);
                Assert.AreEqual(16, correctCompareToBytes.Length);
                Assert.True(expectedResult == -1 || expectedResult == 0 || expectedResult == 1);

                var uuid = new Uuid(correctBytes);
                var compareToUuid = new Uuid(correctCompareToBytes);
                var guid = new Guid(correctBytes);
                var compareToGuid = new Guid(correctCompareToBytes);

                var uuidCompareToResult = uuid.CompareTo(compareToUuid);
                var guidCompareToResult = guid.CompareTo(compareToGuid);

                Assert.AreEqual(guidCompareToResult, uuidCompareToResult);
                Assert.AreEqual(expectedResult, guidCompareToResult);
                Assert.AreEqual(expectedResult, uuidCompareToResult);
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void Equals_Object_SameObjects_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid1 = new Uuid(correctBytes);
                var uuid2 = new Uuid(correctBytes);
                var uuid2Object = (object) uuid2;

                var guid1 = new Guid(correctBytes);
                var guid2 = new Guid(correctBytes);
                var guid2Object = (object) guid2;

                Assert.IsTrue(guid1.Equals(guid2Object));
                Assert.IsTrue(uuid1.Equals(uuid2Object));
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void Equals_Object_NotSameObjects_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuidBytes1 = new byte[16];
                var uuidBytes2 = new byte[16];
                var uuidBytes3 = new byte[16];
                Array.Copy(correctBytes, uuidBytes1, 16);
                Array.Copy(correctBytes, uuidBytes2, 16);
                Array.Copy(correctBytes, uuidBytes3, 16);

                unchecked
                {
                    uuidBytes2[0]++;
                    uuidBytes3[8]++;
                }

                var uuid1 = new Uuid(uuidBytes1);
                var uuid2 = new Uuid(uuidBytes2);
                var uuid2Object = (object) uuid2;
                var uuid3 = new Uuid(uuidBytes3);
                var uuid3Object = (object) uuid3;

                var guid1 = new Guid(uuidBytes1);
                var guid2 = new Guid(uuidBytes2);
                var guid2Object = (object) guid2;
                var guid3 = new Guid(uuidBytes3);
                var guid3Object = (object) guid3;

                Assert.IsFalse(guid1.Equals(guid2Object));
                Assert.IsFalse(guid1.Equals(guid3Object));
                Assert.IsFalse(uuid1.Equals(uuid2Object));
                Assert.IsFalse(uuid1.Equals(uuid3Object));
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void Equals_Object_Null_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                Assert.IsFalse(guid.Equals(null));
                Assert.IsFalse(uuid.Equals(null));
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public void Equals_Object_AnotherType_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                Assert.False(guid.Equals(42));
                Assert.False(uuid.Equals(42));
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void Equals_SameType_SameObjects_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid1 = new Uuid(correctBytes);
                var uuid2 = new Uuid(correctBytes);

                var guid1 = new Guid(correctBytes);
                var guid2 = new Guid(correctBytes);

                Assert.IsTrue(guid1.Equals(guid2));
                Assert.IsTrue(uuid1.Equals(uuid2));
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        public void Equals_SameType_NotSameObjects_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuidBytes1 = new byte[16];
                var uuidBytes2 = new byte[16];
                var uuidBytes3 = new byte[16];
                Array.Copy(correctBytes, uuidBytes1, 16);
                Array.Copy(correctBytes, uuidBytes2, 16);
                Array.Copy(correctBytes, uuidBytes3, 16);

                unchecked
                {
                    uuidBytes2[0]++;
                    uuidBytes3[8]++;
                }

                var uuid1 = new Uuid(uuidBytes1);
                var uuid2 = new Uuid(uuidBytes2);
                var uuid3 = new Uuid(uuidBytes3);

                var guid1 = new Guid(uuidBytes1);
                var guid2 = new Guid(uuidBytes2);
                var guid3 = new Guid(uuidBytes3);

                Assert.IsFalse(guid1.Equals(guid2));
                Assert.IsFalse(guid1.Equals(guid3));
                Assert.IsFalse(uuid1.Equals(uuid2));
                Assert.IsFalse(uuid1.Equals(uuid3));
            });
        }

        [TestCaseSource(nameof(CorrectUuidBytesArrays))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public void GetHashCode_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                Assert.AreEqual(guid.GetHashCode(), uuid.GetHashCode());
            });
        }

        [TestCaseSource(nameof(CorrectUuidD))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void ParseFromDefaultString_SameAsGuid(string correctUuid)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(36, correctUuid.Length);
                var correctUuidNString = string.Create(32, correctUuid, (result, uuidString) =>
                {
                    result[0] = uuidString[0];
                    result[1] = uuidString[1];
                    result[2] = uuidString[2];
                    result[3] = uuidString[3];
                    result[4] = uuidString[4];
                    result[5] = uuidString[5];
                    result[6] = uuidString[6];
                    result[7] = uuidString[7];

                    result[8] = uuidString[9];
                    result[9] = uuidString[10];
                    result[10] = uuidString[11];
                    result[11] = uuidString[12];

                    result[12] = uuidString[14];
                    result[13] = uuidString[15];
                    result[14] = uuidString[16];
                    result[15] = uuidString[17];

                    result[16] = uuidString[19];
                    result[17] = uuidString[20];
                    result[18] = uuidString[21];
                    result[19] = uuidString[22];

                    result[20] = uuidString[24];
                    result[21] = uuidString[25];
                    result[22] = uuidString[26];
                    result[23] = uuidString[27];
                    result[24] = uuidString[28];
                    result[25] = uuidString[29];
                    result[26] = uuidString[30];
                    result[27] = uuidString[31];
                    result[28] = uuidString[32];
                    result[29] = uuidString[33];
                    result[30] = uuidString[34];
                    result[31] = uuidString[35];
                });
                var correctUuidBytes = ConvertHexStringToByteArray(correctUuidNString);

                var guidWithNonMixedEndianBytes = new Guid(correctUuidBytes);
                var guid = new Guid(correctUuid);
                var uuid = new Uuid(correctUuid);
                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                Assert.AreEqual(guid.ToString("D"), uuid.ToString("D"));
                Assert.AreEqual(correctUuidNString, uuid.ToString("N"));
                Assert.AreEqual(correctUuidBytes, uuidArray);
                Assert.AreEqual(guidWithNonMixedEndianBytes.GetHashCode(), uuid.GetHashCode());
                Assert.AreEqual(guidWithNonMixedEndianBytes.ToByteArray(), uuid.ToByteArray());
            });
        }

        [TestCaseSource(nameof(CorrectUuidD))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void ParseFromString_D_SameAsGuid(string correctUuid)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(36, correctUuid.Length);
                var correctUuidNString = string.Create(32, correctUuid, (result, uuidString) =>
                {
                    result[0] = uuidString[0];
                    result[1] = uuidString[1];
                    result[2] = uuidString[2];
                    result[3] = uuidString[3];
                    result[4] = uuidString[4];
                    result[5] = uuidString[5];
                    result[6] = uuidString[6];
                    result[7] = uuidString[7];

                    result[8] = uuidString[9];
                    result[9] = uuidString[10];
                    result[10] = uuidString[11];
                    result[11] = uuidString[12];

                    result[12] = uuidString[14];
                    result[13] = uuidString[15];
                    result[14] = uuidString[16];
                    result[15] = uuidString[17];

                    result[16] = uuidString[19];
                    result[17] = uuidString[20];
                    result[18] = uuidString[21];
                    result[19] = uuidString[22];

                    result[20] = uuidString[24];
                    result[21] = uuidString[25];
                    result[22] = uuidString[26];
                    result[23] = uuidString[27];
                    result[24] = uuidString[28];
                    result[25] = uuidString[29];
                    result[26] = uuidString[30];
                    result[27] = uuidString[31];
                    result[28] = uuidString[32];
                    result[29] = uuidString[33];
                    result[30] = uuidString[34];
                    result[31] = uuidString[35];
                });
                var correctUuidBytes = ConvertHexStringToByteArray(correctUuidNString);

                var guidWithNonMixedEndianBytes = new Guid(correctUuidBytes);
                var guid = new Guid(correctUuid);
                var uuid = new Uuid(correctUuid);
                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                Assert.AreEqual(guid.ToString("D"), uuid.ToString("D"));
                Assert.AreEqual(correctUuidNString, uuid.ToString("N"));
                Assert.AreEqual(correctUuidBytes, uuidArray);
                Assert.AreEqual(guidWithNonMixedEndianBytes.GetHashCode(), uuid.GetHashCode());
                Assert.AreEqual(guidWithNonMixedEndianBytes.ToByteArray(), uuid.ToByteArray());
            });
        }

        [TestCaseSource(nameof(CorrectUuidB))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void ParseFromString_B_SameAsGuid(string correctUuid)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(38, correctUuid.Length);
                var correctUuidNString = string.Create(32, correctUuid, (result, uuidString) =>
                {
                    result[0] = uuidString[1];
                    result[1] = uuidString[2];
                    result[2] = uuidString[3];
                    result[3] = uuidString[4];
                    result[4] = uuidString[5];
                    result[5] = uuidString[6];
                    result[6] = uuidString[7];
                    result[7] = uuidString[8];

                    result[8] = uuidString[10];
                    result[9] = uuidString[11];
                    result[10] = uuidString[12];
                    result[11] = uuidString[13];

                    result[12] = uuidString[15];
                    result[13] = uuidString[16];
                    result[14] = uuidString[17];
                    result[15] = uuidString[18];

                    result[16] = uuidString[20];
                    result[17] = uuidString[21];
                    result[18] = uuidString[22];
                    result[19] = uuidString[23];

                    result[20] = uuidString[25];
                    result[21] = uuidString[26];
                    result[22] = uuidString[27];
                    result[23] = uuidString[28];
                    result[24] = uuidString[29];
                    result[25] = uuidString[30];
                    result[26] = uuidString[31];
                    result[27] = uuidString[32];
                    result[28] = uuidString[33];
                    result[29] = uuidString[34];
                    result[30] = uuidString[35];
                    result[31] = uuidString[36];
                });
                var correctUuidBytes = ConvertHexStringToByteArray(correctUuidNString);

                var guidWithNonMixedEndianBytes = new Guid(correctUuidBytes);
                var guid = new Guid(correctUuid);
                var uuid = new Uuid(correctUuid);
                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                Assert.AreEqual(guid.ToString("B"), uuid.ToString("B"));
                Assert.AreEqual(correctUuidNString, uuid.ToString("N"));
                Assert.AreEqual(correctUuidBytes, uuidArray);
                Assert.AreEqual(guidWithNonMixedEndianBytes.GetHashCode(), uuid.GetHashCode());
                Assert.AreEqual(guidWithNonMixedEndianBytes.ToByteArray(), uuid.ToByteArray());
            });
        }

        [TestCaseSource(nameof(CorrectUuidP))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void ParseFromString_P_SameAsGuid(string correctUuid)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(38, correctUuid.Length);
                var correctUuidNString = string.Create(32, correctUuid, (result, uuidString) =>
                {
                    result[0] = uuidString[1];
                    result[1] = uuidString[2];
                    result[2] = uuidString[3];
                    result[3] = uuidString[4];
                    result[4] = uuidString[5];
                    result[5] = uuidString[6];
                    result[6] = uuidString[7];
                    result[7] = uuidString[8];

                    result[8] = uuidString[10];
                    result[9] = uuidString[11];
                    result[10] = uuidString[12];
                    result[11] = uuidString[13];

                    result[12] = uuidString[15];
                    result[13] = uuidString[16];
                    result[14] = uuidString[17];
                    result[15] = uuidString[18];

                    result[16] = uuidString[20];
                    result[17] = uuidString[21];
                    result[18] = uuidString[22];
                    result[19] = uuidString[23];

                    result[20] = uuidString[25];
                    result[21] = uuidString[26];
                    result[22] = uuidString[27];
                    result[23] = uuidString[28];
                    result[24] = uuidString[29];
                    result[25] = uuidString[30];
                    result[26] = uuidString[31];
                    result[27] = uuidString[32];
                    result[28] = uuidString[33];
                    result[29] = uuidString[34];
                    result[30] = uuidString[35];
                    result[31] = uuidString[36];
                });
                var correctUuidBytes = ConvertHexStringToByteArray(correctUuidNString);

                var guidWithNonMixedEndianBytes = new Guid(correctUuidBytes);
                var guid = new Guid(correctUuid);
                var uuid = new Uuid(correctUuid);
                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                Assert.AreEqual(guid.ToString("P"), uuid.ToString("P"));
                Assert.AreEqual(correctUuidNString, uuid.ToString("N"));
                Assert.AreEqual(correctUuidBytes, uuidArray);
                Assert.AreEqual(guidWithNonMixedEndianBytes.GetHashCode(), uuid.GetHashCode());
                Assert.AreEqual(guidWithNonMixedEndianBytes.ToByteArray(), uuid.ToByteArray());
            });
        }


        [TestCaseSource(nameof(CorrectUuidX))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void ParseFromString_X_SameAsGuid(string correctUuid)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(68, correctUuid.Length);
                var correctUuidNString = string.Create(32, correctUuid, (result, uuidString) =>
                {
                    result[0] = uuidString[3];
                    result[1] = uuidString[4];
                    result[2] = uuidString[5];
                    result[3] = uuidString[6];
                    result[4] = uuidString[7];
                    result[5] = uuidString[8];
                    result[6] = uuidString[9];
                    result[7] = uuidString[10];
                    result[8] = uuidString[14];
                    result[9] = uuidString[15];
                    result[10] = uuidString[16];
                    result[11] = uuidString[17];
                    result[12] = uuidString[21];
                    result[13] = uuidString[22];
                    result[14] = uuidString[23];
                    result[15] = uuidString[24];
                    result[16] = uuidString[29];
                    result[17] = uuidString[30];
                    result[18] = uuidString[34];
                    result[19] = uuidString[35];
                    result[20] = uuidString[39];
                    result[21] = uuidString[40];
                    result[22] = uuidString[44];
                    result[23] = uuidString[45];
                    result[24] = uuidString[49];
                    result[25] = uuidString[50];
                    result[26] = uuidString[54];
                    result[27] = uuidString[55];
                    result[28] = uuidString[59];
                    result[29] = uuidString[60];
                    result[30] = uuidString[64];
                    result[31] = uuidString[65];
                });
                var correctUuidBytes = ConvertHexStringToByteArray(correctUuidNString);

                var guidWithNonMixedEndianBytes = new Guid(correctUuidBytes);
                var guid = new Guid(correctUuid);
                var uuid = new Uuid(correctUuid);
                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                Assert.AreEqual(guid.ToString("X"), uuid.ToString("X"));
                Assert.AreEqual(correctUuidNString, uuid.ToString("N"));
                Assert.AreEqual(correctUuidBytes, uuidArray);
                Assert.AreEqual(guidWithNonMixedEndianBytes.GetHashCode(), uuid.GetHashCode());
                Assert.AreEqual(guidWithNonMixedEndianBytes.ToByteArray(), uuid.ToByteArray());
            });
        }

        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
                throw new ArgumentException($"The binary key cannot have an odd number of digits: {hexString}");

            var data = new byte[hexString.Length / 2];
            for (var index = 0; index < data.Length; index++)
            {
                var byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}