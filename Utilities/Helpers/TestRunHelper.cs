using NUnit.Framework;

namespace Utilities.Helpers
{
    public static class TestRunHelper
    {
        public static string PAT => $"{TestContext.Parameters["pat"]}";
        public static string Emulator => $"{TestContext.Parameters["emulator"]}";
        public static string ApkPath => $"{TestContext.Parameters["apkPath"]}";
        public static bool Reinstall => bool.Parse($"{TestContext.Parameters["reinstall"]}");
        public static string Environment => $"{TestContext.Parameters["environment"]}";
    }
}