namespace RecipeApp.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<Recipe> Recipes { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<UserComment> UserComments { get; set; }
        public List<Favorites> FavoriteLists { get; set; }
    }
}
