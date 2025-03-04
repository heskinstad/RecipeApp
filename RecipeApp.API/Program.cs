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

app.Run();
