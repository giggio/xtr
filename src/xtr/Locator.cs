using System;
using System.IO;
using System.Linq;

namespace Xtr
{
    public class Locator
    {
        private static string GetNugetHome() =>
            Path.Combine(
            Environment.GetEnvironmentVariable("USERPROFILE") ?? Environment.GetEnvironmentVariable("HOME"),
            ".nuget");

        public static string GetDriverPath(string packageName, params string[] directories)
        {
            var nugetHome = GetNugetHome();
            var expandedHome = Environment.ExpandEnvironmentVariables(nugetHome);
            var packagesPath = Path.Combine(expandedHome, "packages", packageName);
            var latestVersion =
                Directory.EnumerateDirectories(packagesPath).Select(p => new Version(Path.GetFileName(p))).Max();
            var driverPath = Path.Combine(packagesPath, latestVersion.ToString(), Path.Combine(directories));
            return driverPath;
        }
    }
}
