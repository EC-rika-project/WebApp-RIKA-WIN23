using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Infrastructure.Dtos;

namespace WebApp.Controllers
{
    public class ProductsController(ProductService productService) : Controller
    {
        private readonly ProductService _productService = productService;

        public async Task<IActionResult> Index(string categoryName)
        {
            var products = await _productService.GetAllProductsAsync(categoryName);
            var category = await _productService.GetOneCategoryAsync(categoryName);
            var categories = await _productService.GetAllCategoriesAsync();

            var headerCategoriesViewModel = new HeaderCatgeoriesViewModel
            {
                Categories = categories
            };

            var viewModel = new ProductsIndexViewModel
            {
                CategoryName = category.Name,
                Result = products,
                Categories = categories
            };

            return View(viewModel);
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
    }
}
