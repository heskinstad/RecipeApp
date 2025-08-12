using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.GET
{
    public class FavoritesGet
    {
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}
