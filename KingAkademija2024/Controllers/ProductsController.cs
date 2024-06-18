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

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterProducts([FromQuery] string? category, [FromQuery] double? minPrice,
        [FromQuery] double? maxPrice)
    {
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
            return NotFound("No products match the given filter");
        }

        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest("Invalid product name");
        var products = await _productService.SearchProductsByNameAsync(name);
        return Ok(products);
    }
}