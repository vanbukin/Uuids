using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text.Json.Serialization;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace Uuids;

/// <summary>
///     Represents a universally unique identifier (UUID).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[TypeConverter(typeof(UuidTypeConverter))]
[JsonConverter(typeof(SystemTextJsonUuidJsonConverter))]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
public unsafe struct Uuid :
    ISpanFormattable,
    IComparable,
    IComparable<Uuid>,
    IEquatable<Uuid>
#if NET7_0_OR_GREATER
    , ISpanParsable<Uuid>, IComparisonOperators<Uuid, Uuid, bool>, IEqualityOperators<Uuid, Uuid, bool>
#endif
{
#pragma warning disable CA2207
    static Uuid()

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
#pragma warning restore CA2207
    private const ushort MaximalChar = 103;

    private static readonly uint* TableToHex;
    private static readonly byte* TableFromHexToBytes;

#pragma warning disable CA1805
    /// <summary>
    ///     A read-only instance of the <see cref="Uuid" /> structure whose value is all zeros.
    /// </summary>
    public static readonly Uuid Empty = new();
#pragma warning restore CA1805

    private readonly byte _byte0;
    private readonly byte _byte1;
    private readonly byte _byte2;
    private readonly byte _byte3;
    private readonly byte _byte4;
    private readonly byte _byte5;
    private readonly byte _byte6;
    private readonly byte _byte7;
    private readonly byte _byte8;
    private readonly byte _byte9;
    private readonly byte _byte10;
    private readonly byte _byte11;
    private readonly byte _byte12;
    private readonly byte _byte13;
    private readonly byte _byte14;
    private readonly byte _byte15;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Uuid" /> structure by using the specified array of bytes.
    /// </summary>
    /// <param name="bytes">A 16-element byte array containing values with which to initialize the <see cref="Uuid" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="bytes" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="bytes" /> is not 16 bytes long.</exception>
    public Uuid(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Byte array for Uuid must be exactly 16 bytes long.", nameof(bytes));
        }

        this = Unsafe.ReadUnaligned<Uuid>(ref MemoryMarshal.GetReference(bytes.AsSpan()));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Uuid" /> structure by using the specified byte pointer.
    /// </summary>
    /// <param name="bytes">A byte pointer containing bytes which used to initialize the <see cref="Uuid" />.</param>
    public Uuid(byte* bytes)
    {
        this = Unsafe.ReadUnaligned<Uuid>(bytes);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Uuid" /> structure by using the value represented by the specified read-only span of bytes.
    /// </summary>
    /// <param name="bytes">A read-only span containing the bytes representing the <see cref="Uuid" />. The span must be exactly 16 bytes long.</param>
    /// <exception cref="ArgumentException"><paramref name="bytes" /> is not 16 bytes long.</exception>
    public Uuid(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Byte array for Uuid must be exactly 16 bytes long.", nameof(bytes));
        }

        this = Unsafe.ReadUnaligned<Uuid>(ref MemoryMarshal.GetReference(bytes));
    }

    /// <summary>
    ///     Returns a 16-element byte array that contains the value of this instance.
    /// </summary>
    /// <returns>A 16-element byte array.</returns>
    public byte[] ToByteArray()
    {
        byte[] result = new byte[16];
        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(new Span<byte>(result)), this);
        return result;
    }

    /// <summary>
    ///     Tries to write the current <see cref="Uuid" /> instance into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns <see langword="true" />, the <see cref="Uuid" /> as a span of bytes.</param>
    /// <returns><see langword="true" /> if the <see cref="Uuid" /> is successfully written to the specified span; <see langword="false" /> otherwise.</returns>
    public bool TryWriteBytes(Span<byte> destination)
    {
        if (Unsafe.SizeOf<Uuid>() > (uint) destination.Length)
        {
            return false;
        }

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);
        return true;
    }

    /// <summary>
    ///     Compares this instance to a specified object or <see cref="Uuid" /> and returns an indication of their relative values.
    /// </summary>
    /// <param name="obj">An object to compare, or <see langword="null" />.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="obj" />.</returns>
    /// <exception cref="ArgumentException"><paramref name="obj" /> must be of type <see cref="Uuid" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is not Uuid other)
        {
            throw new ArgumentException("Object must be of type Uuid.", nameof(obj));
        }

        if (other._byte0 != _byte0)
        {
            return _byte0 < other._byte0 ? -1 : 1;
        }

        if (other._byte1 != _byte1)
        {
            return _byte1 < other._byte1 ? -1 : 1;
        }

        if (other._byte2 != _byte2)
        {
            return _byte2 < other._byte2 ? -1 : 1;
        }

        if (other._byte3 != _byte3)
        {
            return _byte3 < other._byte3 ? -1 : 1;
        }

        if (other._byte4 != _byte4)
        {
            return _byte4 < other._byte4 ? -1 : 1;
        }

        if (other._byte5 != _byte5)
        {
            return _byte5 < other._byte5 ? -1 : 1;
        }

        if (other._byte6 != _byte6)
        {
            return _byte6 < other._byte6 ? -1 : 1;
        }

        if (other._byte7 != _byte7)
        {
            return _byte7 < other._byte7 ? -1 : 1;
        }

        if (other._byte8 != _byte8)
        {
            return _byte8 < other._byte8 ? -1 : 1;
        }

        if (other._byte9 != _byte9)
        {
            return _byte9 < other._byte9 ? -1 : 1;
        }

        if (other._byte10 != _byte10)
        {
            return _byte10 < other._byte10 ? -1 : 1;
        }

        if (other._byte11 != _byte11)
        {
            return _byte11 < other._byte11 ? -1 : 1;
        }

        if (other._byte12 != _byte12)
        {
            return _byte12 < other._byte12 ? -1 : 1;
        }

        if (other._byte13 != _byte13)
        {
            return _byte13 < other._byte13 ? -1 : 1;
        }

        if (other._byte14 != _byte14)
        {
            return _byte14 < other._byte14 ? -1 : 1;
        }

        if (other._byte15 != _byte15)
        {
            return _byte15 < other._byte15 ? -1 : 1;
        }

        return 0;
    }

    /// <summary>
    ///     Compares this instance to a specified <see cref="Uuid" /> object and returns an indication of their relative values.
    /// </summary>
    /// <param name="other">An <see cref="Uuid" /> object to compare to this instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="other" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public int CompareTo(Uuid other)
    {
        if (other._byte0 != _byte0)
        {
            return _byte0 < other._byte0 ? -1 : 1;
        }

        if (other._byte1 != _byte1)
        {
            return _byte1 < other._byte1 ? -1 : 1;
        }

        if (other._byte2 != _byte2)
        {
            return _byte2 < other._byte2 ? -1 : 1;
        }

        if (other._byte3 != _byte3)
        {
            return _byte3 < other._byte3 ? -1 : 1;
        }

        if (other._byte4 != _byte4)
        {
            return _byte4 < other._byte4 ? -1 : 1;
        }

        if (other._byte5 != _byte5)
        {
            return _byte5 < other._byte5 ? -1 : 1;
        }

        if (other._byte6 != _byte6)
        {
            return _byte6 < other._byte6 ? -1 : 1;
        }

        if (other._byte7 != _byte7)
        {
            return _byte7 < other._byte7 ? -1 : 1;
        }

        if (other._byte8 != _byte8)
        {
            return _byte8 < other._byte8 ? -1 : 1;
        }

        if (other._byte9 != _byte9)
        {
            return _byte9 < other._byte9 ? -1 : 1;
        }

        if (other._byte10 != _byte10)
        {
            return _byte10 < other._byte10 ? -1 : 1;
        }

        if (other._byte11 != _byte11)
        {
            return _byte11 < other._byte11 ? -1 : 1;
        }

        if (other._byte12 != _byte12)
        {
            return _byte12 < other._byte12 ? -1 : 1;
        }

        if (other._byte13 != _byte13)
        {
            return _byte13 < other._byte13 ? -1 : 1;
        }

        if (other._byte14 != _byte14)
        {
            return _byte14 < other._byte14 ? -1 : 1;
        }

        if (other._byte15 != _byte15)
        {
            return _byte15 < other._byte15 ? -1 : 1;
        }

        return 0;
    }

    /// <summary>
    ///     Returns a value that indicates whether two instances of <see cref="Uuid" /> represent the same value.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><see langword="true" /> if <paramref name="obj" /> is <see cref="Uuid" /> that has the same value as this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Uuid other)
        {
#if NET7_0_OR_GREATER
            if (Vector128.IsHardwareAccelerated)
            {
                return Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in this))) == Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in other)));
            }
#endif
            ref long rA = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in this));
            ref long rB = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in other));

            // Compare each element
            return rA == rB && Unsafe.Add(ref rA, 1) == Unsafe.Add(ref rB, 1);
        }

        return false;
    }

    /// <summary>
    ///     Returns a value indicating whether this instance and a specified <see cref="Uuid" /> object represent the same value.
    /// </summary>
    /// <param name="other">An object to compare to this instance.</param>
    /// <returns><see langword="true" /> if <paramref name="other" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(Uuid other)
    {
#if NET7_0_OR_GREATER
        if (Vector128.IsHardwareAccelerated)
        {
            return Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in this))) == Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in other)));
        }
#endif
        ref long rA = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in this));
        ref long rB = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in other));

        // Compare each element
        return rA == rB && Unsafe.Add(ref rA, 1) == Unsafe.Add(ref rB, 1);
    }

    /// <summary>
    ///     Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
    {
        ref int r = ref Unsafe.As<Uuid, int>(ref Unsafe.AsRef(in this));
        return r ^ Unsafe.Add(ref r, 1) ^ Unsafe.Add(ref r, 2) ^ Unsafe.Add(ref r, 3);
    }

    /// <summary>
    ///     Indicates whether the values of two specified <see cref="Uuid" /> objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Uuid left, Uuid right)
    {
#if NET7_0_OR_GREATER
        if (Vector128.IsHardwareAccelerated)
        {
            return Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in left))) == Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in right)));
        }
#endif
        ref long rA = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in left));
        ref long rB = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in right));

        // Compare each element
        return rA == rB && Unsafe.Add(ref rA, 1) == Unsafe.Add(ref rB, 1);
    }

    /// <summary>
    ///     Indicates whether the values of two specified <see cref="Uuid" /> objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Uuid left, Uuid right)
    {
#if NET7_0_OR_GREATER
        if (Vector128.IsHardwareAccelerated)
        {
            return Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in left))) != Vector128.LoadUnsafe(ref Unsafe.As<Uuid, byte>(ref Unsafe.AsRef(in right)));
        }
