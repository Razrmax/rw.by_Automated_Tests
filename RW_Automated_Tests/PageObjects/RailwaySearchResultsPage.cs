using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using SeleniumExtras.PageObjects;

namespace RW_Automated_Tests.PageObjects
{
    class RailwaySearchResultsPage
    {
        private IWebDriver Driver { get; }
        private PageMethods PageMethods { get; }

        protected internal RailwaySearchResultsPage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(driver, this);
            PageMethods = new PageMethods();
        }

        [FindsBy(How = How.XPath, Using = "//input[@id='searchinp']")]
        private IWebElement SearchInput { get; set; }

        [FindsBy(How = How.Name, Using = "q")] 
        private IWebElement MiddleSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        private IWebElement SiteSearchSubmitBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='search-result']")]
        private IWebElement SearchResultsPanel { get; set; }

        protected internal void Navigate(string url)
        {
            PageMethods.Navigate(Driver, url);
        }

        protected internal void Search(string query)
        {
            PageMethods.Search(Driver, query, SearchInput, SiteSearchSubmitBtn);
        }

        protected internal bool IsUrlCorrect(string requiredUrl)
        {
            return PageMethods.IsUrlCorrect(requiredUrl, Driver);
        }

        protected internal void ClearSearchInput()
        {
            PageMethods.ClearInput(MiddleSearchInput);
        }

        protected internal void RepeatSearch(string query)
        {
            PageMethods.Search(Driver, query, MiddleSearchInput, SiteSearchSubmitBtn);
        }

        protected internal int CountSearchResults()
        {
            var resultsCount = PageMethods.CountItems(By.XPath("//li//h3"), SearchResultsPanel);
            return resultsCount;
        }

        protected internal ICollection<string> GetSearchResultsText()
        {
            var searchResultsText = PageMethods.ExtractTextFromElements(SearchResultsPanel, By.XPath("//li//h3"));
            return searchResultsText;
        }

        protected internal void ClickLink(string partialLink)
        {
            PageMethods.ClickLink(By.XPath("//a[contains(@href, '" + partialLink + "')]"), SearchResultsPanel);
        }

        protected internal bool SearchResultDisplaysRequiredResponse(string requiredResponse)
        {
            var actualResponse = SearchResultsPanel.Text;
            return actualResponse == requiredResponse;
        }
    }
}
