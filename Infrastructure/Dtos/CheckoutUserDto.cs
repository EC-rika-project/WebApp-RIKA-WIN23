using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class CheckoutUserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    [RegularExpression(@"^[^\s@]+@[^\s@]+.[^\s@]+$", ErrorMessage = "Enter a valid email adress")]
    public string Email { get; set; } = null!;

    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? PhoneNumber { get; set; }

}
