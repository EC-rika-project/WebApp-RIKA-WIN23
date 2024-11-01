using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Infrastructure.Dtos;

public class CartViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
     
        //var cartItems = new List<CartItemDto>
        //{
        //    new CartItemDto { ArticleNumber = "1", Name = "Produkt 1", Price = 100, Ingress = "ingress", Quantity = 2 },
        //    new CartItemDto { ArticleNumber = "2", Name = "Produkt 2", Price = 200, Ingress = "ingress", Quantity = 1 },
        //};

        var viewModel = new CartViewModel();
   
        return View(viewModel); 
    }
}
