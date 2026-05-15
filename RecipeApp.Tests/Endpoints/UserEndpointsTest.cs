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
    public class UserEndpointsTest
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
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };

            // Act
            var response = await _client.PostAsJsonAsync("/user", user);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<UserGet>();
            Assert.That(result?.Name, Is.EqualTo("Jesse"));
            Assert.That(result?.PasswordHash, Is.EqualTo("123"));
        }

        [Test]
        public async Task GetAll_ReturnsMultipleElements()
        {
            // Arrange
            UserPost user1 = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            UserPost user2 = new UserPost() { Name = "Walter", PasswordHash = "123" };

            await _client.PostAsJsonAsync("/user", user1);
            await _client.PostAsJsonAsync("/user", user2);

            // Act
            var response = await _client.GetAsync("/user");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<UserGet>>();
            Assert.That(result?.Count == 2);
            Assert.That(result?[0].Name, Is.EqualTo("Jesse"));
            Assert.That(result?[1].Name, Is.EqualTo("Walter"));
        }

        [Test]
        public async Task GetSingle_ReturnsSingle()
        {
            // Arrange
            UserPost user1 = new UserPost() { Name = "Jesse", PasswordHash = "123" };

            var postResponse = await _client.PostAsJsonAsync("/user", user1);

            // Act
            var response = await _client.GetAsync($"/user/{postResponse.Content.ReadFromJsonAsync<UserGet>().Result.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<UserGet>();
            Assert.That(result?.Name, Is.EqualTo("Jesse"));
        }

        [Test]
        public async Task GetSingle_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/user/{new Guid()}");
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Update_ReturnsValue()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var response = await _client.PostAsJsonAsync("/user", user);

            var result = await response.Content.ReadFromJsonAsync<UserGet>();
            Assert.That(result?.Name, Is.EqualTo("Jesse"));

            // Act
            response = await _client.PutAsJsonAsync($"/user/{result?.Id}", new UserPost() { Name = "Pinkman" });
            result = await response.Content.ReadFromJsonAsync<UserGet>();

            // Assert
            Assert.That(result?.Name, Is.EqualTo("Pinkman"));
            Assert.That(result?.PasswordHash, Is.EqualTo("123"));
        }

        [Test]
        public async Task Update_ReturnsNotFound()
        {
            // Act
            var response = await _client.PutAsJsonAsync($"/user/{new Guid()}", new UserPost() { Name = "Pinkman" });

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            UserPost user1 = new UserPost() { Name = "Jesse", PasswordHash = "123" };

            await _client.PostAsJsonAsync("/user", user1);

            var response = await _client.GetAsync("/user");

            var result = await response.Content.ReadFromJsonAsync<List<UserGet>>();
            Assert.That(result?.Count == 1);

            // Act
            response = await _client.DeleteAsync($"/user/{result?[0].Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/user");
            result = await response.Content.ReadFromJsonAsync<List<UserGet>>();
            Assert.That(result?.Count == 0);
        }

        [Test]
        public async Task Delete_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/user/{new Guid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task InsertFavorites_ReturnFavorites()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var response = await _client.PostAsJsonAsync("/user", user);
            var userResult = await response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipe1Response = await _client.PostAsJsonAsync("recipe", recipe1);
            var recipe1Result = await recipe1Response.Content.ReadFromJsonAsync<RecipeGet>();

            FavoritePost favorite1 = new FavoritePost() { UserId = userResult.Id, RecipeId = recipe1Result.Id };

            // Act
            var response1 = await _client.PostAsJsonAsync($"/user/{userResult?.Id}/AddFavorite", favorite1);
            Assert.That(response1.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result1 = await response1.Content.ReadFromJsonAsync<FavoriteGet>();

            // Assert
            Assert.That(result1?.UserId, Is.EqualTo(userResult?.Id));
            Assert.That(result1?.RecipeId, Is.EqualTo(recipe1Result?.Id));
        }

        [Test]
        public async Task InsertFavoriteAlreadyAdded_ReturnsBadRequest()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var response = await _client.PostAsJsonAsync("/user", user);
            var userResult = await response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipe1Response = await _client.PostAsJsonAsync("recipe", recipe1);
            var recipe1Result = await recipe1Response.Content.ReadFromJsonAsync<RecipeGet>();

            FavoritePost favorite1 = new FavoritePost() { UserId = userResult.Id, RecipeId = recipe1Result.Id };

            // Act
            var response1 = await _client.PostAsJsonAsync($"/user/{userResult?.Id}/AddFavorite", favorite1);
            Assert.That(response1.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            response1 = await _client.PostAsJsonAsync($"/user/{userResult?.Id}/AddFavorite", favorite1);

            // Assert
            Assert.That(response1.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var getResponse = await _client.GetAsync($"/user/{userResult?.Id}/GetFavorites");
            var getResult = await getResponse.Content.ReadFromJsonAsync<List<FavoriteGet>>();

            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getResult?.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetFavorites_ReturnFavoritesList()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var response = await _client.PostAsJsonAsync("/user", user);
            var userResult = await response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipe1Response = await _client.PostAsJsonAsync("recipe", recipe1);
            var recipe1Result = await recipe1Response.Content.ReadFromJsonAsync<RecipeGet>();

            RecipePost recipe2 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipe2Response = await _client.PostAsJsonAsync("recipe", recipe2);
            var recipe2Result = await recipe2Response.Content.ReadFromJsonAsync<RecipeGet>();

            FavoritePost favorite1 = new FavoritePost() { UserId = userResult.Id, RecipeId = recipe1Result.Id };
            FavoritePost favorite2 = new FavoritePost() { UserId = userResult.Id, RecipeId = recipe2Result.Id };

            // Act
            await _client.PostAsJsonAsync($"/user/{userResult?.Id}/AddFavorite", favorite1);
            await _client.PostAsJsonAsync($"/user/{userResult?.Id}/AddFavorite", favorite2);

            var getResponse = await _client.GetAsync($"/user/{userResult?.Id}/GetFavorites");
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getResult = await getResponse.Content.ReadFromJsonAsync<List<FavoriteGet>>();

            // Assert
            Assert.That(getResult?.Count, Is.EqualTo(2));
            Assert.That(getResult?[0].UserId, Is.EqualTo(userResult.Id));
            Assert.That(getResult?[0].RecipeId == recipe1Result.Id || getResult?[0].RecipeId == recipe2Result.Id);
        }

        [Test]
        public async Task DeleteFavorites_ReturnDeleted()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var response = await _client.PostAsJsonAsync("/user", user);
            var userResult = await response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipe1Response = await _client.PostAsJsonAsync("recipe", recipe1);
            var recipe1Result = await recipe1Response.Content.ReadFromJsonAsync<RecipeGet>();

            FavoritePost favorite1 = new FavoritePost() { UserId = userResult.Id, RecipeId = recipe1Result.Id };

            // Act
            await _client.PostAsJsonAsync($"/user/{userResult?.Id}/AddFavorite", favorite1);

            var getResponse = await _client.GetAsync($"/user/{userResult?.Id}/GetFavorites");
            var getResult = await getResponse.Content.ReadFromJsonAsync<List<FavoriteGet>>();

            Assert.That(getResult?.Count, Is.EqualTo(1));

            var deleteResponse = await _client.DeleteAsync($"/user/{userResult?.Id}/RemoveFavorite/{recipe1Result.Id}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            getResponse = await _client.GetAsync($"/user/{userResult?.Id}/GetFavorites");
            getResult = await getResponse.Content.ReadFromJsonAsync<List<FavoriteGet>>();

            // Assert
            Assert.That(getResult?.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteFavorites_ReturnUserNotFound()
        {
            // Act
            var deleteResponse = await _client.DeleteAsync($"/user/{new Guid()}/RemoveFavorite/{new Guid()}");

            // Assert
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task DeleteFavorites_ReturnRecipeNotFound()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var response = await _client.PostAsJsonAsync("/user", user);
            var userResult = await response.Content.ReadFromJsonAsync<UserGet>();

            // Act
            var deleteResponse = await _client.DeleteAsync($"/user/{userResult?.Id}/RemoveFavorite/{new Guid()}");

            // Assert
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
