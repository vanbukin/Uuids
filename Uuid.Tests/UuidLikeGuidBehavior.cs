using System;
using System.Collections;
using System.Text;
using NUnit.Framework;

namespace Uuid.Tests
{
    public class UuidLikeGuidBehavior
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

        public static object[] UuidBytesArrays { get; } =
        {
            new object[] {new byte[] {10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160}},
            new object[] {new byte[] {163, 167, 252, 114, 206, 122, 17, 233, 128, 237, 0, 13, 58, 17, 37, 233}},
            new object[] {new byte[] {241, 186, 230, 119, 206, 55, 78, 240, 175, 188, 141, 114, 36, 63, 217, 193}},
            new object[] {new byte[] {230, 35, 75, 5, 129, 19, 99, 68, 152, 188, 145, 109, 120, 166, 14, 235}},
            new object[] {new byte[] {255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0}},
            new object[] {new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255}},
        };

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        [Test]
        public unsafe void Ctor_From_ByteArray_SameAsGuid()
        {
            var uuid = new Uuid(_uuidBytes);
            var guid = new Guid(_uuidBytes);

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
        }

        [Test]
        public unsafe void Ctor_From_ReadOnlySpan_SameAsGuid()
        {
            var readOnlySpan = new ReadOnlySpan<byte>(_uuidBytes);

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
        }

        [Test]
        public void ToByteArray_SameAsGuid()
        {
            var uuid = new Uuid(_uuidBytes);
            var guid = new Guid(_uuidBytes);

            var uuidArray = uuid.ToByteArray();
            var guidArray = guid.ToByteArray();

            Assert.AreEqual(guidArray, uuidArray);
        }

        [Test]
        public void TryWriteBytes_SameAsGuid()
        {
            var uuid = new Uuid(_uuidBytes);
            var guid = new Guid(_uuidBytes);

            var uuidArray = new byte[16];
            var uuidSpan = new Span<byte>(uuidArray);
            var uuidWriteOk = uuid.TryWriteBytes(uuidSpan);

            var guidArray = new byte[16];
            var guidSpan = new Span<byte>(guidArray);
            var guidWriteOk = guid.TryWriteBytes(guidSpan);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(uuidWriteOk);
                Assert.IsTrue(guidWriteOk);
                Assert.AreEqual(guidArray, uuidArray);
            });
        }

        [Test]
        public void TryWriteBytes_IncorrectSpan_SameAsGuid()
        {
            var uuid = new Uuid(_uuidBytes);
            var guid = new Guid(_uuidBytes);

            var uuidArray = new byte[15];
            var uuidSpan = new Span<byte>(uuidArray);
            var uuidWriteOk = uuid.TryWriteBytes(uuidSpan);

            var guidArray = new byte[15];
            var guidSpan = new Span<byte>(guidArray);
            var guidWriteOk = guid.TryWriteBytes(guidSpan);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(uuidWriteOk);
                Assert.IsFalse(guidWriteOk);
                Assert.AreEqual(guidArray, uuidArray);
            });
        }

        [TestCaseSource(nameof(UuidBytesArrays))]
        public void CompareTo_Object_Null_SameAsGuid(byte[] bytes)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(16, bytes.Length);

                var uuid = new Uuid(bytes);
                var guid = new Guid(bytes);

                Assert.AreEqual(1, guid.CompareTo(null));
                Assert.AreEqual(1, uuid.CompareTo(null));
            });
        }
    }
}