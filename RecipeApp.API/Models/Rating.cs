namespace RecipeApp.API.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int Score { get; set; }
    }
}
