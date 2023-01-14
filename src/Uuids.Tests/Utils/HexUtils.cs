using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Uuids.Tests.Utils;

/// <summary>
///     Utility methods to work with hexadecimal strings.
/// </summary>
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
[SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
public static unsafe class HexUtils
{
    private const ushort MaximalChar = 103;
    private static readonly uint* TableToHex;
    private static readonly byte* TableFromHexToBytes;

    static HexUtils()
    {
        TableToHex = (uint*) Marshal.AllocHGlobal(sizeof(uint) * 256).ToPointer();
        for (int i = 0; i < 256; i++)
        {
            string chars = Convert.ToString(i, 16).PadLeft(2, '0');
            TableToHex[i] = ((uint) chars[1] << 16) | chars[0];
        }

        TableFromHexToBytes = (byte*) Marshal.AllocHGlobal(103).ToPointer();
        for (int i = 0; i < 103; i++)
        {
            TableFromHexToBytes[i] = (char) i switch
            {
                '0' => 0x0,
                '1' => 0x1,
                '2' => 0x2,
                '3' => 0x3,
                '4' => 0x4,
                '5' => 0x5,
                '6' => 0x6,
                '7' => 0x7,
                '8' => 0x8,
                '9' => 0x9,
                'a' => 0xa,
                'A' => 0xa,
                'b' => 0xb,
                'B' => 0xb,
                'c' => 0xc,
                'C' => 0xc,
                'd' => 0xd,
                'D' => 0xd,
                'e' => 0xe,
                'E' => 0xe,
                'f' => 0xf,
                'F' => 0xf,
                _ => byte.MaxValue
            };
        }
    }

    /// <summary>
    ///     Checks that provided string is hexadecimal.
    /// </summary>
    /// <param name="possibleHexString">String to check.</param>
    /// <returns></returns>
    public static bool IsHexString(string? possibleHexString)
    {
        if (string.IsNullOrWhiteSpace(possibleHexString))
        {
            return false;
        }

        if (possibleHexString!.Length % 2 != 0)
        {
            return false;
        }

        int length = possibleHexString.Length;
        fixed (char* stringPtr = &possibleHexString.GetPinnableReference())
        {
            for (int i = 0; i < length;)
            {
                if (stringPtr[i] < MaximalChar
                    && TableFromHexToBytes[stringPtr[i]] != 0xFF
                    && stringPtr[i + 1] < MaximalChar
                    && TableFromHexToBytes[stringPtr[i + 1]] != 0xFF)
                {
                    i += 2;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    ///     Returns bytes of hexadecimal string.
    /// </summary>
    /// <param name="hexString">Hexadecimal string</param>
    /// <returns></returns>
    public static byte[]? GetBytes(string hexString)
    {
        if (string.IsNullOrWhiteSpace(hexString))
        {
            return null;
        }

        if (hexString.Length % 2 != 0)
        {
            return null;
        }

        int length = hexString.Length;
        byte[] result = new byte[length / 2];
        fixed (char* stringPtr = &hexString.GetPinnableReference())
        fixed (byte* resultPtr = &result[0])
        {
            int resultIndex = 0;
            for (int i = 0; i < length;)
            {
                byte hexByteHi;
                byte hexByteLow;
                if (stringPtr[i] < MaximalChar
                    && (hexByteHi = TableFromHexToBytes[stringPtr[i]]) != 0xFF
                    && stringPtr[i + 1] < MaximalChar
                    && (hexByteLow = TableFromHexToBytes[stringPtr[i + 1]]) != 0xFF)
                {
                    byte resultByte = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                    resultPtr[resultIndex] = resultByte;
                    i += 2;
                    resultIndex += 1;
                }
                else
                {
                    return null;
                }
            }
        }

        return result;
    }

    /// <summary>
    ///     Return hexadecimal string representation of provided bytes.
    /// </summary>
    /// <param name="bytes">Bytes that need to be presented as hexadecimal string.</param>
    /// <returns></returns>
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    public static string? GetString(byte[]? bytes)
    {
        if (bytes == null)
        {
            return null;
        }

        if (bytes.Length == 0)
        {
            return string.Empty;
        }

        string resultString = new('\0', bytes.Length * 2);
        fixed (char* stringPtr = &resultString.GetPinnableReference())
        {
            uint* destUints = (uint*) stringPtr;
            for (int i = 0; i < bytes.Length; i++)
            {
                destUints[i] = TableToHex[bytes[i]];
            }
        }

        return resultString;
    }
}
