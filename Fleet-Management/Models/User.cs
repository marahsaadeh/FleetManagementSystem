namespace Fleet_Management.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }

       
        public virtual UserRole? Role { get; set; }
    }
}
