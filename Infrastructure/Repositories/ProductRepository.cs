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
                string request = $"/categories";
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>(request);
                return result!;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!;
        }

        public async Task<PaginationResult<ProductsDto>> GetProductsAsync(string categoryId)
        {
            try
            {
                string request = $"/products?category={categoryId}";
                var result = await _httpClient.GetFromJsonAsync<PaginationResult<ProductsDto>>(request);
                return result!;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!;
        }

        public async Task<ProductResponseDto> GetProductAsync(string productId)
        {
            try
            {
                string requestUri = $"/products/{productId}"; 
                var result = await _httpClient.GetFromJsonAsync<ProductResponseDto>(requestUri);
                return result!;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!;
        }
    }
}
