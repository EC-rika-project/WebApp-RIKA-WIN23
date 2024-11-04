using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class SignUpDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    [RegularExpression(@"^[^\s@]+@[^\s@]+.[^\s@]+$", ErrorMessage = "Enter a valid email adress")]
    public string Email { get; set; } = null!;

 
    [RegularExpression(@"^(?=.[a-z])(?=.[A-Z])(?=.\d)(?=.[@$!%?&])[A-Za-z\d@$!%?&]{8,}$", ErrorMessage = "Password invalid.")]
    public string Password { get; set; } = null!;

    public string? SecurityKey { get; set; }

}
