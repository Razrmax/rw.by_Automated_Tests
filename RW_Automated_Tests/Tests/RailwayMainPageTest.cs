using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using RW_Automated_Tests.PageObjects;

namespace RW_Automated_Tests.Tests
{
    [TestFixture]
    class RailwayMainPageTest
    {
        private string _baseUrl = @"https:\\www.rw.by";
        private IWebDriver _driver;

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
        [TestCaseSource(typeof(DriverFactory), "SelectBrowserToRunWith")]
        public void RailWaySiteMainPageTest(string browser)
        {
            //Arrange
            _driver = DriverFactory.Create(browser);
            var currentPage = new RailwayMainPage(_driver);
            currentPage.Navigate(_baseUrl);
            var _topIndexMenuButtonsNames = new HashSet<string>
                {"press center", "passenger services", "freight", "corporate", "contacts"};
            var _copyrightText = "© 2023 Belarusian Railway";
            //Assert
            Assert.IsTrue(currentPage.SwitchLanguage("ENG"));
            Assert.IsTrue(currentPage.NewsArticlesAreDisplayed(4));
            Assert.IsTrue(currentPage.TopIndexButtonsMatch(_topIndexMenuButtonsNames));
            Assert.IsTrue(currentPage.CopyrightTextIsCorrect(_copyrightText));
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.Close(_driver);
        }
    }
}
