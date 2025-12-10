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
    public IActionResult GetAll(
        [FromQuery] int? exclude,
        [FromQuery] int? limit,
        [FromQuery] int page = 1,
        [FromQuery] string? brands = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int? size = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null)
    {
        var query = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Sizes)
            .AsQueryable();

        if (exclude.HasValue)
        {
            query = query.Where(p => p.Id != exclude.Value);
        }

        if (!string.IsNullOrWhiteSpace(brands))
        {
            var brandList = brands.Split(',').Select(b => b.Trim()).ToList();
            query = query.Where(p => brandList.Contains(p.Brand.Name));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (size.HasValue)
        {
            query = query.Where(p => p.Sizes.Any(s => s.Size == size.Value && s.Available));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search));
        }

        query = sort?.ToLower() switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_asc" => query.OrderBy(p => p.Name),
            _ => query.OrderByDescending(p => p.Id)
        };

        if (limit.HasValue)
        {
            var skip = (page - 1) * limit.Value;
            query = query.Skip(skip).Take(limit.Value);
        }

        var products = query.Select(p => new ProductReadDto(p)).ToList();
        return Ok(products);
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
            .Where(p => p.Id == id)
            .Select(p => new ProductReadDto(p))
            .FirstOrDefault();
        return product == null ? NotFound() : Ok(product);
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

    [HttpPost("batch")]
    public async Task<IActionResult> CreateMany(ProductCreateDto[] items)
    {
        foreach (var productDto in items)
        {
            var brand = await _db.Brands.FindAsync(productDto.BrandId);
            if (brand == null) return BadRequest($"Brand with ID {productDto.BrandId} does not exist.");

            var category = await _db.Categories.FindAsync(productDto.CategoryId);
            if (category == null) return BadRequest($"Category with ID {productDto.CategoryId} does not exist.");

            var product = Product.FromCreateDto(productDto);
            _db.Products.Add(product);
        }

        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }
}
