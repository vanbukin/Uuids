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
        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.IncorrectUuidBytesArraysAndExceptionTypes))]
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

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void Ctor_From_String_Null_SameAsGuid()
        {
            string str = null;

            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var _ = new Guid(str);
                });
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var _ = new Uuid(str);
                });
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.IncorrectUuidBytesArraysAndExceptionTypes))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidD))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void CtorFromDefaultString_SameAsGuid(string correctUuid)
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidD))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void CtorFromString_D_SameAsGuid(string correctUuid)
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidN))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void CtorFromString_N_SameAsGuid(string correctUuid)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(32, correctUuid.Length);
                var correctUuidNString = correctUuid;
                var correctUuidBytes = ConvertHexStringToByteArray(correctUuidNString);

                var guidWithNonMixedEndianBytes = new Guid(correctUuidBytes);
                var guid = new Guid(correctUuid);
                var uuid = new Uuid(correctUuid);
                var uuidArray = new byte[16];
                fixed (byte* pinnedUuidArray = uuidArray)
                {
                    *(Uuid*) pinnedUuidArray = uuid;
                }

                Assert.AreEqual(guid.ToString("N"), uuid.ToString("N"));
                Assert.AreEqual(correctUuidNString, uuid.ToString("N"));
                Assert.AreEqual(correctUuidBytes, uuidArray);
                Assert.AreEqual(guidWithNonMixedEndianBytes.GetHashCode(), uuid.GetHashCode());
                Assert.AreEqual(guidWithNonMixedEndianBytes.ToByteArray(), uuid.ToByteArray());
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidB))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void CtorFromString_B_SameAsGuid(string correctUuid)
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidP))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void CtorFromString_P_SameAsGuid(string correctUuid)
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


        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidX))]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public unsafe void CtorFromString_X_SameAsGuid(string correctUuid)
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToString_IncorrectFormat_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                Assert.Throws<FormatException>(() =>
                {
                    var _ = guid.ToString("Z");
                });
                Assert.Throws<FormatException>(() =>
                {
                    var _ = uuid.ToString("Z");
                });
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToString_TooLongFormat_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                var uuid = new Uuid(correctBytes);
                var guid = new Guid(correctBytes);

                Assert.Throws<FormatException>(() =>
                {
                    var _ = guid.ToString("ZZ");
                });
                Assert.Throws<FormatException>(() =>
                {
                    var _ = uuid.ToString("ZZ");
                });
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectCompareToArraysAndResult))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectCompareToArraysAndResult))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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


        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void Equals_Operator_SameObjects_SameAsGuid(byte[] correctBytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, correctBytes.Length);

                var uuid1 = new Uuid(correctBytes);
                var uuid2 = new Uuid(correctBytes);

                var guid1 = new Guid(correctBytes);
                var guid2 = new Guid(correctBytes);

                Assert.IsTrue(guid1 == guid2);
                Assert.IsTrue(uuid1 == uuid2);
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void Equals_Operator_NotSameObjects_SameAsGuid(byte[] correctBytes)
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

                Assert.IsTrue(guid1 != guid2);
                Assert.IsTrue(guid1 != guid3);
                Assert.IsTrue(uuid1 != uuid2);
                Assert.IsTrue(uuid1 != uuid3);
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
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