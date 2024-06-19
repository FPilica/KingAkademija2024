using KingAkademija2024.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KingAkademija2024.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetProducts()
    {
        _logger.LogInformation("Fething all products");
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }

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

    [HttpGet("filter")]
    public async Task<IActionResult> FilterProducts([FromQuery] string? category, [FromQuery] double? minPrice,
        [FromQuery] double? maxPrice)
    {
        _logger.LogInformation("Filtering products - Category: {Category} - Price range: {MinPrice}-{MaxPrice}", category, minPrice ?? 0, maxPrice ?? double.PositiveInfinity );
        
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

        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            _logger.LogWarning("Invalid product name");
            return BadRequest("Invalid product name");
        }
        
        _logger.LogInformation("Searching products: {Name}", name);
        var products = await _productService.SearchProductsByNameAsync(name);
        return Ok(products);
    }
}