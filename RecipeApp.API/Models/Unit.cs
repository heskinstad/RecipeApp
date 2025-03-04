namespace RecipeApp.API.Models
{
    public class Unit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RecipeIngredients> RecipeIngredients { get; set; }
    }
}
