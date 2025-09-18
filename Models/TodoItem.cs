namespace TodoApi.Models {
    public class TodoItem {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // relationship
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
