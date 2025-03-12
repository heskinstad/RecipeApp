namespace RecipeApp.API.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string ImagePath { get; set; }
        public Guid UploaderId { get; set; }
        public User Uploader { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<RecipeIngredients> RecipeIngredients { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<UserComment> UserComments { get; set; }
        public List<FavoriteList> FavoriteLists { get; set; }
    }
}
