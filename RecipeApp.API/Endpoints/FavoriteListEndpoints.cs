using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class FavoriteListEndpoints
    {
        public static void ConfigureFavoriteLists(this WebApplication app)
        {
            var favoriteLists = app.MapGroup("/favoriteList");

            favoriteLists.MapPost("/", Insert);
            favoriteLists.MapGet("/", Get);
            favoriteLists.MapGet("/{favoriteListId}", GetById);
            favoriteLists.MapDelete("/{favoriteListId}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<FavoriteList> repository, IMapper mapper, FavoriteListPost favoriteList)
        {
            try
            {
                var newFavoriteList = mapper.Map<FavoriteList>(favoriteList);

                await repository.Insert(newFavoriteList);

                return TypedResults.Created($"FavoriteList with id {newFavoriteList.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<FavoriteList> repository, IMapper mapper)
        {
            try
            {
                var favoriteLists = await repository.Get();

                var response = mapper.Map<List<FavoriteListGet>>(favoriteLists);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<FavoriteList> repository, IMapper mapper, Guid id)
        {
            try
            {
                var favoriteList = await repository.GetById(id);

                if (favoriteList == null)
                    return Results.NotFound();

                var response = mapper.Map<FavoriteListGet>(favoriteList);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IRepository<FavoriteList> repository, Guid id)
        {
            try
            {
                var target = await repository.GetById(id);

                if (await repository.Delete(id) != null)
                    return TypedResults.Ok(target);
                return TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
