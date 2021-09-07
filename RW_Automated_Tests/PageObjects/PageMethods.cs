using System.Collections.Generic;
using OpenQA.Selenium;

namespace RW_Automated_Tests.PageObjects
{
    public abstract class PageMethods
    {
        public abstract void Search(string query, IWebElement searchInput, IWebElement submitBtn);
        public abstract void ClickLink(By by, IWebElement container);
        public abstract void ClickElement(IWebElement element);
        public abstract void SwitchLanguage(string targetLanguageAbbr, IWebElement languagePanel);
        public abstract int CountItems(By by, IWebElement element);
        public abstract bool ElementIsVisible(IWebElement element);
        public abstract bool ElementsTextMatch(HashSet<string> textToMatch, IWebElement elementsLocation, By by);
        public abstract bool CopyrightTextIsCorrect(string copyrightText, IWebElement element, By by);
        public abstract List<string> ExtractTextFromElements(IWebElement container, By by);
        public abstract bool IsUrlCorrect(string requiredUrl, IWebDriver driver);
        public abstract void ClearInput(IWebElement element);
        public abstract void EnterTextIntoInput(IWebElement element, string text);

        public abstract void SearchTrains(IWebElement from, IWebElement destination, IWebElement calendar,
            IWebElement targetMonth, int targetDay, IWebElement submitBtn,
            string fromName, string destinationName);

        public abstract void SelectDateFromCalendar(IWebElement targetMonth, string targetDay);
        public abstract IWebElement GetElement(IWebElement container, By by);
        public abstract IReadOnlyCollection<IWebElement> GetElements(IWebElement container, By by);
        public abstract ICollection<string> GetTrainsSchedule(IWebDriver driver);
        public abstract bool ContainsTrainNames(IWebDriver driver);
        public abstract void ClickLogoImage(IWebElement logoImage);
    }
}