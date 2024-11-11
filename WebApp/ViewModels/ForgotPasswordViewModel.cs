using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
}
