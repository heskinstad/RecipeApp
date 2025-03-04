namespace RecipeApp.API.Models
{
    public class UserComment
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int msg { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
