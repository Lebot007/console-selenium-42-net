using System.Collections.ObjectModel;
using ConsoleSelenium.Tests.Infra;
using OpenQA.Selenium;

namespace ConsoleSelenium.Tests.Pages;

/// <summary>
/// Page Object da página inicial do DuckDuckGo.
/// </summary>
public class DuckDuckGoHomePage
{
    public const string BaseUrl = "https://duckduckgo.com";

    private static readonly By SearchInput = By.Name("q");
    private static readonly By Suggestions = By.CssSelector(
        ".search-suggestions-list li, .sbox-suggestions li, [role='option']");

    private readonly IWebDriver _driver;

    public DuckDuckGoHomePage(IWebDriver driver) => _driver = driver;

    public DuckDuckGoHomePage Open()
    {
        _driver.Navigate().GoToUrl(BaseUrl);
        Waits.Visible(_driver, SearchInput);
        return this;
    }

    public DuckDuckGoHomePage Type(string term)
    {
        var input = Waits.Visible(_driver, SearchInput);
        input.Clear();
        input.SendKeys(term);
        return this;
    }

    public ResultsPage Search(string term)
    {
        Type(term);
        _driver.FindElement(SearchInput).SendKeys(Keys.Enter);
        return new ResultsPage(_driver).WaitUntilLoaded();
    }

    /// <summary>
    /// Aguarda (espera explícita) até existirem ao menos <paramref name="minimum"/> sugestões visíveis.
    /// </summary>
    public ReadOnlyCollection<IWebElement> WaitForSuggestions(int minimum) =>
        Waits.For(_driver).Until(d =>
        {
            var visible = d.FindElements(Suggestions).Where(e => e.Displayed).ToList();
            return visible.Count >= minimum ? visible.AsReadOnly() : null;
        })!;

    /// <summary>
    /// Clica na sugestão de posição <paramref name="position"/> (1 = primeira) e devolve o texto dela.
    /// </summary>
    public (string Text, ResultsPage Results) ClickSuggestion(int position)
    {
        var suggestion = WaitForSuggestions(position)[position - 1];
        var text = suggestion.Text.Trim();
        suggestion.Click();
        return (text, new ResultsPage(_driver).WaitUntilLoaded());
    }
}
