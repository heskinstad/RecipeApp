using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.POST
{
    public class RecipeIngredientsPost
    {
        public Guid RecipeId { get; set; }
        public Guid IngredientId { get; set; }
        public float Amount { get; set; }
        public Guid UnitId { get; set; }
        public string? Section { get; set; }
    }
}
