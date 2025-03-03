namespace RecipeApp.API.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string ImagePath { get; set; }
        public string UploaderId { get; set; }
    }
}
