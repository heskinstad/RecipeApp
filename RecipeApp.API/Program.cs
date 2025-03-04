using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeApp.API.Data;
using RecipeApp.API.Endpoints;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RecipeContext>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IRepository<Recipe>, Repository<Recipe>>();
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Rating>, Repository<Rating>>();
builder.Services.AddScoped<IRepository<UserComment>, Repository<UserComment>>();
builder.Services.AddScoped<IRepository<FavoriteList>, Repository<FavoriteList>>();
builder.Services.AddScoped<IRepository<Ingredient>, Repository<Ingredient>>();
builder.Services.AddScoped<IRepository<Unit>, Repository<Unit>>();
builder.Services.AddScoped<IRepository<RecipeIngredients>, Repository<RecipeIngredients>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureRecipes();
app.ConfigureCategories();
app.ConfigureUsers();
app.ConfigureRatings();
app.ConfigureUserComments();
app.ConfigureFavoriteLists();
app.ConfigureIngredients();
app.ConfigureUnits();
app.ConfigureRecipeIngredients();

app.Run();
