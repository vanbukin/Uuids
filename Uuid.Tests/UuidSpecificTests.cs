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
    }
}