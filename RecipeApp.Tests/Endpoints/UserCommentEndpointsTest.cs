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
    public class UserCommentEndpointsTest
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
            UserCommentPost userComment = new UserCommentPost() { Message = "Wowie!" };

            // Act
            var response = await _client.PostAsJsonAsync("/userComment", userComment);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<UserCommentGet>();
            Assert.That(result?.Message, Is.EqualTo("Wowie!"));
        }

        [Test]
        public async Task GetAll_ReturnsMultipleElements()
        {
            // Arrange
            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!" };
            UserCommentPost userComment2 = new UserCommentPost() { Message = "ME LIKE!" };

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
            UserPost user1 = new UserPost() { Name = "John Doe", PasswordHash = "password123" };
            var userPostResponse = await _client.PostAsJsonAsync("/user", user1);
            var userResult = await userPostResponse.Content.ReadFromJsonAsync<UserGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Spaghetti" };
            var recipePostResponse = await _client.PostAsJsonAsync("recipe", recipe1);
            var recipeResult = await recipePostResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost()
            {
                UserId = userResult.Id,
                RecipeId = recipeResult.Id,
                Message = "Wowie!"
            };

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
            UserCommentPost userComment1 = new UserCommentPost() { Message = "Wowie!" };

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
            UserPost user1 = new UserPost() { Name = "John Doe", PasswordHash = "password123" };
            var userPostResponse = await _client.PostAsJsonAsync("/user", user1);
            var userResult = await userPostResponse.Content.ReadFromJsonAsync<UserGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Spaghetti" };
            var recipePostResponse = await _client.PostAsJsonAsync("recipe", recipe1);
            var recipeResult = await recipePostResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost()
            {
                UserId = userResult.Id,
                RecipeId = recipeResult.Id,
                Message = "Wowie!"
            };

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
            UserPost user1 = new UserPost() { Name = "John Doe", PasswordHash = "password123" };
            var userPostResponse = await _client.PostAsJsonAsync("/user", user1);
            var userResult = await userPostResponse.Content.ReadFromJsonAsync<UserGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Spaghetti" };
            var recipePostResponse = await _client.PostAsJsonAsync("recipe", recipe1);
            var recipeResult = await recipePostResponse.Content.ReadFromJsonAsync<RecipeGet>();

            UserCommentPost userComment1 = new UserCommentPost()
            {
                UserId = userResult.Id,
                RecipeId = recipeResult.Id,
                Message = "Wowie!"
            };

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
