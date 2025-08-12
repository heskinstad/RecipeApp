using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.POST
{
    public class FavoritesPost
    {
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}
