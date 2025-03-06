using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class UserCommentEndpoints
    {
        public static void ConfigureUserComments(this WebApplication app)
        {
            var userComments = app.MapGroup("/userComment");

            userComments.MapPost("/", Insert);
            userComments.MapGet("/", Get);
            userComments.MapGet("/{userCommentId}", GetById);
            userComments.MapDelete("/{userCommentId}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<UserComment> repository, IMapper mapper, UserCommentPost userComment)
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
        public static async Task<IResult> Get(IRepository<UserComment> repository, IMapper mapper)
        {
            try
            {
                var userComments = await repository.Get();

                var response = mapper.Map<List<UserCommentGet>>(userComments);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<UserComment> repository, IMapper mapper, Guid id)
        {
            try
            {
                var userComment = await repository.GetById(id);

                if (userComment == null)
                    return Results.NotFound();

                var response = mapper.Map<UserCommentGet>(userComment);

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
        public static async Task<IResult> Delete(IRepository<UserComment> repository, Guid id)
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
