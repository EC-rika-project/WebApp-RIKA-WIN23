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

        // gets all products from a specific category by Id
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(int categoryId)
        {
            return await _productRepository.GetProductsAsync(categoryId);
        }

        // gets one product
        public async Task<ProductDto> GetOneProductAsync(int productId)
        {
            return await _productRepository.GetProductAsync(productId);
        }

        // get one catgory?? can be added, but currently not needed
    }
}
