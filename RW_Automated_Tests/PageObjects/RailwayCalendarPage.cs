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

        [FindsBy(How = How.XPath, Using = "//span[@class='std-button']//input[@type='submit']")]
        private IWebElement ScheduleSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ui-datepicker-group ui-datepicker-group-first']")]
        private IWebElement CurrentMonth { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ui-datepicker-group ui-datepicker-group-middle']")]
        private IWebElement NextMonth { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sch-table__row']")]
        private IWebElement TrainsSchedule { get; set; }

        [FindsBy(How = How.XPath, Using = "//img[@title='БелЖД']")]
        private IWebElement LogoImage { get; set; }

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
            PageMethods.SearchTrains(FromInput, DestinationInput, CalendarInput, targetMonth, targetDay,
                ScheduleSearchBtn, fromLocation, destination);
        }

        protected internal ICollection<string> GetTrainsSchedule()
        {
            return PageMethods.GetTrainsSchedule(Driver);
        }

        protected internal bool ContainsTrainNames()
        {
            return PageMethods.ContainsTrainNames(Driver);
        }

        protected internal void ClickLogo()
        {
            PageMethods.ClickLogoImage(LogoImage);
        }
    }
}
