namespace RecipeApp.API.Models
{
    public class RecipeIngredients
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public float Amount { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
