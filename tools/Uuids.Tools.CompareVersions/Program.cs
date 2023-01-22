using System;
using NuGet.Versioning;

namespace Uuids.Tools.CompareVersions;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args == null)
        {
            throw new ArgumentNullException(
                nameof(args),
                "Application requires to pass 2 versions as command-line parameters, but got nothing.");
        }

        if (args.Length != 2)
        {
            throw new ArgumentNullException(
                nameof(args),
                $"Application requires to pass 2 versions as command-line parameters, but got {args.Length}.");
        }

        if (!NuGetVersion.TryParseStrict(args[0], out NuGetVersion? versionA))
        {
            throw new ArgumentNullException(
                nameof(args),
                "First argument is not a valid nuget version");
        }

        if (!NuGetVersion.TryParseStrict(args[1], out NuGetVersion? versionB))
        {
            throw new ArgumentNullException(
                nameof(args),
                "Second argument is not a valid nuget version");
        }

        int compare = VersionComparer.VersionReleaseMetadata.Compare(versionA, versionB);
        Console.WriteLine(compare);
    }
}
