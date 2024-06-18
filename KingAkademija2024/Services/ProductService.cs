using KingAkademija2024.Interfaces;
using KingAkademija2024.Models;

namespace KingAkademija2024.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ProductApiResponse>("https://dummyjson.com/products");
        return response?.Products ?? new List<Product>();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"https://dummyjson.com/products/{id}");
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        var response = await _httpClient.GetFromJsonAsync<ProductApiResponse>($"https://dummyjson.com/products/category/{category}");
        return response?.Products ?? new List<Product>();
    }

    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(double minPrice, double maxPrice)
    {
        var products = await GetProductsAsync();
        return products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
    }

    public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string name)
    {
        var products = await GetProductsAsync();
        return products.Where(p => p.Title.Contains(name, StringComparison.OrdinalIgnoreCase));
    }
}