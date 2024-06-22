using KingAkademija2024.Interfaces;
using KingAkademija2024.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace KingAkademija2024.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMemoryCache _cache;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger, IMemoryCache cache)
    {
        _productService = productService;
        _logger = logger;
        _cache = cache;
    }
    
    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A list of products.</returns>
    [HttpGet("")]
    public async Task<IActionResult> GetProducts()
    {
        _logger.LogInformation("Fething all products");
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }
    
    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The products with the specified ID.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        _logger.LogInformation("Fatching product: {Id}", id);
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Product: {Id} not found", id);
            return NotFound();
        }
        return Ok(product);
    }
    
    /// <summary>
    /// Filters products by category and/or price.
    /// </summary>
    /// <param name="category">The category of the products.</param>
    /// <param name="minPrice">The minimum price of the products.</param>
    /// <param name="maxPrice">The maximum price of the products.</param>
    /// <returns>A list of products that match the given filter.</returns>
    [HttpGet("filter")]
    public async Task<IActionResult> FilterProducts([FromQuery] string? category, [FromQuery] double? minPrice,
        [FromQuery] double? maxPrice)
    {
        var cacheKey = $"Products_Filter_{category}_{minPrice ?? 0}_{maxPrice ?? double.PositiveInfinity}";
        
        if (_cache.TryGetValue(cacheKey, out IEnumerable<Product>? cachedProducts))
        {
            _logger.LogInformation("Returning cached filtered products - Category: {Category} - Price range: [{MinPrice}, {MaxPrice}]", category, minPrice ?? 0, maxPrice ?? double.PositiveInfinity);
            return Ok(cachedProducts);
        }
        
        _logger.LogInformation("Filtering products - Category: {Category} - Price range: [{MinPrice}, {MaxPrice}]", category, minPrice ?? 0, maxPrice ?? double.PositiveInfinity );
        
        var products = await _productService.GetProductsAsync();

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(product => product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (minPrice.HasValue)
        {
            products = products.Where(product => product.Price >= minPrice).ToList();
        }

        if (maxPrice.HasValue)
        {
            products = products.Where(product => product.Price <= maxPrice).ToList();
        }

        if (!products.Any())
        {
            _logger.LogWarning("No products match the given filter");
            return NotFound("No products match the given filter");
        }

        _cache.Set(cacheKey, products, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

        return Ok(products);
    }
    
    /// <summary>
    /// Searches products by name.
    /// </summary>
    /// <param name="name">The name or part of the name of the products.</param>
    /// <returns>A list of products that match the filter criteria.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            _logger.LogWarning("Invalid product name");
            return BadRequest("Invalid product name");
        }

        var cacheKey = $"Products_Search_{name.ToLower()}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Product> cachedProducts))
        {
            _logger.LogInformation("Returning cached products named: {Name}", name);
            return Ok(cachedProducts);
        }
        
        _logger.LogInformation("Searching products: {Name}", name);
        var products = await _productService.SearchProductsByNameAsync(name);

        if (!products.Any())
        {
            _logger.LogWarning("No products match the given name");
            return NotFound("No products match the given name");
        }

        _cache.Set(cacheKey, products, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
        
        return Ok(products);
    }
}