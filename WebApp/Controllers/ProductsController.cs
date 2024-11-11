using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductsController(ProductService productService) : Controller
    {
        private readonly ProductService _productService = productService;

        public async Task<IActionResult> Products(string categoryName)
        {
            var categories = await _productService.GetAllCategoriesAsync();

            // Pass the list of categories to ViewData
            ViewData["Categories"] = categories;

            // Fetch products 
            var products = await _productService.GetAllProductsAsync(categoryName);

            // Create ProductsIndexViewModel
            var viewmodel = new ProductsIndexViewModel
            {
                CategoryName = categoryName, 
                Result = products
            };

            // Pass BaseIndexViewModel to the view
            return View(viewmodel);
        }

        public async Task<IActionResult> ProductDetails(string articleNumber)
        {
            var product = await _productService.GetOneProductAsync(articleNumber);
            if (product != null)
            {
                return Json(product);
            }
            return NotFound();
        }

        public async Task<IActionResult> Favorites()
        {
            // Fetch categories for the menu
            var categories = await _productService.GetAllCategoriesAsync();

            // Pass the list of categories to ViewData
            ViewData["Categories"] = categories;

            return View();
        }
    }
}

