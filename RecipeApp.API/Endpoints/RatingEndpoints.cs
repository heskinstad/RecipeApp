using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class RatingEndpoints
    {
        public static void ConfigureRatings(this WebApplication app)
        {
            var ratings = app.MapGroup("/rating");

            ratings.MapPost("/", Insert);
            ratings.MapGet("/", Get);
            ratings.MapGet("/{ratingId}", GetById);
            ratings.MapDelete("/{ratingId}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<Rating> repository, IMapper mapper, RatingPost rating)
        {
            try
            {
                var newRating = mapper.Map<Rating>(rating);

                await repository.Insert(newRating);

                return TypedResults.Created($"Rating with id {newRating.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<Rating> repository, IMapper mapper)
        {
            try
            {
                var ratings = await repository.Get();

                var response = mapper.Map<List<RatingGet>>(ratings);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Rating> repository, IMapper mapper, Guid id)
        {
            try
            {
                var rating = await repository.GetById(id);

                if (rating == null)
                    return Results.NotFound();

                var response = mapper.Map<RatingGet>(rating);

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
        public static async Task<IResult> Delete(IRepository<Rating> repository, Guid id)
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
