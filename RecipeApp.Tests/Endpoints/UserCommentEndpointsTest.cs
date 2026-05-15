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
    public class UserCommentEndpointsTest
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
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", UploaderId = userResult.Id, CategoryId = categoryResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!", RecipeId = recipeResult.Id, UserId = userResult.Id };

            await _client.PostAsJsonAsync("/userComment", userComment1);

            // Act
            var response = await _client.PostAsJsonAsync("/userComment", userComment1);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<UserCommentGet>();
            Assert.That(result?.Message, Is.EqualTo("Wowie!"));
        }

        [Test]
        public async Task GetAll_ReturnsMultipleElements()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", UploaderId = userResult.Id, CategoryId = categoryResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!", RecipeId = recipeResult.Id, UserId = userResult.Id };
            UserCommentPost userComment2 = new UserCommentPost() { Message = "ME LIKE!", RecipeId = recipeResult.Id, UserId = userResult.Id };

            await _client.PostAsJsonAsync("/userComment", userComment1);
            await _client.PostAsJsonAsync("/userComment", userComment2);

            // Act
            var response = await _client.GetAsync("/userComment");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<UserCommentGet>>();
            Assert.That(result?.Count == 2);
            Assert.That(result?[0].Message, Is.EqualTo("Wowie!"));
            Assert.That(result?[1].Message, Is.EqualTo("ME LIKE!"));
        }

        [Test]
        public async Task GetSingle_ReturnsSingle()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", UploaderId = userResult.Id, CategoryId = categoryResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!", RecipeId = recipeResult.Id, UserId = userResult.Id };

            await _client.PostAsJsonAsync("/userComment", userComment1);

            var postResponse = await _client.PostAsJsonAsync("/userComment", userComment1);
            var commentResult = await postResponse.Content.ReadFromJsonAsync<UserCommentGet>();

            // Act
            var response = await _client.GetAsync($"/userComment/{commentResult.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<UserCommentGet>();
            Assert.That(result?.Message, Is.EqualTo("Wowie!"));
            Assert.That(result?.UserId, Is.EqualTo(userResult.Id));
        }

        [Test]
        public async Task GetSingle_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/userComment/{new Guid()}");
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", UploaderId = userResult.Id, CategoryId = categoryResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!", RecipeId = recipeResult.Id, UserId = userResult.Id };

            await _client.PostAsJsonAsync("/userComment", userComment1);

            var response = await _client.GetAsync("/userComment");

            var result = await response.Content.ReadFromJsonAsync<List<UserCommentGet>>();
            Assert.That(result?.Count == 1);

            // Act
            response = await _client.DeleteAsync($"/userComment/{result?[0].Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/userComment");
            result = await response.Content.ReadFromJsonAsync<List<UserCommentGet>>();
            Assert.That(result?.Count == 0);
        }

        [Test]
        public async Task Delete_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/userComment/{new Guid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Upvote_ReturnsScore()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", UploaderId = userResult.Id, CategoryId = categoryResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!", RecipeId = recipeResult.Id, UserId = userResult.Id };

            await _client.PostAsJsonAsync("/userComment", userComment1);

            var postResponse = await _client.PostAsJsonAsync("/userComment", userComment1);
            var commentResult = await postResponse.Content.ReadFromJsonAsync<UserCommentGet>();

            // Act
            Assert.That(commentResult.Upvotes, Is.EqualTo(0));
            var response = await _client.PutAsync($"/userComment/{commentResult.Id}/upvote", null);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<int>();
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task Downvote_ReturnsScore()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", UploaderId = userResult.Id, CategoryId = categoryResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!", RecipeId = recipeResult.Id, UserId = userResult.Id };

            await _client.PostAsJsonAsync("/userComment", userComment1);

            var postResponse = await _client.PostAsJsonAsync("/userComment", userComment1);
            var commentResult = await postResponse.Content.ReadFromJsonAsync<UserCommentGet>();

            // Act
            Assert.That(commentResult.Downvotes, Is.EqualTo(0));
            var response = await _client.PutAsync($"/userComment/{commentResult.Id}/downvote", null);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<int>();
            Assert.That(result, Is.EqualTo(1));
        }
    }
}
