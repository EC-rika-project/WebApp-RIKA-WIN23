using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    public List<CartItemDto> cartItems = new List<CartItemDto>
    {
        new CartItemDto { ArticleNumber = "1", Name = "Produkt 1", Price = 100, Ingress = "ingress", Quantity = 2 },
        new CartItemDto { ArticleNumber = "2", Name = "Produkt 2", Price = 200, Ingress = "ingress", Quantity = 1 },
    };

    [Route("/orders")]
    public IActionResult Index()
    {

        return View();
    }

    public async Task<IActionResult> Cart()
    {
        CartViewModel viewModel = new();
        decimal totalAmount = cartItems.Sum(x => x.GetTotalPrice());
        ViewBag.TotalAmount = totalAmount;
        //return View(viewModel);
        return PartialView("Partials/Cart", viewModel);
    }
}
