using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce.Api.Models;

// rename brands now and commit the migration. see if that fixes the erorr.
// if it does, do the same thing for categories
[Table("Brands")]
public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public List<Product> Products { get; set; } = [];
}
