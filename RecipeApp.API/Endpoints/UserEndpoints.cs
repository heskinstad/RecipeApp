using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void ConfigureUsers(this WebApplication app)
        {
            var users = app.MapGroup("/user");

            users.MapPost("/", Insert);
            users.MapGet("/", Get);
            users.MapGet("/{id}", GetById);
            users.MapPut("/{id}", Update);
            users.MapDelete("/{id}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<User> repository, IMapper mapper, UserPost user)
        {
            try
            {
                var newUser = mapper.Map<User>(user);

                await repository.Insert(newUser);

                return TypedResults.Created($"User with id {newUser.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<User> repository, IMapper mapper)
        {
            try
            {
                var recipes = await repository.Get();

                var response = mapper.Map<List<UserGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<User> repository, IMapper mapper, int id)
        {
            try
            {
                var recipe = await repository.GetById(id);

                if (recipe == null)
                    return Results.NotFound();

                var response = mapper.Map<UserGet>(recipe);

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
        public static async Task<IResult> Update(IRepository<User> repository, Guid id, UserPost user)
        {
            try
            {
                var target = await repository.GetById(id);

                if (target == null)
                    return Results.NotFound();
                if (user.Username != null)
                    target.Username = user.Username;
                if (user.PasswordHash != null)
                    target.PasswordHash = user.PasswordHash;
                target.UpdatedAt = DateTime.UtcNow;

                await repository.Update(target);

                return TypedResults.Created($"User with id {target.Id} updated!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IRepository<User> repository, Guid id)
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
