using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace RecipeApp.API.Models
{
    public class FavoriteList
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
