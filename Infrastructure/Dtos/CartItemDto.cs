using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos;

public class CartItem
{
    public string ArticleNumber { get; set; } = null!;
    public string? CategoryName { get; set; }
    public string? Color { get; set; }
    public string CoverImageUrl { get; set; } = null!;
    public string? Description { get; set; }
    public List<string> ImageUrls { get; set; } = [];
    public string Ingress { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public List<CartItemVariation>? Variations { get; set; }
}

public class CartItemVariation
{
    public string? Name { get; set; }
    public int Stock { get; set; }
}

public class CartItemDto
{
    public CartItem Product { get; set; } = null!;
    public string? Size { get; set; }
    public int Quantity { get; set; }
}
