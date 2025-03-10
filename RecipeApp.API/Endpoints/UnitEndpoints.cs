using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

namespace RecipeApp.API.Endpoints
{
    public static class UnitEndpoints
    {
        public static void ConfigureUnits(this WebApplication app)
        {
            var units = app.MapGroup("/unit");

            units.MapPost("/", Insert);
            units.MapGet("/", Get);
            units.MapGet("/{id}", GetById);
            units.MapDelete("/{id}", Delete);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Insert(IRepository<Unit> repository, IMapper mapper, UnitPost unit)
        {
            try
            {
                var newUnit = mapper.Map<Unit>(unit);

                await repository.Insert(newUnit);

                return TypedResults.Created($"Unit with id {newUnit.Id} created!");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Get(IRepository<Unit> repository, IMapper mapper)
        {
            try
            {
                var recipes = await repository.Get();

                var response = mapper.Map<List<UnitGet>>(recipes);

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Unit> repository, IMapper mapper, Guid id)
        {
            try
            {
                var unit = await repository.GetById(id);

                if (unit == null)
                    return Results.NotFound();

                var response = mapper.Map<UnitGet>(unit);

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
        public static async Task<IResult> Delete(IRepository<Unit> repository, Guid id)
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
