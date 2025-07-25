namespace Commerce.Api.Contracts;

public class ProductCreateDto
{
    public string Name { get; set; } = default!;
    public double Price { get; set; } // todo make price an int in the db model
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string CareInstructions { get; set; } = default!;

    public int BrandId { get; set; }
    public int CategoryId { get; set; }

    public List<string> Gallery { get; set; } = [];
    public List<ProductSizeDto> Sizes { get; set; } = [];
    public List<string> Features { get; set; } = [];
}

public class ProductSizeDto
{
    public int Size { get; set; }
    public bool Available { get; set; }
}
