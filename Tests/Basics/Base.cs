using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AdvancedSharpAdbClient;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using TechTalk.SpecFlow;
using Utilities.Helpers;
using TestContext = NUnit.Framework.TestContext;

namespace Tests.Basics
{
    [Binding]
    public static class Base
    {
        public static TestContext TestContext;
        public static int TestCaseId;
        public static AndroidDriver<AppiumWebElement> Driver;
        public static DeviceData Device;

        private static Mode Mode;
        private static OptionCollector serverOptions = new OptionCollector().AddArguments(new KeyValuePair<string, string>("--relaxed-security", string.Empty));
        private static AppiumLocalService appiumLocalService;
        private static AppiumOptions options = new();

        [BeforeTestRun]
        public static void Initializer()
        {
            TestContext = TestContext.CurrentContext;
            Mode = string.IsNullOrEmpty(TestRunHelper.Emulator) ? Mode.PDA : Mode.Emulator;

            AdbHelper.RestartAdbServer();

            switch (Mode)
            {
                case Mode.PDA:
                    Device = AdbHelper.AdbClient.GetDevices().First(device => device.State == DeviceState.Online);
                    break;

                case Mode.Emulator:
                    options.AddAdditionalCapability(AndroidMobileCapabilityType.Avd, TestRunHelper.Emulator);
                    break;
            }

            options.AddAdditionalCapability(MobileCapabilityType.AutomationName, "UIAutomator2");
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().WithArguments(serverOptions).Build();
            appiumLocalService.Start();
            Driver = new AndroidDriver<AppiumWebElement>(appiumLocalService, options, TimeSpan.FromMinutes(5));

            ReInstallApp();
            OpenApp();
        }

        [BeforeScenario]
        public static void TestInitialize(ScenarioContext scenarioContext)
        {
            TestContext = TestContext.CurrentContext;
            TestCaseId = int.Parse(scenarioContext.ScenarioInfo.Tags.Single(tag => int.TryParse(tag, out TestCaseId)));

            if (Mode is Mode.Emulator) Driver.StartRecordingScreen();
        }

        [AfterScenario]
        public static void TestCleanup()
        {
            TestContext = TestContext.CurrentContext;
            var filePath = Path.Combine(TestContext.TestDirectory, $"{TestCaseId}_{TestContext.Test.Name.Replace('"', '_')} [{DateTime.Now:dd MMMM yyyy ± HH·mm·ss}]");

            switch (TestContext.Result.Outcome.Status)
            {
                case TestStatus.Passed:
                    var result = TestContext.Result;
                    var currentResult = (TestCaseResult)result.GetType().GetRuntimeFields().SingleOrDefault()?.GetValue(result);
                    var output = currentResult?.Output;

                    var logPath = $"{filePath}.log";
                    File.WriteAllText(logPath, output);
                    TestContext.AddTestAttachment(logPath);
                    break;

                default:
                    var screenshot = $"{filePath}.png";
                    Driver.GetScreenshot().SaveAsFile(screenshot, ScreenshotImageFormat.Png);
                    TestContext.AddTestAttachment(screenshot);
                    break;
            }

            if (Mode is Mode.PDA) return;
            var recordingBytes = Convert.FromBase64String(Driver.StopRecordingScreen());
            var screenRecord = $"{filePath}.mp4";
            File.WriteAllBytes(screenRecord, recordingBytes);
            TestContext.AddTestAttachment(screenRecord);
        }

        [AfterTestRun]
        public static void Finalizer()
        {
            try
            {
                Driver.ResetApp();
                Driver.Dispose();
                appiumLocalService.Dispose();
            }
            finally
            {
                AdbHelper.CLI.Close();
            }
        }

        #region Private Methods

        private static void ReInstallApp()
        {
            if (!TestRunHelper.Reinstall) return;
            Driver.RemoveApp(ConfigHelper.AndroidApp);
            Driver.InstallApp($"{TestRunHelper.ApkPath}/{ConfigHelper.AndroidApp}.apk");
        }

        private static void OpenApp()
        {
            options.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, ConfigHelper.AndroidApp);
            options.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, ConfigHelper.AndroidActivity);
            Driver = new AndroidDriver<AppiumWebElement>(appiumLocalService, options);
        }

        private static void Scroll(double x1, double y1, double x2, double y2)
        {
            new TouchAction(Driver).Press(x1, y1).Wait(1000).MoveTo(x2, y2).Release().Perform();
        }

        #endregion

    }
}