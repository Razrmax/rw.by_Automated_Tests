﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RW_Automated_Tests.PageObjects;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace RW_Automated_Tests.Helpers
{
    public class PageMethods
    {
        protected internal void Search(IWebDriver driver, string query, IWebElement searchInput, IWebElement submitBtn)
        {
            if (ElementIsVisible(driver, searchInput)) EnterTextIntoInput(searchInput, query);
           
            if (ElementIsVisible(driver, submitBtn)) ClickElement(driver, submitBtn);
        }

        protected internal void ClickLink(By by, IWebElement container)
        {
            var elementToClick = GetElement(container, by);
            elementToClick.Click();
        }

        protected internal void ClickElement(IWebDriver driver, IWebElement element)
        {

            if (ElementIsVisible(driver, element))
            {
                element.Click();
            }
        }

        protected internal void SwitchLanguage(string targetLanguageAbbr, IWebElement languagePanel)
        {
            var by = By.XPath("//a[contains(text(),'" + targetLanguageAbbr + "')]");
            ClickLink(by, languagePanel);
        }

        protected internal bool ElementIsVisible(IWebDriver driver, IWebElement element)
        {
            var elementToBeVisible =
                new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { element });
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(elementToBeVisible));
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected internal int CountItems(By by, IWebElement element)
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
        protected internal bool ElementsTextMatch(HashSet<string> textToMatch, IWebElement elementsLocation, By by)
        {
            var actualTextElements = ExtractTextFromElements(elementsLocation, by);
            if (actualTextElements.Count < textToMatch.Count) return false;

            foreach (var el in actualTextElements)
                if (textToMatch.Contains(el))
                    textToMatch.RemoveWhere(t => t == el);

            return textToMatch.Count == 0;
        }

        protected internal bool CopyrightTextIsCorrect(string copyrightText, IWebElement element)
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
        protected internal List<string> ExtractTextFromElements(IWebElement container, By by)
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
        protected internal bool IsUrlCorrect(string requiredUrl, IWebDriver driver)
        {
            var actualUrl = driver.Url;
            return actualUrl == requiredUrl;
        }

        protected internal void ClearInput(IWebElement element)
        {
            element.Clear();
        }

        protected internal void EnterTextIntoInput(IWebElement element, string text)
        {
            element.SendKeys(text);
        }

        protected internal void SearchTrains(IWebDriver driver, IWebElement from, IWebElement destination, IWebElement calendar,
            IWebElement targetMonth, int targetDay, IWebElement submitBtn,
            string fromName, string destinationName)
        {
            EnterTextIntoInput(from, fromName);
            EnterTextIntoInput(destination, destinationName);
            ClickElement(driver, calendar);
            SelectDateFromCalendar(driver, targetMonth, targetDay.ToString());
            submitBtn.Click();
        }

        protected internal void SelectDateFromCalendar(IWebDriver driver, IWebElement targetMonth, string targetDay)
        {
            var daysOfMonth = GetElements(targetMonth, By.TagName("td"));
            foreach (var t in daysOfMonth)
                if (t.Text == targetDay)
                {
                    var element = GetElement(t, By.LinkText(targetDay));
                    ClickElement(driver, element);
                    break;
                }
        }

        protected internal IWebElement GetElement(IWebElement container, By by)
        {
            return container.FindElement(by);
        }

        protected internal IReadOnlyCollection<IWebElement> GetElements(IWebElement container, By by)
        {
            var elements = container.FindElements(by);
            return elements;
        }

        protected internal ICollection<string> GetTrainsSchedule(IWebDriver driver)
        {
            var departureStation = driver.FindElements(By.XPath("//div[@class='sch-table__station train-from-name']"));
            var arrivalStation = driver.FindElements(By.XPath("//div[@class='sch-table__station train-to-name']"));
            var departureTime = driver.FindElements(By.XPath("//div[@class='sch-table__time train-from-time']"));
            var schedulesText = new List<string>();
            for (var i = 0; i < departureStation.Count; i++)
                schedulesText.Add(@"" + departureStation.ElementAt(i).Text + " — " + arrivalStation.ElementAt(i).Text +
                                  " — " + departureTime.ElementAt(i).Text + "");

            return schedulesText;
        }

        protected internal bool ContainsTextualInformation(IWebElement trainName)
        {
            return trainName.Text != "";
        }

        protected internal void Navigate(IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        protected internal static string GenerateGibberish(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected internal static bool DisplayResults(ICollection<string> results)
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