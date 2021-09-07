using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using RW_Automated_Tests.PageObjects;

namespace RW_Automated_Tests.Helpers
{
    public class PageMethodsUtils : PageMethods
    {
        public override void Search(string query, IWebElement searchInput, IWebElement submitBtn)
        {
            if (ElementIsVisible(searchInput)) EnterTextIntoInput(searchInput, query);

            if (ElementIsVisible(submitBtn)) ClickElement(submitBtn);
        }

        public override void ClickLink(By by, IWebElement container)
        {
            var elementToClick = GetElement(container, by);
            elementToClick.Click();
        }

        public override void ClickElement(IWebElement element)
        {
            element.Click();
        }

        public override void SwitchLanguage(string targetLanguageAbbr, IWebElement languagePanel)
        {
            var by = By.XPath("//a[contains(text(),'" + targetLanguageAbbr + "')]");
            ClickLink(by, languagePanel);
        }

        public override bool ElementIsVisible(IWebElement element)
        {
            Thread.Sleep(200);
            return element.Displayed;
        }

        public override int CountItems(By by, IWebElement element)
        {
            var items = GetElements(element, by);
            return items.Count;
        }

        /// <summary>
        ///     Verifies that the text at elements location matches the required values.
        /// </summary>
        /// <param name="textToMatch">Collection of text that is required to match</param>
        /// <param name="elementsLocation">Location of the elements which must match</param>
        /// <param name="by">Mechanism by which to find the elements inside their location</param>
        /// <returns></returns>
        public override bool ElementsTextMatch(HashSet<string> textToMatch, IWebElement elementsLocation, By by)
        {
            var actualTextElements = ExtractTextFromElements(elementsLocation, by);
            if (actualTextElements.Count < textToMatch.Count) return false;

            foreach (var el in actualTextElements)
                if (textToMatch.Contains(el))
                    textToMatch.RemoveWhere(t => t == el);

            return textToMatch.Count == 0;
        }

        public override bool CopyrightTextIsCorrect(string copyrightText, IWebElement element, By by)
        {
            var text = element.Text;
            return text.Contains(copyrightText);
        }

        /// <summary>
        ///     Extracts the text located inside the element found with specific By locator.
        /// </summary>
        /// <param name="container">location of the elements to be extracted</param>
        /// <param name="by">delimiting tag</param>
        /// <returns></returns>
        public override List<string> ExtractTextFromElements(IWebElement container, By by)
        {
            var elements = GetElements(container, by);
            var text = new List<string>();
            foreach (var el in elements)
            {
                var s = el.Text.ToLower();
                text.Add(s);
            }

            return text;
        }

        /// <summary>
        ///     Checks that the required url matches the actual url.
        /// </summary>
        /// <param name="requiredUrl"></param>
        /// <param name="driver"></param>
        /// <returns>True if actual url matches the required one</returns>
        public override bool IsUrlCorrect(string requiredUrl, IWebDriver driver)
        {
            var actualUrl = driver.Url;
            return actualUrl == requiredUrl;
        }

        public override void ClearInput(IWebElement element)
        {
            element.Clear();
        }

        public override void EnterTextIntoInput(IWebElement element, string text)
        {
            element.SendKeys(text);
        }

        public override void SearchTrains(IWebElement from, IWebElement destination, IWebElement calendar,
            IWebElement targetMonth, int targetDay, IWebElement submitBtn,
            string fromName, string destinationName)
        {
            EnterTextIntoInput(from, fromName);
            EnterTextIntoInput(destination, destinationName);
            ClickElement(calendar);
            SelectDateFromCalendar(targetMonth, targetDay.ToString());
            ClickElement(submitBtn);
        }

        public override void SelectDateFromCalendar(IWebElement targetMonth, string targetDay)
        {
            var daysOfMonth = GetElements(targetMonth, By.TagName("td"));
            foreach (var t in daysOfMonth)
                if (t.Text == targetDay)
                {
                    var element = GetElement(t, By.LinkText(targetDay));
                    ClickElement(element);
                    break;
                }
        }

        public override IWebElement GetElement(IWebElement container, By by)
        {
            return container.FindElement(by);
        }

        public override IReadOnlyCollection<IWebElement> GetElements(IWebElement container, By by)
        {
            var elements = container.FindElements(by);
            return elements;
        }

        public override ICollection<string> GetTrainsSchedule(IWebDriver driver)
        {
            //var schedules = GetElements(scheduleTotal, By.XPath("//div[@class='sch - table__row']"));
            var departureStation = driver.FindElements(By.XPath("//div[@class='sch-table__station train-from-name']"));
            var arrivalStation = driver.FindElements(By.XPath("//div[@class='sch-table__station train-to-name']"));
            var departureTime = driver.FindElements(By.XPath("//div[@class='sch-table__time train-from-time']"));
            var schedulesText = new List<string>();
            for (var i = 0; i < departureStation.Count; i++)
                schedulesText.Add(@"" + departureStation.ElementAt(i).Text + " — " + arrivalStation.ElementAt(i).Text +
                                  " — " + departureTime.ElementAt(i).Text + "");

            return schedulesText;
        }

        public override bool ContainsTrainNames(IWebDriver driver)
        {
            var trainNumbers = driver.FindElements(By.XPath("//span[@class='train-number']"));
            var trainNames = driver.FindElements(By.XPath("//span[@class='train-route']"));
            for (var i = 0; i < trainNumbers.Count; i++)
                if (trainNumbers.ElementAt(i).Text == "" || trainNames.ElementAt(i).Text == "")
                    return false;

            return true;
        }

        public override void ClickLogoImage(IWebElement logoImage)
        {
            Thread.Sleep(100);
            ClickElement(logoImage);
        }
    }
}