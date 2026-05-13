using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.POST
{
    public class FavoritePost
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}
