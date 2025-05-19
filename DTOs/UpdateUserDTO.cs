namespace Taller1.DTOs.User
{
    public class UpdateUserDTO
    {
        public string FullName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }
}