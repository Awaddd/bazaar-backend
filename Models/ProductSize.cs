namespace Commerce.Api.Models;

public class ProductSize
{
    public int Id { get; set; }
    public int Size { get; set; }
    public bool Available { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}
