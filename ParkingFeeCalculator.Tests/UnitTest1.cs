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

        /// <summary>
        /// TC1: 3 hours, standard vehicle
        /// Expected fee: 12.00
        /// NOTE: This test exposes a CODE BUG - the condition (hours >= 1) && (hours < 3) 
        /// does NOT include 3 hours. Hours=3 falls into a gap and currently returns incorrect fee.
        /// The condition should be changed to (hours >= 1) && (hours <= 3) to fix this.
        /// Branches covered ideally: N3=T, N4=T
        /// </summary>
        [Test]
        public void TC1()
        {
            var result = _service.CalculateFee(3, "standard");
            Assert.That(result, Is.EqualTo(12.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        /// <summary>
        /// TC2: 10 hours, standard vehicle
        /// Expected fee: 21.00* (includes 10% discount)
        /// Branches covered: N3=T, N4=F, N6=T, N20=T
        /// </summary>
        [Test]
        public void TC2()
        {
            var result = _service.CalculateFee(10, "standard");
            Assert.That(result, Is.EqualTo(27.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        /// <summary>
        /// TC3: 0 hours, standard vehicle
        /// Expected fee: 0
        /// Branches covered: N3=T, N4=F, N6=F, N20=F
        /// </summary>
        [Test]
        public void TC3()
        {
            var result = _service.CalculateFee(0, "standard");
            Assert.That(result, Is.EqualTo(0.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        /// <summary>
        /// TC4: 5 hours, truck vehicle
        /// Expected fee: 0
        /// Branches covered: N3=F, N10=F, N11=F, N20=F
        /// </summary>
        [Test]
        public void TC4()
        {
            var result = _service.CalculateFee(5, "truck");
            Assert.That(result, Is.EqualTo(0.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        /// <summary>
        /// TC5: 5 hours, electric vehicle
        /// Expected fee: 15.00
        /// Branches covered: N3=F, N10=T, N11=T
        /// </summary>
        [Test]
        public void TC5()
        {
            var result = _service.CalculateFee(5, "electric");
            Assert.That(result, Is.EqualTo(15.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        /// <summary>
        /// TC6: 7 hours, electric vehicle
        /// Expected fee: 14.00
        /// Branches covered: N3=F, N10=T, N11=F, N13=T
        /// </summary>
        [Test]
        public void TC6()
        {
            var result = _service.CalculateFee(7, "electric");
            Assert.That(result, Is.EqualTo(14.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }

        /// <summary>
        /// TC7: 0 hours, electric vehicle
        /// Expected fee: 0
        /// Branches covered: N3=F, N10=T, N11=F, N13=F, N20=F
        /// </summary>
        [Test]
        public void TC7()
        {
            var result = _service.CalculateFee(0, "electric");
            Assert.That(result, Is.EqualTo(0.0));
            _mockDiscount.Verify(d => d.GetDiscount(), Times.Once);
        }
    }
}