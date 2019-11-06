using System;

namespace Uuid.Benchmarks
{
    public static class Utils
    {
        public static Guid[] GenerateRandomGuids(int count)
        {
            var result = new Guid[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = Guid.NewGuid();
            }

            return result;
        }

        public static unsafe string[] GenerateRandomUuidNStringsArray(int count)
        {
            var random = new Random();
            var uuidIntegers = stackalloc int[4];
            var result = new string[count];
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < 4; j++) uuidIntegers[j] = random.Next();

                var bytesOfUuid = new ReadOnlySpan<byte>(uuidIntegers, 16).ToArray();
                var nString = BitConverter
                    .ToString(bytesOfUuid)
                    .Replace("-", string.Empty)
                    .ToLowerInvariant();
                result[i] = nString;
            }

            return result;
        }
    }
}