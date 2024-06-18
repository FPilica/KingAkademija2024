using KingAkademija2024.Models;

namespace KingAkademija2024.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(double minPrice, double maxPrice);
    Task<IEnumerable<Product>> SearchProductsByNameAsync(string name);
}