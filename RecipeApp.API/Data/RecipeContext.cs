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
            modelBuilder.Entity<Favorites>()
                .HasKey(f => new { f.UserId, f.RecipeId });

            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.UserId, r.RecipeId });

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

            modelBuilder.Entity<Favorites>()
                .HasOne(fl => fl.User)
                .WithMany(u => u.FavoriteLists)
                .HasForeignKey(fl => fl.UserId);

            modelBuilder.Entity<Favorites>()
                .HasOne(fl => fl.Recipe)
                .WithMany(r => r.FavoriteLists)
                .HasForeignKey(fl => fl.RecipeId);

            // Make fields unique
            modelBuilder.Entity<Category>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<Ingredient>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<Unit>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<UserComment> UserComments { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
    }
}
