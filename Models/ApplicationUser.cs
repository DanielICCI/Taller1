using Microsoft.AspNetCore.Identity;

namespace Taller1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}