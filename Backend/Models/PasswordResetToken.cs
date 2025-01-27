namespace Backend.Models
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty; // Initialize with empty string
        public string Email { get; set; } = string.Empty;  // Initialize with empty string
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}