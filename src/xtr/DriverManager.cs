using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Runtime.InteropServices;

namespace Xtr
{
    public class DriverManager
    {
        public static IWebDriver CreateChromeDriver(bool quiet)
        {
            var chromeDriverPath = Locator.GetDriverPath("Selenium.WebDriver.ChromeDriver", "driver", "win32"); //todo support linux when webdriver does https://github.com/SeleniumHQ/selenium/issues/4106
            if (chromeDriverPath == null)
                throw new Exception("ChromeDriver not found.");
            var svc = ChromeDriverService.CreateDefaultService(chromeDriverPath);
            var opt = new ChromeOptions();
            opt.AddArgument("--headless");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                opt.AddArgument("--disable-gpu");
            if (quiet)
            {
                opt.SetLoggingPreference("driver", LogLevel.Off);
                opt.SetLoggingPreference("server", LogLevel.Off);
                opt.SetLoggingPreference("browser", LogLevel.Off);
                svc.SuppressInitialDiagnosticInformation = true;
                svc.HideCommandPromptWindow = true;
                opt.AddArgument("--log-level=3");
            }
            var driver = new ChromeDriver(svc, opt);
            var manager = driver.Manage();
            manager.Window.Maximize();
            manager.Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }
    }
}
