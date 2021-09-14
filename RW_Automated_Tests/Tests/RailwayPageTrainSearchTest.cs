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
    class RailwayPageTrainSearchTest
    {
        private string _baseUrl = @"https:\\www.rw.by";
        private IWebDriver _driver;

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
        [Test, Order(1)]
        [TestCaseSource(typeof(DriverFactory), "SelectBrowserToRunWith")]
        public void TrainsSearchTest(string browser)
        {
            //Arrange
            _driver = DriverFactory.Create(browser);
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
            Assert.IsTrue(currentPage.FirstLinkInTrainSearchContainsRequiredInformation());
            DriverFactory.Close(_driver);
        }

        /// <summary>
        ///  i.	Return to the main page of site by clicking on the site’s logo
        ///  j.	Check that main page loaded fine.
        /// </summary>
        [Test, Order(2)]
        [TestCaseSource(typeof(DriverFactory), "SelectBrowserToRunWith")]
        public void ClickingOnLogoFromSchedulePageLoadsMainPage(string browser)
        {
            //Arrange
            _driver = DriverFactory.Create(browser);
            var currentPage = new RailwayCalendarPage(_driver);
            _baseUrl =
                "https://pass.rw.by/ru/route/?from=%D0%91%D1%80%D0%B5%D1%81%D1%82&from_exp=2100200&from_esr=130007&to=%D0%9C%D0%B8%D0%BD%D1%81%D0%BA&to_exp=2100000&to_esr=140210&date=&type=1";

            //Act
            currentPage.Navigate(_baseUrl);
            var clickingOnLogoReturnsToHomepage = currentPage.ClickLogoReturnsToHomepage();
            //Assert
            Assert.IsFalse(clickingOnLogoReturnsToHomepage);
            DriverFactory.Close(_driver);
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.Close(_driver);
        }
    }
}
