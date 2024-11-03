using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.ViewModels;


namespace WebApp.Controllers
{
    public class HomeController(ProductService productService) : Controller
    {
        private readonly ProductService _productService = productService;

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var categories = await _productService.GetAllCategoriesAsync();

            var headerCategoriesViewModel = new HeaderCatgeoriesViewModel
            {
                Categories = categories
            };

            // Fetch products under the "New Arrivals" category
            var newArrivalsCategoryName = "New Arrivals"; 
            var newArrivals = await _productService.GetAllProductsAsync(newArrivalsCategoryName);
            var viewModel = new HomeIndexViewModel
            {
                Categories = categories,
                NewArrivals = newArrivals
            };
            return View(viewModel);
        }        
    }
}
