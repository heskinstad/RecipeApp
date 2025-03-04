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

            CreateMap<User, UserGet>();
            CreateMap<UserPost, User>();

            CreateMap<Rating, RatingGet>();
            CreateMap<RatingPost, Rating>();

            CreateMap<UserComment, UserCommentGet>();
            CreateMap<UserCommentPost, UserComment>();

            CreateMap<FavoriteList, FavoriteListGet>();
            CreateMap<FavoriteListPost, FavoriteList>();

            CreateMap<Ingredient, IngredientGet>();
            CreateMap<IngredientPost, Ingredient>();

            CreateMap<Unit, UnitGet>();
            CreateMap<UnitPost, Unit>();

            CreateMap<RecipeIngredients, RecipeIngredientsGet>();
            CreateMap<RecipeIngredientsPost, RecipeIngredients>();
        }
    }
}
