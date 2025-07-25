namespace Commerce.Api.Models;

class ProductImages
{
    public int Id { get; set; }
    public string Url { get; set; } = default!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}
