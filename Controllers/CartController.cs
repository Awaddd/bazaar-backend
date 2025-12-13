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

    public CartController(AppDbContext db)
    {
        _db = db;
    }

    private string? GetSessionId()
    {
        return Request.Headers["X-Session-Id"].FirstOrDefault();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest("X-Session-Id header is required.");
        }

        var cartItems = await _db.CartItems
            .Where(ci => ci.SessionId == sessionId)
            .ToListAsync();

        var productIds = cartItems.Select(ci => ci.ProductId).ToList();

        var products = await _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var cartItemDtos = cartItems.Select(ci =>
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
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest("X-Session-Id header is required.");
        }

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

        // Check if item already exists in cart (same session + product + size)
        var existingItem = await _db.CartItems.FirstOrDefaultAsync(ci =>
            ci.SessionId == sessionId &&
            ci.ProductId == itemDto.ProductId &&
            ci.Size == itemDto.Size);

        if (existingItem != null)
        {
            // Update quantity
            existingItem.Quantity += itemDto.Quantity;
            await _db.SaveChangesAsync();

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
            SessionId = sessionId,
            ProductId = itemDto.ProductId,
            Size = itemDto.Size,
            Quantity = itemDto.Quantity
        };

        _db.CartItems.Add(newItem);
        await _db.SaveChangesAsync();

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
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest("X-Session-Id header is required.");
        }

        if (updateDto.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than 0.");
        }

        var cartItem = await _db.CartItems.FirstOrDefaultAsync(ci =>
            ci.Id == id && ci.SessionId == sessionId);

        if (cartItem == null)
        {
            return NotFound($"Cart item with ID {id} does not exist.");
        }

        cartItem.Quantity = updateDto.Quantity;
        await _db.SaveChangesAsync();

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
    public async Task<IActionResult> Remove(int id)
    {
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest("X-Session-Id header is required.");
        }

        var cartItem = await _db.CartItems.FirstOrDefaultAsync(ci =>
            ci.Id == id && ci.SessionId == sessionId);

        if (cartItem == null)
        {
            return NotFound($"Cart item with ID {id} does not exist.");
        }

        _db.CartItems.Remove(cartItem);
        await _db.SaveChangesAsync();

        return Ok(new { success = true });
    }
}
