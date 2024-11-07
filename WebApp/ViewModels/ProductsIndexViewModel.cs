using Infrastructure.Dtos;

namespace WebApp.ViewModels
{
    public class ProductsIndexViewModel
    {
        public string CategoryName { get; set; } = null!;
        public PaginationResult<ProductsDto>? Result { get; set; }
    }
}

