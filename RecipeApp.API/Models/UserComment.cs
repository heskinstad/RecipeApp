namespace RecipeApp.API.Models
{
    public class UserComment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public int msg { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