#endif
        ref long rA = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in left));
        ref long rB = ref Unsafe.As<Uuid, long>(ref Unsafe.AsRef(in right));

        // Compare each element
        return rA != rB || Unsafe.Add(ref rA, 1) != Unsafe.Add(ref rB, 1);
    }

    /// <summary>
    ///     Tries to format the value of the current instance into the provided span of characters.
    /// </summary>
    /// <param name="destination">When this method returns <see langword="true" />, the <see cref="Uuid" /> as a span of characters.</param>
    /// <param name="charsWritten">When this method returns <see langword="true" />, the number of characters written in <paramref name="destination" />.</param>
    /// <param name="format">
    ///     A read-only span containing the character representing one of the following specifiers that indicates how to format the value of this <see cref="Uuid" />. The format parameter can be "N", "D", "B", "P", or "X". If format is <see langword="null" /> or an
    ///     empty string (""), "N" is used.
    /// </param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns><see langword="true" /> if the formatting operation was successful; <see langword="false" /> otherwise.</returns>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (format.Length == 0)
        {
            format = "N";
        }

        if (format.Length != 1)
        {
            charsWritten = 0;
            return false;
        }

        switch ((char) (format[0] | 0x20))
        {
            case 'n':
                {
                    if (destination.Length < 32)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatN(uuidChars);
                    }

                    charsWritten = 32;
                    return true;
                }
            case 'd':
                {
                    if (destination.Length < 36)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatD(uuidChars);
                    }

                    charsWritten = 36;
                    return true;
                }
            case 'b':
                {
                    if (destination.Length < 38)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatB(uuidChars);
                    }

                    charsWritten = 38;
                    return true;
                }
            case 'p':
                {
                    if (destination.Length < 38)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatP(uuidChars);
                    }

                    charsWritten = 38;
                    return true;
                }
            case 'x':
                {
                    if (destination.Length < 68)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatX(uuidChars);
                    }

                    charsWritten = 68;
                    return true;
                }
            default:
                {
                    charsWritten = 0;
                    return false;
                }
        }
    }

    /// <summary>
    ///     Tries to format the value of the current instance into the provided span of characters.
    /// </summary>
    /// <param name="destination">When this method returns <see langword="true" />, the <see cref="Uuid" /> as a span of characters.</param>
    /// <param name="charsWritten">When this method returns <see langword="true" />, the number of characters written in <paramref name="destination" />.</param>
    /// <param name="format">
    ///     A read-only span containing the character representing one of the following specifiers that indicates how to format the value of this <see cref="Uuid" />. The format parameter can be "N", "D", "B", "P", or "X". If format is <see langword="null" /> or an
    ///     empty string (""), "N" is used.
    /// </param>
    /// <returns><see langword="true" /> if the formatting operation was successful; <see langword="false" /> otherwise.</returns>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        ReadOnlySpan<char> format = default)
    {
        if (format.Length == 0)
        {
            format = "N";
        }

        if (format.Length != 1)
        {
            charsWritten = 0;
            return false;
        }

        switch ((char) (format[0] | 0x20))
        {
            case 'n':
                {
                    if (destination.Length < 32)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatN(uuidChars);
                    }

                    charsWritten = 32;
                    return true;
                }
            case 'd':
                {
                    if (destination.Length < 36)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatD(uuidChars);
                    }

                    charsWritten = 36;
                    return true;
                }
            case 'b':
                {
                    if (destination.Length < 38)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatB(uuidChars);
                    }

                    charsWritten = 38;
                    return true;
                }
            case 'p':
                {
                    if (destination.Length < 38)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatP(uuidChars);
                    }

                    charsWritten = 38;
                    return true;
                }
            case 'x':
                {
                    if (destination.Length < 68)
                    {
                        charsWritten = 0;
                        return false;
                    }

                    fixed (char* uuidChars = &destination.GetPinnableReference())
                    {
                        FormatX(uuidChars);
                    }

                    charsWritten = 68;
                    return true;
                }
            default:
                {
                    charsWritten = 0;
                    return false;
                }
        }
    }

    /// <summary>
    ///     Returns a string representation of the value of this instance.
    /// </summary>
    /// <returns>The value of this <see cref="Uuid" />, formatted by using the "N" format specifier as follows: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</returns>
    public override string ToString()
    {
        return ToString("N", null);
    }

    /// <summary>
    ///     Returns a string representation of the value of this <see cref="Uuid" /> instance, according to the provided format specifier.
    /// </summary>
    /// <param name="format">A single format specifier that indicates how to format the value of this <see cref="Uuid" />. The format parameter can be "N", "D", "B", "P", or "X". If format is <see langword="null" /> or an empty string (""), "N" is used.</param>
    /// <returns>The value of this <see cref="Uuid" />, represented as a series of lowercase hexadecimal digits in the specified format.</returns>
    public string ToString(
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        string? format)
    {
        // ReSharper disable once IntroduceOptionalParameters.Global
        return ToString(format, null);
    }

    /// <summary>
    ///     Returns a string representation of the value of this <see cref="Uuid" /> instance, according to the provided format specifier and culture-specific format information.
    /// </summary>
    /// <param name="format">A single format specifier that indicates how to format the value of this <see cref="Uuid" />. The format parameter can be "N", "D", "B", "P", or "X". If format is null or an empty string (""), "N" is used.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The value of this <see cref="Uuid" />, represented as a series of lowercase hexadecimal digits in the specified format.</returns>
    /// <exception cref="FormatException">The value of <paramref name="format" /> is not <see langword="null" />, an empty string (""), "N", "D", "B", "P", or "X".</exception>
    public string ToString(
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        string? format,
        IFormatProvider? formatProvider)
    {
        format ??= "N";

        if (string.IsNullOrEmpty(format))
        {
            format = "N";
        }

        if (format.Length != 1)
        {
            throw new FormatException(
                "Format string can be only \"N\", \"n\", \"D\", \"d\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
        }

        switch ((char) (format[0] | 0x20))
        {
            case 'n':
                {
                    string uuidString = new('\0', 32);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatN(uuidChars);
                    }

                    return uuidString;
                }
            case 'd':
                {
                    string uuidString = new('\0', 36);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatD(uuidChars);
                    }

                    return uuidString;
                }
            case 'b':
                {
                    string uuidString = new('\0', 38);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatB(uuidChars);
                    }

                    return uuidString;
                }
            case 'p':
                {
                    string uuidString = new('\0', 38);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatP(uuidChars);
                    }

                    return uuidString;
                }
            case 'x':
                {
                    string uuidString = new('\0', 68);
                    fixed (char* uuidChars = &uuidString.GetPinnableReference())
                    {
                        FormatX(uuidChars);
                    }

                    return uuidString;
                }
            default:
                throw new FormatException(
                    "Format string can be only \"N\", \"n\", \"D\", \"d\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private void FormatN(char* dest)
    {
        // dddddddddddddddddddddddddddddddd
        if (Avx2.IsSupported)
        {
            fixed (Uuid* thisPtr = &this)
            {
                Vector256<short> uuidVector = Avx2.ConvertToVector256Int16(Sse3.LoadDquVector128((byte*) thisPtr));
                Vector256<byte> hi = Avx2.ShiftRightLogical(uuidVector, 4).AsByte();
                Vector256<byte> lo = Avx2.Shuffle(uuidVector.AsByte(),
                    Vector256.Create(
                        255, 0, 255, 2, 255, 4, 255, 6, 255, 8, 255, 10, 255, 12, 255, 14,
                        255, 0, 255, 2, 255, 4, 255, 6, 255, 8, 255, 10, 255, 12, 255, 14));
                Vector256<byte> asciiBytes = Avx2.Shuffle(
                    Vector256.Create(
                        (byte) 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102,
                        48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102),
                    Avx2.And(Avx2.Or(hi, lo), Vector256.Create((byte) 0x0F)));
                Avx.Store((short*) dest, Avx2.ConvertToVector256Int16(asciiBytes.GetLower()));
                Avx.Store((short*) dest + 16, Avx2.ConvertToVector256Int16(asciiBytes.GetUpper()));
            }
        }
        else
        {
            uint* destUints = (uint*) dest;
            destUints[0] = TableToHex[_byte0];
            destUints[1] = TableToHex[_byte1];
            destUints[2] = TableToHex[_byte2];
            destUints[3] = TableToHex[_byte3];
            destUints[4] = TableToHex[_byte4];
            destUints[5] = TableToHex[_byte5];
            destUints[6] = TableToHex[_byte6];
            destUints[7] = TableToHex[_byte7];
            destUints[8] = TableToHex[_byte8];
            destUints[9] = TableToHex[_byte9];
            destUints[10] = TableToHex[_byte10];
            destUints[11] = TableToHex[_byte11];
            destUints[12] = TableToHex[_byte12];
            destUints[13] = TableToHex[_byte13];
            destUints[14] = TableToHex[_byte14];
            destUints[15] = TableToHex[_byte15];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private void FormatD(char* dest)
    {
        // dddddddd-dddd-dddd-dddd-dddddddddddd
        uint* destUints = (uint*) dest;
        char** destUintsAsChars = (char**) &destUints;
        dest[8] = dest[13] = dest[18] = dest[23] = '-';
        destUints[0] = TableToHex[_byte0];
        destUints[1] = TableToHex[_byte1];
        destUints[2] = TableToHex[_byte2];
        destUints[3] = TableToHex[_byte3];
        destUints[7] = TableToHex[_byte6];
        destUints[8] = TableToHex[_byte7];
        destUints[12] = TableToHex[_byte10];
        destUints[13] = TableToHex[_byte11];
        destUints[14] = TableToHex[_byte12];
        destUints[15] = TableToHex[_byte13];
        destUints[16] = TableToHex[_byte14];
        destUints[17] = TableToHex[_byte15];
        *destUintsAsChars += 1;
        destUints[4] = TableToHex[_byte4];
        destUints[5] = TableToHex[_byte5];
        destUints[9] = TableToHex[_byte8];
        destUints[10] = TableToHex[_byte9];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private void FormatB(char* dest)
    {
        // {dddddddd-dddd-dddd-dddd-dddddddddddd}
        uint* destUints = (uint*) dest;
        char** destUintsAsChars = (char**) &destUints;
        dest[0] = '{';
        dest[9] = dest[14] = dest[19] = dest[24] = '-';
        dest[37] = '}';
        destUints[5] = TableToHex[_byte4];
        destUints[6] = TableToHex[_byte5];
        destUints[10] = TableToHex[_byte8];
        destUints[11] = TableToHex[_byte9];
        *destUintsAsChars += 1;
        destUints[0] = TableToHex[_byte0];
        destUints[1] = TableToHex[_byte1];
        destUints[2] = TableToHex[_byte2];
        destUints[3] = TableToHex[_byte3];
        destUints[7] = TableToHex[_byte6];
        destUints[8] = TableToHex[_byte7];
        destUints[12] = TableToHex[_byte10];
        destUints[13] = TableToHex[_byte11];
        destUints[14] = TableToHex[_byte12];
        destUints[15] = TableToHex[_byte13];
        destUints[16] = TableToHex[_byte14];
        destUints[17] = TableToHex[_byte15];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private void FormatP(char* dest)
    {
        // (dddddddd-dddd-dddd-dddd-dddddddddddd)
        uint* destUints = (uint*) dest;
        char** destUintsAsChars = (char**) &destUints;
        dest[0] = '(';
        dest[9] = dest[14] = dest[19] = dest[24] = '-';
        dest[37] = ')';
        destUints[5] = TableToHex[_byte4];
        destUints[6] = TableToHex[_byte5];
        destUints[10] = TableToHex[_byte8];
        destUints[11] = TableToHex[_byte9];
        *destUintsAsChars += 1;
        destUints[0] = TableToHex[_byte0];
        destUints[1] = TableToHex[_byte1];
        destUints[2] = TableToHex[_byte2];
        destUints[3] = TableToHex[_byte3];
        destUints[7] = TableToHex[_byte6];
        destUints[8] = TableToHex[_byte7];
        destUints[12] = TableToHex[_byte10];
        destUints[13] = TableToHex[_byte11];
        destUints[14] = TableToHex[_byte12];
        destUints[15] = TableToHex[_byte13];
        destUints[16] = TableToHex[_byte14];
        destUints[17] = TableToHex[_byte15];
    }

    private const uint ZeroX = 7864368; // 0x
    private const uint CommaBrace = 8060972; // ,{
    private const uint CloseBraces = 8192125; // }}

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private void FormatX(char* dest)
    {
        // {0xdddddddd,0xdddd,0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
        uint* destUints = (uint*) dest;
        char** uintDestAsChars = (char**) &destUints;
        dest[0] = '{';
        dest[11] = dest[18] = dest[31] = dest[36] = dest[41] = dest[46] = dest[51] = dest[56] = dest[61] = ',';
        destUints[6] = destUints[16] = destUints[21] = destUints[26] = destUints[31] = ZeroX; // 0x
        destUints[7] = TableToHex[_byte4];
        destUints[8] = TableToHex[_byte5];
        destUints[17] = TableToHex[_byte9];
        destUints[22] = TableToHex[_byte11];
        destUints[27] = TableToHex[_byte13];
        destUints[32] = TableToHex[_byte15];
        destUints[33] = CloseBraces; // }}
        *uintDestAsChars += 1;
        destUints[0] = destUints[9] = destUints[13] = destUints[18] = destUints[23] = destUints[28] = ZeroX; // 0x
        destUints[1] = TableToHex[_byte0];
        destUints[2] = TableToHex[_byte1];
        destUints[3] = TableToHex[_byte2];
        destUints[4] = TableToHex[_byte3];
        destUints[10] = TableToHex[_byte6];
        destUints[11] = TableToHex[_byte7];
        destUints[12] = CommaBrace; // ,{
        destUints[14] = TableToHex[_byte8];
        destUints[19] = TableToHex[_byte10];
        destUints[24] = TableToHex[_byte12];
        destUints[29] = TableToHex[_byte14];
    }

    /// <summary>
    ///     Converts <see cref="Uuid" /> to <see cref="System.Guid" /> preserve same binary representation.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
    public Guid ToGuidByteLayout()
    {
        Guid result = new();
        Guid* resultPtr = &result;
        byte* resultPtrBytes = (byte*) resultPtr;
        resultPtrBytes[0] = _byte0;
        resultPtrBytes[1] = _byte1;
        resultPtrBytes[2] = _byte2;
        resultPtrBytes[3] = _byte3;
        resultPtrBytes[4] = _byte4;
        resultPtrBytes[5] = _byte5;
        resultPtrBytes[6] = _byte6;
        resultPtrBytes[7] = _byte7;
        resultPtrBytes[8] = _byte8;
        resultPtrBytes[9] = _byte9;
        resultPtrBytes[10] = _byte10;
        resultPtrBytes[11] = _byte11;
        resultPtrBytes[12] = _byte12;
        resultPtrBytes[13] = _byte13;
        resultPtrBytes[14] = _byte14;
        resultPtrBytes[15] = _byte15;
        return result;
    }

    /// <summary>
    ///     Converts <see cref="Uuid" /> to <see cref="System.Guid" /> preserve same string representation.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
    public Guid ToGuidStringLayout()
    {
        Guid result = new();
        Guid* resultPtr = &result;
        byte* resultPtrBytes = (byte*) resultPtr;
        resultPtrBytes[0] = _byte3;
        resultPtrBytes[1] = _byte2;
        resultPtrBytes[2] = _byte1;
        resultPtrBytes[3] = _byte0;
        resultPtrBytes[4] = _byte5;
        resultPtrBytes[5] = _byte4;
        resultPtrBytes[6] = _byte7;
        resultPtrBytes[7] = _byte6;
        resultPtrBytes[8] = _byte8;
        resultPtrBytes[9] = _byte9;
        resultPtrBytes[10] = _byte10;
        resultPtrBytes[11] = _byte11;
        resultPtrBytes[12] = _byte12;
        resultPtrBytes[13] = _byte13;
        resultPtrBytes[14] = _byte14;
        resultPtrBytes[15] = _byte15;
        return result;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Uuid" /> structure by using the value represented by the specified string.
    /// </summary>
    /// <param name="input">A string that contains a UUID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="FormatException"><paramref name="input" /> is not in the correct format.</exception>
    public Uuid(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &input.GetPinnableReference())
        {
            ParseWithExceptions(new(uuidStringPtr, input.Length), uuidStringPtr, resultPtr);
        }

        this = result;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Uuid" /> structure by using the value represented by the specified read-only span of characters.
    /// </summary>
    /// <param name="input">A read-only span of characters that contains a UUID.</param>
    /// <exception cref="FormatException"><paramref name="input" /> is empty or contains unrecognized <see cref="Uuid" /> format.</exception>
    public Uuid(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty)
        {
            throw new FormatException("Unrecognized Uuid format.");
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &input.GetPinnableReference())
        {
            ParseWithExceptions(input, uuidStringPtr, resultPtr);
        }

        this = result;
    }

    /// <summary>
    ///     Converts the string representation of a UUID to the equivalent <see cref="Uuid" /> structure.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="FormatException"><paramref name="input" /> is not in the correct format.</exception>
    public static Uuid Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &input.GetPinnableReference())
        {
            ParseWithExceptions(new(uuidStringPtr, input.Length), uuidStringPtr, resultPtr);
        }

        return result;
    }

    /// <summary>
    ///     Converts a read-only character span that represents a UUID to the equivalent <see cref="Uuid" /> structure.
    /// </summary>
    /// <param name="input">A read-only span containing the bytes representing a <see cref="Uuid" />.</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    /// <exception cref="FormatException"><paramref name="input" /> is not in a recognized format.</exception>
    public static Uuid Parse(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty)
        {
            throw new FormatException("Unrecognized Uuid format.");
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &input.GetPinnableReference())
        {
            ParseWithExceptions(input, uuidStringPtr, resultPtr);
        }

        return result;
    }

    /// <summary>
    ///     Converts the string representation of a <see cref="Uuid" /> to the equivalent <see cref="Uuid" /> structure, provided that the string is in the specified format.
    /// </summary>
    /// <param name="input">The <see cref="Uuid" /> to convert.</param>
    /// <param name="format">One of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="input" /> or <paramref name="format" /> is <see langword="null" />.</exception>
    /// <exception cref="FormatException"><paramref name="input" /> is not in the format specified by <paramref name="format" />.</exception>
    public static Uuid ParseExact(
        string input,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        string format)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(format);

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        switch ((char) (format[0] | 0x20))
        {
            case 'n':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsN((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'd':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsD((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'b':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsB((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'p':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsP((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'x':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsX((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            default:
                {
                    throw new FormatException(
                        "Format string can be only \"N\", \"n\", \"D\", \"d\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
                }
        }
    }

    /// <summary>
    ///     Converts the character span representation of a <see cref="Uuid" /> to the equivalent <see cref="Uuid" /> structure, provided that the string is in the specified format.
    /// </summary>
    /// <param name="input">A read-only span containing the characters representing the <see cref="Uuid" /> to convert.</param>
    /// <param name="format">A read-only span of characters representing one of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    /// <exception cref="FormatException"><paramref name="input" /> is not in the format specified by <paramref name="format" />.</exception>
    public static Uuid ParseExact(
        ReadOnlySpan<char> input,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        ReadOnlySpan<char> format)
    {
        if (input.IsEmpty)
        {
            throw new FormatException("Unrecognized Uuid format.");
        }

        if (format.Length != 1)
        {
            throw new FormatException(
                "Format string can be only \"N\", \"n\", \"D\", \"d\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        switch ((char) (format[0] | 0x20))
        {
            case 'n':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsN((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'd':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsD((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'b':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsB((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'p':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsP((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            case 'x':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        ParseWithExceptionsX((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    return result;
                }
            default:
                {
                    throw new FormatException(
                        "Format string can be only \"N\", \"n\", \"D\", \"d\", \"P\", \"p\", \"B\", \"b\", \"X\" or \"x\".");
                }
        }
    }

    /// <summary>
    ///     Converts the string representation of a UUID to the equivalent <see cref="Uuid" /> structure.
    /// </summary>
    /// <param name="input">A string containing the UUID to convert.</param>
    /// <param name="output">
    ///     A <see cref="Uuid" /> instance to contain the parsed value. If the method returns <see langword="true" />, <paramref name="output" /> contains a valid <see cref="Uuid" />. If the method returns <see langword="false" />, <paramref name="output" /> equals
    ///     <see cref="Empty" />.
    /// </param>
    /// <returns><see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? input, out Uuid output)
    {
        if (input == null)
        {
            output = default;
            return false;
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &input.GetPinnableReference())
        {
            if (ParseWithoutExceptions(input.AsSpan(), uuidStringPtr, resultPtr))
            {
                output = result;
                return true;
            }
        }

        output = default;
        return false;
    }

    /// <summary>
    ///     Converts the specified read-only span of characters containing the representation of a UUID to the equivalent <see cref="Uuid" /> structure.
    /// </summary>
    /// <param name="input">A span containing the characters representing the UUID to convert.</param>
    /// <param name="output">
    ///     A <see cref="Uuid" /> instance to contain the parsed value. If the method returns <see langword="true" />, <paramref name="output" /> contains a valid <see cref="Uuid" />. If the method returns <see langword="false" />, <paramref name="output" /> equals
    ///     <see cref="Empty" />.
    /// </param>
    /// <returns><see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> input, out Uuid output)
    {
        if (input.IsEmpty)
        {
            output = default;
            return false;
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &input.GetPinnableReference())
        {
            if (ParseWithoutExceptions(input, uuidStringPtr, resultPtr))
            {
                output = result;
                return true;
            }
        }

        output = default;
        return false;
    }

    /// <summary>
    ///     Converts the specified read-only span bytes of UTF-8 characters containing the representation of a UUID to the equivalent <see cref="Uuid" /> structure.
    /// </summary>
    /// <param name="uuidUtf8String">A span containing the bytes of UTF-8 characters representing the UUID to convert.</param>
    /// <param name="output">
    ///     A <see cref="Uuid" /> instance to contain the parsed value. If the method returns <see langword="true" />, <paramref name="output" /> contains a valid <see cref="Uuid" />. If the method returns <see langword="false" />, <paramref name="output" /> equals
    ///     <see cref="Empty" />.
    /// </param>
    /// <returns><see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<byte> uuidUtf8String, out Uuid output)
    {
        if (uuidUtf8String.IsEmpty)
        {
            output = default;
            return false;
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (byte* uuidUtf8StringPtr = &uuidUtf8String.GetPinnableReference())
        {
            if (ParseWithoutExceptionsUtf8(uuidUtf8String, uuidUtf8StringPtr, resultPtr))
            {
                output = result;
                return true;
            }
        }

        output = default;
        return false;
    }

    /// <summary>
    ///     Converts the string representation of a UUID to the equivalent <see cref="Uuid" /> structure, provided that the string is in the specified format.
    /// </summary>
    /// <param name="input">The UUID to convert.</param>
    /// <param name="format">One of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <param name="output">
    ///     A <see cref="Uuid" /> instance to contain the parsed value. If the method returns <see langword="true" />, <paramref name="output" /> contains a valid <see cref="Uuid" />. If the method returns <see langword="false" />, <paramref name="output" /> equals
    ///     <see cref="Empty" />.
    /// </param>
    /// <returns><see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
        [NotNullWhen(true)] string? input,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        string format,
        out Uuid output)
    {
        if (input == null || format?.Length != 1)
        {
            output = default;
            return false;
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        bool parsed = false;
        switch ((char) (format[0] | 0x20))
        {
            case 'd':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsD((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'n':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsN((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'b':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsB((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'p':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsP((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'x':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsX((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
        }

        if (parsed)
        {
            output = result;
            return true;
        }

        output = default;
        return false;
    }

    /// <summary>
    ///     Converts span of characters representing the UUID to the equivalent <see cref="Uuid" /> structure, provided that the string is in the specified format.
    /// </summary>
    /// <param name="input">A read-only span containing the characters representing the UUID to convert.</param>
    /// <param name="format">A read-only span containing a character representing one of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <param name="output">
    ///     A <see cref="Uuid" /> instance to contain the parsed value. If the method returns <see langword="true" />, <paramref name="output" /> contains a valid <see cref="Uuid" />. If the method returns <see langword="false" />, <paramref name="output" /> equals
    ///     <see cref="Empty" />.
    /// </param>
    /// <returns><see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
        ReadOnlySpan<char> input,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.GuidFormat)]
#endif
        ReadOnlySpan<char> format,
        out Uuid output)
    {
        if (format.Length != 1)
        {
            output = default;
            return false;
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        bool parsed = false;
        switch ((char) (format[0] | 0x20))
        {
            case 'd':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsD((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'n':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsN((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'b':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsB((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'p':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsP((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
            case 'x':
                {
                    fixed (char* uuidStringPtr = &input.GetPinnableReference())
                    {
                        parsed = ParseWithoutExceptionsX((uint) input.Length, uuidStringPtr, resultPtr);
                    }

                    break;
                }
        }

        if (parsed)
        {
            output = result;
            return true;
        }

        output = default;
        return false;
    }

    private static bool ParseWithoutExceptions(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
    {
        uint length = (uint) uuidString.Length;
        if (length == 0u)
        {
            return false;
        }

        char* dashBuffer = stackalloc char[1];
        dashBuffer[0] = '-';
        ReadOnlySpan<char> dashSpan = new(dashBuffer, 1);
        switch (uuidString[0])
        {
            case '(':
                {
                    return ParseWithoutExceptionsP(length, uuidStringPtr, resultPtr);
                }
            case '{':
                {
                    return uuidString.Contains(dashSpan, StringComparison.Ordinal)
                        ? ParseWithoutExceptionsB(length, uuidStringPtr, resultPtr)
                        : ParseWithoutExceptionsX(length, uuidStringPtr, resultPtr);
                }
            default:
                {
                    return uuidString.Contains(dashSpan, StringComparison.Ordinal)
                        ? ParseWithoutExceptionsD(length, uuidStringPtr, resultPtr)
                        : ParseWithoutExceptionsN(length, uuidStringPtr, resultPtr);
                }
        }
    }

    private static bool ParseWithoutExceptionsD(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 36u)
        {
            return false;
        }

        if (uuidStringPtr[8] != '-' || uuidStringPtr[13] != '-' || uuidStringPtr[18] != '-' || uuidStringPtr[23] != '-')
        {
            return false;
        }

        return TryParsePtrD(uuidStringPtr, resultPtr);
    }

    private static bool ParseWithoutExceptionsN(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        return uuidStringLength == 32u && TryParsePtrN(uuidStringPtr, resultPtr);
    }

    private static bool ParseWithoutExceptionsB(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 38u)
        {
            return false;
        }

        if (uuidStringPtr[0] != '{'
            || uuidStringPtr[9] != '-'
            || uuidStringPtr[14] != '-'
            || uuidStringPtr[19] != '-'
            || uuidStringPtr[24] != '-'
            || uuidStringPtr[37] != '}')
        {
            return false;
        }

        return TryParsePtrD(uuidStringPtr + 1, resultPtr);
    }

    private static bool ParseWithoutExceptionsP(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 38u)
        {
            return false;
        }

        if (uuidStringPtr[0] != '('
            || uuidStringPtr[9] != '-'
            || uuidStringPtr[14] != '-'
            || uuidStringPtr[19] != '-'
            || uuidStringPtr[24] != '-'
            || uuidStringPtr[37] != ')')
        {
            return false;
        }

        return TryParsePtrD(uuidStringPtr + 1, resultPtr);
    }

    private static bool ParseWithoutExceptionsX(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 68u)
        {
            return false;
        }

        if (uuidStringPtr[0] != '{'
            || uuidStringPtr[1] != '0'
            || uuidStringPtr[2] != 'x'
            || uuidStringPtr[11] != ','
            || uuidStringPtr[12] != '0'
            || uuidStringPtr[13] != 'x'
            || uuidStringPtr[18] != ','
            || uuidStringPtr[19] != '0'
            || uuidStringPtr[20] != 'x'
            || uuidStringPtr[25] != ','
            || uuidStringPtr[26] != '{'
            || uuidStringPtr[27] != '0'
            || uuidStringPtr[28] != 'x'
            || uuidStringPtr[31] != ','
            || uuidStringPtr[32] != '0'
            || uuidStringPtr[33] != 'x'
            || uuidStringPtr[36] != ','
            || uuidStringPtr[37] != '0'
            || uuidStringPtr[38] != 'x'
            || uuidStringPtr[41] != ','
            || uuidStringPtr[42] != '0'
            || uuidStringPtr[43] != 'x'
            || uuidStringPtr[46] != ','
            || uuidStringPtr[47] != '0'
            || uuidStringPtr[48] != 'x'
            || uuidStringPtr[51] != ','
            || uuidStringPtr[52] != '0'
            || uuidStringPtr[53] != 'x'
            || uuidStringPtr[56] != ','
            || uuidStringPtr[57] != '0'
            || uuidStringPtr[58] != 'x'
            || uuidStringPtr[61] != ','
            || uuidStringPtr[62] != '0'
            || uuidStringPtr[63] != 'x'
            || uuidStringPtr[66] != '}'
            || uuidStringPtr[67] != '}')
        {
            return false;
        }

        return TryParsePtrX(uuidStringPtr, resultPtr);
    }

    /// <summary>
    ///     (
    /// </summary>
    private const byte Utf8LeftParenthesis = 0x28;

    /// <summary>
    ///     )
    /// </summary>
    private const byte Utf8RightParenthesis = 0x29;

    /// <summary>
    ///     {
    /// </summary>
    private const byte Utf8LeftCurlyBracket = 0x7B;

    /// <summary>
    ///     }
    /// </summary>
    private const byte Utf8RightCurlyBracket = 0x7D;

    /// <summary>
    ///     -
    /// </summary>
    private const byte Utf8HyphenMinus = 0x2D;

    /// <summary>
    ///     0
    /// </summary>
    private const byte Utf8DigitZero = 0x30;

    /// <summary>
    ///     x
    /// </summary>
    private const byte Utf8LatinSmallLetterX = 0x78;

    /// <summary>
    ///     ,
    /// </summary>
    private const byte Utf8Comma = 0x2C;

    private static bool ParseWithoutExceptionsUtf8(ReadOnlySpan<byte> uuidUtf8String, byte* uuidUtf8StringPtr, byte* resultPtr)
    {
        uint length = (uint) uuidUtf8String.Length;
        return uuidUtf8String[0] switch
        {
            Utf8LeftParenthesis => // (
                ParseWithoutExceptionsPUtf8(length, uuidUtf8StringPtr, resultPtr),
            Utf8LeftCurlyBracket => // {
                uuidUtf8String.Contains(Utf8HyphenMinus) // -
                    ? ParseWithoutExceptionsBUtf8(length, uuidUtf8StringPtr, resultPtr)
                    : ParseWithoutExceptionsXUtf8(length, uuidUtf8StringPtr, resultPtr),
            _ => uuidUtf8String.IndexOf(Utf8HyphenMinus) >= 0 // -
                ? ParseWithoutExceptionsDUtf8(length, uuidUtf8StringPtr, resultPtr)
                : ParseWithoutExceptionsNUtf8(length, uuidUtf8StringPtr, resultPtr)
        };
    }

    private static bool ParseWithoutExceptionsDUtf8(uint uuidStringLength, byte* uuidUtf8StringPtr, byte* resultPtr)
    {
        return uuidStringLength == 36u
               && uuidUtf8StringPtr[8] == Utf8HyphenMinus && //-
               uuidUtf8StringPtr[13] == Utf8HyphenMinus && //-
               uuidUtf8StringPtr[18] == Utf8HyphenMinus && //-
               uuidUtf8StringPtr[23] == Utf8HyphenMinus && //-
               TryParsePtrDUtf8(uuidUtf8StringPtr, resultPtr);
    }

    private static bool ParseWithoutExceptionsNUtf8(uint uuidStringLength, byte* uuidUtf8StringPtr, byte* resultPtr)
    {
        return uuidStringLength == 32u && TryParsePtrNUtf8(uuidUtf8StringPtr, resultPtr);
    }

    private static bool ParseWithoutExceptionsBUtf8(uint uuidStringLength, byte* uuidUtf8StringPtr, byte* resultPtr)
    {
        return uuidStringLength == 38u
               && uuidUtf8StringPtr[0] == Utf8LeftCurlyBracket && // {
               uuidUtf8StringPtr[9] == Utf8HyphenMinus && //-
               uuidUtf8StringPtr[14] == Utf8HyphenMinus && // -
               uuidUtf8StringPtr[19] == Utf8HyphenMinus && // -
               uuidUtf8StringPtr[24] == Utf8HyphenMinus && // -
               uuidUtf8StringPtr[37] == Utf8RightCurlyBracket && // }
               TryParsePtrDUtf8(uuidUtf8StringPtr + 1, resultPtr);
    }

    private static bool ParseWithoutExceptionsPUtf8(uint uuidStringLength, byte* uuidUtf8StringPtr, byte* resultPtr)
    {
        return uuidStringLength == 38u
               && uuidUtf8StringPtr[0] == Utf8LeftParenthesis && // (
               uuidUtf8StringPtr[9] == Utf8HyphenMinus && // -
               uuidUtf8StringPtr[14] == Utf8HyphenMinus && // -
               uuidUtf8StringPtr[19] == Utf8HyphenMinus && // -
               uuidUtf8StringPtr[24] == Utf8HyphenMinus && // -
               uuidUtf8StringPtr[37] == Utf8RightParenthesis && // )
               TryParsePtrDUtf8(uuidUtf8StringPtr + 1, resultPtr);
    }

    private static bool ParseWithoutExceptionsXUtf8(uint uuidStringLength, byte* uuidUtf8StringPtr, byte* resultPtr)
    {
        return uuidStringLength == 68u
               && uuidUtf8StringPtr[0] == Utf8LeftCurlyBracket && // {
               uuidUtf8StringPtr[1] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[2] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[11] == Utf8Comma && // ,
               uuidUtf8StringPtr[12] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[13] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[18] == Utf8Comma && // ,
               uuidUtf8StringPtr[19] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[20] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[25] == Utf8Comma && // ,
               uuidUtf8StringPtr[26] == Utf8LeftCurlyBracket && // {
               uuidUtf8StringPtr[27] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[28] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[31] == Utf8Comma && // ,
               uuidUtf8StringPtr[32] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[33] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[36] == Utf8Comma && // ,
               uuidUtf8StringPtr[37] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[38] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[41] == Utf8Comma && // ,
               uuidUtf8StringPtr[42] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[43] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[46] == Utf8Comma && // ,
               uuidUtf8StringPtr[47] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[48] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[51] == Utf8Comma && // ,
               uuidUtf8StringPtr[52] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[53] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[56] == Utf8Comma && // ,
               uuidUtf8StringPtr[57] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[58] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[61] == Utf8Comma && // ,
               uuidUtf8StringPtr[62] == Utf8DigitZero && // 0
               uuidUtf8StringPtr[63] == Utf8LatinSmallLetterX && // x
               uuidUtf8StringPtr[66] == Utf8RightCurlyBracket && // }
               uuidUtf8StringPtr[67] == Utf8RightCurlyBracket && // }
               TryParsePtrXUtf8(uuidUtf8StringPtr, resultPtr);
    }

    private static void ParseWithExceptions(ReadOnlySpan<char> uuidString, char* uuidStringPtr, byte* resultPtr)
    {
        uint length = (uint) uuidString.Length;
        if (length == 0u)
        {
            throw new FormatException("Unrecognized Uuid format.");
        }

        char* dashBuffer = stackalloc char[1];
        dashBuffer[0] = '-';
        ReadOnlySpan<char> dashSpan = new(dashBuffer, 1);
        switch (uuidStringPtr[0])
        {
            case '(':
                {
                    ParseWithExceptionsP(length, uuidStringPtr, resultPtr);
                    break;
                }
            case '{':
                {
                    if (uuidString.Contains(dashSpan, StringComparison.Ordinal))
                    {
                        ParseWithExceptionsB(length, uuidStringPtr, resultPtr);
                        break;
                    }

                    ParseWithExceptionsX(length, uuidStringPtr, resultPtr);
                    break;
                }
            default:
                {
                    if (uuidString.Contains(dashSpan, StringComparison.Ordinal))
                    {
                        ParseWithExceptionsD(length, uuidStringPtr, resultPtr);
                        break;
                    }

                    ParseWithExceptionsN(length, uuidStringPtr, resultPtr);
                    break;
                }
        }
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static void ParseWithExceptionsD(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 36u)
        {
            throw new FormatException("Uuid should contain 32 digits with 4 dashes xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.");
        }

        if (uuidStringPtr[8] != '-' || uuidStringPtr[13] != '-' || uuidStringPtr[18] != '-' || uuidStringPtr[23] != '-')
        {
            throw new FormatException("Dashes are in the wrong position for Uuid parsing.");
        }

        if (!TryParsePtrD(uuidStringPtr, resultPtr))
        {
            throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static void ParseWithExceptionsN(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 32u)
        {
            throw new FormatException("Uuid should contain only 32 digits xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.");
        }

        if (!TryParsePtrN(uuidStringPtr, resultPtr))
        {
            throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static void ParseWithExceptionsB(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 38u)
        {
            throw new FormatException("Uuid should contain 32 digits with 4 dashes {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}.");
        }

        if (uuidStringPtr[0] != '{' || uuidStringPtr[37] != '}')
        {
            throw new FormatException("Uuid should contain 32 digits with 4 dashes {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}.");
        }

        if (uuidStringPtr[9] != '-' || uuidStringPtr[14] != '-' || uuidStringPtr[19] != '-' || uuidStringPtr[24] != '-')
        {
            throw new FormatException("Dashes are in the wrong position for Uuid parsing.");
        }

        if (!TryParsePtrD(uuidStringPtr + 1, resultPtr))
        {
            throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static void ParseWithExceptionsP(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 38u)
        {
            throw new FormatException("Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
        }

        if (uuidStringPtr[0] != '(' || uuidStringPtr[37] != ')')
        {
            throw new FormatException("Uuid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
        }

        if (uuidStringPtr[9] != '-' || uuidStringPtr[14] != '-' || uuidStringPtr[19] != '-' || uuidStringPtr[24] != '-')
        {
            throw new FormatException("Dashes are in the wrong position for Uuid parsing.");
        }

        if (!TryParsePtrD(uuidStringPtr + 1, resultPtr))
        {
            throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static void ParseWithExceptionsX(uint uuidStringLength, char* uuidStringPtr, byte* resultPtr)
    {
        if (uuidStringLength != 68u)
        {
            throw new FormatException(
                "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");
        }

        if (uuidStringPtr[0] != '{'
            || uuidStringPtr[26] != '{'
            || uuidStringPtr[66] != '}')
        {
            throw new FormatException(
                "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).");
        }

        if (uuidStringPtr[67] != '}')
        {
            throw new FormatException("Could not find the ending brace.");
        }

        if (uuidStringPtr[11] != ','
            || uuidStringPtr[18] != ','
            || uuidStringPtr[25] != ','
            || uuidStringPtr[31] != ','
            || uuidStringPtr[36] != ','
            || uuidStringPtr[41] != ','
            || uuidStringPtr[46] != ','
            || uuidStringPtr[51] != ','
            || uuidStringPtr[56] != ','
            || uuidStringPtr[61] != ',')
        {
            throw new FormatException(
                "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).");
        }

        if (uuidStringPtr[1] != '0'
            || uuidStringPtr[2] != 'x'
            || uuidStringPtr[12] != '0'
            || uuidStringPtr[13] != 'x'
            || uuidStringPtr[19] != '0'
            || uuidStringPtr[20] != 'x'
            || uuidStringPtr[27] != '0'
            || uuidStringPtr[28] != 'x'
            || uuidStringPtr[32] != '0'
            || uuidStringPtr[33] != 'x'
            || uuidStringPtr[37] != '0'
            || uuidStringPtr[38] != 'x'
            || uuidStringPtr[42] != '0'
            || uuidStringPtr[43] != 'x'
            || uuidStringPtr[47] != '0'
            || uuidStringPtr[48] != 'x'
            || uuidStringPtr[52] != '0'
            || uuidStringPtr[53] != 'x'
            || uuidStringPtr[57] != '0'
            || uuidStringPtr[58] != 'x'
            || uuidStringPtr[62] != '0'
            || uuidStringPtr[63] != 'x')
        {
            throw new FormatException("Expected 0x prefix.");
        }


        if (!TryParsePtrX(uuidStringPtr, resultPtr))
        {
            throw new FormatException("Uuid string should only contain hexadecimal characters.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static bool TryParsePtrN(char* value, byte* resultPtr)
    {
        // e.g. "d85b1407351d4694939203acc5870eb1"
        byte hi;
        byte lo;
        // 0 byte
        if (value[0] < MaximalChar
            && (hi = TableFromHexToBytes[value[0]]) != 0xFF
            && value[1] < MaximalChar
            && (lo = TableFromHexToBytes[value[1]]) != 0xFF)
        {
            resultPtr[0] = (byte) ((byte) (hi << 4) | lo);
            // 1 byte
            if (value[2] < MaximalChar
                && (hi = TableFromHexToBytes[value[2]]) != 0xFF
                && value[3] < MaximalChar
                && (lo = TableFromHexToBytes[value[3]]) != 0xFF)
            {
                resultPtr[1] = (byte) ((byte) (hi << 4) | lo);
                // 2 byte
                if (value[4] < MaximalChar
                    && (hi = TableFromHexToBytes[value[4]]) != 0xFF
                    && value[5] < MaximalChar
                    && (lo = TableFromHexToBytes[value[5]]) != 0xFF)
                {
                    resultPtr[2] = (byte) ((byte) (hi << 4) | lo);
                    // 3 byte
                    if (value[6] < MaximalChar
                        && (hi = TableFromHexToBytes[value[6]]) != 0xFF
                        && value[7] < MaximalChar
                        && (lo = TableFromHexToBytes[value[7]]) != 0xFF)
                    {
                        resultPtr[3] = (byte) ((byte) (hi << 4) | lo);
                        // 4 byte
                        if (value[8] < MaximalChar
                            && (hi = TableFromHexToBytes[value[8]]) != 0xFF
                            && value[9] < MaximalChar
                            && (lo = TableFromHexToBytes[value[9]]) != 0xFF)
                        {
                            resultPtr[4] = (byte) ((byte) (hi << 4) | lo);
                            // 5 byte
                            if (value[10] < MaximalChar
                                && (hi = TableFromHexToBytes[value[10]]) != 0xFF
                                && value[11] < MaximalChar
                                && (lo = TableFromHexToBytes[value[11]]) != 0xFF)
                            {
                                resultPtr[5] = (byte) ((byte) (hi << 4) | lo);
                                // 6 byte
                                if (value[12] < MaximalChar
                                    && (hi = TableFromHexToBytes[value[12]]) != 0xFF
                                    && value[13] < MaximalChar
                                    && (lo = TableFromHexToBytes[value[13]]) != 0xFF)
                                {
                                    resultPtr[6] = (byte) ((byte) (hi << 4) | lo);
                                    // 7 byte
                                    if (value[14] < MaximalChar
                                        && (hi = TableFromHexToBytes[value[14]]) != 0xFF
                                        && value[15] < MaximalChar
                                        && (lo = TableFromHexToBytes[value[15]]) != 0xFF)
                                    {
                                        resultPtr[7] = (byte) ((byte) (hi << 4) | lo);
                                        // 8 byte
                                        if (value[16] < MaximalChar
                                            && (hi = TableFromHexToBytes[value[16]]) != 0xFF
                                            && value[17] < MaximalChar
                                            && (lo = TableFromHexToBytes[value[17]]) != 0xFF)
                                        {
                                            resultPtr[8] = (byte) ((byte) (hi << 4) | lo);
                                            // 9 byte
                                            if (value[18] < MaximalChar
                                                && (hi = TableFromHexToBytes[value[18]]) != 0xFF
                                                && value[19] < MaximalChar
                                                && (lo = TableFromHexToBytes[value[19]]) != 0xFF)
                                            {
                                                resultPtr[9] = (byte) ((byte) (hi << 4) | lo);
                                                // 10 byte
                                                if (value[20] < MaximalChar
                                                    && (hi = TableFromHexToBytes[value[20]]) != 0xFF
                                                    && value[21] < MaximalChar
                                                    && (lo = TableFromHexToBytes[value[21]]) != 0xFF)
                                                {
                                                    resultPtr[10] = (byte) ((byte) (hi << 4) | lo);
                                                    // 11 byte
                                                    if (value[22] < MaximalChar
                                                        && (hi = TableFromHexToBytes[value[22]]) != 0xFF
                                                        && value[23] < MaximalChar
                                                        && (lo = TableFromHexToBytes[value[23]]) != 0xFF)
                                                    {
                                                        resultPtr[11] = (byte) ((byte) (hi << 4) | lo);
                                                        // 12 byte
                                                        if (value[24] < MaximalChar
                                                            && (hi = TableFromHexToBytes[value[24]]) != 0xFF
                                                            && value[25] < MaximalChar
                                                            && (lo = TableFromHexToBytes[value[25]]) != 0xFF)
                                                        {
                                                            resultPtr[12] = (byte) ((byte) (hi << 4) | lo);
                                                            // 13 byte
                                                            if (value[26] < MaximalChar
                                                                && (hi = TableFromHexToBytes[value[26]]) != 0xFF
                                                                && value[27] < MaximalChar
                                                                && (lo = TableFromHexToBytes[value[27]]) != 0xFF)
                                                            {
                                                                resultPtr[13] = (byte) ((byte) (hi << 4) | lo);
                                                                // 14 byte
                                                                if (value[28] < MaximalChar
                                                                    && (hi = TableFromHexToBytes[value[28]]) != 0xFF
                                                                    && value[29] < MaximalChar
                                                                    && (lo = TableFromHexToBytes[value[29]]) != 0xFF)
                                                                {
                                                                    resultPtr[14] = (byte) ((byte) (hi << 4) | lo);
                                                                    // 15 byte
                                                                    if (value[30] < MaximalChar
                                                                        && (hi = TableFromHexToBytes[value[30]]) != 0xFF
                                                                        && value[31] < MaximalChar
                                                                        && (lo = TableFromHexToBytes[value[31]]) != 0xFF)
                                                                    {
                                                                        resultPtr[15] = (byte) ((byte) (hi << 4) | lo);
                                                                        return true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static bool TryParsePtrD(char* value, byte* resultPtr)
    {
        // e.g. "d85b1407-351d-4694-9392-03acc5870eb1"
        byte hi;
        byte lo;
        // 0 byte
        if (value[0] < MaximalChar
            && (hi = TableFromHexToBytes[value[0]]) != 0xFF
            && value[1] < MaximalChar
            && (lo = TableFromHexToBytes[value[1]]) != 0xFF)
        {
            resultPtr[0] = (byte) ((byte) (hi << 4) | lo);
            // 1 byte
            if (value[2] < MaximalChar
                && (hi = TableFromHexToBytes[value[2]]) != 0xFF
                && value[3] < MaximalChar
                && (lo = TableFromHexToBytes[value[3]]) != 0xFF)
            {
                resultPtr[1] = (byte) ((byte) (hi << 4) | lo);
                // 2 byte
                if (value[4] < MaximalChar
                    && (hi = TableFromHexToBytes[value[4]]) != 0xFF
                    && value[5] < MaximalChar
                    && (lo = TableFromHexToBytes[value[5]]) != 0xFF)
                {
                    resultPtr[2] = (byte) ((byte) (hi << 4) | lo);
                    // 3 byte
                    if (value[6] < MaximalChar
                        && (hi = TableFromHexToBytes[value[6]]) != 0xFF
                        && value[7] < MaximalChar
                        && (lo = TableFromHexToBytes[value[7]]) != 0xFF)
                    {
                        resultPtr[3] = (byte) ((byte) (hi << 4) | lo);

                        // value[8] == '-'

                        // 4 byte
                        if (value[9] < MaximalChar
                            && (hi = TableFromHexToBytes[value[9]]) != 0xFF
                            && value[10] < MaximalChar
                            && (lo = TableFromHexToBytes[value[10]]) != 0xFF)
                        {
                            resultPtr[4] = (byte) ((byte) (hi << 4) | lo);
                            // 5 byte
                            if (value[11] < MaximalChar
                                && (hi = TableFromHexToBytes[value[11]]) != 0xFF
                                && value[12] < MaximalChar
                                && (lo = TableFromHexToBytes[value[12]]) != 0xFF)
                            {
                                resultPtr[5] = (byte) ((byte) (hi << 4) | lo);

                                // value[13] == '-'

                                // 6 byte
                                if (value[14] < MaximalChar
                                    && (hi = TableFromHexToBytes[value[14]]) != 0xFF
                                    && value[15] < MaximalChar
                                    && (lo = TableFromHexToBytes[value[15]]) != 0xFF)
                                {
                                    resultPtr[6] = (byte) ((byte) (hi << 4) | lo);
                                    // 7 byte
                                    if (value[16] < MaximalChar
                                        && (hi = TableFromHexToBytes[value[16]]) != 0xFF
                                        && value[17] < MaximalChar
                                        && (lo = TableFromHexToBytes[value[17]]) != 0xFF)
                                    {
                                        resultPtr[7] = (byte) ((byte) (hi << 4) | lo);

                                        // value[18] == '-'

                                        // 8 byte
                                        if (value[19] < MaximalChar
                                            && (hi = TableFromHexToBytes[value[19]]) != 0xFF
                                            && value[20] < MaximalChar
                                            && (lo = TableFromHexToBytes[value[20]]) != 0xFF)
                                        {
                                            resultPtr[8] = (byte) ((byte) (hi << 4) | lo);
                                            // 9 byte
                                            if (value[21] < MaximalChar
                                                && (hi = TableFromHexToBytes[value[21]]) != 0xFF
                                                && value[22] < MaximalChar
                                                && (lo = TableFromHexToBytes[value[22]]) != 0xFF)
                                            {
                                                resultPtr[9] = (byte) ((byte) (hi << 4) | lo);

                                                // value[23] == '-'

                                                // 10 byte
                                                if (value[24] < MaximalChar
                                                    && (hi = TableFromHexToBytes[value[24]]) != 0xFF
                                                    && value[25] < MaximalChar
                                                    && (lo = TableFromHexToBytes[value[25]]) != 0xFF)
                                                {
                                                    resultPtr[10] = (byte) ((byte) (hi << 4) | lo);
                                                    // 11 byte
                                                    if (value[26] < MaximalChar
                                                        && (hi = TableFromHexToBytes[value[26]]) != 0xFF
                                                        && value[27] < MaximalChar
                                                        && (lo = TableFromHexToBytes[value[27]]) != 0xFF)
                                                    {
                                                        resultPtr[11] = (byte) ((byte) (hi << 4) | lo);
                                                        // 12 byte
                                                        if (value[28] < MaximalChar
                                                            && (hi = TableFromHexToBytes[value[28]]) != 0xFF
                                                            && value[29] < MaximalChar
                                                            && (lo = TableFromHexToBytes[value[29]]) != 0xFF)
                                                        {
                                                            resultPtr[12] = (byte) ((byte) (hi << 4) | lo);
                                                            // 13 byte
                                                            if (value[30] < MaximalChar
                                                                && (hi = TableFromHexToBytes[value[30]]) != 0xFF
                                                                && value[31] < MaximalChar
                                                                && (lo = TableFromHexToBytes[value[31]]) != 0xFF)
                                                            {
                                                                resultPtr[13] = (byte) ((byte) (hi << 4) | lo);
                                                                // 14 byte
                                                                if (value[32] < MaximalChar
                                                                    && (hi = TableFromHexToBytes[value[32]]) != 0xFF
                                                                    && value[33] < MaximalChar
                                                                    && (lo = TableFromHexToBytes[value[33]]) != 0xFF)
                                                                {
                                                                    resultPtr[14] = (byte) ((byte) (hi << 4) | lo);
                                                                    // 15 byte
                                                                    if (value[34] < MaximalChar
                                                                        && (hi = TableFromHexToBytes[value[34]]) != 0xFF
                                                                        && value[35] < MaximalChar
                                                                        && (lo = TableFromHexToBytes[value[35]]) != 0xFF)
                                                                    {
                                                                        resultPtr[15] = (byte) ((byte) (hi << 4) | lo);
                                                                        return true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static bool TryParsePtrX(char* value, byte* resultPtr)
    {
        // e.g. "{0xd85b1407,0x351d,0x4694,{0x93,0x92,0x03,0xac,0xc5,0x87,0x0e,0xb1}}"

        byte hexByteHi;
        byte hexByteLow;
        // value[0] == '{'
        // value[1] == '0'
        // value[2] == 'x'
        // 0 byte
        if (value[3] < MaximalChar
            && (hexByteHi = TableFromHexToBytes[value[3]]) != 0xFF
            && value[4] < MaximalChar
            && (hexByteLow = TableFromHexToBytes[value[4]]) != 0xFF)
        {
            resultPtr[0] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
            // 1 byte
            if (value[5] < MaximalChar
                && (hexByteHi = TableFromHexToBytes[value[5]]) != 0xFF
                && value[6] < MaximalChar
                && (hexByteLow = TableFromHexToBytes[value[6]]) != 0xFF)
            {
                resultPtr[1] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                // 2 byte
                if (value[7] < MaximalChar
                    && (hexByteHi = TableFromHexToBytes[value[7]]) != 0xFF
                    && value[8] < MaximalChar
                    && (hexByteLow = TableFromHexToBytes[value[8]]) != 0xFF)
                {
                    resultPtr[2] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                    // 3 byte
                    if (value[9] < MaximalChar
                        && (hexByteHi = TableFromHexToBytes[value[9]]) != 0xFF
                        && value[10] < MaximalChar
                        && (hexByteLow = TableFromHexToBytes[value[10]]) != 0xFF)
                    {
                        resultPtr[3] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                        // value[11] == ','
                        // value[12] == '0'
                        // value[13] == 'x'

                        // 4 byte
                        if (value[14] < MaximalChar
                            && (hexByteHi = TableFromHexToBytes[value[14]]) != 0xFF
                            && value[15] < MaximalChar
                            && (hexByteLow = TableFromHexToBytes[value[15]]) != 0xFF)
                        {
                            resultPtr[4] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                            // 5 byte
                            if (value[16] < MaximalChar
                                && (hexByteHi = TableFromHexToBytes[value[16]]) != 0xFF
                                && value[17] < MaximalChar
                                && (hexByteLow = TableFromHexToBytes[value[17]]) != 0xFF)
                            {
                                resultPtr[5] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                // value[18] == ','
                                // value[19] == '0'
                                // value[20] == 'x'

                                // 6 byte
                                if (value[21] < MaximalChar
                                    && (hexByteHi = TableFromHexToBytes[value[21]]) != 0xFF
                                    && value[22] < MaximalChar
                                    && (hexByteLow = TableFromHexToBytes[value[22]]) != 0xFF)
                                {
                                    resultPtr[6] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                    // 7 byte
                                    if (value[23] < MaximalChar
                                        && (hexByteHi = TableFromHexToBytes[value[23]]) != 0xFF
                                        && value[24] < MaximalChar
                                        && (hexByteLow = TableFromHexToBytes[value[24]]) != 0xFF)
                                    {
                                        resultPtr[7] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                        // value[25] == ','
                                        // value[26] == '{'
                                        // value[27] == '0'
                                        // value[28] == 'x'

                                        // 8 byte
                                        if (value[29] < MaximalChar
                                            && (hexByteHi = TableFromHexToBytes[value[29]]) != 0xFF
                                            && value[30] < MaximalChar
                                            && (hexByteLow = TableFromHexToBytes[value[30]]) != 0xFF)
                                        {
                                            resultPtr[8] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                            // value[31] == ','
                                            // value[32] == '0'
                                            // value[33] == 'x'

                                            // 9 byte
                                            if (value[34] < MaximalChar
                                                && (hexByteHi = TableFromHexToBytes[value[34]]) != 0xFF
                                                && value[35] < MaximalChar
                                                && (hexByteLow = TableFromHexToBytes[value[35]]) != 0xFF)
                                            {
                                                resultPtr[9] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                // value[36] == ','
                                                // value[37] == '0'
                                                // value[38] == 'x'

                                                // 10 byte
                                                if (value[39] < MaximalChar
                                                    && (hexByteHi = TableFromHexToBytes[value[39]]) != 0xFF
                                                    && value[40] < MaximalChar
                                                    && (hexByteLow = TableFromHexToBytes[value[40]]) != 0xFF)
                                                {
                                                    resultPtr[10] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                    // value[41] == ','
                                                    // value[42] == '0'
                                                    // value[43] == 'x'

                                                    // 11 byte
                                                    if (value[44] < MaximalChar
                                                        && (hexByteHi = TableFromHexToBytes[value[44]]) != 0xFF
                                                        && value[45] < MaximalChar
                                                        && (hexByteLow = TableFromHexToBytes[value[45]]) != 0xFF)
                                                    {
                                                        resultPtr[11] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                        // value[46] == ','
                                                        // value[47] == '0'
                                                        // value[48] == 'x'

                                                        // 12 byte
                                                        if (value[49] < MaximalChar
                                                            && (hexByteHi = TableFromHexToBytes[value[49]]) != 0xFF
                                                            && value[50] < MaximalChar
                                                            && (hexByteLow = TableFromHexToBytes[value[50]]) != 0xFF)
                                                        {
                                                            resultPtr[12] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                            // value[51] == ','
                                                            // value[52] == '0'
                                                            // value[53] == 'x'

                                                            // 13 byte
                                                            if (value[54] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[54]]) != 0xFF
                                                                && value[55] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[55]]) != 0xFF)
                                                            {
                                                                resultPtr[13] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                // value[56] == ','
                                                                // value[57] == '0'
                                                                // value[58] == 'x'

                                                                // 14 byte
                                                                if (value[59] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[59]]) != 0xFF
                                                                    && value[60] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[60]]) != 0xFF)
                                                                {
                                                                    resultPtr[14] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                    // value[61] == ','
                                                                    // value[62] == '0'
                                                                    // value[63] == 'x'

                                                                    // 15 byte
                                                                    if (value[64] < MaximalChar
                                                                        && (hexByteHi = TableFromHexToBytes[value[64]]) != 0xFF
                                                                        && value[65] < MaximalChar
                                                                        && (hexByteLow = TableFromHexToBytes[value[65]]) != 0xFF)
                                                                    {
                                                                        resultPtr[15] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                        return true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static bool TryParsePtrNUtf8(byte* value, byte* resultPtr)
    {
        // e.g. "d85b1407351d4694939203acc5870eb1"
        byte hi;
        byte lo;
        // 0 byte
        if (value[0] < MaximalChar
            && (hi = TableFromHexToBytes[value[0]]) != 0xFF
            && value[1] < MaximalChar
            && (lo = TableFromHexToBytes[value[1]]) != 0xFF)
        {
            resultPtr[0] = (byte) ((byte) (hi << 4) | lo);
            // 1 byte
            if (value[2] < MaximalChar
                && (hi = TableFromHexToBytes[value[2]]) != 0xFF
                && value[3] < MaximalChar
                && (lo = TableFromHexToBytes[value[3]]) != 0xFF)
            {
                resultPtr[1] = (byte) ((byte) (hi << 4) | lo);
                // 2 byte
                if (value[4] < MaximalChar
                    && (hi = TableFromHexToBytes[value[4]]) != 0xFF
                    && value[5] < MaximalChar
                    && (lo = TableFromHexToBytes[value[5]]) != 0xFF)
                {
                    resultPtr[2] = (byte) ((byte) (hi << 4) | lo);
                    // 3 byte
                    if (value[6] < MaximalChar
                        && (hi = TableFromHexToBytes[value[6]]) != 0xFF
                        && value[7] < MaximalChar
                        && (lo = TableFromHexToBytes[value[7]]) != 0xFF)
                    {
                        resultPtr[3] = (byte) ((byte) (hi << 4) | lo);
                        // 4 byte
                        if (value[8] < MaximalChar
                            && (hi = TableFromHexToBytes[value[8]]) != 0xFF
                            && value[9] < MaximalChar
                            && (lo = TableFromHexToBytes[value[9]]) != 0xFF)
                        {
                            resultPtr[4] = (byte) ((byte) (hi << 4) | lo);
                            // 5 byte
                            if (value[10] < MaximalChar
                                && (hi = TableFromHexToBytes[value[10]]) != 0xFF
                                && value[11] < MaximalChar
                                && (lo = TableFromHexToBytes[value[11]]) != 0xFF)
                            {
                                resultPtr[5] = (byte) ((byte) (hi << 4) | lo);
                                // 6 byte
                                if (value[12] < MaximalChar
                                    && (hi = TableFromHexToBytes[value[12]]) != 0xFF
                                    && value[13] < MaximalChar
                                    && (lo = TableFromHexToBytes[value[13]]) != 0xFF)
                                {
                                    resultPtr[6] = (byte) ((byte) (hi << 4) | lo);
                                    // 7 byte
                                    if (value[14] < MaximalChar
                                        && (hi = TableFromHexToBytes[value[14]]) != 0xFF
                                        && value[15] < MaximalChar
                                        && (lo = TableFromHexToBytes[value[15]]) != 0xFF)
                                    {
                                        resultPtr[7] = (byte) ((byte) (hi << 4) | lo);
                                        // 8 byte
                                        if (value[16] < MaximalChar
                                            && (hi = TableFromHexToBytes[value[16]]) != 0xFF
                                            && value[17] < MaximalChar
                                            && (lo = TableFromHexToBytes[value[17]]) != 0xFF)
                                        {
                                            resultPtr[8] = (byte) ((byte) (hi << 4) | lo);
                                            // 9 byte
                                            if (value[18] < MaximalChar
                                                && (hi = TableFromHexToBytes[value[18]]) != 0xFF
                                                && value[19] < MaximalChar
                                                && (lo = TableFromHexToBytes[value[19]]) != 0xFF)
                                            {
                                                resultPtr[9] = (byte) ((byte) (hi << 4) | lo);
                                                // 10 byte
                                                if (value[20] < MaximalChar
                                                    && (hi = TableFromHexToBytes[value[20]]) != 0xFF
                                                    && value[21] < MaximalChar
                                                    && (lo = TableFromHexToBytes[value[21]]) != 0xFF)
                                                {
                                                    resultPtr[10] = (byte) ((byte) (hi << 4) | lo);
                                                    // 11 byte
                                                    if (value[22] < MaximalChar
                                                        && (hi = TableFromHexToBytes[value[22]]) != 0xFF
                                                        && value[23] < MaximalChar
                                                        && (lo = TableFromHexToBytes[value[23]]) != 0xFF)
                                                    {
                                                        resultPtr[11] = (byte) ((byte) (hi << 4) | lo);
                                                        // 12 byte
                                                        if (value[24] < MaximalChar
                                                            && (hi = TableFromHexToBytes[value[24]]) != 0xFF
                                                            && value[25] < MaximalChar
                                                            && (lo = TableFromHexToBytes[value[25]]) != 0xFF)
                                                        {
                                                            resultPtr[12] = (byte) ((byte) (hi << 4) | lo);
                                                            // 13 byte
                                                            if (value[26] < MaximalChar
                                                                && (hi = TableFromHexToBytes[value[26]]) != 0xFF
                                                                && value[27] < MaximalChar
                                                                && (lo = TableFromHexToBytes[value[27]]) != 0xFF)
                                                            {
                                                                resultPtr[13] = (byte) ((byte) (hi << 4) | lo);
                                                                // 14 byte
                                                                if (value[28] < MaximalChar
                                                                    && (hi = TableFromHexToBytes[value[28]]) != 0xFF
                                                                    && value[29] < MaximalChar
                                                                    && (lo = TableFromHexToBytes[value[29]]) != 0xFF)
                                                                {
                                                                    resultPtr[14] = (byte) ((byte) (hi << 4) | lo);
                                                                    // 15 byte
                                                                    if (value[30] < MaximalChar
                                                                        && (hi = TableFromHexToBytes[value[30]]) != 0xFF
                                                                        && value[31] < MaximalChar
                                                                        && (lo = TableFromHexToBytes[value[31]]) != 0xFF)
                                                                    {
                                                                        resultPtr[15] = (byte) ((byte) (hi << 4) | lo);
                                                                        return true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static bool TryParsePtrDUtf8(byte* value, byte* resultPtr)
    {
        // e.g. "d85b1407-351d-4694-9392-03acc5870eb1"
        byte hi;
        byte lo;
        // 0 byte
        if (value[0] < MaximalChar
            && (hi = TableFromHexToBytes[value[0]]) != 0xFF
            && value[1] < MaximalChar
            && (lo = TableFromHexToBytes[value[1]]) != 0xFF)
        {
            resultPtr[0] = (byte) ((byte) (hi << 4) | lo);
            // 1 byte
            if (value[2] < MaximalChar
                && (hi = TableFromHexToBytes[value[2]]) != 0xFF
                && value[3] < MaximalChar
                && (lo = TableFromHexToBytes[value[3]]) != 0xFF)
            {
                resultPtr[1] = (byte) ((byte) (hi << 4) | lo);
                // 2 byte
                if (value[4] < MaximalChar
                    && (hi = TableFromHexToBytes[value[4]]) != 0xFF
                    && value[5] < MaximalChar
                    && (lo = TableFromHexToBytes[value[5]]) != 0xFF)
                {
                    resultPtr[2] = (byte) ((byte) (hi << 4) | lo);
                    // 3 byte
                    if (value[6] < MaximalChar
                        && (hi = TableFromHexToBytes[value[6]]) != 0xFF
                        && value[7] < MaximalChar
                        && (lo = TableFromHexToBytes[value[7]]) != 0xFF)
                    {
                        resultPtr[3] = (byte) ((byte) (hi << 4) | lo);

                        // value[8] == '-'

                        // 4 byte
                        if (value[9] < MaximalChar
                            && (hi = TableFromHexToBytes[value[9]]) != 0xFF
                            && value[10] < MaximalChar
                            && (lo = TableFromHexToBytes[value[10]]) != 0xFF)
                        {
                            resultPtr[4] = (byte) ((byte) (hi << 4) | lo);
                            // 5 byte
                            if (value[11] < MaximalChar
                                && (hi = TableFromHexToBytes[value[11]]) != 0xFF
                                && value[12] < MaximalChar
                                && (lo = TableFromHexToBytes[value[12]]) != 0xFF)
                            {
                                resultPtr[5] = (byte) ((byte) (hi << 4) | lo);

                                // value[13] == '-'

                                // 6 byte
                                if (value[14] < MaximalChar
                                    && (hi = TableFromHexToBytes[value[14]]) != 0xFF
                                    && value[15] < MaximalChar
                                    && (lo = TableFromHexToBytes[value[15]]) != 0xFF)
                                {
                                    resultPtr[6] = (byte) ((byte) (hi << 4) | lo);
                                    // 7 byte
                                    if (value[16] < MaximalChar
                                        && (hi = TableFromHexToBytes[value[16]]) != 0xFF
                                        && value[17] < MaximalChar
                                        && (lo = TableFromHexToBytes[value[17]]) != 0xFF)
                                    {
                                        resultPtr[7] = (byte) ((byte) (hi << 4) | lo);

                                        // value[18] == '-'

                                        // 8 byte
                                        if (value[19] < MaximalChar
                                            && (hi = TableFromHexToBytes[value[19]]) != 0xFF
                                            && value[20] < MaximalChar
                                            && (lo = TableFromHexToBytes[value[20]]) != 0xFF)
                                        {
                                            resultPtr[8] = (byte) ((byte) (hi << 4) | lo);
                                            // 9 byte
                                            if (value[21] < MaximalChar
                                                && (hi = TableFromHexToBytes[value[21]]) != 0xFF
                                                && value[22] < MaximalChar
                                                && (lo = TableFromHexToBytes[value[22]]) != 0xFF)
                                            {
                                                resultPtr[9] = (byte) ((byte) (hi << 4) | lo);

                                                // value[23] == '-'

                                                // 10 byte
                                                if (value[24] < MaximalChar
                                                    && (hi = TableFromHexToBytes[value[24]]) != 0xFF
                                                    && value[25] < MaximalChar
                                                    && (lo = TableFromHexToBytes[value[25]]) != 0xFF)
                                                {
                                                    resultPtr[10] = (byte) ((byte) (hi << 4) | lo);
                                                    // 11 byte
                                                    if (value[26] < MaximalChar
                                                        && (hi = TableFromHexToBytes[value[26]]) != 0xFF
                                                        && value[27] < MaximalChar
                                                        && (lo = TableFromHexToBytes[value[27]]) != 0xFF)
                                                    {
                                                        resultPtr[11] = (byte) ((byte) (hi << 4) | lo);
                                                        // 12 byte
                                                        if (value[28] < MaximalChar
                                                            && (hi = TableFromHexToBytes[value[28]]) != 0xFF
                                                            && value[29] < MaximalChar
                                                            && (lo = TableFromHexToBytes[value[29]]) != 0xFF)
                                                        {
                                                            resultPtr[12] = (byte) ((byte) (hi << 4) | lo);
                                                            // 13 byte
                                                            if (value[30] < MaximalChar
                                                                && (hi = TableFromHexToBytes[value[30]]) != 0xFF
                                                                && value[31] < MaximalChar
                                                                && (lo = TableFromHexToBytes[value[31]]) != 0xFF)
                                                            {
                                                                resultPtr[13] = (byte) ((byte) (hi << 4) | lo);
                                                                // 14 byte
                                                                if (value[32] < MaximalChar
                                                                    && (hi = TableFromHexToBytes[value[32]]) != 0xFF
                                                                    && value[33] < MaximalChar
                                                                    && (lo = TableFromHexToBytes[value[33]]) != 0xFF)
                                                                {
                                                                    resultPtr[14] = (byte) ((byte) (hi << 4) | lo);
                                                                    // 15 byte
                                                                    if (value[34] < MaximalChar
                                                                        && (hi = TableFromHexToBytes[value[34]]) != 0xFF
                                                                        && value[35] < MaximalChar
                                                                        && (lo = TableFromHexToBytes[value[35]]) != 0xFF)
                                                                    {
                                                                        resultPtr[15] = (byte) ((byte) (hi << 4) | lo);
                                                                        return true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static bool TryParsePtrXUtf8(byte* value, byte* resultPtr)
    {
        // e.g. "{0xd85b1407,0x351d,0x4694,{0x93,0x92,0x03,0xac,0xc5,0x87,0x0e,0xb1}}"

        byte hexByteHi;
        byte hexByteLow;
        // value[0] == '{'
        // value[1] == '0'
        // value[2] == 'x'
        // 0 byte
        if (value[3] < MaximalChar
            && (hexByteHi = TableFromHexToBytes[value[3]]) != 0xFF
            && value[4] < MaximalChar
            && (hexByteLow = TableFromHexToBytes[value[4]]) != 0xFF)
        {
            resultPtr[0] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
            // 1 byte
            if (value[5] < MaximalChar
                && (hexByteHi = TableFromHexToBytes[value[5]]) != 0xFF
                && value[6] < MaximalChar
                && (hexByteLow = TableFromHexToBytes[value[6]]) != 0xFF)
            {
                resultPtr[1] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                // 2 byte
                if (value[7] < MaximalChar
                    && (hexByteHi = TableFromHexToBytes[value[7]]) != 0xFF
                    && value[8] < MaximalChar
                    && (hexByteLow = TableFromHexToBytes[value[8]]) != 0xFF)
                {
                    resultPtr[2] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                    // 3 byte
                    if (value[9] < MaximalChar
                        && (hexByteHi = TableFromHexToBytes[value[9]]) != 0xFF
                        && value[10] < MaximalChar
                        && (hexByteLow = TableFromHexToBytes[value[10]]) != 0xFF)
                    {
                        resultPtr[3] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                        // value[11] == ','
                        // value[12] == '0'
                        // value[13] == 'x'

                        // 4 byte
                        if (value[14] < MaximalChar
                            && (hexByteHi = TableFromHexToBytes[value[14]]) != 0xFF
                            && value[15] < MaximalChar
                            && (hexByteLow = TableFromHexToBytes[value[15]]) != 0xFF)
                        {
                            resultPtr[4] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                            // 5 byte
                            if (value[16] < MaximalChar
                                && (hexByteHi = TableFromHexToBytes[value[16]]) != 0xFF
                                && value[17] < MaximalChar
                                && (hexByteLow = TableFromHexToBytes[value[17]]) != 0xFF)
                            {
                                resultPtr[5] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                // value[18] == ','
                                // value[19] == '0'
                                // value[20] == 'x'

                                // 6 byte
                                if (value[21] < MaximalChar
                                    && (hexByteHi = TableFromHexToBytes[value[21]]) != 0xFF
                                    && value[22] < MaximalChar
                                    && (hexByteLow = TableFromHexToBytes[value[22]]) != 0xFF)
                                {
                                    resultPtr[6] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                    // 7 byte
                                    if (value[23] < MaximalChar
                                        && (hexByteHi = TableFromHexToBytes[value[23]]) != 0xFF
                                        && value[24] < MaximalChar
                                        && (hexByteLow = TableFromHexToBytes[value[24]]) != 0xFF)
                                    {
                                        resultPtr[7] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                        // value[25] == ','
                                        // value[26] == '{'
                                        // value[27] == '0'
                                        // value[28] == 'x'

                                        // 8 byte
                                        if (value[29] < MaximalChar
                                            && (hexByteHi = TableFromHexToBytes[value[29]]) != 0xFF
                                            && value[30] < MaximalChar
                                            && (hexByteLow = TableFromHexToBytes[value[30]]) != 0xFF)
                                        {
                                            resultPtr[8] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                            // value[31] == ','
                                            // value[32] == '0'
                                            // value[33] == 'x'

                                            // 9 byte
                                            if (value[34] < MaximalChar
                                                && (hexByteHi = TableFromHexToBytes[value[34]]) != 0xFF
                                                && value[35] < MaximalChar
                                                && (hexByteLow = TableFromHexToBytes[value[35]]) != 0xFF)
                                            {
                                                resultPtr[9] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                // value[36] == ','
                                                // value[37] == '0'
                                                // value[38] == 'x'

                                                // 10 byte
                                                if (value[39] < MaximalChar
                                                    && (hexByteHi = TableFromHexToBytes[value[39]]) != 0xFF
                                                    && value[40] < MaximalChar
                                                    && (hexByteLow = TableFromHexToBytes[value[40]]) != 0xFF)
                                                {
                                                    resultPtr[10] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                    // value[41] == ','
                                                    // value[42] == '0'
                                                    // value[43] == 'x'

                                                    // 11 byte
                                                    if (value[44] < MaximalChar
                                                        && (hexByteHi = TableFromHexToBytes[value[44]]) != 0xFF
                                                        && value[45] < MaximalChar
                                                        && (hexByteLow = TableFromHexToBytes[value[45]]) != 0xFF)
                                                    {
                                                        resultPtr[11] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                        // value[46] == ','
                                                        // value[47] == '0'
                                                        // value[48] == 'x'

                                                        // 12 byte
                                                        if (value[49] < MaximalChar
                                                            && (hexByteHi = TableFromHexToBytes[value[49]]) != 0xFF
                                                            && value[50] < MaximalChar
                                                            && (hexByteLow = TableFromHexToBytes[value[50]]) != 0xFF)
                                                        {
                                                            resultPtr[12] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                            // value[51] == ','
                                                            // value[52] == '0'
                                                            // value[53] == 'x'

                                                            // 13 byte
                                                            if (value[54] < MaximalChar
                                                                && (hexByteHi = TableFromHexToBytes[value[54]]) != 0xFF
                                                                && value[55] < MaximalChar
                                                                && (hexByteLow = TableFromHexToBytes[value[55]]) != 0xFF)
                                                            {
                                                                resultPtr[13] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                // value[56] == ','
                                                                // value[57] == '0'
                                                                // value[58] == 'x'

                                                                // 14 byte
                                                                if (value[59] < MaximalChar
                                                                    && (hexByteHi = TableFromHexToBytes[value[59]]) != 0xFF
                                                                    && value[60] < MaximalChar
                                                                    && (hexByteLow = TableFromHexToBytes[value[60]]) != 0xFF)
                                                                {
                                                                    resultPtr[14] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);

                                                                    // value[61] == ','
                                                                    // value[62] == '0'
                                                                    // value[63] == 'x'

                                                                    // 15 byte
                                                                    if (value[64] < MaximalChar
                                                                        && (hexByteHi = TableFromHexToBytes[value[64]]) != 0xFF
                                                                        && value[65] < MaximalChar
                                                                        && (hexByteLow = TableFromHexToBytes[value[65]]) != 0xFF)
                                                                    {
                                                                        resultPtr[15] = (byte) ((byte) (hexByteHi << 4) | hexByteLow);
                                                                        return true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    //
    // IComparisonOperators
    //
#if NET7_0_OR_GREATER
    /// <inheritdoc cref="System.Numerics.IComparisonOperators{TSelf, TOther, TResult}.op_LessThan(TSelf, TOther)" />
#else
    /// <summary>
    ///     Compares two values to determine which is less.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right" />.</param>
    /// <param name="right">The value to compare with <paramref name="left" />.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
#endif
    public static bool operator <(Uuid left, Uuid right)
    {
        if (left._byte0 != right._byte0)
        {
            return left._byte0 < right._byte0;
        }

        if (left._byte1 != right._byte1)
        {
            return left._byte1 < right._byte1;
        }

        if (left._byte2 != right._byte2)
        {
            return left._byte2 < right._byte2;
        }

        if (left._byte3 != right._byte3)
        {
            return left._byte3 < right._byte3;
        }

        if (left._byte4 != right._byte4)
        {
            return left._byte4 < right._byte4;
        }

        if (left._byte5 != right._byte5)
        {
            return left._byte5 < right._byte5;
        }

        if (left._byte6 != right._byte6)
        {
            return left._byte6 < right._byte6;
        }

        if (left._byte7 != right._byte7)
        {
            return left._byte7 < right._byte7;
        }

        if (left._byte8 != right._byte8)
        {
            return left._byte8 < right._byte8;
        }

        if (left._byte9 != right._byte9)
        {
            return left._byte9 < right._byte9;
        }

        if (left._byte10 != right._byte10)
        {
            return left._byte10 < right._byte10;
        }

        if (left._byte11 != right._byte11)
        {
            return left._byte11 < right._byte11;
        }

        if (left._byte12 != right._byte12)
        {
            return left._byte12 < right._byte12;
        }

        if (left._byte13 != right._byte13)
        {
            return left._byte13 < right._byte13;
        }

        if (left._byte14 != right._byte14)
        {
            return left._byte14 < right._byte14;
        }

        if (left._byte15 != right._byte15)
        {
            return left._byte15 < right._byte15;
        }

        return false;
    }

#if NET7_0_OR_GREATER
    /// <inheritdoc cref="System.Numerics.IComparisonOperators{TSelf, TOther, TResult}.op_LessThanOrEqual(TSelf, TOther)" />
#else
    /// <summary>
    ///     Compares two values to determine which is less or equal.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right" />.</param>
    /// <param name="right">The value to compare with <paramref name="left" />.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
#endif
    public static bool operator <=(Uuid left, Uuid right)
    {
        if (left._byte0 != right._byte0)
        {
            return left._byte0 < right._byte0;
        }

        if (left._byte1 != right._byte1)
        {
            return left._byte1 < right._byte1;
        }

        if (left._byte2 != right._byte2)
        {
            return left._byte2 < right._byte2;
        }

        if (left._byte3 != right._byte3)
        {
            return left._byte3 < right._byte3;
        }

        if (left._byte4 != right._byte4)
        {
            return left._byte4 < right._byte4;
        }

        if (left._byte5 != right._byte5)
        {
            return left._byte5 < right._byte5;
        }

        if (left._byte6 != right._byte6)
        {
            return left._byte6 < right._byte6;
        }

        if (left._byte7 != right._byte7)
        {
            return left._byte7 < right._byte7;
        }

        if (left._byte8 != right._byte8)
        {
            return left._byte8 < right._byte8;
        }

        if (left._byte9 != right._byte9)
        {
            return left._byte9 < right._byte9;
        }

        if (left._byte10 != right._byte10)
        {
            return left._byte10 < right._byte10;
        }

        if (left._byte11 != right._byte11)
        {
            return left._byte11 < right._byte11;
        }

        if (left._byte12 != right._byte12)
        {
            return left._byte12 < right._byte12;
        }

        if (left._byte13 != right._byte13)
        {
            return left._byte13 < right._byte13;
        }

        if (left._byte14 != right._byte14)
        {
            return left._byte14 < right._byte14;
        }

        if (left._byte15 != right._byte15)
        {
            return left._byte15 < right._byte15;
        }

        return true;
    }

#if NET7_0_OR_GREATER
    /// <inheritdoc cref="System.Numerics.IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThan(TSelf, TOther)" />
#else
    /// <summary>
    ///     Compares two values to determine which is greater.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right" />.</param>
    /// <param name="right">The value to compare with <paramref name="left" />.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
#endif
    public static bool operator >(Uuid left, Uuid right)
    {
        if (left._byte0 != right._byte0)
        {
            return left._byte0 > right._byte0;
        }

        if (left._byte1 != right._byte1)
        {
            return left._byte1 > right._byte1;
        }

        if (left._byte2 != right._byte2)
        {
            return left._byte2 > right._byte2;
        }

        if (left._byte3 != right._byte3)
        {
            return left._byte3 > right._byte3;
        }

        if (left._byte4 != right._byte4)
        {
            return left._byte4 > right._byte4;
        }

        if (left._byte5 != right._byte5)
        {
            return left._byte5 > right._byte5;
        }

        if (left._byte6 != right._byte6)
        {
            return left._byte6 > right._byte6;
        }

        if (left._byte7 != right._byte7)
        {
            return left._byte7 > right._byte7;
        }

        if (left._byte8 != right._byte8)
        {
            return left._byte8 > right._byte8;
        }

        if (left._byte9 != right._byte9)
        {
            return left._byte9 > right._byte9;
        }

        if (left._byte10 != right._byte10)
        {
            return left._byte10 > right._byte10;
        }

        if (left._byte11 != right._byte11)
        {
            return left._byte11 > right._byte11;
        }

        if (left._byte12 != right._byte12)
        {
            return left._byte12 > right._byte12;
        }

        if (left._byte13 != right._byte13)
        {
            return left._byte13 > right._byte13;
        }

        if (left._byte14 != right._byte14)
        {
            return left._byte14 > right._byte14;
        }

        if (left._byte15 != right._byte15)
        {
            return left._byte15 > right._byte15;
        }

        return false;
    }

#if NET7_0_OR_GREATER
    /// <inheritdoc cref="System.Numerics.IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThanOrEqual(TSelf, TOther)" />
#else
    /// <summary>
    ///     Compares two values to determine which is greater or equal.
    /// </summary>
    /// <param name="left">The value to compare with <paramref name="right" />.</param>
    /// <param name="right">The value to compare with <paramref name="left" />.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
#endif
    public static bool operator >=(Uuid left, Uuid right)
    {
        if (left._byte0 != right._byte0)
        {
            return left._byte0 > right._byte0;
        }

        if (left._byte1 != right._byte1)
        {
            return left._byte1 > right._byte1;
        }

        if (left._byte2 != right._byte2)
        {
            return left._byte2 > right._byte2;
        }

        if (left._byte3 != right._byte3)
        {
            return left._byte3 > right._byte3;
        }

        if (left._byte4 != right._byte4)
        {
            return left._byte4 > right._byte4;
        }

        if (left._byte5 != right._byte5)
        {
            return left._byte5 > right._byte5;
        }

        if (left._byte6 != right._byte6)
        {
            return left._byte6 > right._byte6;
        }

        if (left._byte7 != right._byte7)
        {
            return left._byte7 > right._byte7;
        }

        if (left._byte8 != right._byte8)
        {
            return left._byte8 > right._byte8;
        }

        if (left._byte9 != right._byte9)
        {
            return left._byte9 > right._byte9;
        }

        if (left._byte10 != right._byte10)
        {
            return left._byte10 > right._byte10;
        }

        if (left._byte11 != right._byte11)
        {
            return left._byte11 > right._byte11;
        }

        if (left._byte12 != right._byte12)
        {
            return left._byte12 > right._byte12;
        }

        if (left._byte13 != right._byte13)
        {
            return left._byte13 > right._byte13;
        }

        if (left._byte14 != right._byte14)
        {
            return left._byte14 > right._byte14;
        }

        if (left._byte15 != right._byte15)
        {
            return left._byte15 > right._byte15;
        }

        return true;
    }

    //
    // IParsable
    //
#if NET7_0_OR_GREATER
    /// <inheritdoc cref="IParsable{TSelf}.Parse(string, IFormatProvider?)" />
#else
    /// <summary>
    ///     Parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="FormatException"><paramref name="s" /> is not in the correct format.</exception>
#endif
    public static Uuid Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &s.GetPinnableReference())
        {
            ParseWithExceptions(new(uuidStringPtr, s.Length), uuidStringPtr, resultPtr);
        }

        return result;
    }

#if NET7_0_OR_GREATER
    /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)" />
#else
    /// <summary>
    ///     Tries to parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
    /// <returns><see langword="true" /> if <paramref name="s" /> was successfully parsed; otherwise, <see langword="false" />.</returns>
#endif
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Uuid result)
    {
        return TryParse(s, out result);
    }

    //
    // ISpanParsable
    //
#if NET7_0_OR_GREATER
    /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
#else
    /// <summary>
    ///     Parses a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="FormatException"><paramref name="s" /> is not in a recognized format.</exception>
#endif
    public static Uuid Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (s.IsEmpty)
        {
            throw new FormatException("Unrecognized Uuid format.");
        }

        Uuid result = new();
        byte* resultPtr = (byte*) &result;
        fixed (char* uuidStringPtr = &s.GetPinnableReference())
        {
            ParseWithExceptions(s, uuidStringPtr, resultPtr);
        }

        return result;
    }

#if NET7_0_OR_GREATER
    /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
#else
    /// <summary>
    ///     Tries to parses a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">On return, contains the result of successfully parsing <paramref name="s" /> or an undefined value on failure.</param>
    /// <returns><see langword="true" /> if <paramref name="s" /> was successfully parsed; otherwise, <see langword="false" />.</returns>
#endif
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Uuid result)
    {
        return TryParse(s, out result);
    }

    #region Generator

    private const long ChristianCalendarGregorianReformTicksDate = 499_163_040_000_000_000L;

    private const byte ResetVersionMask = 0b0000_1111;
    private const byte Version1Flag = 0b0001_0000;

    private const byte ResetReservedMask = 0b0011_1111;
    private const byte ReservedFlag = 0b1000_0000;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Uuid" /> structure that represents Uuid v1 (RFC4122).
    /// </summary>
    /// <returns></returns>
    public static Uuid NewTimeBased()
    {
        byte* resultPtr = stackalloc byte[16];
        Guid* resultAsGuidPtr = (Guid*) resultPtr;
        Guid guid = Guid.NewGuid();
        resultAsGuidPtr[0] = guid;
        long currentTicks = DateTime.UtcNow.Ticks - ChristianCalendarGregorianReformTicksDate;
        byte* ticksPtr = (byte*) &currentTicks;
        resultPtr[0] = ticksPtr[3];
        resultPtr[1] = ticksPtr[2];
        resultPtr[2] = ticksPtr[1];
        resultPtr[3] = ticksPtr[0];
        resultPtr[4] = ticksPtr[5];
        resultPtr[5] = ticksPtr[4];
        resultPtr[6] = (byte) ((ticksPtr[7] & ResetVersionMask) | Version1Flag);
        resultPtr[7] = ticksPtr[6];
        resultPtr[8] = (byte) ((resultPtr[8] & ResetReservedMask) | ReservedFlag);
        return new(resultPtr);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Uuid" /> structure that works the same way as UUID_TO_BIN(UUID(), 1) from MySQL 8.0.
    /// </summary>
    /// <returns></returns>
    public static Uuid NewMySqlOptimized()
    {
        byte* resultPtr = stackalloc byte[16];
        Guid* resultAsGuidPtr = (Guid*) resultPtr;
        Guid guid = Guid.NewGuid();
        resultAsGuidPtr[0] = guid;
        long currentTicks = DateTime.UtcNow.Ticks - ChristianCalendarGregorianReformTicksDate;
        byte* ticksPtr = (byte*) &currentTicks;
        resultPtr[0] = (byte) ((ticksPtr[7] & ResetVersionMask) | Version1Flag);
        resultPtr[1] = ticksPtr[6];
        resultPtr[2] = ticksPtr[5];
        resultPtr[3] = ticksPtr[4];
        resultPtr[4] = ticksPtr[3];
        resultPtr[5] = ticksPtr[2];
        resultPtr[6] = ticksPtr[1];
        resultPtr[7] = ticksPtr[0];
        resultPtr[8] = (byte) ((resultPtr[8] & ResetReservedMask) | ReservedFlag);
        return new(resultPtr);
    }

    #endregion
}
