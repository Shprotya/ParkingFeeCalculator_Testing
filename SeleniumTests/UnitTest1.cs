using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ParkingSeleniumTests
{
    [TestFixture]
    public class ParkingSeleniumTests
    {
        private IWebDriver _driver;
        private const string BaseUrl = "https://localhost:7251/"; // change to your actual port

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl(BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        private string SubmitForm(string vehicleType, string hours)
        {
            var vehicleSelect = new SelectElement(_driver.FindElement(By.Id("VehicleType")));
            vehicleSelect.SelectByValue(vehicleType);

            var hoursInput = _driver.FindElement(By.Id("Hours"));
            hoursInput.Clear();
            hoursInput.SendKeys(hours);

            _driver.FindElement(By.Id("submitBtn")).Click();

            return _driver.FindElement(By.Id("result")).Text;
        }

        [Test]
        public void BB1_Standard_1Hour_Returns4()
            => Assert.That(SubmitForm("standard", "1"), Does.Contain("4"));

        [Test]
        public void BB2_Standard_3Hours_Returns12()
            => Assert.That(SubmitForm("standard", "3"), Does.Contain("12"));

        [Test]
        public void BB3_Standard_4Hours_Returns12()
            => Assert.That(SubmitForm("standard", "4"), Does.Contain("12"));

        [Test]
        public void BB4_Standard_0Hours_Returns0()
            => Assert.That(SubmitForm("standard", "0"), Does.Contain("0"));

        [Test]
        public void BB5_Electric_1Hour_Returns3()
            => Assert.That(SubmitForm("electric", "1"), Does.Contain("3"));

        [Test]
        public void BB6_Electric_5Hours_Returns15()
            => Assert.That(SubmitForm("electric", "5"), Does.Contain("15"));

        [Test]
        public void BB7_Electric_6Hours_Returns12()
            => Assert.That(SubmitForm("electric", "6"), Does.Contain("12"));

        [Test]
        public void BB8_Electric_0Hours_Returns0()
            => Assert.That(SubmitForm("electric", "0"), Does.Contain("0"));

        [Test]
        public void BB9_InvalidVehicle_Returns0()
            => Assert.That(SubmitForm("xyz", "5"), Does.Contain("0"));

        [Test]
        public void BB11_Standard_10Hours_WithDiscount_Returns27()
            => Assert.That(SubmitForm("standard", "10"), Does.Contain("27"));

        [Test]
        public void BB12_Standard_9Hours_NoDiscount_Returns27()
            => Assert.That(SubmitForm("standard", "9"), Does.Contain("27"));
    }
}