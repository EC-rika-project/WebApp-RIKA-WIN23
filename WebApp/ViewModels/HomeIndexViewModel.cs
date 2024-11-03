using Infrastructure.Dtos;

namespace WebApp.ViewModels;

public class HomeIndexViewModel
{
    public string PageTitle { get; set; } = "Home";
    public IEnumerable<CategoryDto>? Categories { get; set; }
    public PaginationResult<ProductsDto>? NewArrivals { get; set; } 
}