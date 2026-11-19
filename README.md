# console-selenium-42-net

Lista de Exercícios — **Garantia da Qualidade de Software**
Gestão e Qualidade de Software — Prof. Daniel Henrique Matos de Paiva

| Aluno | RA |
|---|---|
| João Vitor Alves Rodrigues | 32513480 |

Automação de cenários reais no [DuckDuckGo](https://duckduckgo.com) com **Selenium WebDriver**, **MSTest** e **.NET 10**.

## Estrutura

```
ConsoleSelenium42/
├── ConsoleSelenium42.sln
├── ConsoleSelenium/                 # Console Application (.NET 10)
│   └── Program.cs
└── ConsoleSelenium.Tests/           # Projeto de testes (MSTest)
    ├── BuscaDesafioTest.cs          # Exercícios 1, 2 e 3 ([TestMethod])
    ├── Infra/
    │   ├── WebDriverFactory.cs      # Criação/configuração do ChromeDriver
    │   └── Waits.cs                 # Esperas explícitas (WebDriverWait)
    └── Pages/                       # Page Objects
        ├── DuckDuckGoHomePage.cs
        └── ResultsPage.cs
```

## Cenários

| # | Teste | O que valida |
|---|---|---|
| 1 | `PesquisaBasica_DeveExibirTermoNoTituloOuUrl` | Digita "Selenium WebDriver C#" no campo `name="q"`, envia com `Keys.Enter` e verifica o termo no título ou na URL. |
| 2 | `Autocompletar_CliqueNaTerceiraSugestao_DeveAbrirResultadosDaOpcao` | Digita "C# unit testing", aguarda as sugestões com `WebDriverWait`, clica na 3ª e valida os resultados da opção escolhida. |
| 3 | `Historico_AbrirResultadoEVoltar_DeveRestaurarPaginaDeResultados` | Busca "Automated Testing", abre o 1º resultado orgânico, valida a URL externa (`_driver.Url`), volta com `Navigate().Back()` e valida os resultados restaurados. |

## Como executar

Pré-requisitos: .NET 10 SDK e Google Chrome (o driver é baixado automaticamente pelo Selenium Manager).

```bash
cd ConsoleSelenium42
dotnet test
```

Para ver o navegador durante os testes:

```bash
HEADLESS=false dotnet test          # Linux/macOS
$env:HEADLESS="false"; dotnet test  # PowerShell
```

## Comandos usados na criação

```bash
dotnet new sln -n ConsoleSelenium42
dotnet new console -n ConsoleSelenium
dotnet new mstest -n ConsoleSelenium.Tests
dotnet sln add ConsoleSelenium/ConsoleSelenium.csproj
dotnet sln add ConsoleSelenium.Tests/ConsoleSelenium.Tests.csproj
dotnet add ConsoleSelenium.Tests/ConsoleSelenium.Tests.csproj reference ConsoleSelenium/ConsoleSelenium.csproj
dotnet add ConsoleSelenium.Tests/ConsoleSelenium.Tests.csproj package Selenium.WebDriver
dotnet add ConsoleSelenium.Tests/ConsoleSelenium.Tests.csproj package Selenium.Support
```
