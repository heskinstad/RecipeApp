using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.API.Data;
using RecipeApp.API.DTO.GET;
using RecipeApp.API.Models;

namespace RecipeApp.Tests.Endpoints
{
    [TestFixture]
    public class UnitEndpointstest
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
            Unit unit = new Unit() { Name = "Kg" };

            // Act
            var response = await _client.PostAsJsonAsync("/unit", unit);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<Unit>();
            Assert.That(result?.Name, Is.EqualTo("Kg"));
        }

        [Test]
        public async Task GetAll_ReturnsMultipleElements()
        {
            // Arrange
            Unit unit1 = new Unit() { Name = "Kg" };
            Unit unit2 = new Unit() { Name = "L" };

            await _client.PostAsJsonAsync("/unit", unit1);
            await _client.PostAsJsonAsync("/unit", unit2);

            // Act
            var response = await _client.GetAsync("/unit");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<UnitGet>>();
            Assert.That(result?.Count == 2);
            Assert.That(result?[0].Name, Is.EqualTo("Kg"));
            Assert.That(result?[1].Name, Is.EqualTo("L"));
        }

        [Test]
        public async Task GetSingle_ReturnsSingle()
        {
            // Arrange
            Unit unit1 = new Unit() { Name = "Kg" };

            var postResponse = await _client.PostAsJsonAsync("/unit", unit1);

            // Act
            var response = await _client.GetAsync($"/unit/{postResponse.Content.ReadFromJsonAsync<UnitGet>().Result.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<UnitGet>();
            Assert.That(result?.Name, Is.EqualTo("Kg"));
        }

        [Test]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            Unit unit1 = new Unit() { Name = "Kg" };

            await _client.PostAsJsonAsync("/unit", unit1);

            var response = await _client.GetAsync("/unit");

            var result = await response.Content.ReadFromJsonAsync<List<UnitGet>>();
            Assert.That(result?.Count == 1);

            // Act
            response = await _client.DeleteAsync($"/unit/{result?[0].Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/unit");
            result = await response.Content.ReadFromJsonAsync<List<UnitGet>>();
            Assert.That(result?.Count == 0);
        }

        [Test]
        public async Task Delete_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/unit/{new Guid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
