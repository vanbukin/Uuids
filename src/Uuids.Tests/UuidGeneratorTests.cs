using System;
using NUnit.Framework;

namespace Uuids.Tests;

public class UuidGeneratorTests
{
    private const long ChristianCalendarGregorianReformTicksDate = 499_163_040_000_000_000L;

    private const byte ResetVersionMask = 0b0000_1111;
    private const byte Version1Flag = 0b0001_0000;

    private const byte ResetReservedMask = 0b0011_1111;
    private const byte ReservedFlag = 0b1000_0000;

    [Test]
    public unsafe void NewTimeBased()
    {
        DateTimeOffset startDate = DateTimeOffset.UtcNow;
        Uuid uuid = Uuid.NewTimeBased();
        DateTimeOffset endDate = DateTimeOffset.UtcNow;
        byte* uuidPtr = (byte*) &uuid;
        long ticks = (endDate - startDate).Ticks + 1;

        for (int i = 0; i < ticks; i++)
        {
            long attemptTicks = startDate.Ticks + i - ChristianCalendarGregorianReformTicksDate;
            byte* ticksPtr = (byte*) &attemptTicks;
            if (IsTimeBasedUuidForSpecifiedTime(ticksPtr, uuidPtr))
            {
                Assert.Pass();
            }
        }

        Assert.Fail("Could not find time when uuid was generated, or generation was broken");
    }

    [Test]
    public unsafe void NewMySqlOptimized()
    {
        DateTimeOffset startDate = DateTimeOffset.UtcNow;
        Uuid uuid = Uuid.NewMySqlOptimized();
        DateTimeOffset endDate = DateTimeOffset.UtcNow;
        byte* uuidPtr = (byte*) &uuid;
        long ticks = (endDate - startDate).Ticks + 1;

        for (int i = 0; i < ticks; i++)
        {
            long attemptTicks = startDate.Ticks + i - ChristianCalendarGregorianReformTicksDate;
            byte* ticksPtr = (byte*) &attemptTicks;
            if (IsMySqlOptimizedUuidForSpecifiedTime(ticksPtr, uuidPtr))
            {
                Assert.Pass();
            }
        }

        Assert.Fail("Could not find time when uuid was generated, or generation was broken");
    }

    private unsafe bool IsTimeBasedUuidForSpecifiedTime(byte* ticksPtr, byte* uuidBytes)
    {
        return uuidBytes[0] == ticksPtr[3]
               && uuidBytes[1] == ticksPtr[2]
               && uuidBytes[2] == ticksPtr[1]
               && uuidBytes[3] == ticksPtr[0]
               && uuidBytes[4] == ticksPtr[5]
               && uuidBytes[5] == ticksPtr[4]
               && uuidBytes[6] == (byte) ((ticksPtr[7] & ResetVersionMask) | Version1Flag)
               && uuidBytes[7] == ticksPtr[6]
               && uuidBytes[8] == (byte) ((uuidBytes[8] & ResetReservedMask) | ReservedFlag);
    }

    private unsafe bool IsMySqlOptimizedUuidForSpecifiedTime(byte* ticksPtr, byte* uuidBytes)
    {
        return uuidBytes[0] == (byte) ((ticksPtr[7] & ResetVersionMask) | Version1Flag)
               && uuidBytes[1] == ticksPtr[6]
               && uuidBytes[2] == ticksPtr[5]
               && uuidBytes[3] == ticksPtr[4]
               && uuidBytes[4] == ticksPtr[3]
               && uuidBytes[5] == ticksPtr[2]
               && uuidBytes[6] == ticksPtr[1]
               && uuidBytes[7] == ticksPtr[0]
               && uuidBytes[8] == (byte) ((uuidBytes[8] & ResetReservedMask) | ReservedFlag);
    }
}
