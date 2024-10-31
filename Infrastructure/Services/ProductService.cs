using Infrastructure.Dtos;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class ProductService(ProductRepository productRepository)
    {
        private readonly ProductRepository _productRepository = productRepository;

        // gets all categories
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _productRepository.GetCategoriesAsync();
        }

        // gets one category
        public async Task<CategoryDto> GetOneCategoryAsync(string categoryName)
        {
            return await _productRepository.GetCategoryAsync(categoryName);
        }

        // gets all products from a specific category by Id
        public async Task<IEnumerable<ProductsDto>> GetAllProductsAsync(string categoryName)
        {
            return await _productRepository.GetProductsAsync(categoryName);
        }

        // gets one product
        public async Task<ProductDetailsDto> GetOneProductAsync(string articleNumber)
        {
            return await _productRepository.GetProductAsync(articleNumber);
        }
    }
}
