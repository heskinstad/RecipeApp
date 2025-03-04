namespace RecipeApp.API.Models
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RecipeIngredients> RecipeIngredients { get; set; }
    }
}
