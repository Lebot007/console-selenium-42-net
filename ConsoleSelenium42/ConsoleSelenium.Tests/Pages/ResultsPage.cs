using ConsoleSelenium.Tests.Infra;
using OpenQA.Selenium;

namespace ConsoleSelenium.Tests.Pages;

/// <summary>
/// Page Object da página de resultados do DuckDuckGo.
/// </summary>
public class ResultsPage
{
    private static readonly By OrganicResultLinks = By.CssSelector(
        "a[data-testid='result-title-a'], article[data-testid='result'] h2 a");

    private readonly IWebDriver _driver;

    public ResultsPage(IWebDriver driver) => _driver = driver;

    public string Title => _driver.Title;
    public string Url => _driver.Url;

    public static bool IsResultsUrl(string url) =>
        url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase) &&
        url.Contains("q=", StringComparison.OrdinalIgnoreCase);

    public ResultsPage WaitUntilLoaded()
    {
        Waits.UrlMatches(_driver, IsResultsUrl);
        Waits.Visible(_driver, OrganicResultLinks);
        return this;
    }

    public bool HasResults() => _driver.FindElements(OrganicResultLinks).Any(e => e.Displayed);

    /// <summary>
    /// Clica no primeiro link orgânico e aguarda sair do DuckDuckGo. Retorna a URL de destino.
    /// </summary>
    public string OpenFirstOrganicResult()
    {
        var firstLink = Waits.Visible(_driver, OrganicResultLinks);
        var href = firstLink.GetAttribute("href") ?? string.Empty;

        // Garante abertura na mesma aba, preservando o histórico para o Back().
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].removeAttribute('target');", firstLink);
        firstLink.Click();

        Waits.UrlMatches(_driver, url => !url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase));
        Waits.For(_driver).Until(d =>
            ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.ToString() == "complete");

        return href;
    }
}
