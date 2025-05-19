using System.ComponentModel.DataAnnotations.Schema;

namespace Taller1.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        // FK al producto
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // FK al usuario
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}