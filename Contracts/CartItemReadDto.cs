namespace Commerce.Api.Contracts;

public class CartItemReadDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public double Price { get; set; }
    public string ImageUrl { get; set; } = default!;
    public int Size { get; set; }
    public int Quantity { get; set; }
}
