using System;
using System.Diagnostics.CodeAnalysis;
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
    }
}