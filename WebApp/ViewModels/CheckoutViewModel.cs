using Infrastructure.Dtos;
using WebApp.Models;

namespace WebApp.ViewModels;

public class CheckoutViewModel
{
    public List<CartItemDto> ProductList { get; set; } = [];
    public CheckoutUserDto CheckoutUser { get; set; } = new CheckoutUserDto();
    public RadioOptionsModel SelectedOption { get; set; }

    public Dictionary<RadioOptionsModel, string> OptionLabels => new Dictionary<RadioOptionsModel, string>
    {
        { RadioOptionsModel.CreditCard, "Credit Card" },
        { RadioOptionsModel.Swish, "Swish" },
        { RadioOptionsModel.Paypal, "PayPal" },
        { RadioOptionsModel.Invoice, "Invoice" }
    };
}
