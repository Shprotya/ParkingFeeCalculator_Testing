# Parking Fee Calculator

A C# web application that calculates parking fees based on vehicle type and hours parked. Built with ASP.NET Core Razor Pages, backed by a testable service layer, and covered by both unit and Selenium end-to-end tests with a GitHub Actions CI pipeline.

---

## 💡 How It Works

Fees are calculated based on vehicle type and duration, with a 10% discount applied automatically for stays of 10 hours or more.

| Vehicle Type | Rate | Condition |
|---|---|---|
| Standard | €4.00/hr | 1–2 hours |
| Standard | €3.00/hr | 4+ hours |
| Electric | €3.00/hr | 1–5 hours |
| Electric | €2.00/hr | 6+ hours |
| Truck / Unknown | €0.00 | Not supported |

---

## 🏗️ Project Structure
```
ParkingFeeCalculator/        # Core business logic (ParkingService, IDiscountService)
ParkingWebApp/               # ASP.NET Core Razor Pages front-end
ParkingFeeCalculator.Tests/  # NUnit unit tests with Moq
SeleniumTests/               # Selenium browser (end-to-end) tests
.github/workflows/           # GitHub Actions CI pipeline
```

---

## 🧪 Testing

**Unit tests** (NUnit + Moq) cover the `ParkingService.CalculateFee` logic directly, mocking out the discount service to isolate behaviour.

**Selenium tests** drive a real browser against the running web app, submitting the form and asserting on displayed output for boundary cases across both vehicle types.

To run unit tests locally:
```bash
dotnet test ParkingFeeCalculator.Tests/ParkingFeeCalculator.Tests.csproj
```

To run Selenium tests, start the web app first, then:
```bash
dotnet test SeleniumTests/SeleniumTests.csproj
```

> ⚠️ Selenium tests require Chrome and ChromeDriver installed. Update `BaseUrl` in `SeleniumTests/UnitTest1.cs` to match your local port if needed.

---

## ⚙️ CI/CD

A GitHub Actions workflow runs on every push to `main` and on pull requests. It builds the solution and executes the unit test suite automatically inside a .NET 8 container.

---

## 🚀 Getting Started

1. Clone the repository
2. Open `ParkingFeeCalculator.sln` in Visual Studio 2022
3. Set `ParkingWebApp` as the startup project
4. Press **F5** to run

No external dependencies beyond the .NET 8 SDK.

---

> [!IMPORTANT]
> If you want to create, run, test **Github Actions** locally, use the instructions below as starting point:

1. Install docker for your system if you haven't already.
2. Install [GitHub Actions Extension](https://marketplace.visualstudio.com/items?itemName=GitHub.vscode-github-actions)
   - Gives syntax highlighting for `.yml` files
3. Install [Github Local Actions Extension](https://sanjulaganepola.github.io/github-local-actions-docs/)
   - Provides a UI in VS Code through which you can run actions
4. Through the extension above install `act` or `gh-act` (if installed from github) because it's dependent on this powerful CLI tool for running actions locally
5. Run in VS Code!
