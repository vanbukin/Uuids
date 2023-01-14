using System;

namespace Uuids.Tests.Data;

public class UuidRng
{
    private const long A = 25214903917;
    private const long C = 11;
    private long _seed;

    public UuidRng(long seed)
    {
        if (seed < 0)
        {
            throw new ArgumentException("Bad seed", nameof(seed));
        }

        _seed = seed;
    }

    private int Next(int bits) // helper
    {
        _seed = ((_seed * A) + C) & ((1L << 48) - 1);
        return (int) (_seed >> (48 - bits));
    }

    public unsafe int Next()
    {
        double resultDouble = (((long) Next(26) << 27) + Next(27)) / (double) (1L << 53);
        double* resultDoublePtr = &resultDouble;
        int* resultInt32Ptr = (int*) resultDoublePtr;
        int hi = resultInt32Ptr[0];
        int lo = resultInt32Ptr[1];
        int result = hi ^ lo;
        return result;
    }
}
