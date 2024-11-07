using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController(ProductService productService) : Controller
    {
        private readonly ProductService _productService = productService;

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            // Fetch categories
            var categories = await _productService.GetAllCategoriesAsync();

            // Pass the list of categories to ViewData
            ViewData["Categories"] = categories;

            // You can also fetch products if needed, like the "New Arrivals"
            // var newArrivalsCategoryName = "New Arrivals"; 
            // var newArrivals = await _productService.GetAllProductsAsync(newArrivalsCategoryName);

            var viewModel = new HomeIndexViewModel
            {
                // NewArrivals = newArrivals
            };

            return View(viewModel);
        }
    }
}

