namespace SchoolApi.Models
{ 
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
