using System;
using NUnit.Framework;

namespace Uuid.Tests
{
    public class UuidSpecificTests
    {
        private byte[] _uuidBytes;

        [OneTimeSetUp]
        public void Setup()
        {
            //0a141e28-323c-4650-5a64-6e78828c96a0
            _uuidBytes = new byte[]
            {
                10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160
            };
        }

        [Test]
        public unsafe void Ctor_From_Ptr_IsOk()
        {
            var stackUuidBytes = stackalloc byte[16];
            for (var i = 0; i < 16; i++) stackUuidBytes[i] = _uuidBytes[i];

            var uuid = new Uuid(stackUuidBytes);

            var guidArray = new byte[16];
            fixed (byte* pinnedUuidArray = guidArray)
            {
                *(Uuid*) pinnedUuidArray = uuid;
            }

            Assert.AreEqual(_uuidBytes, guidArray);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToStringDefault_IsOk(byte[] correctBytes)
        {
            var expectedString = ConvertHexBytesToStringD(correctBytes);

            var uuid = new Uuid(correctBytes);

            Assert.AreEqual(expectedString, uuid.ToString());
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToString_WithNullFormat_IsOk(byte[] correctBytes)
        {
            var expectedString = ConvertHexBytesToStringD(correctBytes);

            var uuid = new Uuid(correctBytes);

            Assert.AreEqual(expectedString, uuid.ToString(null));
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToStringD_IsOk(byte[] correctBytes)
        {
            var expectedString = ConvertHexBytesToStringD(correctBytes);

            var uuid = new Uuid(correctBytes);

            Assert.AreEqual(expectedString, uuid.ToString("D"));
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToStringN_IsOk(byte[] correctBytes)
        {
            var expectedString = ConvertHexBytesToStringN(correctBytes);

            var uuid = new Uuid(correctBytes);

            Assert.AreEqual(expectedString, uuid.ToString("N"));
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToStringB_IsOk(byte[] correctBytes)
        {
            var expectedString = ConvertHexBytesToStringB(correctBytes);

            var uuid = new Uuid(correctBytes);

            Assert.AreEqual(expectedString, uuid.ToString("B"));
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToStringP_IsOk(byte[] correctBytes)
        {
            var expectedString = ConvertHexBytesToStringP(correctBytes);

            var uuid = new Uuid(correctBytes);

            Assert.AreEqual(expectedString, uuid.ToString("P"));
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CorrectUuidBytesArrays))]
        public void ToStringX_IsOk(byte[] correctBytes)
        {
            var expectedString = ConvertHexBytesToStringX(correctBytes);

            var uuid = new Uuid(correctBytes);

            Assert.AreEqual(expectedString, uuid.ToString("X"));
        }

        private static string ConvertHexBytesToStringN(byte[] bytesToHex)
        {
            // dddddddddddddddddddddddddddddddd
            if (bytesToHex == null)
                throw new ArgumentNullException(nameof(bytesToHex));
            if (bytesToHex.Length != 16)
                throw new ArgumentException(nameof(bytesToHex));
            var nString = BitConverter
                .ToString(bytesToHex)
                .Replace("-", string.Empty)
                .ToLowerInvariant();
            return nString;
        }

        private static string ConvertHexBytesToStringD(byte[] bytesToHex)
        {
            // dddddddd-dddd-dddd-dddd-dddddddddddd
            var nString = ConvertHexBytesToStringN(bytesToHex);
            var dString = string.Create(36, nString, (result, nStr) =>
            {
                result[8] = result[13] = result[18] = result[23] = '-';
                result[0] = nStr[0];
                result[1] = nStr[1];
                result[2] = nStr[2];
                result[3] = nStr[3];
                result[4] = nStr[4];
                result[5] = nStr[5];
                result[6] = nStr[6];
                result[7] = nStr[7];

                result[9] = nStr[8];
                result[10] = nStr[9];
                result[11] = nStr[10];
                result[12] = nStr[11];

                result[14] = nStr[12];
                result[15] = nStr[13];
                result[16] = nStr[14];
                result[17] = nStr[15];

                result[19] = nStr[16];
                result[20] = nStr[17];
                result[21] = nStr[18];
                result[22] = nStr[19];

                result[24] = nStr[20];
                result[25] = nStr[21];
                result[26] = nStr[22];
                result[27] = nStr[23];
                result[28] = nStr[24];
                result[29] = nStr[25];
                result[30] = nStr[26];
                result[31] = nStr[27];
                result[32] = nStr[28];
                result[33] = nStr[29];
                result[34] = nStr[30];
                result[35] = nStr[31];
            });
            return dString;
        }

        private static string ConvertHexBytesToStringB(byte[] bytesToHex)
        {
            // {dddddddd-dddd-dddd-dddd-dddddddddddd}
            var nString = ConvertHexBytesToStringN(bytesToHex);
            var dString = string.Create(38, nString, (result, nStr) =>
            {
                result[0] = '{';
                result[37] = '}';
                result[9] = result[14] = result[19] = result[24] = '-';
                result[1] = nStr[0];
                result[2] = nStr[1];
                result[3] = nStr[2];
                result[4] = nStr[3];
                result[5] = nStr[4];
                result[6] = nStr[5];
                result[7] = nStr[6];
                result[8] = nStr[7];

                result[10] = nStr[8];
                result[11] = nStr[9];
                result[12] = nStr[10];
                result[13] = nStr[11];

                result[15] = nStr[12];
                result[16] = nStr[13];
                result[17] = nStr[14];
                result[18] = nStr[15];

                result[20] = nStr[16];
                result[21] = nStr[17];
                result[22] = nStr[18];
                result[23] = nStr[19];

                result[25] = nStr[20];
                result[26] = nStr[21];
                result[27] = nStr[22];
                result[28] = nStr[23];
                result[29] = nStr[24];
                result[30] = nStr[25];
                result[31] = nStr[26];
                result[32] = nStr[27];
                result[33] = nStr[28];
                result[34] = nStr[29];
                result[35] = nStr[30];
                result[36] = nStr[31];
            });
            return dString;
        }

        private static string ConvertHexBytesToStringP(byte[] bytesToHex)
        {
            // (dddddddd-dddd-dddd-dddd-dddddddddddd)
            var nString = ConvertHexBytesToStringN(bytesToHex);
            var dString = string.Create(38, nString, (result, nStr) =>
            {
                result[0] = '(';
                result[37] = ')';
                result[9] = result[14] = result[19] = result[24] = '-';
                result[1] = nStr[0];
                result[2] = nStr[1];
                result[3] = nStr[2];
                result[4] = nStr[3];
                result[5] = nStr[4];
                result[6] = nStr[5];
                result[7] = nStr[6];
                result[8] = nStr[7];

                result[10] = nStr[8];
                result[11] = nStr[9];
                result[12] = nStr[10];
                result[13] = nStr[11];

                result[15] = nStr[12];
                result[16] = nStr[13];
                result[17] = nStr[14];
                result[18] = nStr[15];

                result[20] = nStr[16];
                result[21] = nStr[17];
                result[22] = nStr[18];
                result[23] = nStr[19];

                result[25] = nStr[20];
                result[26] = nStr[21];
                result[27] = nStr[22];
                result[28] = nStr[23];
                result[29] = nStr[24];
                result[30] = nStr[25];
                result[31] = nStr[26];
                result[32] = nStr[27];
                result[33] = nStr[28];
                result[34] = nStr[29];
                result[35] = nStr[30];
                result[36] = nStr[31];
            });
            return dString;
        }

        private static string ConvertHexBytesToStringX(byte[] bytesToHex)
        {
            // {0xdddddddd,0xdddd,0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
            var nString = ConvertHexBytesToStringN(bytesToHex);
            var dString = string.Create(68, nString, (result, nStr) =>
            {
                result[0] = result[26] = '{';
                result[66] = result[67] = '}';
                result[25] = ',';
                result[1] = result[12] = result[19] = result[27] = result[32] =
                    result[37] = result[42] = result[47] = result[52] = result[57] = result[62] = '0';
                result[2] = result[13] = result[20] = result[28] = result[33] =
                    result[38] = result[43] = result[48] = result[53] = result[58] = result[63] = 'x';
                result[11] = result[18] = result[31] =
                    result[36] = result[41] = result[46] = result[51] = result[56] = result[61] = ',';

                result[3] = nStr[0];
                result[4] = nStr[1];
                result[5] = nStr[2];
                result[6] = nStr[3];
                result[7] = nStr[4];
                result[8] = nStr[5];
                result[9] = nStr[6];
                result[10] = nStr[7];

                result[14] = nStr[8];
                result[15] = nStr[9];
                result[16] = nStr[10];
                result[17] = nStr[11];

                result[21] = nStr[12];
                result[22] = nStr[13];
                result[23] = nStr[14];
                result[24] = nStr[15];

                result[29] = nStr[16];
                result[30] = nStr[17];

                result[34] = nStr[18];
                result[35] = nStr[19];

                result[39] = nStr[20];
                result[40] = nStr[21];

                result[44] = nStr[22];
                result[45] = nStr[23];

                result[49] = nStr[24];
                result[50] = nStr[25];

                result[54] = nStr[26];
                result[55] = nStr[27];

                result[59] = nStr[28];
                result[60] = nStr[29];

                result[64] = nStr[30];
                result[65] = nStr[31];
            });
            return dString;
        }
    }
}