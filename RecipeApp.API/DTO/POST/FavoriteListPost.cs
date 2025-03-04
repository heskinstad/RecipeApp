using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.POST
{
    public class FavoriteListPost
    {
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}
