using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsoleSelenium.Tests.Infra;

/// <summary>
/// Cria instâncias configuradas do ChromeDriver.
/// O driver é baixado automaticamente pelo Selenium Manager.
/// Defina a variável de ambiente HEADLESS=false para ver o navegador.
/// </summary>
public static class WebDriverFactory
{
    public static IWebDriver CreateChrome()
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1366,768");
        options.AddArgument("--lang=pt-BR");
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddExcludedArgument("enable-automation");

        if (!string.Equals(Environment.GetEnvironmentVariable("HEADLESS"), "false", StringComparison.OrdinalIgnoreCase))
        {
            options.AddArgument("--headless=new");
        }

        var driver = new ChromeDriver(options);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        return driver;
    }
}
