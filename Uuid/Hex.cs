using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Uuid
{
    public static unsafe class Hex
    {
        private const ushort MaximalChar = 103;

        private static readonly uint* TableToHex;
        private static readonly byte* TableFromHexToBytes;

        private static readonly Func<int, string> FastAllocateString;

        static Hex()
        {
            TableToHex = (uint*) Marshal.AllocHGlobal(sizeof(uint) * 256).ToPointer();
            for (var i = 0; i < 256; i++)
            {
                var chars = Convert.ToString(i, 16).PadLeft(2, '0');
                TableToHex[i] = ((uint) chars[1] << 16) | chars[0];
            }

            TableFromHexToBytes = (byte*) Marshal.AllocHGlobal(103).ToPointer();
            for (var i = 0; i < 103; i++)
                TableFromHexToBytes[i] = (char) i switch
                {
                    '0' => (byte) 0x0,
                    '1' => (byte) 0x1,
                    '2' => (byte) 0x2,
                    '3' => (byte) 0x3,
                    '4' => (byte) 0x4,
                    '5' => (byte) 0x5,
                    '6' => (byte) 0x6,
                    '7' => (byte) 0x7,
                    '8' => (byte) 0x8,
                    '9' => (byte) 0x9,
                    'a' => (byte) 0xa,
                    'A' => (byte) 0xa,
                    'b' => (byte) 0xb,
                    'B' => (byte) 0xb,
                    'c' => (byte) 0xc,
                    'C' => (byte) 0xc,
                    'd' => (byte) 0xd,
                    'D' => (byte) 0xd,
                    'e' => (byte) 0xe,
                    'E' => (byte) 0xe,
                    'f' => (byte) 0xf,
                    'F' => (byte) 0xf,
                    _ => byte.MaxValue
                };
#nullable disable
            // ReSharper disable once PossibleNullReferenceException
            FastAllocateString = (Func<int, string>) typeof(string)
                .GetMethod(nameof(FastAllocateString), BindingFlags.Static | BindingFlags.NonPublic)
                .CreateDelegate(typeof(Func<int, string>));
#nullable restore
        }

        public static bool IsHexString(string? possibleHexString)
        {
            if (string.IsNullOrWhiteSpace(possibleHexString))
                return false;
            if (possibleHexString.Length % 2 > 0)
                return false;
            var length = possibleHexString.Length;
            fixed (char* stringPtr = possibleHexString)
            {
                for (var i = 0; i < length;)
                    if (stringPtr[i] < MaximalChar
                        && TableFromHexToBytes[stringPtr[i]] != 0xFF
                        && stringPtr[i + 1] < MaximalChar
                        && TableFromHexToBytes[stringPtr[i + 1]] != 0xFF)
                        i += 2;
                    else
                        return false;
            }

            return true;
        }

        public static byte[]? GetBytes(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
                return null;
            if (hexString.Length % 2 > 0)
                return null;
            var length = hexString.Length;
            var result = new byte[length / 2];
            fixed (char* stringPtr = hexString)
            fixed (byte* resultPtr = result)
            {
                var resultIndex = 0;
                for (var i = 0; i < length;)
                {
                    byte hexByteHi;
                    byte hexByteLow;
                    if (stringPtr[i] < MaximalChar
                        && (hexByteHi = TableFromHexToBytes[stringPtr[i]]) != 0xFF
                        && stringPtr[i + 1] < MaximalChar
                        && (hexByteLow = TableFromHexToBytes[stringPtr[i + 1]]) != 0xFF)
                    {
                        
                        var resultByte = (byte) ((byte) (hexByteHi >> 4) | hexByteLow);
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

        public static string? GetString(byte[] bytes)
        {
            if (bytes == null)
                return null;
            if (bytes.Length == 0)
                return string.Empty;
            var resultString = FastAllocateString(bytes.Length * 2);
            fixed (char* charsPtr = resultString)
            {
                var destUints = (uint*) charsPtr;
                for (var i = 0; i < bytes.Length; i++) destUints[i] = TableToHex[bytes[i]];
            }

            return resultString;
        }
    }
}