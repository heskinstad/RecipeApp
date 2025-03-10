using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class RecipeEndpoints
    {
        public static void ConfigureRecipes(this WebApplication app)
        {
            var recipes = app.MapGroup("/recipe");

            recipes.MapPost("/", Insert);
            recipes.MapGet("/", Get);
            recipes.MapGet("/{id}", GetById);
            recipes.MapPut("/{id}", Update);
            recipes.MapDelete("/{id}", Delete);

            recipes.MapGet("/search", Search);
            recipes.MapGet("/category", GetByCategory);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<Recipe> repository, IMapper mapper, RecipePost recipe)
        {
            try
            {
                var newRecipe = mapper.Map<Recipe>(recipe);

                await repository.Insert(newRecipe);

                return TypedResults.Created($"Recipe with id {newRecipe.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<Recipe> repository, IMapper mapper)
        {
            try
            {
                var recipes = await repository.Get();

                var response = mapper.Map<List<RecipeGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Recipe> repository, IMapper mapper, Guid id)
        {
            try
            {
                var recipe = await repository.GetById(id);

                if (recipe == null)
                    return Results.NotFound();

                var response = mapper.Map<RecipeGet>(recipe);

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
        public static async Task<IResult> Update(IRepository<Recipe> repository, Guid id, RecipePost recipe)
        {
            try
            {
                var target = await repository.GetById(id);

                if (target == null)
                    return Results.NotFound();
                if (recipe.Name != null)
                    target.Name = recipe.Name;
                if (recipe.Description != null)
                    target.Description = recipe.Description;
                if (recipe.ImagePath != null)
                    target.ImagePath = recipe.ImagePath;
                target.UpdatedAt = DateTime.UtcNow;

                await repository.Update(target);

                return TypedResults.Created($"Recipe with id {target.Id} updated!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IRepository<Recipe> repository, Guid id)
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



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Search(IRepository<Recipe> repository, IMapper mapper, string? name)
        {
            try
            {
                // Use the GetQueryable method to filter based on the name query parameter
                var recipes = await repository.GetQueryable(r =>
                    string.IsNullOrEmpty(name) || r.Name.ToLower().Contains(name.ToLower()));

                var response = mapper.Map<List<RecipeGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> GetByCategory(IRepository<Recipe> repository, IMapper mapper, string? name)
        {
            try
            {
                var recipes = await repository.GetQueryable(r =>
                    string.IsNullOrEmpty(name) || r.Category.Name.ToLower().Equals(name.ToLower()));

                if (string.IsNullOrEmpty(name))
                    recipes = [];

                var response = mapper.Map<List<RecipeGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
