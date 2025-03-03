namespace RecipeApp.API.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string ImagePath { get; set; }
        public string UploaderId { get; set; }
        public User Uploader { get; set; }
        public List<RecipeIngredients> RecipeIngredients { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<UserComment> UserComments { get; set; }
        public List<FavoriteList> FavoriteLists { get; set; }
    }
}
