using System.IO;
using Microsoft.Extensions.Configuration;

namespace Utilities.Helpers
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot Config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json").Build();
        public static string AzureCollectionURL => Config["AzureCollectionURL"];
        public static string AzureHost => Config["AzureHost"];
        public static string AndroidApp => Config["AndroidPackage"];
        public static string AndroidActivity => Config["AndroidActivity"];
    }

    public enum Mode
    {
        Emulator,
        PDA
    }
}