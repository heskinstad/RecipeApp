using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace RecipeApp.API.Models
{
    public class FavoriteList
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
