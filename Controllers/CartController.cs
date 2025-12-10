using Microsoft.AspNetCore.Mvc;
using Commerce.Api.Data;
using Commerce.Api.Models;
using Commerce.Api.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly AppDbContext _db;
    private static readonly List<CartItem> _cartItems = new();
    private static int _nextId = 1;
    private static bool _seeded = false;

    public CartController(AppDbContext db)
    {
        _db = db;

        // Seed demo data on first controller instantiation
        if (!_seeded)
        {
            SeedDemoData();
            _seeded = true;
        }
    }

    private static void SeedDemoData()
    {
        _cartItems.Add(new CartItem { Id = _nextId++, ProductId = 1, Size = 9, Quantity = 1 });
        _cartItems.Add(new CartItem { Id = _nextId++, ProductId = 2, Size = 10, Quantity = 2 });
        _cartItems.Add(new CartItem { Id = _nextId++, ProductId = 3, Size = 8, Quantity = 1 });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var productIds = _cartItems.Select(ci => ci.ProductId).ToList();

        var products = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => productIds.Contains(p.Id))
            .ToDictionary(p => p.Id);

        var cartItemDtos = _cartItems.Select(ci =>
        {
            if (!products.TryGetValue(ci.ProductId, out var product))
            {
                return null;
            }

            return new CartItemReadDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = product.Name,
                Price = (double)product.Price,
                ImageUrl = product.ImageUrl,
                Size = ci.Size,
                Quantity = ci.Quantity
            };
        })
        .Where(dto => dto != null)
        .ToList();

        return Ok(cartItemDtos);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CartItemCreateDto itemDto)
    {
        if (itemDto.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than 0.");
        }

        var product = await _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Sizes)
            .FirstOrDefaultAsync(p => p.Id == itemDto.ProductId);

        if (product == null)
        {
            return NotFound($"Product with ID {itemDto.ProductId} does not exist.");
        }

        var productSize = product.Sizes.FirstOrDefault(s => s.Size == itemDto.Size);
        if (productSize == null || !productSize.Available)
        {
            return BadRequest($"Size {itemDto.Size} is not available for this product.");
        }

        // Check if item already exists in cart (same product + size)
        var existingItem = _cartItems.FirstOrDefault(ci =>
            ci.ProductId == itemDto.ProductId && ci.Size == itemDto.Size);

        if (existingItem != null)
        {
            // Update quantity
            existingItem.Quantity += itemDto.Quantity;

            var responseDto = new CartItemReadDto
            {
                Id = existingItem.Id,
                ProductId = existingItem.ProductId,
                ProductName = product.Name,
                Price = (double)product.Price,
                ImageUrl = product.ImageUrl,
                Size = existingItem.Size,
                Quantity = existingItem.Quantity
            };

            return Ok(responseDto);
        }

        // Add new item
        var newItem = new CartItem
        {
            Id = _nextId++,
            ProductId = itemDto.ProductId,
            Size = itemDto.Size,
            Quantity = itemDto.Quantity
        };

        _cartItems.Add(newItem);

        var result = new CartItemReadDto
        {
            Id = newItem.Id,
            ProductId = newItem.ProductId,
            ProductName = product.Name,
            Price = (double)product.Price,
            ImageUrl = product.ImageUrl,
            Size = newItem.Size,
            Quantity = newItem.Quantity
        };

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CartItemUpdateDto updateDto)
    {
        if (updateDto.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than 0.");
        }

        var cartItem = _cartItems.FirstOrDefault(ci => ci.Id == id);
        if (cartItem == null)
        {
            return NotFound($"Cart item with ID {id} does not exist.");
        }

        cartItem.Quantity = updateDto.Quantity;

        var product = await _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);

        if (product == null)
        {
            return NotFound($"Product with ID {cartItem.ProductId} does not exist.");
        }

        var result = new CartItemReadDto
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            ProductName = product.Name,
            Price = (double)product.Price,
            ImageUrl = product.ImageUrl,
            Size = cartItem.Size,
            Quantity = cartItem.Quantity
        };

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Remove(int id)
    {
        var cartItem = _cartItems.FirstOrDefault(ci => ci.Id == id);
        if (cartItem == null)
        {
            return NotFound($"Cart item with ID {id} does not exist.");
        }

        _cartItems.Remove(cartItem);
        return Ok(new { success = true });
    }
}
