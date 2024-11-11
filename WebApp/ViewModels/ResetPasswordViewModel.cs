using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,}$", ErrorMessage = "Password invalid.")]
    public string NewPassword { get; set; } = null! ;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    public string Token { get; set; } = null!;
}
