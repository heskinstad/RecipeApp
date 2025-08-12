using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeApp.API.Data;
using RecipeApp.API.Endpoints;
using RecipeApp.API.Mapper;
using RecipeApp.API.Models;
using RecipeApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RecipeContext>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});

builder.Services.AddScoped<IRepository<Recipe>, Repository<Recipe>>();
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Rating>, Repository<Rating>>();
builder.Services.AddScoped<IRepository<UserComment>, Repository<UserComment>>();
builder.Services.AddScoped<IRepository<Favorites>, Repository<Favorites>>();
builder.Services.AddScoped<IRepository<Ingredient>, Repository<Ingredient>>();
builder.Services.AddScoped<IRepository<Unit>, Repository<Unit>>();
builder.Services.AddScoped<IRepository<RecipeIngredients>, Repository<RecipeIngredients>>();

builder.Services.AddCors();

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
app.ConfigureUserComments();
app.ConfigureIngredients();
app.ConfigureUnits();

// Initializer to seed db for empty tables
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RecipeContext>();
    DbInitializer.Initialize(context);
}

    app.UseCors(options =>
        options.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());

app.Run();
