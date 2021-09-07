using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using SeleniumExtras.PageObjects;

namespace RW_Automated_Tests.PageObjects
{
    internal class RailwayPage
    {
        public RailwayPage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(driver, this);
            PageMethodsUtils = new PageMethodsUtils();
        }

        private IWebDriver Driver { get; }

        [FindsBy(How = How.XPath, Using = "//input[@id='searchinp']")]
        private IWebElement SearchInput { get; set; }

        [FindsBy(How = How.Name, Using = "q")] public IWebElement MiddleSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        private IWebElement SiteSearchSubmitBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='top-lang']")]
        private IWebElement LanguagePanel { get; set; }

        [FindsBy(How = How.ClassName, Using = "copyright")]
        private IWebElement CopyrightPanel { get; set; }

        [FindsBy(How = How.XPath, Using = "//table[@class='menu-items']")]
        private IWebElement TopIndexMenu { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='search-result']")]
        public IWebElement SearchResultsPanel { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='index-news-list-wrap']")]
        public IWebElement NewsListSummary { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='acFrom']")]
        public IWebElement FromInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='acTo']")]
        public IWebElement DestinationInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='yDate']")]
        public IWebElement CalendarInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='std-button']//input[@type='submit']")]
        public IWebElement ScheduleSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ui-datepicker-group ui-datepicker-group-first']")]
        public IWebElement CurrentMonth { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ui-datepicker-group ui-datepicker-group-middle']")]
        public IWebElement NextMonth { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sch-table__row']")]
        public IWebElement TrainsSchedule { get; set; }

        [FindsBy(How = How.XPath, Using = "//img[@title='БелЖД']")]
        public IWebElement LogoImage { get; set; }


        private PageMethodsUtils PageMethodsUtils { get; }

        public void Navigate(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void Search(string query)
        {
            PageMethodsUtils.Search(query, SearchInput, SiteSearchSubmitBtn);
        }

        public void ClickLink(string partialLink)
        {
            PageMethodsUtils.ClickLink(By.XPath("//a[contains(@href, '" + partialLink + "')]"), SearchResultsPanel);
        }

        public bool SwitchLanguage(string targetLanguageAbbr)
        {
            PageMethodsUtils.SwitchLanguage(targetLanguageAbbr, LanguagePanel);
            Thread.Sleep(100);
            var currentUrl = Driver.Url;
            switch (targetLanguageAbbr)
            {
                case "ENG":
                    return currentUrl == "https://www.rw.by/en/";
                case "RUS":
                    return currentUrl == "https://www.rw.by";
                case "БЕЛ":
                    return currentUrl == "https://www.rw.by/be/";
                default:
                    return false;
            }
        }

        public bool NewsArticlesAreDisplayed(int requiredNumberOfArticles)
        {
            var actualNumberOfArticles = PageMethodsUtils.CountItems(By.XPath("//dt"), NewsListSummary);
            return requiredNumberOfArticles <= actualNumberOfArticles;
        }

        public bool TopIndexButtonsMatch(HashSet<string> buttonNamesLeft)
        {
            var indexButtonsTextMatch =
                PageMethodsUtils.ElementsTextMatch(buttonNamesLeft, TopIndexMenu, By.XPath("//td"));
            return indexButtonsTextMatch;
        }

        public bool SearchResultDisplaysRequiredResponse(string requiredResponse)
        {
            var actualResponse = SearchResultsPanel.Text;
            return actualResponse == requiredResponse;
        }

        public bool CopyrightTextIsCorrect(string copyrightText)
        {
            var copyrightTextIsCorrect =
                PageMethodsUtils.CopyrightTextIsCorrect(copyrightText, CopyrightPanel, By.XPath("//"));
            return copyrightTextIsCorrect;
        }

        public bool IsUrlCorrect(string requiredUrl)
        {
            return PageMethodsUtils.IsUrlCorrect(requiredUrl, Driver);
        }

        public void ClearSearchInput()
        {
            PageMethodsUtils.ClearInput(MiddleSearchInput);
        }

        public void RepeatSearch(string query)
        {
            PageMethodsUtils.Search(query, MiddleSearchInput, SiteSearchSubmitBtn);
        }

        public int CountSearchResults()
        {
            var resultsCount = PageMethodsUtils.CountItems(By.XPath("//li//h3"), SearchResultsPanel);
            return resultsCount;
        }

        public ICollection<string> GetSearchResultsText()
        {
            var searchResultsText = PageMethodsUtils.ExtractTextFromElements(SearchResultsPanel, By.XPath("//li//h3"));
            return searchResultsText;
        }

        public void SearchTrains(string fromLocation, string destination, int daysFromToday)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysFromToday);
            var targetDay = futureDate.Day;
            var targetMonth = futureDate.Month == today.Month ? CurrentMonth : NextMonth;
            PageMethodsUtils.SearchTrains(FromInput, DestinationInput, CalendarInput, targetMonth, targetDay,
                ScheduleSearchBtn, fromLocation, destination);
        }

        public ICollection<string> GetTrainsSchedule()
        {
            return PageMethodsUtils.GetTrainsSchedule(Driver);
        }

        public bool ContainsTrainNames()
        {
            return PageMethodsUtils.ContainsTrainNames(Driver);
        }

        public void ClickLogo()
        {
            PageMethodsUtils.ClickLogoImage(LogoImage);
        }
    }
}