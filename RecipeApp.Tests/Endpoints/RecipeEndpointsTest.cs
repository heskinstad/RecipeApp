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
    public class RecipeEndpointsTest
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

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };

            // Act
            var response = await _client.PostAsJsonAsync("/recipe", recipe);

            // Assert
            var errorDetails = await response.Content.ReadAsStringAsync();
            TestContext.WriteLine(errorDetails); // Prints exact SQL/EF Core failure logs

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<RecipeGet>();
            Assert.That(result?.Name, Is.EqualTo("Pizza"));
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

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe2 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };

            await _client.PostAsJsonAsync("/recipe", recipe1);
            await _client.PostAsJsonAsync("/recipe", recipe2);

            // Act
            var response = await _client.GetAsync("/recipe");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<RecipeGet>>();
            Assert.That(result?.Count, Is.EqualTo(2));
            Assert.That(result?[0].Name, Is.EqualTo("Pizza"));
            Assert.That(result?[1].Name, Is.EqualTo("Spaghetti"));
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

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };

            var postResponse = await _client.PostAsJsonAsync("/recipe", recipe1);

            // Act
            var response = await _client.GetAsync($"/recipe/{postResponse.Content.ReadFromJsonAsync<RecipeGet>().Result.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<RecipeGet>();
            Assert.That(result?.Name, Is.EqualTo("Pizza"));
        }

        [Test]
        public async Task GetSingle_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/recipe/{new Guid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Update_ReturnsValue()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "123", Description = "123", ImagePath = "123", CategoryId = categoryResult.Id, UploaderId = userResult.Id };

            var response = await _client.PostAsJsonAsync("/recipe", recipe1);

            var result = await response.Content.ReadFromJsonAsync<RecipeGet>();
            Assert.That(result?.Name, Is.EqualTo("Pizza"));

            // Act
            result.Name = "Spaghetti";

            response = await _client.PutAsJsonAsync($"/recipe/{result?.Id}", result);
            result = await response.Content.ReadFromJsonAsync<RecipeGet>();

            // Assert
            Assert.That(result?.Name, Is.EqualTo("Spaghetti"));
            Assert.That(result?.Summary, Is.EqualTo("123"));
            Assert.That(result?.Description, Is.EqualTo("123"));
            Assert.That(result?.ImagePath, Is.EqualTo("123"));
        }

        [Test]
        public async Task Update_ReturnsNotFound()
        {
            // Act
            var response = await _client.PutAsJsonAsync($"/recipe/{new Guid()}", new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "" });

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

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "123", Description = "123", ImagePath = "123", CategoryId = categoryResult.Id, UploaderId = userResult.Id };

            await _client.PostAsJsonAsync("/recipe", recipe1);

            var response = await _client.GetAsync("/recipe");

            var result = await response.Content.ReadFromJsonAsync<List<RecipeGet>>();
            Assert.That(result?.Count, Is.EqualTo(1));

            // Act
            response = await _client.DeleteAsync($"/recipe/{result?[0].Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync("/recipe");
            result = await response.Content.ReadFromJsonAsync<List<RecipeGet>>();
            Assert.That(result?.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Delete_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync($"/recipe/{new Guid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetRandom_ReturnsRandomRecipe()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe2 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            await _client.PostAsJsonAsync("/recipe", recipe1);
            await _client.PostAsJsonAsync("/recipe", recipe2);

            // Act
            var response = await _client.GetAsync("/recipe/random");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadFromJsonAsync<RecipeGet>();
            Assert.That(result?.Name == "Pizza" || result?.Name == "Spaghetti");
        }

        [Test]
        public async Task GetRandom_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/recipe/random");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetRandomMultiple_ReturnsMultipleRandomRecipes()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe2 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe3 = new RecipePost() { Name = "Taco", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            await _client.PostAsJsonAsync("/recipe", recipe1);
            await _client.PostAsJsonAsync("/recipe", recipe2);
            await _client.PostAsJsonAsync("/recipe", recipe3);

            // Act
            var response = await _client.GetAsync("/recipe/randomMultiple?count=2");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadFromJsonAsync<List<RecipeGet>>();
            Assert.That(result?.Count, Is.EqualTo(2));
            Assert.That(result?[0].Name == "Pizza" || result?[0].Name == "Spaghetti" || result?[0].Name == "Taco");
            Assert.That(result?[1].Name == "Pizza" || result?[1].Name == "Spaghetti" || result?[1].Name == "Taco");
            Assert.That(result?[0].Name != result?[1].Name);
        }

        [Test]
        public async Task GetRandomMultipleInvalidCount_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/recipe/randomMultiple?count=-1");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetRandomMultiple_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/recipe/randomMultiple?count=2");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetRandomMultipleCountExceedsTotal_ReturnsAllRecipes()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe2 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            await _client.PostAsJsonAsync("/recipe", recipe1);
            await _client.PostAsJsonAsync("/recipe", recipe2);

            // Act
            var response = await _client.GetAsync("/recipe/randomMultiple?count=5");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadFromJsonAsync<List<RecipeGet>>();
            Assert.That(result?.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Search_ReturnRecipes()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe2 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            await _client.PostAsJsonAsync("/recipe", recipe1);
            await _client.PostAsJsonAsync("/recipe", recipe2);

            // Act
            var response = await _client.GetAsync("/recipe/search?searchString=Pizza");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<RecipeGet>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items, Is.Not.Null);
            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.Items[0].Name, Is.EqualTo("Pizza"));
        }

        [Test]
        public async Task GetByCategory_ReturnRecipes()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            CategoryPost category2 = new CategoryPost() { Name = "Mexican" };
            var categoryResponse2 = await _client.PostAsJsonAsync("/category", category);
            var categoryResult2 = await categoryResponse2.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe1 = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe2 = new RecipePost() { Name = "Spaghetti", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            RecipePost recipe3 = new RecipePost() { Name = "Taco", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult2.Id, UploaderId = userResult.Id };

            await _client.PostAsJsonAsync("/recipe", recipe1);
            await _client.PostAsJsonAsync("/recipe", recipe2);
            await _client.PostAsJsonAsync("/recipe", recipe3);

            // Act
            var response = await _client.GetAsync($"/recipe/category?name=Italian");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadFromJsonAsync<List<RecipeGet>>();
            Assert.That(result?.Count, Is.EqualTo(2));
            Assert.That(result?[0].Name == "Pizza" || result?[0].Name == "Spaghetti");
        }
    }
}
