using ParkingFeeCalculator;
using Moq;
using NUnit.Framework;
using static ParkingFeeCalculator.ParkingService;

namespace ParkingFeeCalculator.Test
{
    [TestFixture]
    public class Tests
    {
        private Mock<IDiscountService> _mockDiscount;
        private ParkingService _service;

        [SetUp]
        public void Setup()
        {
            _mockDiscount = new Mock<IDiscountService>();
            _mockDiscount.Setup(d => d.GetDiscount()).Returns(0.9);
            _service = new ParkingService(_mockDiscount.Object);
        }


        [Test]
        public void TC1()
        {
            var result = _service.CalculateFee(3, "standard");

            Assert.That(result, Is.EqualTo(12.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        [Test]
        public void TC2()
        {
            var result = _service.CalculateFee(5, "standard");

            Assert.That(result, Is.EqualTo(15.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        [Test]
        public void TC3()
        {
            var result = _service.CalculateFee(0, "standard");

            Assert.That(result, Is.EqualTo(0.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        [Test]
        public void TC4()
        {
            var result = _service.CalculateFee(3, "electric");

            Assert.That(result, Is.EqualTo(9.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        [Test]
        public void TC5()
        {
            var result = _service.CalculateFee(7, "electric");

            Assert.That(result, Is.EqualTo(14.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        [Test]
        public void TC6()
        {
            var result = _service.CalculateFee(0, "electric");

            Assert.That(result, Is.EqualTo(0.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        [Test]
        public void TC7()
        {
            var result = _service.CalculateFee(5, "truck");

            Assert.That(result, Is.EqualTo(0.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        [Test]
        public void TC8()
        {
            var result = _service.CalculateFee(12, "standard");

            Assert.That(result, Is.EqualTo(32.4));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }



    }
}
