using Infrastructure.Dtos;

namespace WebApp.ViewModels;

public class CartViewModel
{
    public string PageTitle { get; set; } = "Cart";

    public List<CartItemDto> cartList = new List<CartItemDto>
    {
        new CartItemDto { ArticleNumber = "1", Name = "Produkt 1", Price = 100, Ingress = "ingress", Quantity = 2 },
        new CartItemDto { ArticleNumber = "2", Name = "Produkt 2", Price = 200, Ingress = "ingress", Quantity = 1 },
    };
}
