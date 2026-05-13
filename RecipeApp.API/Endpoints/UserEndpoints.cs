using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            users.MapPost("/{id}/AddFavorite", InsertFavorite);
            users.MapGet("/{id}/GetFavorites", GetFavorites);
            users.MapDelete("/{id}/RemoveFavorite/{recipeId}", DeleteFavorite);

            users.MapGet("/{id}/GetRatings", GetRatings);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<User> repository, IMapper mapper, UserPost user)
        {
            try
            {
                var newUser = mapper.Map<User>(user);

                await repository.Insert(newUser);

                return TypedResults.Created($"/user/{newUser.Id}", newUser);
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
                var users = await repository.Get();

                var response = mapper.Map<List<UserGet>>(users);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<User> repository, IMapper mapper, Guid id)
        {
            try
            {
                var user = await repository.GetById(id);

                if (user == null)
                    return Results.NotFound();

                var response = mapper.Map<UserGet>(user);

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
                if (user.Name != null)
                    target.Name = user.Name;
                if (user.PasswordHash != null)
                    target.PasswordHash = user.PasswordHash;
                target.UpdatedAt = DateTime.UtcNow;

                await repository.Update(target);

                return TypedResults.Ok(target);
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

                if (target == null)
                    return TypedResults.NotFound();

                if (await repository.Delete(id) != null)
                    return TypedResults.Ok(target);
                return TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        ///////////////
        // Favorites //
        ///////////////
        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> InsertFavorite(IRepository<Favorite> repository, IMapper mapper, FavoritePost favoriteList)
        {
            try
            {
                bool exists = await repository.GetQueryable(f => f.UserId == favoriteList.UserId && f.RecipeId == favoriteList.RecipeId).AnyAsync();
                if (exists)
                    return TypedResults.BadRequest("This recipe is already in the user's favorites.");

                var newFavorites = mapper.Map<Favorite>(favoriteList);

                await repository.Insert(newFavorites);

                return TypedResults.Created($"/user/{newFavorites.UserId}/AddFavorite", newFavorites);
            }
            catch (DbUpdateException)
            {
                return TypedResults.BadRequest("This recipe is already in the user's favorites.");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetFavorites(IRepository<Favorite> repository, IMapper mapper, Guid id)
        {
            try
            {
                var favorites = await repository.GetQueryable(f => f.UserId == id).ToListAsync();

                var response = mapper.Map<List<FavoriteGet>>(favorites);

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
        public static async Task<IResult> DeleteFavorite(IRepository<Favorite> repository, Guid id, Guid recipeId)
        {
            try
            {
                var favorite = await repository
                    .GetQueryable(f => f.UserId == id && f.RecipeId == recipeId)
                    .FirstOrDefaultAsync();

                if (favorite == null)
                    return TypedResults.NotFound();

                if (await repository.Delete(favorite.Id) != null)
                    return TypedResults.Ok(favorite);

                return TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        /////////////
        // Ratings //
        ////////////
        public static async Task<IResult> GetRatings(IRepository<Rating> repository, IMapper mapper, Guid id)
        {
            try
            {
                var recipeRatings = await repository.GetQueryable(r => r.UserId == id).ToListAsync();

                var response = mapper.Map<List<RatingGet>>(recipeRatings);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
