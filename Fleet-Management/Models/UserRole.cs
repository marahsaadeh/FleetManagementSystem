namespace Fleet_Management.Models
{
    public class UserRole
    {
        public int RoleId { get; set; }
        // "Admin", "User", "Viewer"
        public string? RoleName { get; set; } 
        public virtual ICollection<User>? Users { get; set; }
    }
}
