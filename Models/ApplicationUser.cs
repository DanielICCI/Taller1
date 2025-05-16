using Microsoft.AspNetCore.Identity;

namespace Taller1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}