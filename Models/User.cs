namespace TodoApi.Models {
    public class User {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
