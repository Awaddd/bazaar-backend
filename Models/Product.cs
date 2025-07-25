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
}
