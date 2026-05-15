namespace RecipeApp.API.Models
{
    public class UserComment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string Message { get; set; }
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
