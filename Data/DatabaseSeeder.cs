using Commerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Only seed if database is empty
        if (await context.Brands.AnyAsync())
            return;

        // Seed Brands
        var brands = new List<Brand>
        {
            new() { Id = 1, Name = "Nike" },
            new() { Id = 2, Name = "Jordan" },
            new() { Id = 3, Name = "Converse" },
            new() { Id = 4, Name = "Vans" },
            new() { Id = 5, Name = "Balenciaga" },
            new() { Id = 6, Name = "Fila" },
            new() { Id = 7, Name = "Asics" }
        };

        // Seed Categories
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Sneakers" }
        };

        // Seed Products (IDs 1-8)
        var products = new List<Product>
        {
            new()
            {
                Id = 1,
                BrandId = 1,
                CategoryId = 1,
                Name = "Nike Air Max 90 Orange",
                Price = 149.0m,
                Description = "The Nike Air Max 90 Orange reinvents the classic silhouette with striking color pops and the iconic Max Air cushioning that delivers timeless comfort and street-ready style.",
                ImageUrl = "/assets/products/nike-air-max-90-orange.png",
                CareInstructions = "Clean with a soft brush and warm water. Avoid machine washing. Let air dry away from direct sunlight."
            },
            new()
            {
                Id = 2,
                BrandId = 1,
                CategoryId = 1,
                Name = "Nike Air Max 90 Beige",
                Price = 149.0m,
                Description = "The Nike Air Max 90 Beige delivers refined streetwear style with a neutral tone palette, iconic Max Air cushioning, and premium materials built for everyday comfort.",
                ImageUrl = "/assets/products/nike-air-max-90-beige.png",
                CareInstructions = "Use a suede brush or soft cloth for cleaning. Avoid direct heat. Air dry naturally."
            },
            new()
            {
                Id = 3,
                BrandId = 7,
                CategoryId = 1,
                Name = "Asics GEL-1130",
                Price = 119.0m,
                Description = "The Asics GEL-1130 blends early 2000s running design with modern comfort, featuring GEL cushioning and a breathable build that's perfect for daily wear.",
                ImageUrl = "/assets/products/asics-gel-1130.png",
                CareInstructions = "Clean with a soft damp cloth. Avoid soaking. Air dry away from direct sunlight."
            },
            new()
            {
                Id = 4,
                BrandId = 1,
                CategoryId = 1,
                Name = "Nike P-6000",
                Price = 129.0m,
                Description = "The Nike P-6000 merges retro running aesthetics with breathable mesh and layered overlays, offering all-day comfort in a bold, Y2K-inspired silhouette.",
                ImageUrl = "/assets/products/nike-p-6000.png",
                CareInstructions = "Wipe with a damp cloth. Air dry. Avoid harsh cleaning products or machine washing."
            },
            new()
            {
                Id = 5,
                BrandId = 1,
                CategoryId = 1,
                Name = "Nike Air Force 1 Black",
                Price = 119.0m,
                Description = "The Nike Air Force 1 Black keeps it classic with a clean monochrome look and legendary Air cushioning. A timeless street icon built for everyday wear.",
                ImageUrl = "/assets/products/nike-air-force-1-black.png",
                CareInstructions = "Wipe with a damp cloth. Use leather cleaner as needed. Do not machine wash or dry."
            },
            new()
            {
                Id = 6,
                BrandId = 1,
                CategoryId = 1,
                Name = "Nike Air Force 1 White",
                Price = 119.0m,
                Description = "The Nike Air Force 1 White delivers iconic style with a crisp all-white finish, soft cushioning, and a durable build that stands the test of time.",
                ImageUrl = "/assets/products/nike-air-force-1-white.png",
                CareInstructions = "Clean with a soft brush and mild soap. Air dry. Avoid bleach or harsh chemicals."
            },
            new()
            {
                Id = 7,
                BrandId = 2,
                CategoryId = 1,
                Name = "Jordan 4 Retro Blue",
                Price = 189.0m,
                Description = "The Jordan 4 Retro Blue brings back a fan-favorite silhouette with bold blue accents, premium materials, and signature Flight details that celebrate heritage and performance.",
                ImageUrl = "/assets/products/jordan-4-retro-blue.png",
                CareInstructions = "Spot clean with a soft brush and gentle soap. Do not submerge in water. Air dry only."
            },
            new()
            {
                Id = 8,
                BrandId = 2,
                CategoryId = 1,
                Name = "Jordan 1 Green",
                Price = 159.0m,
                Description = "The Jordan 1 Green updates the original high-top silhouette with rich green accents and timeless design. Featuring premium materials and Air cushioning, it's a staple on and off the court.",
                ImageUrl = "/assets/products/jordan-1-green.png",
                CareInstructions = "Wipe clean with a soft cloth. Use leather conditioner as needed. Do not machine wash."
            }
        };

        // Seed ProductFeatures (4 per product, IDs 1-32)
        var features = new List<ProductFeature>
        {
            // Product 1: Nike Air Max 90 Orange
            new() { Id = 1, ProductId = 1, Value = "Durable leather and mesh upper for breathability" },
            new() { Id = 2, ProductId = 1, Value = "Visible Max Air unit for lightweight cushioning" },
            new() { Id = 3, ProductId = 1, Value = "Rubber outsole with waffle pattern for traction" },
            new() { Id = 4, ProductId = 1, Value = "Padded collar and flex grooves for natural movement" },
            // Product 2: Nike Air Max 90 Beige
            new() { Id = 5, ProductId = 2, Value = "Smooth suede and mesh upper in neutral tones" },
            new() { Id = 6, ProductId = 2, Value = "Signature Max Air unit for responsive cushioning" },
            new() { Id = 7, ProductId = 2, Value = "Waffle outsole for multi-surface traction" },
            new() { Id = 8, ProductId = 2, Value = "Low-top design with padded collar for classic comfort" },
            // Product 3: Asics GEL-1130
            new() { Id = 9, ProductId = 3, Value = "Mesh and synthetic upper for lightweight support" },
            new() { Id = 10, ProductId = 3, Value = "GEL technology for shock absorption" },
            new() { Id = 11, ProductId = 3, Value = "TRUSSTIC system for stability" },
            new() { Id = 12, ProductId = 3, Value = "Padded tongue and collar for all-day comfort" },
            // Product 4: Nike P-6000
            new() { Id = 13, ProductId = 4, Value = "Breathable mesh with horizontal and vertical overlays" },
            new() { Id = 14, ProductId = 4, Value = "Foam midsole with lightweight cushioning" },
            new() { Id = 15, ProductId = 4, Value = "Rubber outsole for durable traction" },
            new() { Id = 16, ProductId = 4, Value = "Padded collar and tongue for comfort and support" },
            // Product 5: Nike Air Force 1 Black
            new() { Id = 17, ProductId = 5, Value = "Full-grain leather upper for a premium feel" },
            new() { Id = 18, ProductId = 5, Value = "Encapsulated Air-Sole unit for lightweight cushioning" },
            new() { Id = 19, ProductId = 5, Value = "Non-marking rubber outsole with pivot point for traction" },
            new() { Id = 20, ProductId = 5, Value = "Low-cut silhouette for a streamlined, versatile look" },
            // Product 6: Nike Air Force 1 White
            new() { Id = 21, ProductId = 6, Value = "Classic leather upper for a clean, timeless look" },
            new() { Id = 22, ProductId = 6, Value = "Nike Air cushioning for all-day comfort" },
            new() { Id = 23, ProductId = 6, Value = "Perforated toe box for breathability" },
            new() { Id = 24, ProductId = 6, Value = "Rubber outsole for grip and durability" },
            // Product 7: Jordan 4 Retro Blue
            new() { Id = 25, ProductId = 7, Value = "Durable leather upper with mesh inserts for breathability" },
            new() { Id = 26, ProductId = 7, Value = "Visible Air-Sole unit in the heel for responsive cushioning" },
            new() { Id = 27, ProductId = 7, Value = "Molded TPU wings and lace locks for secure fit" },
            new() { Id = 28, ProductId = 7, Value = "Herringbone outsole pattern for traction and grip" },
            // Product 8: Jordan 1 Green
            new() { Id = 29, ProductId = 8, Value = "Premium leather upper with classic color blocking" },
            new() { Id = 30, ProductId = 8, Value = "Encapsulated Air-Sole unit for responsive cushioning" },
            new() { Id = 31, ProductId = 8, Value = "Solid rubber outsole for traction and durability" },
            new() { Id = 32, ProductId = 8, Value = "Padded collar for ankle support and comfort" }
        };

        // Seed ProductImages (3 per product, IDs 1-24)
        var images = new List<ProductImage>
        {
            // Product 1
            new() { Id = 1, ProductId = 1, Url = "/assets/products/nike-air-max-90-orange.png" },
            new() { Id = 2, ProductId = 1, Url = "/assets/products/nike-air-max-90-orange-2.png" },
            new() { Id = 3, ProductId = 1, Url = "/assets/products/nike-air-max-90-orange-3.png" },
            // Product 2
            new() { Id = 4, ProductId = 2, Url = "/assets/products/nike-air-max-90-beige.png" },
            new() { Id = 5, ProductId = 2, Url = "/assets/products/nike-air-max-90-beige-2.png" },
            new() { Id = 6, ProductId = 2, Url = "/assets/products/nike-air-max-90-beige-3.png" },
            // Product 3
            new() { Id = 7, ProductId = 3, Url = "/assets/products/asics-gel-1130.png" },
            new() { Id = 8, ProductId = 3, Url = "/assets/products/asics-gel-1130-2.png" },
            new() { Id = 9, ProductId = 3, Url = "/assets/products/asics-gel-1130-3.png" },
            // Product 4
            new() { Id = 10, ProductId = 4, Url = "/assets/products/nike-p-6000.png" },
            new() { Id = 11, ProductId = 4, Url = "/assets/products/nike-p-6000-2.png" },
            new() { Id = 12, ProductId = 4, Url = "/assets/products/nike-p-6000-3.png" },
            // Product 5
            new() { Id = 13, ProductId = 5, Url = "/assets/products/nike-air-force-1-black.png" },
            new() { Id = 14, ProductId = 5, Url = "/assets/products/nike-air-force-1-black-2.png" },
            new() { Id = 15, ProductId = 5, Url = "/assets/products/nike-air-force-1-black-3.png" },
            // Product 6
            new() { Id = 16, ProductId = 6, Url = "/assets/products/nike-air-force-1-white.png" },
            new() { Id = 17, ProductId = 6, Url = "/assets/products/nike-air-force-1-white-2.png" },
            new() { Id = 18, ProductId = 6, Url = "/assets/products/nike-air-force-1-white-3.png" },
            // Product 7
            new() { Id = 19, ProductId = 7, Url = "/assets/products/jordan-4-retro-blue.png" },
            new() { Id = 20, ProductId = 7, Url = "/assets/products/jordan-4-retro-blue-2.png" },
            new() { Id = 21, ProductId = 7, Url = "/assets/products/jordan-4-retro-blue-3.png" },
            // Product 8
            new() { Id = 22, ProductId = 8, Url = "/assets/products/jordan-1-green.png" },
            new() { Id = 23, ProductId = 8, Url = "/assets/products/jordan-1-green-2.png" },
            new() { Id = 24, ProductId = 8, Url = "/assets/products/jordan-1-green-3.png" }
        };

        // Seed ProductSizes (6 per product, IDs 1-48)
        var sizes = new List<ProductSize>
        {
            // Product 1: Nike Air Max 90 Orange - only small sizes left
            new() { Id = 1, ProductId = 1, Size = 6, Available = true },
            new() { Id = 2, ProductId = 1, Size = 7, Available = true },
            new() { Id = 3, ProductId = 1, Size = 8, Available = false },
            new() { Id = 4, ProductId = 1, Size = 9, Available = false },
            new() { Id = 5, ProductId = 1, Size = 10, Available = false },
            new() { Id = 6, ProductId = 1, Size = 11, Available = false },
            // Product 2: Nike Air Max 90 Beige - mid sizes available
            new() { Id = 7, ProductId = 2, Size = 6, Available = false },
            new() { Id = 8, ProductId = 2, Size = 7, Available = true },
            new() { Id = 9, ProductId = 2, Size = 8, Available = true },
            new() { Id = 10, ProductId = 2, Size = 9, Available = false },
            new() { Id = 11, ProductId = 2, Size = 10, Available = false },
            new() { Id = 12, ProductId = 2, Size = 11, Available = true },
            // Product 3: Asics GEL-1130 - large sizes only
            new() { Id = 13, ProductId = 3, Size = 6, Available = false },
            new() { Id = 14, ProductId = 3, Size = 7, Available = false },
            new() { Id = 15, ProductId = 3, Size = 8, Available = false },
            new() { Id = 16, ProductId = 3, Size = 9, Available = true },
            new() { Id = 17, ProductId = 3, Size = 10, Available = true },
            new() { Id = 18, ProductId = 3, Size = 11, Available = true },
            // Product 4: Nike P-6000 - scattered availability
            new() { Id = 19, ProductId = 4, Size = 6, Available = true },
            new() { Id = 20, ProductId = 4, Size = 7, Available = false },
            new() { Id = 21, ProductId = 4, Size = 8, Available = false },
            new() { Id = 22, ProductId = 4, Size = 9, Available = true },
            new() { Id = 23, ProductId = 4, Size = 10, Available = false },
            new() { Id = 24, ProductId = 4, Size = 11, Available = false },
            // Product 5: Nike Air Force 1 Black - popular, mostly sold out
            new() { Id = 25, ProductId = 5, Size = 6, Available = false },
            new() { Id = 26, ProductId = 5, Size = 7, Available = false },
            new() { Id = 27, ProductId = 5, Size = 8, Available = false },
            new() { Id = 28, ProductId = 5, Size = 9, Available = false },
            new() { Id = 29, ProductId = 5, Size = 10, Available = true },
            new() { Id = 30, ProductId = 5, Size = 11, Available = true },
            // Product 6: Nike Air Force 1 White - well stocked
            new() { Id = 31, ProductId = 6, Size = 6, Available = true },
            new() { Id = 32, ProductId = 6, Size = 7, Available = true },
            new() { Id = 33, ProductId = 6, Size = 8, Available = true },
            new() { Id = 34, ProductId = 6, Size = 9, Available = true },
            new() { Id = 35, ProductId = 6, Size = 10, Available = false },
            new() { Id = 36, ProductId = 6, Size = 11, Available = false },
            // Product 7: Jordan 4 Retro Blue - limited release, few sizes
            new() { Id = 37, ProductId = 7, Size = 6, Available = false },
            new() { Id = 38, ProductId = 7, Size = 7, Available = false },
            new() { Id = 39, ProductId = 7, Size = 8, Available = true },
            new() { Id = 40, ProductId = 7, Size = 9, Available = false },
            new() { Id = 41, ProductId = 7, Size = 10, Available = false },
            new() { Id = 42, ProductId = 7, Size = 11, Available = true },
            // Product 8: Jordan 1 Green - ends of size range
            new() { Id = 43, ProductId = 8, Size = 6, Available = true },
            new() { Id = 44, ProductId = 8, Size = 7, Available = false },
            new() { Id = 45, ProductId = 8, Size = 8, Available = false },
            new() { Id = 46, ProductId = 8, Size = 9, Available = false },
            new() { Id = 47, ProductId = 8, Size = 10, Available = false },
            new() { Id = 48, ProductId = 8, Size = 11, Available = true }
        };

        // Use raw SQL to insert with explicit IDs (SQLite identity insert)
        await using var transaction = await context.Database.BeginTransactionAsync();

        foreach (var brand in brands)
        {
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Brands (Id, Name) VALUES ({0}, {1})", brand.Id, brand.Name);
        }

        foreach (var category in categories)
        {
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Categories (Id, Name) VALUES ({0}, {1})", category.Id, category.Name);
        }

        foreach (var product in products)
        {
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Products (Id, Name, Price, Description, ImageUrl, CareInstructions, BrandId, CategoryId) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                product.Id, product.Name, product.Price, product.Description, product.ImageUrl, product.CareInstructions, product.BrandId, product.CategoryId);
        }

        foreach (var feature in features)
        {
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO ProductFeatures (Id, ProductId, Value) VALUES ({0}, {1}, {2})",
                feature.Id, feature.ProductId, feature.Value);
        }

        foreach (var image in images)
        {
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO ProductImages (Id, ProductId, Url) VALUES ({0}, {1}, {2})",
                image.Id, image.ProductId, image.Url);
        }

        foreach (var size in sizes)
        {
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO ProductSizes (Id, ProductId, Size, Available) VALUES ({0}, {1}, {2}, {3})",
                size.Id, size.ProductId, size.Size, size.Available ? 1 : 0);
        }

        await transaction.CommitAsync();
    }
}
