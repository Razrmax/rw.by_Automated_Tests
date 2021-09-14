using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using RW_Automated_Tests.PageObjects;

namespace RW_Automated_Tests.Tests
{
    [TestFixture][Parallelizable]
    class RailwayPageSearchTest
    {
        private string _baseUrl = @"https:\\www.rw.by";
        private IWebDriver _driver;

        /// <summary>
        ///     a. Open site
        ///     b. Type in "Поиск" 20 random symbols (they should be different for each execution)
        ///     c. Check, that address in browser changed to the correct one
        ///     d. Check, that text "К сожалению, на ваш поисковый запрос ничего не найдено." is displayed on a page
        ///     
        /// </summary>
        [Test, Order(1)]
        [TestCaseSource(typeof(DriverFactory), "SelectBrowserToRunWith")]
        public void SiteSearchTestWithGibberishQuery(string browser)
        {
            //Arrange
            _driver = DriverFactory.Create(browser);
            var requiredSearchResponse = "К сожалению, на ваш поисковый запрос ничего не найдено.";
            var gibberishQuery = PageMethods.GenerateGibberish(20);
            var currentPage = new RailwaySearchResultsPage(_driver);
            //Act
            currentPage.Navigate(_baseUrl);
            currentPage.Search(gibberishQuery);
            //Assert
            Assert.IsTrue(currentPage.IsUrlCorrect("https://www.rw.by/search/?s=Y&q=" + gibberishQuery + ""));
            Assert.IsTrue(currentPage.SearchResultDisplaysRequiredResponse(requiredSearchResponse));
        }

        /// <summary>
        ///     a. from the search result page, enter “Санкт-Петербург”
        ///     f. Click "Найти"
        ///     g. Check, that 15 links to the articles are displayed on a screen
        ///     h. Write the text from above mentioned links into the console
        /// </summary>
        [Test, Order(2)]
        [TestCaseSource(typeof(DriverFactory), "SelectBrowserToRunWith")]
        public void SiteSearchTestWithCorrectQuery(string browser)
        {
            //Arrange
            _driver = DriverFactory.Create(browser);
            var correctQuery = "Санкт-Петербург";
            _baseUrl = "https://www.rw.by/search/?s=Y&q=avgfdgvsfgsgvfdsggfdwgfgsggfdg";
            var currentPage = new RailwaySearchResultsPage(_driver);
            //Act
            currentPage.Navigate(_baseUrl);
            currentPage.ClearSearchInput();
            currentPage.RepeatSearch(correctQuery);
            //Assert
            Assert.AreEqual(currentPage.CountSearchResults(), 15);
            Assert.IsTrue(PageMethods.DisplayResults(currentPage.GetSearchResultsText()));
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.Close(_driver);
        }
    }
}
