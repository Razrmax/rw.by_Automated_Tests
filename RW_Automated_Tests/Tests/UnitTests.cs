using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using RW_Automated_Tests.PageObjects;

namespace RW_Automated_Tests.Tests
{
    public class UnitTests
    {
        private string _baseUrl;
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = DriverFactory.Create();
            _baseUrl = "https:\\www.rw.by";
        }

        /// <summary>
        ///     Performs 1st sequence of tests:
        ///     a) opens browser and loads google.com
        ///     b) in the search input field, types in query "белорусская железная дорога"
        ///     c) clicks on Google search button to begin search
        ///     d) in the Google search results page, finds and clicks on a link which contains the https://www.rw.by/ partial
        ///     address
        ///     e) verifies that the page has been fully loaded by locating a copyright(c) web element
        ///     f) closes the driver and disposes it
        /// </summary>
        [Test]
        public void OpenGoogleFindAndLoadRailroadSite()
        {
            //Arrange
            string _searchQuery = "белорусская железная дорога";
            _baseUrl = "https://google.com";
            //Act
            var currentPage = new GooglePage(_driver);
            currentPage.Navigate(_baseUrl);
            currentPage.Search(_searchQuery);
            currentPage.ClickLink("https://www.rw.by/");
            //Assert
            Assert.IsTrue(currentPage.PageIsLoaded("https://www.rw.by/be/"));
        }

        /// <summary>
        ///     Performs 1st sequence of tests:
        ///     a) opens browser and loads rw.com
        ///     b) switches to English language
        ///     c) Verifies that at less 4 news are displayed in the "News" section:
        ///     d) checks that "© 2021 Belarusian Railway" is displayed in the bottom of the page
        ///     e) checks that 5 buttons are located in the top part of the page:
        ///     - “Press Center”,
        ///     - “Tickets”,
        ///     - “Passenger Services”,
        ///     - “Freight”,
        ///     - “Corporate”
        /// </summary>
        [Test]
        public void RailWaySiteMainPageTest()
        {
            //Arrange
            var currentPage = new RailwayMainPage(_driver);
            currentPage.Navigate(_baseUrl);
            var _topIndexMenuButtonsNames = new HashSet<string>
                {"press center", "tickets", "passenger services", "freight", "corporate"};
            var _copyrightText = "© 2021 Belarusian Railway";
            //Assert
            Assert.IsTrue(currentPage.SwitchLanguage("ENG"));
            Assert.IsTrue(currentPage.NewsArticlesAreDisplayed(4));
            Assert.IsTrue(currentPage.TopIndexButtonsMatch(_topIndexMenuButtonsNames));
            Assert.IsTrue(currentPage.CopyrightTextIsCorrect(_copyrightText));
        }

        /// <summary>
        ///     Performs the 2nd sequence of tests. Tests search functionality of the Railway site:
        ///     a. Open site
        ///     b. Type in "Поиск" input 20 random symbols (they should be different for each execution)
        ///     c. Check, that address in browser changed to the correct one
        ///     d. Check, that text "К сожалению, на ваш поисковый запрос ничего не найдено." is displayed on a page
        ///     e. Clear entered on step b value and enter “Санкт-Петербург” instead
        ///     f. Click "Найти"
        ///     g. Check, that 15 links to the articles are displayed on a screen
        ///     h. Write the text from above mentioned links into the console
        /// </summary>
        [Test]
        public void RailWaySiteSearchTest()
        {
            //Arrange
            var requiredSearchResponse = "К сожалению, на ваш поисковый запрос ничего не найдено.";
            var correctQuery = "Санкт-Петербург";
            var gibberishQuerry = PageMethods.GenerateGibberish(20);
            var currentPage = new RailwaySearchResultsPage(_driver);
            //Act
            currentPage.Navigate(_baseUrl);
            currentPage.Search(gibberishQuerry);
            //Assert
            Assert.IsTrue(currentPage.IsUrlCorrect("https://www.rw.by/search/?s=Y&q=" + gibberishQuerry + ""));
            Assert.IsTrue(currentPage.SearchResultDisplaysRequiredResponse(requiredSearchResponse));
            currentPage.ClearSearchInput();
            currentPage.RepeatSearch(correctQuery);
            Assert.AreEqual(currentPage.CountSearchResults(), 15);
            Assert.IsTrue(PageMethods.DisplayResults(currentPage.GetSearchResultsText()));
        }

        /// <summary>
        ///     Test the calendar functionality:
        ///     a.	Open site
        ///     b.	Type in "Откуда" and "Куда" some valid locations
        ///     c.	Click on calendar button, and select +5 days from today
        ///     d.	Click on "Найти"
        ///     e.	Write to the console departure time and trains name, use format:
        ///     i.	"Минск-Пассажирский — Брест-Центральный" – 19:25
        ///     ii.  "Витебск-Пассажирский — Брест-Центральный" – 23:06
        ///     f.	Click on the first link
        ///     g.	Check that name of train is displayed on a page.
        ///     h.	Check that text under the text "Дни курсирования" is not empty
        ///     i.	Return to the main page of site by clicking on the site’s logo
        ///     j.	Check that main page loaded fine.
        /// </summary>
        [Test]
        public void RailWaySiteCalendarTest()
        {
            //Arrange
            var fromLocaion = "Брест";
            var destination = "Минск";
            var currentPage = new RailwayCalendarPage(_driver);
            var daysFromToday = 30;
            currentPage.Navigate(_baseUrl);
            //Assert
            currentPage.SearchTrains(fromLocaion, destination, daysFromToday);
            var trainsSchedule = currentPage.GetTrainsSchedule();
            Assert.IsTrue(PageMethods.DisplayResults(trainsSchedule));
            Assert.IsTrue(currentPage.ContainsTrainNames());
            currentPage.ClickLogo();
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.Close(_driver);
        }
    }
}