using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using SeleniumExtras.PageObjects;

namespace RW_Automated_Tests.PageObjects
{
    internal class GooglePage
    {
        public GooglePage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(driver, this);
            PageMethods = new PageMethods();
        }

        private IWebDriver Driver { get; }

        [FindsBy(How = How.XPath, Using = "//input[@class='gLFyf gsfi']")]
        private IWebElement SearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='lJ9FBc']//input[@name='btnK']")]
        private IWebElement SubmitBtn { get; set; }

        [FindsBy(How = How.Id, Using = "search")]
        private IWebElement ResultsPanel { get; set; }

        private PageMethods PageMethods { get; }

        public void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void Search(string query)
        {
            PageMethods.Search(Driver, query, SearchInput, SubmitBtn);
        }

        public void ClickFirstLink()
        {
            PageMethods.ClickLink(By.XPath("(//h3)[1]/../../a"), ResultsPanel);
        }

        public bool PageIsLoaded(string url)
        {
            var elementIsLoaded = PageMethods.IsUrlCorrect(url, Driver);
            return elementIsLoaded;
        }
    }
}