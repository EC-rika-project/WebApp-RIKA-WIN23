using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.ViewModels;


namespace WebApp.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly OrderService _orderService;

    public OrderController(ILogger<OrderController> logger, OrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }
  
    [Route("/checkout")]
    public IActionResult Checkout()
    {
        CheckoutViewModel viewModel = new CheckoutViewModel();

        return View(viewModel);
    }

    [HttpPost]
    [Route("checkout/loadCartItems")]
    public IActionResult LoadCartItems([FromBody] List<CartItemDto> cartData)
    {
        return PartialView("~/Views/Order/Partials/CartItems.cshtml", cartData);
    }

    public async Task<IActionResult> CreateOrder(OrderDto order)
    {
        var result = await _orderService.CreateOrderAsync(order);

        if (result)
        {
            return RedirectToAction("OrderSuccess", "Order"); //eller vart vi nu vill
        }

        return BadRequest();
    }

    public async Task<IActionResult> OrderSuccess()
    {    
        return View();
    }
}


    public class CartRequest
    {
        public List<CartItemDto> Cart { get; set; }
    }