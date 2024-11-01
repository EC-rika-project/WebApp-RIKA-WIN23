using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class TestFormModel2
{
    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
    [Display(Name = "First Name", Prompt = "Enter your first name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
    [Display(Name = "Last Name", Prompt = "Enter your last name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sign up email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Sign Up Email", Prompt = "exemple@domain.com")]
    public string SignUpEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least 8 characters, one uppercase, one lowercase, one number and one special character")]
    [Display(Name = "Account Password")]
    [DataType(DataType.Password)]
    public string AccountPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password verification is required")]
    [Compare("AccountPassword", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Verify Password")]
    [DataType(DataType.Password)]
    public string VerifyPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address line is required")]
    [MinLength(2, ErrorMessage = "Address must be at least 2 characters")]
    [Display(Name = "Address Line", Prompt = "Enter your street address")]
    public string AddressLine { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address postal code is required")]
    [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Please enter a valid postal code (5 digits)")]
    [Display(Name = "Address Postal Code", Prompt = "12345")]
    public string AddressPostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address city is required")]
    [MinLength(2, ErrorMessage = "City must be at least 2 characters")]
    [Display(Name = "Address City", Prompt = "Enter city name")]
    public string AddressCity { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact phone number is required")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number (10 digits)")]
    [Display(Name = "Contact Phone Number", Prompt = "Enter your phone number")]
    public string ContactPhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Payment card number is required")]
    [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Please enter a valid 16-digit card number")]
    [Display(Name = "Payment Card Number", Prompt = "Enter your card number")]
    public string PaymentCardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Card expiry date is required")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Please enter date as MM/YY")]
    [Display(Name = "Card Expiry Date", Prompt = "MM/YY")]
    public string CardExpiryDate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Security code is required")]
    [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Please enter a valid 3-digit security code")]
    [Display(Name = "Security Code", Prompt = "123")]
    public string SecurityCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "You must accept the terms and conditions")]
    [Display(Name = "Accept Terms and Conditions")]
    public bool AcceptTermsAndConditions { get; set; }
}
