using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SeleniumCSharpHelper
{
    public static class WebDriverExtensions
    {
        public static void Browser_NavigateChrome(ref IWebDriver driver, string appUrl, string title, bool maximize = true)
        {
            if (driver == null)
            {
                driver = new ChromeDriver();
                driver.Navigate().GoToUrl(appUrl);
            }
            else
            {
                driver.Url = appUrl;
            }

            Browser_Navigate(driver, title, maximize);
        }

        public static void Browser_NavigateInternetExplorer(ref IWebDriver driver, string appUrl, string title, bool maximize = true)
        {
            if (driver == null)
            {
                driver = new InternetExplorerDriver();
                driver.Navigate().GoToUrl(appUrl);
            }
            else
            {
                driver.Url = appUrl;
            }

            Browser_Navigate(driver, title, maximize);
        }

        private static void Browser_Navigate(IWebDriver driver, string title, bool maximize)
        {
            driver.Driver_WaitUntilTitleContains(title);

            if (maximize)
            {
                driver.Manage().Window.Maximize();
            }
        }

        public static IWebElement Element_Find(this IWebDriver driver, By by, int timeoutInSeconds = 60)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return driver.FindElement(by);
        }

        public static IWebElement Elements_ClickElementByText(this IWebDriver driver, By by, string text, int timeoutInSeconds = 60, int sleepTime = 1000)
        {
            int retries = 4;

            IWebElement element = null;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                    IReadOnlyCollection<IWebElement> elements = wait.Until(drv => drv.FindElements(by));

                    element = elements.FirstOrDefault(w => w.Text == text);

                    element?.Click();

                    break;
                }
                catch (StaleElementReferenceException)
                { }
            }

            return element;
        }

        public static IWebElement Element_Click(this IWebDriver driver, By by, int timeoutInSeconds = 60)
        {
            int retries = 4;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    IWebElement webElement = Element_Find(driver, by);

                    if (webElement != null && webElement.Displayed)
                    {
                        var actions = new Actions(driver);

                        actions.MoveToElement(webElement).Click().Perform();
                        return webElement;
                    }
                }
                catch (StaleElementReferenceException)
                { }
            }

            return null;
        }

        public static void Element_MouseOver(this IWebDriver driver, By by)
        {
            int retries = 4;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    var action = new Actions(driver);

                    IWebElement webElement = driver.FindElement(by);

                    action.MoveToElement(webElement).Perform();

                    return;
                }
                catch (StaleElementReferenceException)
                { }
            }
        }

        public static bool Element_Visible(this IWebDriver driver, By by)
        {
            int retries = 4;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    IWebElement webElement = driver.FindElement(by);

                    bool visible = webElement.Displayed;

                    return visible;
                }
                catch (StaleElementReferenceException)
                {
                }
                catch (NoSuchElementException)
                {
                    return false;
                }

                Thread.Sleep(200);
            }

            return false;
        }

        public static IWebElement Element_MoveToElement(this IWebDriver driver, By by)
        {
            int retries = 4;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    var actions = new Actions(driver);

                    IWebElement webElement = driver.FindElement(by);
                    actions.MoveToElement(webElement);

                    actions.Perform();
                    
                    return webElement;
                }
                catch (StaleElementReferenceException)
                {
                }

                Thread.Sleep(200);
            }

            return null;
        }

        public static IWebElement Element_SendKeys(this IWebDriver driver, By by, string text, int timeoutInSeconds = 60)
        {
            IWebElement webElement = Element_Find(driver, by);

            webElement.SendKeys(text);

            return webElement;
        }


        public static SelectElement SelectElement_SelectByTextRetry(this IWebDriver driver, By by, string textToFind, int timeout = 5)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    IWebElement webElement = Element_Find(driver, by, timeout);

                    var selectElement = new SelectElement(webElement);

                    selectElement.SelectByText(textToFind);

                    return selectElement;
                }
                catch (NoSuchElementException)
                { }
                catch (StaleElementReferenceException)
                { }

                Thread.Sleep(200);
            }

            throw new NoSuchElementException();
        }

        public static string SelectElement_GetText(this IWebDriver driver, By by, int timeout = 5)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    IWebElement webElement = Element_Find(driver, by, timeout);

                    var selectElement = new SelectElement(webElement);

                    string text = selectElement.SelectedOption.Text;

                    return text;
                }
                catch (StaleElementReferenceException)
                { }
            }

            return null;
        }

        public static void Element_SetAttribute(this IWebDriver driver, By by, string attributeName, string value)
        {
            IWebElement webElement = Element_Find(driver, by);

            var wrappedElement = webElement as IWrapsDriver;

            if (wrappedElement == null)
            {
                throw new ArgumentException("element", "Element must wrap a web driver");
            }

            IWebDriver wrappedDriver = wrappedElement.WrappedDriver;

            IJavaScriptExecutor javascript = wrappedDriver as IJavaScriptExecutor;

            if (javascript == null)
            {
                throw new ArgumentException("element", "Element must wrap a web driver that supports javascript execution");
            }

            javascript.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])", webElement, attributeName, value);
        }

        public static string Element_GetAttribute(this IWebDriver driver, By by, string attributeName)
        {
            IWebElement webElement = Element_Find(driver, by);

            string value = String.Empty;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    value = webElement.GetAttribute(attributeName);

                    break;
                }
                catch (StaleElementReferenceException)
                {}
            }

            return value;
        }

        public static object Driver_ExecuteJavascript(this IWebDriver driver, string javascript)
        {
            object value = ((IJavaScriptExecutor)driver).ExecuteScript(javascript);

            return value;
        }
        public static bool Element_WaitUntilTextIsVisible(this IWebDriver driver, By elementName, string textToFind)
        {
            bool visible = false;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    var elementGetText = driver.Element_GetText(elementName);

                    if (elementGetText == textToFind)
                    {
                        visible = true;
                        break;
                    }
                }
                catch (StaleElementReferenceException)
                { }

                Thread.Sleep(200);
            }

            return visible;
        }

        public static void Element_WaitUntilClickable(this IWebDriver driver, By by, int timeoutInSeconds = 60)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 60));

            wait.Until((d) =>
            {
                try
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(by));

                    var element = driver.FindElement(by);

                    element.Click();

                    return element;
                }
                catch (StaleElementReferenceException)
                { }

                return null;
            });
        }

        public static void Element_WaitUntilNotVisible(this IWebDriver driver, By by, int timeoutInSeconds = 5)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 60));

            wait.Until((d) =>
            {
                try
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));

                    return d;
                }
                catch (StaleElementReferenceException)
                { }

                return null;
            });
        }

        public static bool Element_WaitUntilVisible(this IWebDriver driver, By by, Action<By> optionalFunctionToRun = null, By optionalArg = null, int sleepTime = 500)
        {
            bool visible = false;

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (optionalFunctionToRun != null && optionalArg != null)
                    {
                        optionalFunctionToRun(optionalArg);

                        Thread.Sleep(sleepTime);
                    }

                    visible = driver.Element_Visible(by);

                    if (visible)
                    {
                        break;
                    }
                }
                catch (NoSuchElementException)
                { }

                Thread.Sleep(sleepTime);
            }

            return visible;
        }

        public static void Driver_WaitUntilTitleContains(this IWebDriver driver, string title, int timeoutInSeconds = 15)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutInSeconds));

            try
            {
                wait.Until(ExpectedConditions.TitleContains(title));
            }
            catch (Exception e) when (e is WebDriverTimeoutException || e is InvalidOperationException)
            {
                if (e.Message == "chrome not reachable")
                {
                    throw new Exception("Web browser not running!", e);
                }
                else
                {
                    throw;
                }
            }
        }

        public static string Element_GetText(this IWebDriver driver, By by)
        {
            int retries = 4;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    IWebElement webElement = driver.FindElement(by);

                    string text = webElement.Text;

                    return text;
                }
                catch (StaleElementReferenceException)
                { }
            }

            return null;
        }

        public static void RadioButtonElement_SetValue(this IWebDriver driver, By by, int itemToSelect)
        {
            int retries = 4;

            IReadOnlyCollection<IWebElement> webElements = null;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    webElements = driver.FindElements(by);

                    webElements.ElementAt(itemToSelect).Click();
                }
                catch (StaleElementReferenceException)
                { }
            }
        }

        public static object HiddenElement_GetText(this IWebDriver driver, string id)
        {
            object text = driver.Driver_ExecuteJavascript($"return document.getElementById('{id}').value;");

            return text;
        }

        public static void Element_WaitUntil(this IWebDriver driver, Action<By> functionToRun, By by, Action<By> optionalFunctionToRun = null, By optionalArg = null, int sleepTime = 500)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (optionalFunctionToRun != null && optionalArg != null)
                    {
                        optionalFunctionToRun(optionalArg);

                        Thread.Sleep(sleepTime);
                    }

                    functionToRun(by);

                    break;
                }
                catch (NoSuchElementException)
                { }
                catch (WebDriverException)
                { }

                Thread.Sleep(sleepTime);
            }
        }

        public static bool Element_Selected(this IWebDriver driver, By by)
        {
            int retries = 4;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    IWebElement webElement = driver.FindElement(by);

                    bool selected = webElement.Selected;

                    return selected;
                }
                catch (StaleElementReferenceException)
                { }
            }

            return false;
        }


    }

}
