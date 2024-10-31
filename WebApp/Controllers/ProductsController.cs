using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Rika.WebApp.ViewModels;
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

            var viewModel = new ProductsViewModel
            {
                CategoryName = category.Name,
                Products = products
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
