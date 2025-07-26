using Commerce.Api.Contracts;

namespace Commerce.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string CareInstructions { get; set; } = default!;

    public int BrandId { get; set; }
    public Brand Brand { get; set; } = default!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public List<ProductImage> Gallery { get; set; } = [];
    public List<ProductFeature> Features { get; set; } = [];
    public List<ProductSize> Sizes { get; set; } = [];

    public static Product FromCreateDto(ProductCreateDto dto)
    {
        var features = dto.Features.Select(f => new ProductFeature { Value = f }).ToList();
        var gallery = dto.Gallery.Select(url => new ProductImage { Url = url }).ToList();
        var sizes = dto.Sizes.Select(s => new ProductSize { Size = s.Size, Available = s.Available }).ToList();

        return new Product
        {
            Name = dto.Name,
            Price = (int)Math.Round(dto.Price), // temp round
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            CareInstructions = dto.CareInstructions,
            BrandId = dto.BrandId,
            CategoryId = dto.CategoryId,

            Features = features,
            Gallery = gallery,
            Sizes = sizes
        };
    }
}
