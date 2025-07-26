using Microsoft.AspNetCore.Mvc;
using Commerce.Api.Data;
using Commerce.Api.Models;
using Commerce.Api.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Gallery)
            .Include(p => p.Sizes)
            .Include(p => p.Features)
            .Select(p => new ProductReadDto(p))
            .ToList();
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto productDto)
    {
        var brand = await _db.Brands.FindAsync(productDto.BrandId);
        if (brand == null) return BadRequest($"Brand with ID {productDto.BrandId} does not exist.");

        var category = await _db.Categories.FindAsync(productDto.CategoryId);
        if (category == null) return BadRequest($"Category with ID {productDto.CategoryId} does not exist.");

        var product = Product.FromCreateDto(productDto);

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        var result = new { id = product.Id };
        return CreatedAtAction(nameof(GetById), result, result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Gallery)
            .Include(p => p.Sizes)
            .Include(p => p.Features)
            .Select(p => new ProductReadDto(p))
            .FirstOrDefault();
        return product == null ? NotFound() : Ok(product);
    }
}
