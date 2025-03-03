namespace RecipeApp.API.Models
{
    public class Rating
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public int Score { get; set; }
    }
}
