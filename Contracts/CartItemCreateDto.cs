namespace Commerce.Api.Contracts;

public class CartItemCreateDto
{
    public int ProductId { get; set; }
    public int Size { get; set; }
    public int Quantity { get; set; }
}
