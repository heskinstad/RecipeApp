using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.API.Data;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.DTO.POST;
using RecipeApp.API.Models;

namespace RecipeApp.Tests.Endpoints
{
    [TestFixture]
    public class CategoryEndpointsTest
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            string dbName = Guid.NewGuid().ToString();

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<RecipeContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<RecipeContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [TearDown]
        public void Dispose()
        {
            _factory?.Dispose();
            _client?.Dispose();
        }

        [Test]
        public async Task Insert_ReturnsValue()
        {
            // Arrange
            CategoryPost category = new CategoryPost() { Name = "Bri'ish" };

            // Act
            var response = await _client.PostAsJsonAsync("/category", category);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<CategoryGet>();
            Assert.That(result?.Name, Is.EqualTo("Bri'ish"));
        }

        [Test]
        public async Task GetAll_ReturnsMultipleElements()
        {
            // Arrange
            CategoryPost category1 = new CategoryPost() { Name = "Bri'ish" };
            CategoryPost category2 = new CategoryPost() { Name = "French" };

            await _client.PostAsJsonAsync("/category", category1);
            await _client.PostAsJsonAsync("/category", category2);

            // Act
            var response = await _client.GetAsync("/category");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<CategoryGet>>();
            Assert.That(result?.Count == 2);
            Assert.That(result?[0].Name, Is.EqualTo("Bri'ish"));
            Assert.That(result?[1].Name, Is.EqualTo("French"));
        }

        [Test]
        public async Task GetSingle_ReturnsSingle()
        {
            // Arrange
            CategoryPost category1 = new CategoryPost() { Name = "Bri'ish" };

            var postResponse = await _client.PostAsJsonAsync("/category", category1);

            // Act
            var response = await _client.GetAsync($"/category/{postResponse.Content.ReadFromJsonAsync<CategoryGet>().Result.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<CategoryGet>();
            Assert.That(result?.Name, Is.EqualTo("Bri'ish"));
        }

        [Test]
        public async Task GetSingle_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/category/{new Guid()}");
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            CategoryPost category1 = new CategoryPost() { Name = "Bri'ish" };

            await _client.PostAsJsonAsync("/category", category1);

            var response = await _client.GetAsync("/category");

            var result = await response.Content.ReadFromJsonAsync<List<CategoryGet>>();
            Assert.That(result?.Count == 1);

            // Act
            response = await _client.DeleteAsync($"/category/{result?[0].Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/category");
            result = await response.Content.ReadFromJsonAsync<List<CategoryGet>>();
            Assert.That(result?.Count == 0);
        }

        [Test]
        public async Task Delete_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/category/{new Guid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
