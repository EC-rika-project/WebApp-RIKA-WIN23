using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos;

public class CartItemDto
{
    public string ArticleNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Ingress { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
