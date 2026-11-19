using ConsoleSelenium.Tests.Infra;
using ConsoleSelenium.Tests.Pages;
using OpenQA.Selenium;

namespace ConsoleSelenium.Tests;

[TestClass]
public class BuscaDesafioTest
{
    private IWebDriver _driver = null!;
    private DuckDuckGoHomePage _home = null!;

    [TestInitialize]
    public void Setup()
    {
        _driver = WebDriverFactory.CreateChrome();
        _home = new DuckDuckGoHomePage(_driver);
    }

    [TestCleanup]
    public void TearDown()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    // Exercício 1: pesquisa básica e validação de título/URL
    [TestMethod]
    public void PesquisaBasica_DeveExibirTermoNoTituloOuUrl()
    {
        const string termo = "Selenium WebDriver C#";

        var resultados = _home.Open().Search(termo);

        var urlDecodificada = Uri.UnescapeDataString(resultados.Url).Replace('+', ' ');
        Assert.IsTrue(
            resultados.Title.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
            urlDecodificada.Contains(termo, StringComparison.OrdinalIgnoreCase),
            $"Termo não encontrado. Título: '{resultados.Title}' | URL: '{resultados.Url}'");
    }
}
