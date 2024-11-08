using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos;

public class OrderDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public List<CartItemDto> CartItems { get; set; } = new();
    public decimal TotalPrice { get; set; }
}
