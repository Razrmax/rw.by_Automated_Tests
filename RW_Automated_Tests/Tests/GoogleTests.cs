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
    [TestFixture]
    public class GoogleTests
    {
        private string _baseUrl;
        private IWebDriver _driver;

        //[SetUp]
        //public void Setup()
        //{
        //    _driver = DriverFactory.Create();
        //    _baseUrl = "https:\\www.rw.by";
        //}

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
        [TestCaseSource(typeof(DriverFactory), "SelectBrowserToRunWith")]
        public void OpenGoogleFindAndLoadRailwaySite(string browser)
        {
            //Arrange
            _driver = DriverFactory.Create(browser);
            string _searchQuery = "белорусская железная дорога";
            _baseUrl= "https://google.com";
            var currentPage = new GooglePage(_driver);
            //Act
            currentPage.Navigate(_baseUrl);
            currentPage.Search(_searchQuery);
            currentPage.ClickFirstLink();
            //Assert
            Assert.IsTrue(currentPage.PageIsLoaded("https://www.rw.by/be/"));
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.Close(_driver);
        }
    }
}