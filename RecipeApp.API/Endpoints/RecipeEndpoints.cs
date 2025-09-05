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
using Microsoft.EntityFrameworkCore;
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

            recipes.MapGet("/random", GetRandom);
            recipes.MapGet("/randomMultiple", GetRandomMultiple);

            recipes.MapGet("/search", Search);
            recipes.MapGet("/category", GetByCategory);

            recipes.MapPost("{id}/ingredients", InsertIngredient);
            recipes.MapGet("{id}/ingredients", GetIngredients);
            recipes.MapDelete("{id}/ingredients/{ingredientId}", DeleteIngredient);

            recipes.MapPost("/{id}/ratings", InsertRating);
            recipes.MapGet("/{id}/ratings", GetRatings);
            recipes.MapGet("/{id}/ratingsCount", GetRatingsCount);
            recipes.MapGet("/{id}/averageRating", GetAverageRating);

            recipes.MapPost("/{id}/comments", InsertComment);
            recipes.MapGet("/{id}/comments", GetComments);
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
                if (recipe.Summary != null)
                    target.Summary = recipe.Summary;
                if (recipe.Description != null)
                    target.Description = recipe.Description;
                if (recipe.ImagePath != null)
                    target.ImagePath = recipe.ImagePath;
                if (recipe.CategoryId != null)
                    target.CategoryId = recipe.CategoryId;
                if (recipe.UploaderId != null)
                    target.UploaderId = recipe.UploaderId;
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

        //https://dotnetfullstackdev.substack.com/p/react-implementing-server-side-pagination-24-04-15

        // Endpoint to search for a recipe. Includes pagination and sorting.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Search(
            IRepository<Recipe> repository,
            IMapper mapper,
            string searchString = "",
            int pageNumber = 1,
            int pageSize = 4,
            string sortBy = "date_desc")
        {
            try
            {


                // Use the GetQueryable method to filter based on the name query parameter
                var recipes = repository.GetQueryable(r =>
                    string.IsNullOrEmpty(searchString) || r.Name.ToLower().Contains(searchString.ToLower()))
                    .Include(r => r.Uploader);

                var totalCount = await recipes.CountAsync();

                var ordered_recipes = recipes.OrderByDescending(r => r.UpdatedAt);

                switch (sortBy)
                {
                    case "date_desc":
                        break;
                    case "date":
                        ordered_recipes = recipes.OrderBy(r => r.UpdatedAt);
                        break;
                    case "name":
                        ordered_recipes = recipes.OrderBy(r => r.Name);
                        break;
                    case "name_desc":
                        ordered_recipes = recipes.OrderByDescending(r => r.Name);
                        break;
                    case "rating":
                        //TODO: implement
                        break;
                    case "rating_desc":
                        //TODO: implement
                        break;
                }

                var paginatedRecipes = await ordered_recipes
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var responseItems = mapper.Map<List<RecipeGet>>(paginatedRecipes);

                var paginatedResponse = new PaginatedResponse<RecipeGet>(
                    responseItems,
                    totalCount,
                    pageSize
                );

                return TypedResults.Ok(paginatedResponse);
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
                    string.IsNullOrEmpty(name) || r.Category.Name.ToLower().Equals(name.ToLower())).Include(r => r.Uploader).ToListAsync();

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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetRandom(IRepository<Recipe> repository, IMapper mapper)
        {
            try
            {
                var queryable = repository.GetQueryable();
                int count = await queryable.CountAsync();

                if (count == 0)
                    return Results.NotFound();

                var random = new Random();
                int skip = random.Next(count);

                var randomRecipe = await queryable.Skip(skip).Take(1).FirstOrDefaultAsync();

                if (randomRecipe == null)
                    return Results.NotFound();

                var response = mapper.Map<RecipeGet>(randomRecipe);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> GetRandomMultiple(IRepository<Recipe> repository, IMapper mapper, int count)
        {
            try
            {
                if (count <= 0)
                    return TypedResults.BadRequest("Count must be a positive integer.");

                var queryable = repository.GetQueryable();
                int totalCount = await queryable.CountAsync();

                if (totalCount == 0)
                    return TypedResults.NotFound("No recipes found.");

                int actualCount = Math.Min(count, totalCount);

                var random = new Random();
                var skipIndices = new HashSet<int>();
                while (skipIndices.Count < actualCount)
                {
                    skipIndices.Add(random.Next(totalCount));
                }

                var randomRecipes = new List<Recipe>();

                // Run queries sequentially to avoid DbContext concurrency
                foreach (int index in skipIndices)
                {
                    var recipe = await queryable.Skip(index).Take(1).FirstOrDefaultAsync();
                    if (recipe != null)
                        randomRecipes.Add(recipe);
                }

                var response = mapper.Map<List<RecipeGet>>(randomRecipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        /////////////////
        // Ingredients //
        /////////////////

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> InsertIngredient(IRepository<RecipeIngredients> repository, IMapper mapper, RecipeIngredientsPost recipeIngredients)
        {
            try
            {
                var newRecipeIngredients = mapper.Map<RecipeIngredients>(recipeIngredients);

                await repository.Insert(newRecipeIngredients);

                return TypedResults.Created($"New RecipeIngredients combination created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetIngredients(IRepository<RecipeIngredients> repository, IMapper mapper, Guid id)
        {
            try
            {
                var recipeIngredients = await repository
                    .GetQueryable(r => r.RecipeId == id)
                    .Include(r => r.Ingredient)
                    .Include(r => r.Unit)
                    .ToListAsync();

                var response = mapper.Map<List<RecipeIngredientsGet>>(recipeIngredients);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        //UNFINISHED
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteIngredient(IRepository<RecipeIngredients> repository, Guid id, Guid ingredientId)
        {
            try
            {
                var recipeIngredients = await repository.GetQueryable(r => r.RecipeId == id).ToListAsync();

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

        /////////////
        // Ratings //
        /////////////

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> InsertRating(IRepository<Rating> repository, IMapper mapper, RatingPost rating, Guid id)
        {
            try
            {
                var newRating = mapper.Map<Rating>(rating);

                await repository.Insert(newRating);

                return TypedResults.Created($"New UserRecipeRating combination created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetRatings(IRepository<Rating> repository, IMapper mapper, Guid id)
        {
            try
            {
                var recipeRatings = await repository.GetQueryable(r => r.RecipeId == id).ToListAsync();

                var response = mapper.Map<List<RatingGet>>(recipeRatings);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        public static async Task<IResult> GetRatingsCount(IRepository<Recipe> repository, IRepository<Rating> ratingRepository, Guid id)
        {
            try
            {
                var recipe = await repository.GetById(id);
                if (recipe == null)
                    return Results.NotFound();

                var ratings = await ratingRepository.GetQueryable(r => r.RecipeId == id).ToListAsync();

                int ratingsCount = ratings.Count();

                return TypedResults.Ok(ratingsCount);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        public static async Task<IResult> GetAverageRating(IRepository<Recipe> repository, IRepository<Rating> ratingRepository, Guid id)
        {
            try
            {
                var recipe = await repository.GetById(id);
                if (recipe == null)
                    return Results.NotFound();

                var ratings = await ratingRepository.GetQueryable(r => r.RecipeId == id).ToListAsync();

                int ratingsCount = ratings.Count();

                if (ratingsCount == 0)
                    return TypedResults.Ok(0);

                double averageRating = Math.Round(ratings.Average(r => r.Score), 1);

                return TypedResults.Ok(averageRating);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        //////////////
        // Comments //
        //////////////

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> InsertComment(IRepository<UserComment> repository, IMapper mapper, UserCommentPost userComment)
        {
            try
            {
                var newUserComment = mapper.Map<UserComment>(userComment);

                await repository.Insert(newUserComment);

                return TypedResults.Created($"UserComment with id {newUserComment.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetComments(IRepository<UserComment> repository, IMapper mapper, Guid id)
        {
            try
            {
                var comments = await repository
                    .GetQueryable(c => c.RecipeId == id)
                    .Include(c => c.User)
                    .ToListAsync();

                var response = mapper.Map<List<UserCommentGet>>(comments);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
