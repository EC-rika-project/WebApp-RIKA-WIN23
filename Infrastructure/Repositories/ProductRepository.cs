using Infrastructure.Dtos;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http.Json;

namespace Infrastructure.Repositories
{
    public class ProductRepository
    {
        private readonly HttpClient _httpClient;
        public ProductRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:ProductsApiUrl"] 
                ?? throw new InvalidOperationException("Products API URL is missing in the configuration."));
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                string request = $"api/categories";
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>(request);
                return result!;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int categoryId)
        {
            try
            {
                string request = $"api/products?categoryId={categoryId}";
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>(request);
                return result!;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!;
        }
        public async Task<ProductDto> GetProductAsync(int productId)
        {
            try
            {
                string requestUri = $"api/products/{productId}";
                var result = await _httpClient.GetFromJsonAsync<ProductDto>(requestUri);
                return result!;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!;
        }
    }
}
