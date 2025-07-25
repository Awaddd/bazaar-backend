using Commerce.Api.Models;

namespace Commerce.Api.Contracts;

public class ProductReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; } // todo make price an int in the db model
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string CareInstructions { get; set; } = default!;

    public string Brand { get; set; } = default!;
    public string Category { get; set; } = default!;

    public List<string> Gallery { get; set; } = [];
    public List<ProductSizeDto> Sizes { get; set; } = [];
    public List<string> Features { get; set; } = [];

    public ProductReadDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Price = (double)product.Price;
        Description = product.Description;
        ImageUrl = product.ImageUrl;
        CareInstructions = product.CareInstructions;
        Brand = product.Brand.Name;
        Category = product.Category.Name;
        Gallery = product.Gallery.Select(img => img.Url).ToList();
        Sizes = product.Sizes.Select(s => new ProductSizeDto { Size = s.Size, Available = s.Available }).ToList();
        Features = product.Features.Select(f => f.Value).ToList();
    }
}
