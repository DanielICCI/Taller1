using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Taller1.Data;
using Taller1.Models;

namespace Taller1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Confirmar pedido desde el carrito
        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest("El carrito está vacío.");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Items = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            // Limpiar carrito
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok("Pedido confirmado.");
        }

        // Ver historial de pedidos
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return Ok(orders);
        }
    }
}