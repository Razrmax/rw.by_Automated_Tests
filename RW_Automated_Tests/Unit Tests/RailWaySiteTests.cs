using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using RW_Automated_Tests.PageObjects;

namespace RW_Automated_Tests.Unit_Tests
{
    public class RailWaySiteTests
    {
        private string _baseUrl;

        private string _copyrightText;
        private IWebDriver _driver;
        private HashSet<string> _topIndexMenuButtonsNames;

        [SetUp]
        public void Setup()
        {
            _driver = DriverFactory.Create();
            _baseUrl = "https:\\www.rw.by";
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
            var currentPage = new RailwayPage(_driver);
            currentPage.Navigate(_baseUrl);
            _topIndexMenuButtonsNames = new HashSet<string>
                {"press center", "tickets", "passenger services", "freight", "corporate"};
            _copyrightText = "© 2021 Belarusian Railway";
            //Assert
            try
            {
                Assert.IsTrue(currentPage.SwitchLanguage("ENG"));
                Assert.IsTrue(currentPage.NewsArticlesAreDisplayed(4));
                Assert.IsTrue(currentPage.TopIndexButtonsMatch(_topIndexMenuButtonsNames));
                Assert.IsTrue(currentPage.CopyrightTextIsCorrect(_copyrightText));
            }
            catch (Exception)
            {
                DriverFactory.Close(_driver);
                throw;
            }

            DriverFactory.Close(_driver);
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
            var gibberishQuerry = GenerateGibberish(20);
            var currentPage = new RailwayPage(_driver);
            currentPage.Navigate(_baseUrl);
            //Assert
            try
            {
                currentPage.Search(gibberishQuerry);
                Assert.IsTrue(currentPage.IsUrlCorrect("https://www.rw.by/search/?s=Y&q=" + gibberishQuerry + ""));
                Assert.IsTrue(currentPage.SearchResultDisplaysRequiredResponse(requiredSearchResponse));
                currentPage.ClearSearchInput();
                currentPage.RepeatSearch(correctQuery);
                Assert.AreEqual(currentPage.CountSearchResults(), 15);
                var searchResultsText = currentPage.GetSearchResultsText();
                DisplayResults(searchResultsText);
                Console.WriteLine();
            }
            catch (Exception)
            {
                DriverFactory.Close(_driver);
                throw;
            }

            DriverFactory.Close(_driver);
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
            var currentPage = new RailwayPage(_driver);
            var daysFromToday = 30;
            currentPage.Navigate(_baseUrl);
            //Assert
            try
            {
                currentPage.SearchTrains(fromLocaion, destination, daysFromToday);
                var trainsSchedule = currentPage.GetTrainsSchedule();
                Assert.IsTrue(DisplayResults(trainsSchedule));
                Assert.IsTrue(currentPage.ContainsTrainNames());
                currentPage.ClickLogo();
            }
            catch (Exception)
            {
                DriverFactory.Close(_driver);
                throw;
            }

            DriverFactory.Close(_driver);
        }

        private string GenerateGibberish(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool DisplayResults(ICollection<string> results)
        {
            try
            {
                foreach (var t in results) Debug.WriteLine(t);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}