using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using SeleniumExtras.PageObjects;

namespace RW_Automated_Tests.PageObjects
{
    internal class RailwayMainPage
    {
        private IWebDriver Driver { get; }
        private PageMethods PageMethods { get; }

        public RailwayMainPage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(driver, this);
            PageMethods = new PageMethods();
        }

        [FindsBy(How = How.XPath, Using = "//div[@class='top-lang']")]
        private IWebElement LanguagePanel { get; set; }

        [FindsBy(How = How.ClassName, Using = "copyright")]
        private IWebElement CopyrightPanel { get; set; }

        [FindsBy(How = How.XPath, Using = "//table[@class='menu-items']")]
        private IWebElement TopIndexMenu { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='index-news-list-wrap']")]
        public IWebElement NewsListSummary { get; set; }


        public void Navigate(string url)
        {
            PageMethods.Navigate(Driver, url);
        }

        public bool SwitchLanguage(string targetLanguageAbbr)
        {
            PageMethods.SwitchLanguage(targetLanguageAbbr, LanguagePanel);
            Thread.Sleep(100);
            var currentUrl = Driver.Url;
            return targetLanguageAbbr switch
            {
                "ENG" => currentUrl == "https://www.rw.by/en/",
                "RUS" => currentUrl == "https://www.rw.by",
                "БЕЛ" => currentUrl == "https://www.rw.by/be/",
                _ => false,
            };
        }

        public bool NewsArticlesAreDisplayed(int requiredNumberOfArticles)
        {
            var actualNumberOfArticles = PageMethods.CountItems(By.XPath("//dt"), NewsListSummary);
            return requiredNumberOfArticles <= actualNumberOfArticles;
        }

        public bool TopIndexButtonsMatch(HashSet<string> buttonNamesLeft)
        {
            var indexButtonsTextMatch =
                PageMethods.ElementsTextMatch(buttonNamesLeft, TopIndexMenu, By.XPath("//td"));
            return indexButtonsTextMatch;
        }

        public bool CopyrightTextIsCorrect(string copyrightText)
        {
            var copyrightTextIsCorrect =
                PageMethods.CopyrightTextIsCorrect(copyrightText, CopyrightPanel);
            return copyrightTextIsCorrect;
        }
    }
}