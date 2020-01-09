using System;

namespace Uuids
{
    public unsafe partial struct Uuid
    {
        private const long ChristianCalendarGregorianReformTicksDate = 499_163_040_000_000_000L;

        private const byte ResetVersionMask = 0b0000_1111;
        private const byte Version1Flag = 0b0001_0000;

        private const byte ResetReservedMask = 0b0011_1111;
        private const byte ReservedFlag = 0b1000_0000;

        public static Uuid NewTimeBased()
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

        public static Uuid NewMySqlOptimized()
        {
            var result = stackalloc byte[16];
            CoreLib.Internal.GetRandomBytes(result + 8, 8);
            var currentTicks = DateTime.UtcNow.Ticks - ChristianCalendarGregorianReformTicksDate;
            var ticksPtr = (byte*) &currentTicks;
            result[0] = (byte) ((ticksPtr[7] & ResetVersionMask) | Version1Flag);
            result[1] = ticksPtr[6];
            result[2] = ticksPtr[5];
            result[3] = ticksPtr[4];
            result[4] = ticksPtr[3];
            result[5] = ticksPtr[2];
            result[6] = ticksPtr[1];
            result[7] = ticksPtr[0];
            result[8] = (byte) ((result[8] & ResetReservedMask) | ReservedFlag);
            return new Uuid(result);
        }
    }
}