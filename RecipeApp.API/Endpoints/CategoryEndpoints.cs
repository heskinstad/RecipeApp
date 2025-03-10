using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void ConfigureCategories(this WebApplication app)
        {
            var categories = app.MapGroup("/category");

            categories.MapPost("/", Insert);
            categories.MapGet("/", Get);
            categories.MapGet("/{id}", GetById);
            categories.MapDelete("/{id}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<Category> repository, IMapper mapper, CategoryPost category)
        {
            try
            {
                var newCategory = mapper.Map<Category>(category);

                await repository.Insert(newCategory);

                return TypedResults.Created($"Category with id {newCategory.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<Category> repository, IMapper mapper)
        {
            try
            {
                var categories = await repository.Get();

                var response = mapper.Map<List<CategoryGet>>(categories);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Category> repository, IMapper mapper, Guid id)
        {
            try
            {
                var category = await repository.GetById(id);

                if (category == null)
                    return Results.NotFound();

                var response = mapper.Map<CategoryGet>(category);

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
        public static async Task<IResult> Delete(IRepository<Category> repository, Guid id)
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
