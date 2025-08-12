using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace RecipeApp.API.Models
{
    public class Favorites
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
