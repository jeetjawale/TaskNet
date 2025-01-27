namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;        // Initialize
        public string PasswordHash { get; set; } = string.Empty; // Initialize
        public List<TaskItem> Tasks { get; set; } = new();
    }
}