// TaskItem.cs
namespace Backend.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.Low;
        public string Tags { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User? User { get; set; } // Mark as nullable
    }

    public enum PriorityLevel { Low, Medium, High, Critical }
}