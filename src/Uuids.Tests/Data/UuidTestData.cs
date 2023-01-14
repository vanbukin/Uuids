using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Uuids.Tests.Data.Models;
using Uuids.Tests.Utils;

namespace Uuids.Tests.Data;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public static class UuidTestData
{
    public static object[] CorrectUuidBytesArrays { get; } =
    {
        new object[]
        {
            new byte[]
            {
                10,
                20,
                30,
                40,
                50,
                60,
                70,
                80,
                90,
                100,
                110,
                120,
                130,
                140,
                150,
                160
            }
        },
        new object[]
        {
            new byte[]
            {
                163,
                167,
                252,
                114,
                206,
                122,
                17,
                233,
                128,
                237,
                0,
                13,
                58,
                17,
                37,
                233
            }
        },
        new object[]
        {
            new byte[]
            {
                241,
                186,
                230,
                119,
                206,
                55,
                78,
                240,
                175,
                188,
                141,
                114,
                36,
                63,
                217,
                193
            }
        },
        new object[]
        {
            new byte[]
            {
                230,
                35,
                75,
                5,
                129,
                19,
                99,
                68,
                152,
                188,
                145,
                109,
                120,
                166,
                14,
                235
            }
        },
        new object[]
        {
            new byte[]
            {
                255,
                255,
                255,
                255,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            }
        },
        new object[]
        {
            new byte[]
            {
                0,
                0,
                0,
                0,
                255,
                255,
                255,
                255,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            }
        },
        new object[]
        {
            new byte[]
            {
                0,
                0,
                0,
                0,
                255,
                255,
                255,
                255,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            }
        },
        new object[]
        {
            new byte[]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                255,
                255,
                255,
                255,
                0,
                0,
                0,
                0
            }
        },
        new object[]
        {
            new byte[]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                255,
                255,
                255,
                255
            }
        }
    };

    public static object[] CorrectCompareToArraysAndResult { get; } =
    {
        new object[]
        {
            new byte[]
            {
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0
            },
            new byte[]
            {
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0,
                0
            },
            new byte[]
            {
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0
            },
            new byte[]
            {
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0,
                0
            },
            new byte[]
            {
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0
            },
            new byte[]
            {
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13,
                0
            },
            new byte[]
            {
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42,
                0
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                72,
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42
            },
            new byte[]
            {
                72,
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13
            },
            1
        },
        new object[]
        {
            new byte[]
            {
                72,
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                13
            },
            new byte[]
            {
                72,
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17,
                42
            },
            -1
        },
        new object[]
        {
            new byte[]
            {
                1,
                72,
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17
            },
            new byte[]
            {
                1,
                72,
                99,
                252,
                201,
                77,
                117,
                69,
                125,
                81,
                23,
                97,
                234,
                173,
                29,
                17
            },
            0
        }
    };

    public static object[] CorrectEqualsToBytesAndResult { get; } =
    {
        new object[]
        {
            new byte[]
            {
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            false
        },
        new object[]
        {
            new byte[]
            {
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            true
        },
        new object[]
        {
            new byte[]
            {
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                37,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            false
        },
        new object[]
        {
            new byte[]
            {
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            new byte[]
            {
                13,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                42,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            },
            true
        }
    };

    // ---------
    // --- N ---
    // ---------
    public static UuidStringWithBytes[] CorrectNStrings { get; } = UuidTestsUtils.GenerateNStrings();

    public static string[] LargeNStrings { get; } = CorrectNStrings
        .Select(x => x.UuidString + "f")
        .ToArray();

    public static string[] SmallNStrings { get; } = CorrectNStrings
        .Select(x => x.UuidString.Substring(x.UuidString.Length / 2))
        .ToArray();

    public static string[] BrokenNStrings { get; } = UuidTestsUtils.GenerateBrokenNStringsArray();

    // ---------
    // --- D ---
    // ---------
    public static UuidStringWithBytes[] CorrectDStrings { get; } = UuidTestsUtils.GenerateDStrings();

    public static string[] LargeDStrings { get; } = CorrectDStrings
        .Select(x => x.UuidString + "f")
        .ToArray();

    public static string[] SmallDStrings { get; } = CorrectDStrings
        .Select(x => x.UuidString.Substring(x.UuidString.Length / 2))
        .ToArray();

    public static string[] BrokenDStrings { get; } = UuidTestsUtils.GenerateBrokenDStringsArray();

    // ---------
    // --- B ---
    // ---------
    public static UuidStringWithBytes[] CorrectBStrings { get; } = UuidTestsUtils.GenerateBStrings();

    public static string[] LargeBStrings { get; } = CorrectBStrings
        .Select(x => x.UuidString + "f")
        .ToArray();

    public static string[] SmallBStrings { get; } = CorrectBStrings
        .Select(x => x.UuidString.Substring(x.UuidString.Length / 2))
        .ToArray();

    public static string[] BrokenBStrings { get; } = UuidTestsUtils.GenerateBrokenBStringsArray();

    // ---------
    // --- P ---
    // ---------
    public static UuidStringWithBytes[] CorrectPStrings { get; } = UuidTestsUtils.GeneratePStrings();

    public static string[] LargePStrings { get; } = CorrectPStrings
        .Select(x => x.UuidString + "f")
        .ToArray();

    public static string[] SmallPStrings { get; } = CorrectPStrings
        .Select(x => x.UuidString.Substring(x.UuidString.Length / 2))
        .ToArray();

    public static string[] BrokenPStrings { get; } = UuidTestsUtils.GenerateBrokenPStringsArray();

    // ---------
    // --- X ---
    // ---------
    public static UuidStringWithBytes[] CorrectXStrings { get; } = UuidTestsUtils.GenerateXStrings();

    public static string[] LargeXStrings { get; } = CorrectXStrings
        .Select(x => x.UuidString + "f")
        .ToArray();

    public static string[] SmallXStrings { get; } = CorrectXStrings
        .Select(x => x.UuidString.Substring(x.UuidString.Length / 2))
        .ToArray();

    public static string[] BrokenXStrings { get; } = UuidTestsUtils.GenerateBrokenXStringsArray();

    public static object[] LeftLessThanRight()
    {
        object[] src = CorrectCompareToArraysAndResult;
        List<object> results = new();
        foreach (object arg in src)
        {
            object[] args = (object[]) arg;
            byte[] left = (byte[]) args[0];
            byte[] right = (byte[]) args[1];
            int flag = (int) args[2];
            if (flag == -1)
            {
                object[] outputArgs =
                {
                    new Uuid(left),
                    new Uuid(right)
                };
                results.Add(outputArgs);
            }
        }

        return results.ToArray();
    }

    public static object[] RightLessThanLeft()
    {
        object[] src = CorrectCompareToArraysAndResult;
        List<object> results = new();
        foreach (object arg in src)
        {
            object[] args = (object[]) arg;
            byte[] left = (byte[]) args[0];
            byte[] right = (byte[]) args[1];
            int flag = (int) args[2];
            if (flag == 1)
            {
                object[] outputArgs =
                {
                    new Uuid(left),
                    new Uuid(right)
                };
                results.Add(outputArgs);
            }
        }

        return results.ToArray();
    }
}
