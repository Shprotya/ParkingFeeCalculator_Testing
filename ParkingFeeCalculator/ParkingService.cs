using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingFeeCalculator
{
    public class ParkingService
    {
        public interface IDiscountService
        {
            double GetDiscount();
        }

        private readonly IDiscountService _discountService;

        public ParkingService(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public double CalculateFee(int hours, string vehicleType)
        {
            double fee;

            if (vehicleType == "standard")
            {
                if (hours >= 1 && hours < 3)
                    fee = hours * 4.0;
                else if (hours >= 4)
                    fee = hours * 3.0;
                else
                    fee = 0.0;
            }
            else if (vehicleType == "electric")
            {
                if (hours >= 1 && hours <= 5)
                    fee = hours * 3.0;
                else if (hours >= 6)
                    fee = hours * 2.0;
                else
                    fee = 0.0;
            }
            else
            {
                fee = 0.0;
            }

            double discount = _discountService.GetDiscount();

            if (hours >= 10)
                fee = fee * discount;

            return fee;
        }
    }
}

