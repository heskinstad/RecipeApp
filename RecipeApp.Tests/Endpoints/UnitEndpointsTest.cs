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

namespace RecipeApp.Tests.Endpoints
{
    [TestFixture]
    public class UnitEndpointstest
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
            UnitPost unit = new UnitPost() { Name = "Kg" };

            // Act
            var response = await _client.PostAsJsonAsync("/unit", unit);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<UnitGet>();
            Assert.That(result?.Name, Is.EqualTo("Kg"));
        }

        [Test]
        public async Task GetAll_ReturnsMultipleElements()
        {
            // Arrange
            UnitPost unit1 = new UnitPost() { Name = "Kg" };
            UnitPost unit2 = new UnitPost() { Name = "L" };

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
            UnitPost unit1 = new UnitPost() { Name = "Kg" };

            var postResponse = await _client.PostAsJsonAsync("/unit", unit1);

            // Act
            var response = await _client.GetAsync($"/unit/{postResponse.Content.ReadFromJsonAsync<UnitGet>().Result.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<UnitGet>();
            Assert.That(result?.Name, Is.EqualTo("Kg"));
        }

        [Test]
        public async Task GetSingle_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/unit/{new Guid()}");
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            UnitPost unit1 = new UnitPost() { Name = "Kg" };

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
