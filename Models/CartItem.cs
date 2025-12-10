namespace Commerce.Api.Models;

public class CartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Size { get; set; }
    public int Quantity { get; set; }
}
