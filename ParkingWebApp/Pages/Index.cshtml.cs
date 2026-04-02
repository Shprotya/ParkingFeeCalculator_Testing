using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingFeeCalculator;
using static ParkingFeeCalculator.ParkingService;

namespace ParkingWebApp.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        //public void OnGet()
        //{

        //}

        [BindProperty]
        public string VehicleType { get; set; }

        [BindProperty]
        public int Hours { get; set; }

        public string Result { get; set; }

        //runs when the page is accessed via GET request
        public void OnGet() { }

        //runs when the form is submitted via POST request
        //automatically connected to the form due to the method name and attributes
        public void OnPost()
        {
            var service = new ParkingService(new DefaultDiscount());
            var fee = service.CalculateFee(Hours, VehicleType);
            Result = $"Fee: €{fee}";
        }

        //fake the discount service for demonstration purposes
        public class DefaultDiscount : IDiscountService
        {
            public double GetDiscount()
            {
                return 0.9; // 10% discount
            }
        }
    }
}
