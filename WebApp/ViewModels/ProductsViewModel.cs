using Infrastructure.Dtos;

namespace Rika.WebApp.ViewModels
{
    public class ProductsViewModel
    {
        public string CategoryName { get; set; } = null!;
        public IEnumerable<ProductsDto>? Products { get; set; }
    }
}

