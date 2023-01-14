using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Uuids.Tests.Data;
using Uuids.Tests.Data.Models;

namespace Uuids.Tests.Utils;

public static class UuidTestsUtils
{
    public static byte[] ConvertHexStringToByteArray(string hexString)
    {
        ArgumentNullException.ThrowIfNull(hexString);
        if (hexString.Length % 2 != 0)
        {
            throw new ArgumentException($"The binary key cannot have an odd number of digits: {hexString}");
        }

        byte[] data = new byte[hexString.Length / 2];
        for (int index = 0; index < data.Length; index++)
        {
            string byteValue = hexString.Substring(index * 2, 2);
            data[index] = Convert.ToByte(byteValue, 16);
        }

        return data;
    }

    public static string GetStringN(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        // dddddddddddddddddddddddddddddddd
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Uuid bytes count should be 16", nameof(bytes));
        }

        return BitConverter
            .ToString(bytes)
            .Replace("-", string.Empty)
            .ToLowerInvariant();
    }

    public static unsafe string GetStringD(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        // dddddddd-dddd-dddd-dddd-dddddddddddd
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Uuid bytes count should be 16", nameof(bytes));
        }

        string stringN = GetStringN(bytes);
        char* stringDPtr = stackalloc char[36];
        stringDPtr[0] = stringN[0];
        stringDPtr[1] = stringN[1];
        stringDPtr[2] = stringN[2];
        stringDPtr[3] = stringN[3];
        stringDPtr[4] = stringN[4];
        stringDPtr[5] = stringN[5];
        stringDPtr[6] = stringN[6];
        stringDPtr[7] = stringN[7];
        stringDPtr[8] = '-';
        stringDPtr[9] = stringN[8];
        stringDPtr[10] = stringN[9];
        stringDPtr[11] = stringN[10];
        stringDPtr[12] = stringN[11];
        stringDPtr[13] = '-';
        stringDPtr[14] = stringN[12];
        stringDPtr[15] = stringN[13];
        stringDPtr[16] = stringN[14];
        stringDPtr[17] = stringN[15];
        stringDPtr[18] = '-';
        stringDPtr[19] = stringN[16];
        stringDPtr[20] = stringN[17];
        stringDPtr[21] = stringN[18];
        stringDPtr[22] = stringN[19];
        stringDPtr[23] = '-';
        stringDPtr[24] = stringN[20];
        stringDPtr[25] = stringN[21];
        stringDPtr[26] = stringN[22];
        stringDPtr[27] = stringN[23];
        stringDPtr[28] = stringN[24];
        stringDPtr[29] = stringN[25];
        stringDPtr[30] = stringN[26];
        stringDPtr[31] = stringN[27];
        stringDPtr[32] = stringN[28];
        stringDPtr[33] = stringN[29];
        stringDPtr[34] = stringN[30];
        stringDPtr[35] = stringN[31];
        return new(stringDPtr, 0, 36);
    }

    public static unsafe string GetStringB(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        // {dddddddd-dddd-dddd-dddd-dddddddddddd}
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Uuid bytes count should be 16", nameof(bytes));
        }

        string stringN = GetStringN(bytes);
        char* stringBPtr = stackalloc char[38];
        stringBPtr[0] = '{';
        stringBPtr[1] = stringN[0];
        stringBPtr[2] = stringN[1];
        stringBPtr[3] = stringN[2];
        stringBPtr[4] = stringN[3];
        stringBPtr[5] = stringN[4];
        stringBPtr[6] = stringN[5];
        stringBPtr[7] = stringN[6];
        stringBPtr[8] = stringN[7];
        stringBPtr[9] = '-';
        stringBPtr[10] = stringN[8];
        stringBPtr[11] = stringN[9];
        stringBPtr[12] = stringN[10];
        stringBPtr[13] = stringN[11];
        stringBPtr[14] = '-';
        stringBPtr[15] = stringN[12];
        stringBPtr[16] = stringN[13];
        stringBPtr[17] = stringN[14];
        stringBPtr[18] = stringN[15];
        stringBPtr[19] = '-';
        stringBPtr[20] = stringN[16];
        stringBPtr[21] = stringN[17];
        stringBPtr[22] = stringN[18];
        stringBPtr[23] = stringN[19];
        stringBPtr[24] = '-';
        stringBPtr[25] = stringN[20];
        stringBPtr[26] = stringN[21];
        stringBPtr[27] = stringN[22];
        stringBPtr[28] = stringN[23];
        stringBPtr[29] = stringN[24];
        stringBPtr[30] = stringN[25];
        stringBPtr[31] = stringN[26];
        stringBPtr[32] = stringN[27];
        stringBPtr[33] = stringN[28];
        stringBPtr[34] = stringN[29];
        stringBPtr[35] = stringN[30];
        stringBPtr[36] = stringN[31];
        stringBPtr[37] = '}';
        return new(stringBPtr, 0, 38);
    }

    public static unsafe string GetStringP(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        // (dddddddd-dddd-dddd-dddd-dddddddddddd)
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Uuid bytes count should be 16", nameof(bytes));
        }

        string stringN = GetStringN(bytes);
        char* stringPPtr = stackalloc char[38];
        stringPPtr[0] = '(';
        stringPPtr[1] = stringN[0];
        stringPPtr[2] = stringN[1];
        stringPPtr[3] = stringN[2];
        stringPPtr[4] = stringN[3];
        stringPPtr[5] = stringN[4];
        stringPPtr[6] = stringN[5];
        stringPPtr[7] = stringN[6];
        stringPPtr[8] = stringN[7];
        stringPPtr[9] = '-';
        stringPPtr[10] = stringN[8];
        stringPPtr[11] = stringN[9];
        stringPPtr[12] = stringN[10];
        stringPPtr[13] = stringN[11];
        stringPPtr[14] = '-';
        stringPPtr[15] = stringN[12];
        stringPPtr[16] = stringN[13];
        stringPPtr[17] = stringN[14];
        stringPPtr[18] = stringN[15];
        stringPPtr[19] = '-';
        stringPPtr[20] = stringN[16];
        stringPPtr[21] = stringN[17];
        stringPPtr[22] = stringN[18];
        stringPPtr[23] = stringN[19];
        stringPPtr[24] = '-';
        stringPPtr[25] = stringN[20];
        stringPPtr[26] = stringN[21];
        stringPPtr[27] = stringN[22];
        stringPPtr[28] = stringN[23];
        stringPPtr[29] = stringN[24];
        stringPPtr[30] = stringN[25];
        stringPPtr[31] = stringN[26];
        stringPPtr[32] = stringN[27];
        stringPPtr[33] = stringN[28];
        stringPPtr[34] = stringN[29];
        stringPPtr[35] = stringN[30];
        stringPPtr[36] = stringN[31];
        stringPPtr[37] = ')';
        return new(stringPPtr, 0, 38);
    }

    public static unsafe string GetStringX(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        // (dddddddd-dddd-dddd-dddd-dddddddddddd)
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Uuid bytes count should be 16", nameof(bytes));
        }

        string stringN = GetStringN(bytes);
        char* stringXPtr = stackalloc char[68];
        stringXPtr[0] = stringXPtr[26] = '{';
        stringXPtr[66] = stringXPtr[67] = '}';
        stringXPtr[1] = stringXPtr[12] = stringXPtr[19] = stringXPtr[27] = stringXPtr[32] =
            stringXPtr[37] = stringXPtr[42] = stringXPtr[47] = stringXPtr[52] = stringXPtr[57] = stringXPtr[62] = '0';

        stringXPtr[2] = stringXPtr[13] = stringXPtr[20] = stringXPtr[28] = stringXPtr[33] =
            stringXPtr[38] = stringXPtr[43] = stringXPtr[48] = stringXPtr[53] = stringXPtr[58] = stringXPtr[63] = 'x';

        stringXPtr[11] = stringXPtr[18] = stringXPtr[25] = stringXPtr[31] =
            stringXPtr[36] = stringXPtr[41] = stringXPtr[46] = stringXPtr[51] = stringXPtr[56] = stringXPtr[61] = ',';

        stringXPtr[3] = stringN[0];
        stringXPtr[4] = stringN[1];
        stringXPtr[5] = stringN[2];
        stringXPtr[6] = stringN[3];
        stringXPtr[7] = stringN[4];
        stringXPtr[8] = stringN[5];
        stringXPtr[9] = stringN[6];
        stringXPtr[10] = stringN[7];

        stringXPtr[14] = stringN[8];
        stringXPtr[15] = stringN[9];
        stringXPtr[16] = stringN[10];
        stringXPtr[17] = stringN[11];

        stringXPtr[21] = stringN[12];
        stringXPtr[22] = stringN[13];
        stringXPtr[23] = stringN[14];
        stringXPtr[24] = stringN[15];

        stringXPtr[29] = stringN[16];
        stringXPtr[30] = stringN[17];

        stringXPtr[34] = stringN[18];
        stringXPtr[35] = stringN[19];

        stringXPtr[39] = stringN[20];
        stringXPtr[40] = stringN[21];

        stringXPtr[44] = stringN[22];
        stringXPtr[45] = stringN[23];

        stringXPtr[49] = stringN[24];
        stringXPtr[50] = stringN[25];

        stringXPtr[54] = stringN[26];
        stringXPtr[55] = stringN[27];

        stringXPtr[59] = stringN[28];
        stringXPtr[60] = stringN[29];

        stringXPtr[64] = stringN[30];
        stringXPtr[65] = stringN[31];
        return new(stringXPtr, 0, 68);
    }

    public static UuidStringWithBytes[] GenerateNStrings()
    {
        List<string> resultStrings = new();
        for (int stringsToCreate = 32, itemsToFill = 1; stringsToCreate > 0; stringsToCreate >>= 1, itemsToFill <<= 1)
        {
            for (int stringIndex = 0; stringIndex < stringsToCreate; stringIndex++)
            {
                resultStrings.Add(
                    string.Create(
                        32,
                        (stringIndex * itemsToFill, itmesToFill: itemsToFill),
                        (result, state) =>
                        {
                            (int startPositionToFill, int itemsToFillCount) = state;
                            for (int j = 0; j < 32; j++)
                            {
                                result[j] = '0';
                            }

                            result[startPositionToFill] = '1';
                            for (int j = 0; j < itemsToFillCount; j++)
                            {
                                result[startPositionToFill + j] = '1';
                            }
                        }));
            }
        }

        for (int stringsToCreate = 32, itemsToFill = 1; stringsToCreate > 0; stringsToCreate >>= 1, itemsToFill <<= 1)
        {
            for (int stringIndex = 0; stringIndex < stringsToCreate; stringIndex++)
            {
                resultStrings.Add(
                    string.Create(
                        32,
                        (stringIndex * itemsToFill, itmesToFill: itemsToFill),
                        (result, state) =>
                        {
                            (int startPositionToFill, int itemsToFillCount) = state;
                            for (int j = 0; j < 32; j++)
                            {
                                result[j] = '1';
                            }

                            result[startPositionToFill] = '1';
                            for (int j = 0; j < itemsToFillCount; j++)
                            {
                                result[startPositionToFill + j] = '0';
                            }
                        }));
            }
        }

        string[] nStrings = resultStrings.Distinct().ToArray();
        UuidStringWithBytes[] output = new UuidStringWithBytes[nStrings.Length];
        for (int i = 0; i < nStrings.Length; i++)
        {
            byte[] bytes = ConvertHexStringToByteArray(nStrings[i]);
            output[i] = new(nStrings[i], bytes);
        }

        return output;
    }

    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public static UuidStringWithBytes[] GenerateDStrings()
    {
        UuidStringWithBytes[] nStrings = GenerateNStrings();
        UuidStringWithBytes[] dStrings = nStrings
            .Select(x => new UuidStringWithBytes(GetStringD(x.Bytes), x.Bytes))
            .ToArray();
        return dStrings;
    }

    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public static UuidStringWithBytes[] GenerateBStrings()
    {
        UuidStringWithBytes[] nStrings = GenerateNStrings();
        UuidStringWithBytes[] dStrings = nStrings
            .Select(x => new UuidStringWithBytes(GetStringB(x.Bytes), x.Bytes))
            .ToArray();
        return dStrings;
    }

    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public static UuidStringWithBytes[] GeneratePStrings()
    {
        UuidStringWithBytes[] nStrings = GenerateNStrings();
        UuidStringWithBytes[] dStrings = nStrings
            .Select(x => new UuidStringWithBytes(GetStringP(x.Bytes), x.Bytes))
            .ToArray();
        return dStrings;
    }

    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public static UuidStringWithBytes[] GenerateXStrings()
    {
        UuidStringWithBytes[] nStrings = GenerateNStrings();
        UuidStringWithBytes[] dStrings = nStrings
            .Select(x => new UuidStringWithBytes(GetStringX(x.Bytes), x.Bytes))
            .ToArray();
        return dStrings;
    }

    public static string[] GenerateBrokenNStringsArray()
    {
        return GenerateBrokenStringsArray(32, GetStringN);
    }

    public static string[] GenerateBrokenDStringsArray()
    {
        return GenerateBrokenStringsArray(36, GetStringD);
    }

    public static string[] GenerateBrokenBStringsArray()
    {
        return GenerateBrokenStringsArray(38, GetStringB);
    }

    public static string[] GenerateBrokenPStringsArray()
    {
        return GenerateBrokenStringsArray(38, GetStringP);
    }

    public static string[] GenerateBrokenXStringsArray()
    {
        return GenerateBrokenStringsArray(68, GetStringX);
    }

    private static unsafe string[] GenerateBrokenStringsArray(
        int outputFormatSize,
        Func<byte[], string> formatString)
    {
        int count = outputFormatSize * 2;
        UuidRng rng = new(1337);
        int* uuidIntegers = stackalloc int[4];
        char* charToBreakPtr = stackalloc char[1];
        byte* charBytesPtr = (byte*) charToBreakPtr;
        string[] result = new string[count];
        bool[] breakUpperByteOnCharArray = new bool[outputFormatSize];
        for (int i = 0; i < breakUpperByteOnCharArray.Length; i++)
        {
            breakUpperByteOnCharArray[i] = false;
        }

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                uuidIntegers[j] = rng.Next();
            }

            byte[] bytesOfUuid = new ReadOnlySpan<byte>(uuidIntegers, 16).ToArray();
            string uuidString = formatString(bytesOfUuid);
            Span<char> spanOfString = MemoryMarshal.CreateSpan(
                ref MemoryMarshal.GetReference(uuidString.AsSpan()),
                uuidString.Length);
            int brokenCharIndex = i % outputFormatSize;
            bool shouldBreakUpperByte = breakUpperByteOnCharArray[brokenCharIndex];
            breakUpperByteOnCharArray[brokenCharIndex] = !shouldBreakUpperByte;
            charToBreakPtr[0] = uuidString[brokenCharIndex];
            if (shouldBreakUpperByte)
            {
                charBytesPtr[0] = 110;
            }
            else
            {
                charBytesPtr[1] = 110;
            }

            spanOfString[brokenCharIndex] = charToBreakPtr[0];
            result[i] = uuidString;
        }

        return result;
    }
}
