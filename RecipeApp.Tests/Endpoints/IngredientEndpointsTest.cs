using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.API.Data;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;

namespace RecipeApp.Tests.Endpoints
{
    [TestFixture]
    public class IngredientEndpointsTest
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private SqliteConnection _sqliteConnection;

        [SetUp]
        public void SetUp()
        {
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");
            _sqliteConnection.Open();

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<RecipeContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<RecipeContext>(options =>
                    {
                        options.UseSqlite(_sqliteConnection);
                    });
                });
            });

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RecipeContext>();
                context.Database.EnsureCreated();
            }

            _client = _factory.CreateClient();
        }

        [TearDown]
        public void Dispose()
        {
            _client?.Dispose();
            _factory?.Dispose();

            _sqliteConnection?.Close();
            _sqliteConnection?.Dispose();
        }

        [Test]
        public async Task Insert_ReturnsValue()
        {
            // Arrange
            IngredientPost ingredient = new IngredientPost() { Name = "Cheese" };

            // Act
            var response = await _client.PostAsJsonAsync("/ingredient", ingredient);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<IngredientGet>();
            Assert.That(result?.Name, Is.EqualTo("Cheese"));
        }

        [Test]
        public async Task GetAll_ReturnsMultipleElements()
        {
            // Arrange
            IngredientPost ingredient1 = new IngredientPost() { Name = "Cheese" };
            IngredientPost ingredient2 = new IngredientPost() { Name = "Tomato" };

            await _client.PostAsJsonAsync("/ingredient", ingredient1);
            await _client.PostAsJsonAsync("/ingredient", ingredient2);

            // Act
            var response = await _client.GetAsync("/ingredient");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<IngredientGet>>();
            Assert.That(result?.Count == 2);
            Assert.That(result?[0].Name, Is.EqualTo("Cheese"));
            Assert.That(result?[1].Name, Is.EqualTo("Tomato"));
        }

        [Test]
        public async Task GetSingle_ReturnsSingle()
        {
            // Arrange
            IngredientPost ingredient1 = new IngredientPost() { Name = "Cheese" };

            var postResponse = await _client.PostAsJsonAsync("/ingredient", ingredient1);

            // Act
            var response = await _client.GetAsync($"/ingredient/{postResponse.Content.ReadFromJsonAsync<IngredientGet>().Result.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<IngredientGet>();
            Assert.That(result?.Name, Is.EqualTo("Cheese"));
        }

        [Test]
        public async Task GetSingle_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/ingredient/{new Guid()}");
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            IngredientPost ingredient1 = new IngredientPost() { Name = "Cheese" };

            await _client.PostAsJsonAsync("/ingredient", ingredient1);

            var response = await _client.GetAsync("/ingredient");

            var result = await response.Content.ReadFromJsonAsync<List<IngredientGet>>();
            Assert.That(result?.Count == 1);

            // Act
            response = await _client.DeleteAsync($"/ingredient/{result?[0].Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/ingredient");
            result = await response.Content.ReadFromJsonAsync<List<IngredientGet>>();
            Assert.That(result?.Count == 0);
        }

        [Test]
        public async Task Delete_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/ingredient/{new Guid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
