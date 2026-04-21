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

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(d => !string.IsNullOrEmpty(d.FindElement(By.Id("result")).Text));

            return _driver.FindElement(By.Id("result")).Text;
        }

        // BB1 - EP: mid-range of standard 1-3hr bracket (4/hr)
        [Test]
        public void BB1_Standard_2Hours_Returns8()
            => Assert.That(SubmitForm("standard", "2"), Does.Contain("8"));

        // BB2 - EP: mid-range of standard 4-9hr bracket (3/hr)
        [Test]
        public void BB2_Standard_6Hours_Returns18()
            => Assert.That(SubmitForm("standard", "6"), Does.Contain("18"));

        // BB3 - EP: standard 10+ hours with discount (12 x 3 x 0.9 = 32.40)
        [Test]
        public void BB3_Standard_12Hours_WithDiscount_Returns32_40()
            => Assert.That(SubmitForm("standard", "12"), Does.Contain("32.4"));

        // BB4 - EP: mid-range of electric 1-5hr bracket (3/hr)
        [Test]
        public void BB4_Electric_3Hours_Returns9()
            => Assert.That(SubmitForm("electric", "3"), Does.Contain("9"));

        // BB5 - EP: mid-range of electric 6-9hr bracket (2/hr)
        [Test]
        public void BB5_Electric_7Hours_Returns14()
            => Assert.That(SubmitForm("electric", "7"), Does.Contain("14"));

        // BB6 - EP: electric 10+ hours with discount (11 x 2 x 0.9 = 19.80)
        [Test]
        public void BB6_Electric_11Hours_WithDiscount_Returns19_80()
            => Assert.That(SubmitForm("electric", "11"), Does.Contain("19.8"));

        // BB7 - EP: invalid vehicle type ("truck") - NOT TESTABLE VIA WEB FORM
        // The dropdown only exposes "standard" and "electric"; "truck" cannot be submitted.
        // This case is covered by the NUnit unit tests (TC7).

        // BB8 - EP: invalid hours (0) for standard - returns 0
        [Test]
        public void BB8_Standard_0Hours_Returns0()
            => Assert.That(SubmitForm("standard", "0"), Does.Contain("0"));

        // BB9 - EP: invalid hours (negative) for standard - returns 0
        [Test]
        public void BB9_Standard_Negative1Hours_Returns0()
            => Assert.That(SubmitForm("standard", "-1"), Does.Contain("0"));

        // BB10 - BVA: lower boundary of valid hours for standard (1hr = 4)
        [Test]
        public void BB10_Standard_1Hour_Returns4()
            => Assert.That(SubmitForm("standard", "1"), Does.Contain("4"));

        // BB11 - BVA: upper boundary of 1-3hr bracket - exposes off-by-one defect (hours < 3 vs <= 3)
        [Test]
        public void BB11_Standard_3Hours_Returns12_OffByOneDefect()
            => Assert.That(SubmitForm("standard", "3"), Does.Contain("12"));

        // BB12 - BVA: lower boundary of 4+ hr bracket for standard (4 x 3 = 12)
        [Test]
        public void BB12_Standard_4Hours_Returns12()
            => Assert.That(SubmitForm("standard", "4"), Does.Contain("12"));

        // BB13 - BVA: just below discount boundary for standard - no discount (9 x 3 = 27)
        [Test]
        public void BB13_Standard_9Hours_NoDiscount_Returns27()
            => Assert.That(SubmitForm("standard", "9"), Does.Contain("27"));

        // BB14 - BVA: discount boundary for standard - discount activates (10 x 3 x 0.9 = 27)
        [Test]
        public void BB14_Standard_10Hours_WithDiscount_Returns27()
            => Assert.That(SubmitForm("standard", "10"), Does.Contain("27"));

        // BB15 - BVA: lower boundary of valid hours for electric (1hr = 3)
        [Test]
        public void BB15_Electric_1Hour_Returns3()
            => Assert.That(SubmitForm("electric", "1"), Does.Contain("3"));

        // BB16 - BVA: upper boundary of 1-5hr bracket for electric (5 x 3 = 15)
        [Test]
        public void BB16_Electric_5Hours_Returns15()
            => Assert.That(SubmitForm("electric", "5"), Does.Contain("15"));

        // BB17 - BVA: lower boundary of 6+ hr bracket for electric (6 x 2 = 12)
        [Test]
        public void BB17_Electric_6Hours_Returns12()
            => Assert.That(SubmitForm("electric", "6"), Does.Contain("12"));

        // BB18 - BVA: just below valid range for electric (0 hours = 0)
        [Test]
        public void BB18_Electric_0Hours_Returns0()
            => Assert.That(SubmitForm("electric", "0"), Does.Contain("0"));

        // BB19 - EP: case-insensitive vehicle type ("STANDARD") - NOT TESTABLE VIA WEB FORM
        // The dropdown only exposes lowercase "standard" and "electric"; uppercase cannot be selected.
        // This defect (strict string equality in code vs case-insensitive spec) is documented in Task 3.
    }
}
