﻿namespace RecipeApp.API.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
