using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SignUpViewModel
{
    [Required (ErrorMessage = "A valid Username is required")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "A valid Password is required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "A valid Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "The password must be confirmed")]
    [Compare(nameof(Password), ErrorMessage = "Password must be confirmed")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Terms and conditions must be accepted")]
    public bool TermsAndConditions { get; set; }
}
