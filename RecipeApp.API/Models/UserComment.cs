namespace RecipeApp.API.Models
{
    public class UserComment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string message { get; set; }
        public int upvotes { get; set; }
        public int downvotes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
