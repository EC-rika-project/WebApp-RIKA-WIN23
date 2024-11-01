using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class TestFormModel
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    [Display(Name = "Name", Prompt = "firstname lastname")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email", Prompt = "exemple@gmail.com")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least 8 characters, one uppercase, one lowercase, one number and one special character")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Street name is required")]
    [MinLength(2, ErrorMessage = "Street name must be at least 2 characters")]
    [Display(Name = "Street Name", Prompt = "Enter street name")]
    public string StreetName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postal code is required")]
    [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Please enter a valid postal code (5 digits)")]
    [Display(Name = "Postal Code", Prompt = "12345")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    [MinLength(2, ErrorMessage = "City must be at least 2 characters")]
    [Display(Name = "City", Prompt = "Enter city")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid phone number (10 digits)")]
    [Display(Name = "Phone Number", Prompt = "Enter phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Card number is required")]
    [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Please enter a valid 16-digit card number")]
    [Display(Name = "Card Number", Prompt = "Enter card number")]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Expiry date is required")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Please enter date as MM/YY")]
    [Display(Name = "Expiry Date", Prompt = "MM/YY")]
    public string ExpiryDate { get; set; } = string.Empty;

    [Required(ErrorMessage = "CVV is required")]
    [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Please enter a valid 3-digit CVV")]
    [Display(Name = "CVV", Prompt = "123")]
    public string CVV { get; set; } = string.Empty;

    [Required(ErrorMessage = "You must accept the terms")]
    [Display(Name = "Accept Terms")]
    public bool AcceptTerms { get; set; }
}
