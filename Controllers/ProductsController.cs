using Microsoft.AspNetCore.Mvc;
using Commerce.Api.Data;
using Commerce.Api.Models;
using Commerce.Api.Contracts;

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
    public IActionResult GetAll() => Ok(_db.Products.ToList());

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto productDto)
    {
        var brand = await _db.Brands.FindAsync(productDto.Brand);
        if (brand == null) return BadRequest($"Brand with ID {productDto.Brand} does not exist.");

        var category = await _db.Categories.FindAsync(productDto.Category);
        if (category == null) return BadRequest($"Category with ID {productDto.Category} does not exist.");

        var features = productDto.Features.Select(f => new ProductFeature { Value = f }).ToList();
        var gallery = productDto.Gallery.Select(url => new ProductImage { Url = url }).ToList();
        var sizes = productDto.Sizes.Select(s => new ProductSize { Size = s.Size, Available = s.Available }).ToList();

        var product = new Product
        {
            Name = productDto.Name,
            Price = (int)Math.Round(productDto.Price), // temp round
            Description = productDto.Description,
            ImageUrl = productDto.ImageUrl,
            CareInstructions = productDto.CareInstructions,
            BrandId = productDto.Brand,
            CategoryId = productDto.Category,

            Features = features,
            Gallery = gallery,
            Sizes = sizes
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _db.Products.Find(id);
        return product == null ? NotFound() : Ok(product);
    }
}
