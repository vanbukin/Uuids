using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        public unsafe void ToByteArray_SameAsGuid()
        {
            var uuid = new Uuid(_uuidBytes);
            var guid = new Guid(_uuidBytes);

            var uuidArray = uuid.ToByteArray();
            var guidArray = guid.ToByteArray();

            Assert.AreEqual(guidArray, uuidArray);
        }

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

            Assert.IsTrue(uuidWriteOk);
            Assert.IsTrue(guidWriteOk);
            Assert.AreEqual(guidArray, uuidArray);
        }
    }
}