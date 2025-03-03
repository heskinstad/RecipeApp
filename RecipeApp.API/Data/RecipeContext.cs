using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using RecipeApp.API.Models;

namespace RecipeApp.API.Data
{
    public class RecipeContext : DbContext
    {
        private string _connectionString;
        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngredients>()
                .HasOne(ri => ri.Unit)
                .WithMany(u => u.RecipeIngredients)
                .HasForeignKey(ri => ri.UnitId);

            modelBuilder.Entity<RecipeIngredients>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

            modelBuilder.Entity<RecipeIngredients>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CategoryId);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Uploader)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UploaderId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Recipe)
                .WithMany(ra => ra.Ratings)
                .HasForeignKey(r => r.RecipeId);

            modelBuilder.Entity<UserComment>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserComments)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserComment>()
                .HasOne(uc => uc.Recipe)
                .WithMany(r => r.UserComments)
                .HasForeignKey(uc => uc.RecipeId);

            modelBuilder.Entity<FavoriteList>()
                .HasOne(fl => fl.User)
                .WithMany(u => u.FavoriteLists)
                .HasForeignKey(fl => fl.UserId);

            modelBuilder.Entity<FavoriteList>()
                .HasOne(fl => fl.Recipe)
                .WithMany(r => r.FavoriteLists)
                .HasForeignKey(fl => fl.RecipeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
