using System.Configuration;
using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace RW_Automated_Tests.Helpers
{
    public class DriverFactory
    {
        public static IWebDriver Create()
        {
            IWebDriver driver;
            var browser = ConfigurationManager.AppSettings["Browser"];

            switch (browser)
            {
                case "IE":
                    var options = new EdgeOptions();
                    options.UseChromium = true;
                    //options.AddArgument("--allow-running-insecure-content");
                    //options.AddArgument("--start - maximized");
                    driver = new EdgeDriver(options);
                    driver.Manage().Window.Maximize();
                    break;
                case "Firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AcceptInsecureCertificates = true;
                    var geckoService = FirefoxDriverService.CreateDefaultService();
                    geckoService.Host = "::1";
                    driver = new FirefoxDriver(geckoService, firefoxOptions);
                    driver.Manage().Window.Maximize();
                    break;
                default:
                    var chromeOptions = new ChromeOptions();
                    //chromeOptions.AddArgument("--allow-running-insecure-content");
                    //chromeOptions.AddArgument("--start - maximized");
                    driver = new ChromeDriver(chromeOptions);
                    driver.Manage().Window.Maximize();
                    break;
            }

            return driver;
        }

        public static void Close(IWebDriver driver)
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}