using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class IngredientEndpoints
    {
        public static void ConfigureIngredients(this WebApplication app)
        {
            var ingredients = app.MapGroup("/ingredient");

            ingredients.MapPost("/", Insert);
            ingredients.MapGet("/", Get);
            ingredients.MapGet("/{id}", GetById);
            ingredients.MapDelete("/{id}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<Ingredient> repository, IMapper mapper, IngredientPost ingredient)
        {
            try
            {
                var newIngredient = mapper.Map<Ingredient>(ingredient);

                await repository.Insert(newIngredient);

                return TypedResults.Created($"Ingredient with id {newIngredient.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<Ingredient> repository, IMapper mapper)
        {
            try
            {
                var ingredients = await repository.Get();

                var response = mapper.Map<List<IngredientGet>>(ingredients);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Ingredient> repository, IMapper mapper, Guid id)
        {
            try
            {
                var ingredient = await repository.GetById(id);

                if (ingredient == null)
                    return Results.NotFound();

                var response = mapper.Map<IngredientGet>(ingredient);

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
        public static async Task<IResult> Delete(IRepository<Ingredient> repository, Guid id)
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
