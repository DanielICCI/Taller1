using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Taller1.Data;
using Taller1.Models;
using Taller1.DTOs.Cart;

namespace Taller1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los productos del carrito del usuario autenticado
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Ok(cart);
        }
        [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var existingItem = await _context.CartItems
        .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == dto.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
        var item = new CartItem
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UserId = userId
        };

        _context.CartItems.Add(item);
        }

    await _context.SaveChangesAsync();
    return Ok("Producto agregado al carrito.");
}

        // Actualizar cantidad
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (item == null) return NotFound();

            item.Quantity = quantity;
            await _context.SaveChangesAsync();
            return Ok("Cantidad actualizada.");
        }

        // Eliminar producto del carrito
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok("Producto eliminado.");
        }
    }
}