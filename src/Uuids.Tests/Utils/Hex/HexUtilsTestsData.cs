using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Uuids.Tests.Utils.Hex;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public static class HexUtilsTestsData
{
    public static readonly string[] ValidHexStrings =
    {
        "De",
        "Dead",
        "DeadBeef1337",
        "1B6f",
        "1b6f",
        "1B6F",
        "9C045ABE1B6211EA978F2E728CE88125",
        "9c045abe1b6211ea978f2e728ce88125"
    };

    public static readonly string[] BrokenHexStrings = GetBrokenHexStrings(ValidHexStrings);

    public static readonly byte[][] ValidByteArrays =
    {
        new byte[]
        {
            13,
            37,
            42
        },
        new byte[]
        {
            42,
            43,
            43,
            34,
            35,
            9,
            12,
            19,
            93
        },
        Enumerable.Repeat((byte) 42, 17935).Select((b, index) =>
        {
            byte result;
            unchecked
            {
                result = (byte) (b + (byte) (index % byte.MaxValue));
            }

            return result;
        }).ToArray()
    };

    private static unsafe string[] GetBrokenHexStrings(string[] strings)
    {
        string[] resultStrings = new string[strings.Length];
        for (int i = 0; i < strings.Length; i++)
        {
            string brokenString = new(strings[i].ToCharArray());
            fixed (char* brokenPtr = brokenString)
            {
                byte* bytePtr = (byte*) brokenPtr;
                int problemType = i % 3;
                switch (problemType)
                {
                    case 0:
                        bytePtr[0] = 47;
                        break;
                    case 1:
                        bytePtr[0] = 104;
                        break;
                    default:
                        bytePtr[1] = 47;
                        break;
                }
            }

            resultStrings[i] = brokenString;
        }

        return resultStrings;
    }
}
