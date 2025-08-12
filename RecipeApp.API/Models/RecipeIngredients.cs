namespace RecipeApp.API.Models
{
    public class RecipeIngredients
    {
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public float Amount { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
