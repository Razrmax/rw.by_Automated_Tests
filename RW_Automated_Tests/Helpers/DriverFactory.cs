using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.Edge.SeleniumTools;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;

namespace RW_Automated_Tests.Helpers
{
    public class DriverFactory
    {
        protected internal static IWebDriver Create(string browser)
        {
            var browsers = SelectBrowserToRunWith();
            IWebDriver driver;

            switch (browser)
            {
                //case "IE":
                //    var edgeOptions = new EdgeOptions();
                //    edgeOptions.UseChromium = true;
                //    driver = new EdgeDriver(edgeOptions);
                //    break;
                //case "Firefox":
                //    var firefoxOptions = new FirefoxOptions();
                //    firefoxOptions.AcceptInsecureCertificates = true;
                //    var geckoService = FirefoxDriverService.CreateDefaultService();
                //    geckoService.Host = "::1";
                //    driver = new FirefoxDriver(firefoxOptions);
                //    break;
                //case "Opera":
                //    var operaOptions = new OperaOptions();
                //    operaOptions.AcceptInsecureCertificates = true;
                //    driver = new OperaDriver(operaOptions);
                //    break;
                default:
                    var chromeOptions = new ChromeOptions();
                    driver = new ChromeDriver(chromeOptions);
                    break;
            }

            driver.Manage().Window.Maximize();
            return driver;
        }

        protected internal static void Close(IWebDriver driver)
        {
            driver.Quit();
            driver.Dispose();
        }

        protected internal static IEnumerable<string> SelectBrowserToRunWith()
        {
            var browsers = ConfigurationManager.AppSettings["Browsers"].Split(",");
            foreach (var b in browsers)
            {
                yield return b;
            }
        }
    }
}