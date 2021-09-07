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
            PageMethodsUtils = new PageMethodsUtils();
        }

        private IWebDriver Driver { get; }

        [FindsBy(How = How.XPath, Using = "//input[@class='gLFyf gsfi']")]
        private IWebElement SearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='lJ9FBc']//input[@name='btnK']")]
        private IWebElement SubmitBtn { get; set; }

        [FindsBy(How = How.Id, Using = "search")]
        private IWebElement ResultsPanel { get; set; }

        private PageMethodsUtils PageMethodsUtils { get; }

        public void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void Search(string query)
        {
            PageMethodsUtils.Search(query, SearchInput, SubmitBtn);
        }

        public void ClickLink(string partialLink)
        {
            PageMethodsUtils.ClickLink(By.XPath("//a[contains(@href, '" + partialLink + "')]"), ResultsPanel);
        }

        public bool PageIsLoaded(string url)
        {
            var elementIsLoaded = PageMethodsUtils.IsUrlCorrect(url, Driver);
            return elementIsLoaded;
        }
    }
}