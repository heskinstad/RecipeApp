namespace RecipeApp.API.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RecipeIngredients> RecipeIngredients { get; set; }
    }
}
