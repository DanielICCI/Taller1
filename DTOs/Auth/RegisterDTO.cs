namespace Taller1.DTOs.Auth
{
    public class RegisterDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string Description { get; set; } = null!;
    }
}