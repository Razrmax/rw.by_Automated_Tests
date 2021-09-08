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
    internal class RailwayCalendarPage
    {
        private IWebDriver Driver { get; }
        private PageMethods PageMethods { get; }

        protected internal RailwayCalendarPage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(driver, this);
            PageMethods = new PageMethods();
        }

        [FindsBy(How = How.XPath, Using = "//input[@id='acFrom']")]
        private IWebElement FromInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='acTo']")]
        private IWebElement DestinationInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='yDate']")]
        private IWebElement CalendarInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='std-button']/input")]
        private IWebElement ScheduleSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ui-datepicker-group ui-datepicker-group-first']")]
        private IWebElement CurrentMonth { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ui-datepicker-group ui-datepicker-group-middle']")]
        private IWebElement NextMonth { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sch-title__title h2']")]
        private IWebElement TrainNameDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='sch-title__train-num']")]
        private IWebElement TrainNumberDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sch-title__descr']")]
        private IWebElement TravelingDaysInfoLocation { get; set; }

        [FindsBy(How = How.XPath, Using = "//img[@title='БелЖД']")]
        private IWebElement LogoImage { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='sch-table__route']")]
        private IWebElement FirstLinkLocation { get; set; }



        protected internal void Navigate(string url)
        {
            PageMethods.Navigate(Driver, url);
        }

        protected internal void SearchTrains(string fromLocation, string destination, int daysFromToday)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysFromToday);
            var targetDay = futureDate.Day;
            var targetMonth = futureDate.Month == today.Month ? CurrentMonth : NextMonth;
            PageMethods.SearchTrains(Driver,FromInput, DestinationInput, CalendarInput, targetMonth, targetDay,
                ScheduleSearchBtn, fromLocation, destination);
        }

        protected internal ICollection<string> GetTrainsSchedule()
        {
            return PageMethods.GetTrainsSchedule(Driver);
        }

        protected internal bool FirstLinkInTrainSearchContainsInformation()
        {
            FirstLinkLocation.Click();
            bool containsTrainNames = PageMethods.ContainsTextualInformation(TrainNameDiv);
            bool containsTrainNumbers = PageMethods.ContainsTextualInformation(TrainNumberDiv);
            bool containsTravelingDays = PageMethods.ContainsTextualInformation(TravelingDaysInfoLocation); 

            return containsTrainNames && containsTrainNumbers;
        }

        protected internal bool ClickLogoReturnsToHomepage()
        {
            string homeUrl = "https://www.rw.by/";
            PageMethods.ClickElement(Driver, LogoImage);
            bool logoImageReturnsToHomepage = PageMethods.IsUrlCorrect(homeUrl, Driver);
            return logoImageReturnsToHomepage;
        }
    }
}
