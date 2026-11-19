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

    // Exercício 2: autocompletar (sugestões dinâmicas) com espera explícita
    [TestMethod]
    public void Autocompletar_CliqueNaTerceiraSugestao_DeveAbrirResultadosDaOpcao()
    {
        const string termo = "C# unit testing";

        _home.Open().Type(termo);
        var (textoSugestao, resultados) = _home.ClickSuggestion(3);

        Assert.IsFalse(string.IsNullOrWhiteSpace(textoSugestao), "A terceira sugestão não possui texto.");
        Assert.IsTrue(ResultsPage.IsResultsUrl(resultados.Url), $"Não redirecionou para resultados: {resultados.Url}");

        var urlDecodificada = Uri.UnescapeDataString(resultados.Url).Replace('+', ' ');
        Assert.IsTrue(
            resultados.Title.Contains(textoSugestao, StringComparison.OrdinalIgnoreCase) ||
            urlDecodificada.Contains(textoSugestao, StringComparison.OrdinalIgnoreCase),
            $"Resultados não correspondem à sugestão '{textoSugestao}'. Título: '{resultados.Title}'");
    }

    // Exercício 3: navegação por histórico (Voltar)
    [TestMethod]
    public void Historico_AbrirResultadoEVoltar_DeveRestaurarPaginaDeResultados()
    {
        const string termo = "Automated Testing";

        var resultados = _home.Open().Search(termo);
        var urlResultados = resultados.Url;

        resultados.OpenFirstOrganicResult();

        Assert.IsTrue(Uri.IsWellFormedUriString(_driver.Url, UriKind.Absolute), $"URL externa inválida: {_driver.Url}");
        Assert.IsFalse(_driver.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase),
            $"A página externa não foi carregada. URL atual: {_driver.Url}");

        _driver.Navigate().Back();
        var restaurada = new ResultsPage(_driver).WaitUntilLoaded();

        Assert.IsTrue(ResultsPage.IsResultsUrl(restaurada.Url), $"Não voltou para os resultados: {restaurada.Url}");
        Assert.IsTrue(restaurada.HasResults(), "A lista de resultados não foi restaurada.");
        StringAssert.Contains(
            Uri.UnescapeDataString(restaurada.Url).Replace('+', ' ').ToLowerInvariant(),
            termo.ToLowerInvariant(),
            $"A busca restaurada difere da original ({urlResultados}).");
    }
}
