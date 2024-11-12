using Infrastructure.Dtos;

namespace WebApp.ViewModels;

public class HomeIndexViewModel
{
    public PaginationResult<ProductsDto>? NewArrivals { get; set; } 
}