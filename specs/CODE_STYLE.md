# Bazaar Backend Code Style Guide

## Existing Patterns

### Controller Structure
```csharp
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
}
```

### Query Building Pattern
- Use `AsQueryable()` for incremental query building
- Apply `Include()` for eager loading
- Use conditional `if (param.HasValue)` blocks for optional filters
- Project to DTOs with `Select(p => new ProductReadDto(p))`
- Execute with `.ToList()` at the end

### Query Parameters
- Use `[FromQuery]` attribute explicitly
- Nullable types for optional parameters (`int?`, `string?`)
- Default values for required parameters (`int page = 1`)
- camelCase naming

### Response Patterns
- `Ok()` for successful GETs
- `NotFound()` for missing resources
- `BadRequest()` with messages for validation
- `CreatedAtAction()` for POST

---

## Adding Filtering to GetAll

### Parameter Signature
```csharp
[HttpGet]
public IActionResult GetAll(
    [FromQuery] int? exclude,
    [FromQuery] int? limit,
    [FromQuery] int page = 1,
    // New filter params:
    [FromQuery] string? brands = null,      // comma-separated: "Nike,Jordan"
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null,
    [FromQuery] string? sizes = null,
    [FromQuery] string? search = null,
    [FromQuery] string? sort = null)        // price_asc, price_desc, name_asc, featured
```

### Filter Implementation
```csharp
var query = _db.Products
    .Include(p => p.Brand)
    .Include(p => p.Category)
    .Include(p => p.Sizes)
    .AsQueryable();

// Existing exclude filter
if (exclude.HasValue)
{
    query = query.Where(p => p.Id != exclude.Value);
}

// Brand filter (comma-separated)
if (!string.IsNullOrWhiteSpace(brands))
{
    var brandList = brands.Split(',').Select(b => b.Trim()).ToList();
    query = query.Where(p => brandList.Contains(p.Brand.Name));
}

// Price range
if (minPrice.HasValue)
{
    query = query.Where(p => p.Price >= minPrice.Value);
}

if (maxPrice.HasValue)
{
    query = query.Where(p => p.Price <= maxPrice.Value);
}

// Size filter (products that have any of these sizes available)
if (!string.IsNullOrWhiteSpace(sizes))
{
    var sizeList = sizes.Split(',').Select(s => int.Parse(s.Trim())).ToList();
    query = query.Where(p => p.Sizes.Any(s => sizeList.Contains(s.Size) && s.Available));
}

// Search in name
if (!string.IsNullOrWhiteSpace(search))
{
    query = query.Where(p => p.Name.Contains(search));
}

// Sorting with switch expression
query = sort?.ToLower() switch
{
    "price_asc" => query.OrderBy(p => p.Price),
    "price_desc" => query.OrderByDescending(p => p.Price),
    "name_asc" => query.OrderBy(p => p.Name),
    _ => query.OrderByDescending(p => p.Id)  // "featured" = default
};

// Pagination (existing)
if (limit.HasValue)
{
    var skip = (page - 1) * limit.Value;
    query = query.Skip(skip).Take(limit.Value);
}

var products = query.Select(p => new ProductReadDto(p)).ToList();
return Ok(products);
```

### Example API Calls
```
GET /api/products                              # All products
GET /api/products?brands=Nike                  # Nike only
GET /api/products?brands=Nike,Jordan           # Nike or Jordan
GET /api/products?minPrice=100&maxPrice=200    # Price range
GET /api/products?sizes=9                      # Has size 9 available
GET /api/products?sizes=8,9,10                 # Has size 8, 9, or 10 available
GET /api/products?search=Air                   # Name contains "Air"
GET /api/products?sort=price_asc               # Cheapest first
GET /api/products?brands=Nike&sort=price_desc  # Combined filters
```

