using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SignUpViewModel
{
	[DataType(DataType.Text)]
	[Display(Name = "First Name", Prompt = "Enter your first name", Order = 0)]
	[Required (ErrorMessage = "A valid first name is required")]
	[MinLength(2, ErrorMessage = "Enter a valid first name")]
	public string FirstName { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "Last Name", Prompt = "Enter your last name", Order = 0)]
    [Required(ErrorMessage = "A valid Username is required")]
    [MinLength(2, ErrorMessage = "Enter a valid last name")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter your Email address", Order = 1)]
	[DataType(DataType.EmailAddress)]
	[Required(ErrorMessage = "A valid Email is required")]
	[RegularExpression(@"^[^\s@]+@[^\s@]+.[^\s@]{2,}$", ErrorMessage = "Please enter a valid email adress")]
	public string Email { get; set; } = null!;

	[Display(Name = "Password", Prompt = "Enter your password", Order = 2)]
	[DataType(DataType.Password)]
	[Required(ErrorMessage = "A valid Password is required")]
	[RegularExpression(@"^(?=.\d)(?=.[a-z])(?=.[A-Z])(?=.[^a-zA-Z0-9])(?!.*\s).{8,}$", ErrorMessage = "Please enter a valid password.")]
	public string Password { get; set; } = null!;

	[Display(Name = "Confirm password", Prompt = "Confirm your password", Order = 3)]
	[DataType(DataType.Password)]
	[Required(ErrorMessage = "The password must be confirmed")]
    [Compare(nameof(Password), ErrorMessage = "Password does not match")]
    public string ConfirmPassword { get; set; } = null!;

	[Display(Name = "I agree to the Terms & Conditions", Order = 4)]
    [RequiredCheckbox(ErrorMessage = "Terms and conditions must be accepted")]
    public bool TermsAndConditions { get; set; }
}
