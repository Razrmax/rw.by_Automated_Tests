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
        public void OpenGoogleFindAndLoadRailwaySite()
        {
            //Arrange
            string _searchQuery = "белорусская железная дорога";
            _baseUrl = "https://google.com";
            //Act
            var currentPage = new GooglePage(_driver);
            currentPage.Navigate(_baseUrl);
            currentPage.Search(_searchQuery);
            currentPage.ClickFirstLink();
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
        ///     a. Open site
        ///     b. Type in "Поиск" 20 random symbols (they should be different for each execution)
        ///     c. Check, that address in browser changed to the correct one
        ///     d. Check, that text "К сожалению, на ваш поисковый запрос ничего не найдено." is displayed on a page
        ///     
        /// </summary>
        [Test]
        public void SiteSearchTestWithGibberishQuery()
        {
            //Arrange
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
        [Test]
        public void SiteSearchTestWithCorrectQuery()
        {
            //Arrange
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

        /// <summary>
        ///     Test the train search functionality:
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
        public void TrainsSearchTest()
        {
            //Arrange
            var fromLocaion = "Брест";
            var destination = "Минск";
            var currentPage = new RailwayCalendarPage(_driver);
            var daysFromToday = 30;
            
            //Act
            currentPage.Navigate(_baseUrl);
            currentPage.SearchTrains(fromLocaion, destination, daysFromToday);
            var trainsSchedule = currentPage.GetTrainsSchedule();
            //Assert
            Assert.IsTrue(PageMethods.DisplayResults(trainsSchedule));
            Assert.IsTrue(currentPage.FirstLinkInTrainSearchContainsInformation());
        }

        /// <summary>
        ///  i.	Return to the main page of site by clicking on the site’s logo
        ///  j.	Check that main page loaded fine.
        /// </summary>
        [Test]
        public void ClickingOnLogoFromSchedulePageLoadsMainPage()
        {
            //Arrange
            var currentPage = new RailwayCalendarPage(_driver);
            _baseUrl =
                "https://pass.rw.by/ru/route/?from=%D0%91%D1%80%D0%B5%D1%81%D1%82&from_exp=2100200&from_esr=130007&to=%D0%9C%D0%B8%D0%BD%D1%81%D0%BA&to_exp=2100000&to_esr=140210&date=&type=1";

            //Act
            currentPage.Navigate(_baseUrl);
            var clickingOnLogoReturnsToHomepage = currentPage.ClickLogoReturnsToHomepage();
            //Assert
            Assert.IsFalse(clickingOnLogoReturnsToHomepage);
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.Close(_driver);
        }
    }
}