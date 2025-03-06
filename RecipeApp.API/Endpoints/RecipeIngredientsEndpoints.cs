using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class RecipeIngredientsEndpoints
    {
        public static void ConfigureRecipeIngredients(this WebApplication app)
        {
            var recipeIngredients = app.MapGroup("/recipeIngredients");

            recipeIngredients.MapPost("/", Insert);
            recipeIngredients.MapGet("/", Get);
            recipeIngredients.MapGet("/{recipeIngredientsId}", GetById);
            recipeIngredients.MapDelete("/{recipeIngredientsId}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<RecipeIngredients> repository, IMapper mapper, RecipeIngredientsPost recipeIngredients)
        {
            try
            {
                var newRecipeIngredients = mapper.Map<RecipeIngredients>(recipeIngredients);

                await repository.Insert(newRecipeIngredients);

                return TypedResults.Created($"RecipeIngredients with id {newRecipeIngredients.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<RecipeIngredients> repository, IMapper mapper)
        {
            try
            {
                var recipeIngredients = await repository.Get();

                var response = mapper.Map<List<RecipeIngredientsGet>>(recipeIngredients);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<RecipeIngredients> repository, IMapper mapper, Guid id)
        {
            try
            {
                var recipeIngredients = await repository.GetById(id);

                if (recipeIngredients == null)
                    return Results.NotFound();

                var response = mapper.Map<RecipeIngredientsGet>(recipeIngredients);

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
        public static async Task<IResult> Delete(IRepository<RecipeIngredients> repository, int id)
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
