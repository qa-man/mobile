using System;
using System.Diagnostics;
using AdvancedSharpAdbClient;

namespace Utilities.Helpers
{
    public static class AdbHelper
    {
        public static Process CLI = Process.Start(ProcessInfo);

        public static AdvancedAdbClient AdbClient = new();

        public static string AdbPath => $"{Environment.GetEnvironmentVariable("ANDROID_HOME")}\\platform-tools\\adb.exe";

        public static void RestartAdbServer()
        {
            Execute($"{AdbPath} start-server");
            Execute($"{AdbPath} kill-server");
            Execute($"{AdbPath} start-server");
            Execute($"{AdbPath} devices");
        }

        #region Private Methods

        private static ProcessStartInfo ProcessInfo => new()
        {
            FileName = "cmd.exe",
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            UseShellExecute = false
        };

        private static void Execute(string command)
        {
            try
            {
                CLI.StandardInput.WriteLine(command);
                CLI.WaitForExit(TimeSpan.FromSeconds(5).Milliseconds);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong with with command line execution \t {e}");
            }
        }

        #endregion
    }
}