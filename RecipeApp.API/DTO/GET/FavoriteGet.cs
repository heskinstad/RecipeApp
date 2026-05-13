using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.GET
{
    public class FavoriteGet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}
