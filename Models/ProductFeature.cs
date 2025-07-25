namespace Commerce.Api.Models;

public class ProductFeature
{
    public int Id { get; set; }
    public string Value { get; set; } = default!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}
