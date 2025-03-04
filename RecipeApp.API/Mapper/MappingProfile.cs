using AutoMapper;
using RecipeApp.API.Models;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;

namespace RecipeApp.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, RecipeGet>();
            CreateMap<RecipePost, Recipe>();

            CreateMap<Category, CategoryGet>();
            CreateMap<CategoryPost, Category>();
        }
    }
}
