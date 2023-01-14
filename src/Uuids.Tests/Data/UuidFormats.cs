namespace Uuids.Tests.Data;

// ReSharper disable once InconsistentNaming
public static class UuidFormats
{
    public static string[] All { get; } =
    {
        "N",
        "n",
        "D",
        "d",
        "B",
        "b",
        "P",
        "p",
        "X",
        "x"
    };

    public static string[] N { get; } =
    {
        "N",
        "n"
    };

    public static string[] D { get; } =
    {
        "D",
        "d"
    };

    public static string[] B { get; } =
    {
        "B",
        "b"
    };

    public static string[] P { get; } =
    {
        "P",
        "p"
    };

    public static string[] X { get; } =
    {
        "X",
        "x"
    };

    public static string[] AllExceptN { get; } =
    {
        "D",
        "d",
        "B",
        "b",
        "P",
        "p",
        "X",
        "x"
    };

    public static string[] AllExceptD { get; } =
    {
        "N",
        "n",
        "B",
        "b",
        "P",
        "p",
        "X",
        "x"
    };

    public static string[] AllExceptB { get; } =
    {
        "N",
        "n",
        "D",
        "d",
        "P",
        "p",
        "X",
        "x"
    };

    public static string[] AllExceptP { get; } =
    {
        "N",
        "n",
        "D",
        "d",
        "B",
        "b",
        "X",
        "x"
    };

    public static string[] AllExceptX { get; } =
    {
        "N",
        "n",
        "D",
        "d",
        "B",
        "b",
        "P",
        "p"
    };
}
