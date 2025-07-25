namespace Commerce.Api.Models;

public class ProductImage
{
    public int Id { get; set; }
    public string Url { get; set; } = default!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}
