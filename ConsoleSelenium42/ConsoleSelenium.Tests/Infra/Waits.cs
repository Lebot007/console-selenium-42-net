using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Infra;

/// <summary>
/// Centraliza as esperas explícitas (WebDriverWait) usadas pelos Page Objects.
/// </summary>
public static class Waits
{
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(15);

    public static WebDriverWait For(IWebDriver driver, TimeSpan? timeout = null)
    {
        var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        return wait;
    }

    public static IWebElement Visible(IWebDriver driver, By locator) =>
        For(driver).Until(d =>
        {
            var element = d.FindElement(locator);
            return element.Displayed ? element : null;
        })!;

    public static void UrlMatches(IWebDriver driver, Func<string, bool> condition) =>
        For(driver).Until(d => condition(d.Url));
}
