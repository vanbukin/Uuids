using System;

namespace Uuid
{
    public unsafe partial struct Uuid
    {
        public static Uuid NewUuid(int version = 1)
        {
            switch (version)
            {
                case 1:
                    return GenerateUuidVersion1();
                case 2:
                    throw new NotImplementedException();
                case 3:
                    throw new NotImplementedException();
                case 4:
                    throw new NotImplementedException();
                case 5:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(version));
            }
        }

        private const long ChristianCalendarGregorianReformTicksDate = 499_163_040_000_000_000L;

        private const byte ResetVersionMask = 0b0000_1111;
        private const byte Version1Flag = 0b0001_0000;

        private const byte ResetReservedMask = 0b1011_1111;
        private const byte ReservedFlag = 0b1000_0000;

        private static Uuid GenerateUuidVersion1()
        {
            var result = stackalloc byte[16];
            CoreLib.Internal.GetRandomBytes(result + 8, 8);
            var currentTicks = DateTime.UtcNow.Ticks - ChristianCalendarGregorianReformTicksDate;
            var ticksPtr = (byte*) &currentTicks;
            result[0] = ticksPtr[3];
            result[1] = ticksPtr[2];
            result[2] = ticksPtr[1];
            result[3] = ticksPtr[0];
            result[4] = ticksPtr[5];
            result[5] = ticksPtr[4];
            result[6] = (byte) ((ticksPtr[7] & ResetVersionMask) | Version1Flag);
            result[7] = ticksPtr[6];
            result[8] = (byte) ((result[8] & ResetReservedMask) | ReservedFlag);
            return new Uuid(result);
        }
    }
}