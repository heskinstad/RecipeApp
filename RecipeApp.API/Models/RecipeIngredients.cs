namespace RecipeApp.API.Models
{
    public class RecipeIngredients
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public float Amount { get; set; }
        public int UnitId { get; set; }
    }
}
