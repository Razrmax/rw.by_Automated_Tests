using System;
using NUnit.Framework;
using OpenQA.Selenium;
using RW_Automated_Tests.Helpers;
using RW_Automated_Tests.PageObjects;

namespace RW_Automated_Tests.Unit_Tests
{
    public class GoogleTest
    {
        private string _baseUrl;
        private IWebDriver _driver;
        private string _searchQuery;

        [SetUp]
        public void Setup()
        {
            _driver = DriverFactory.Create();
            _baseUrl = "http:\\google.com";
            _searchQuery = "белорусская железная дорога";
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
            try
            {
                var currentPage = new GooglePage(_driver);
                currentPage.Navigate(_baseUrl);
                currentPage.Search(_searchQuery);
                currentPage.ClickLink("https://www.rw.by/");
                //Verify that the page has been successfully loaded
                Assert.That(currentPage.PageIsLoaded("https://www.rw.by/be/"));
                DriverFactory.Close(_driver);
            }
            catch (Exception)
            {
                DriverFactory.Close(_driver);
                throw;
            }
        }
    }
}