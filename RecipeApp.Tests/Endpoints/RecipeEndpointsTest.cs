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

        [Test]
        public async Task GetByCategoryInvalidCategory_ReturnsNoRecipes()
        {
            // Act
            var getResponse = await _client.GetAsync($"/recipe/category?name=NonExistentCategory");
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var getResult = await getResponse.Content.ReadFromJsonAsync<List<RecipeGet>>();

            // Assert
            Assert.That(getResult?.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task AddVisitor_ReturnsValue()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            // Act
            var getResponse = await _client.PutAsJsonAsync($"/recipe/{recipeResult.Id}/addVisitor", recipe);

            // Assert
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var getResult = await getResponse.Content.ReadFromJsonAsync<int>();

            // Assert
            Assert.That(getResult, Is.EqualTo(1));
        }

        [Test]
        public async Task InsertIngredient_ReturnsIngredient()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            IngredientPost ingredient = new IngredientPost() { Name = "Cheese" };
            var ingredientResponse = await _client.PostAsJsonAsync("/ingredient", ingredient);
            var ingredientResult = await ingredientResponse.Content.ReadFromJsonAsync<IngredientGet>();

            UnitPost unit = new UnitPost() { Name = "grams" };
            var unitResponse = await _client.PostAsJsonAsync("/unit", unit);
            var unitResult = await unitResponse.Content.ReadFromJsonAsync<UnitGet>();

            // Act
            var recipeIngredientsPost = new RecipeIngredientsPost() { RecipeId = recipeResult.Id, IngredientId = ingredientResult.Id, Amount = 100, UnitId = unitResult.Id, Section = "" };
            var response = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ingredients", recipeIngredientsPost);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var result = await response.Content.ReadFromJsonAsync<RecipeIngredientsGet>();

            // Assert
            Assert.That(result?.IngredientName, Is.EqualTo(ingredientResult.Name));
            Assert.That(result?.Amount, Is.EqualTo(100));
        }

        [Test]
        public async Task GetIngredients_ReturnIngredients()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            IngredientPost ingredient1 = new IngredientPost() { Name = "Cheese" };
            var ingredient1Response = await _client.PostAsJsonAsync("/ingredient", ingredient1);
            var ingredient1Result = await ingredient1Response.Content.ReadFromJsonAsync<IngredientGet>();

            IngredientPost ingredient2 = new IngredientPost() { Name = "Tomato" };
            var ingredient2Response = await _client.PostAsJsonAsync("/ingredient", ingredient2);
            var ingredient2Result = await ingredient2Response.Content.ReadFromJsonAsync<IngredientGet>();

            UnitPost unit = new UnitPost() { Name = "grams" };
            var unitResponse = await _client.PostAsJsonAsync("/unit", unit);
            var unitResult = await unitResponse.Content.ReadFromJsonAsync<UnitGet>();

            // Act
            var recipeIngredients1Post = new RecipeIngredientsPost() { RecipeId = recipeResult.Id, IngredientId = ingredient1Result.Id, Amount = 100, UnitId = unitResult.Id, Section = "" };
            var recipeIngredients1Response = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ingredients", recipeIngredients1Post);
            Assert.That(recipeIngredients1Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var recipeIngredients2Post = new RecipeIngredientsPost() { RecipeId = recipeResult.Id, IngredientId = ingredient2Result.Id, Amount = 1000, UnitId = unitResult.Id, Section = "" };
            var recipeIngredients2Response = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ingredients", recipeIngredients2Post);
            Assert.That(recipeIngredients2Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var response = await _client.GetAsync($"/recipe/{recipeResult.Id}/ingredients");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<RecipeIngredientsGet>>();

            // Assert
            Assert.That(result?.Count, Is.EqualTo(2));
            Assert.That(result?[0].IngredientName == "Cheese" || result?[1].IngredientName == "Tomato");
            Assert.That(result?[0].Amount == 100 || result?[0].Amount == 1000);
        }

        [Test]
        public async Task DeleteIngredient_ReturnsOK()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            IngredientPost ingredient1 = new IngredientPost() { Name = "Cheese" };
            var ingredient1Response = await _client.PostAsJsonAsync("/ingredient", ingredient1);
            var ingredient1Result = await ingredient1Response.Content.ReadFromJsonAsync<IngredientGet>();

            IngredientPost ingredient2 = new IngredientPost() { Name = "Tomato" };
            var ingredient2Response = await _client.PostAsJsonAsync("/ingredient", ingredient2);
            var ingredient2Result = await ingredient2Response.Content.ReadFromJsonAsync<IngredientGet>();

            UnitPost unit = new UnitPost() { Name = "grams" };
            var unitResponse = await _client.PostAsJsonAsync("/unit", unit);
            var unitResult = await unitResponse.Content.ReadFromJsonAsync<UnitGet>();

            // Act
            var recipeIngredients1Post = new RecipeIngredientsPost() { RecipeId = recipeResult.Id, IngredientId = ingredient1Result.Id, Amount = 100, UnitId = unitResult.Id, Section = "" };
            var recipeIngredients1Response = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ingredients", recipeIngredients1Post);
            Assert.That(recipeIngredients1Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var recipeIngredients2Post = new RecipeIngredientsPost() { RecipeId = recipeResult.Id, IngredientId = ingredient2Result.Id, Amount = 1000, UnitId = unitResult.Id, Section = "" };
            var recipeIngredients2Response = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ingredients", recipeIngredients2Post);
            Assert.That(recipeIngredients2Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var response = await _client.GetAsync($"/recipe/{recipeResult.Id}/ingredients");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Assert
            var result = await response.Content.ReadFromJsonAsync<List<RecipeIngredientsGet>>();
            Assert.That(result?.Count, Is.EqualTo(2));

            response = await _client.DeleteAsync($"/recipe/{recipeResult.Id}/ingredients/{ingredient1Result.Id}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = await _client.GetAsync($"/recipe/{recipeResult.Id}/ingredients");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            result = await response.Content.ReadFromJsonAsync<List<RecipeIngredientsGet>>();
            Assert.That(result?.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task InsertRating_GetNewAverageRating()
        {
            // Arrange
            UserPost user1 = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var user1Response = await _client.PostAsJsonAsync("/user", user1);
            var user1Result = await user1Response.Content.ReadFromJsonAsync<UserGet>();

            UserPost user2 = new UserPost() { Name = "Walter", PasswordHash = "123" };
            var user2Response = await _client.PostAsJsonAsync("/user", user2);
            var user2Result = await user2Response.Content.ReadFromJsonAsync<UserGet>();

            UserPost user3 = new UserPost() { Name = "Mike", PasswordHash = "123" };
            var user3Response = await _client.PostAsJsonAsync("/user", user3);
            var user3Result = await user3Response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = user1Result.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            // Act
            RatingPost rating1Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user1Result.Id, Score = 4 };
            var ratingResponse1 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating1Post);
            Assert.That(ratingResponse1.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult1 = await ratingResponse1.Content.ReadFromJsonAsync<float>();

            RatingPost rating2Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user2Result.Id, Score = 1 };
            var ratingResponse2 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating2Post);
            Assert.That(ratingResponse2.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult2 = await ratingResponse2.Content.ReadFromJsonAsync<float>();

            RatingPost rating3Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user3Result.Id, Score = 5 };
            var ratingResponse3 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating3Post);
            Assert.That(ratingResponse3.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult3 = await ratingResponse3.Content.ReadFromJsonAsync<float>();

            //Assert
            Assert.That(ratingResult1, Is.EqualTo((float)4));
            Assert.That(ratingResult2, Is.EqualTo((float)Math.Round((4f+1f)/2, 1)));
            Assert.That(ratingResult3, Is.EqualTo((float)Math.Round((4f+1f+5f)/3f, 1)));
        }

        [Test]
        public async Task GetRatings_ReturnRatings()
        {
            // Arrange
            UserPost user1 = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var user1Response = await _client.PostAsJsonAsync("/user", user1);
            var user1Result = await user1Response.Content.ReadFromJsonAsync<UserGet>();

            UserPost user2 = new UserPost() { Name = "Walter", PasswordHash = "123" };
            var user2Response = await _client.PostAsJsonAsync("/user", user2);
            var user2Result = await user2Response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = user1Result.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            // Act
            RatingPost rating1Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user1Result.Id, Score = 4 };
            var ratingResponse1 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating1Post);
            Assert.That(ratingResponse1.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult1 = await ratingResponse1.Content.ReadFromJsonAsync<float>();

            RatingPost rating2Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user2Result.Id, Score = 1 };
            var ratingResponse2 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating2Post);
            Assert.That(ratingResponse2.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult2 = await ratingResponse2.Content.ReadFromJsonAsync<float>();

            var ratingsResponse = await _client.GetAsync($"/recipe/{recipeResult.Id}/ratings");
            Assert.That(ratingsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var ratingsResult = await ratingsResponse.Content.ReadFromJsonAsync<List<RatingGet>>();

            //Assert
            Assert.That(ratingsResult.Count, Is.EqualTo(2));
            Assert.That(ratingsResult[0].Score == 1 || ratingsResult[0].Score == 4);
        }

        [Test]
        public async Task GetRatingsCount_ReturnRatingsCount()
        {
            // Arrange
            UserPost user1 = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var user1Response = await _client.PostAsJsonAsync("/user", user1);
            var user1Result = await user1Response.Content.ReadFromJsonAsync<UserGet>();

            UserPost user2 = new UserPost() { Name = "Walter", PasswordHash = "123" };
            var user2Response = await _client.PostAsJsonAsync("/user", user2);
            var user2Result = await user2Response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = user1Result.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            // Act
            RatingPost rating1Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user1Result.Id, Score = 4 };
            var ratingResponse1 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating1Post);
            Assert.That(ratingResponse1.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult1 = await ratingResponse1.Content.ReadFromJsonAsync<float>();

            RatingPost rating2Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user2Result.Id, Score = 1 };
            var ratingResponse2 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating2Post);
            Assert.That(ratingResponse2.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult2 = await ratingResponse2.Content.ReadFromJsonAsync<float>();

            var ratingsResponse = await _client.GetAsync($"/recipe/{recipeResult.Id}/ratingsCount");
            Assert.That(ratingsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            int ratingsResult = await ratingsResponse.Content.ReadFromJsonAsync<int>();

            //Assert
            Assert.That(ratingsResult, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAverageRating_ReturnAverageRating()
        {
            // Arrange
            UserPost user1 = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var user1Response = await _client.PostAsJsonAsync("/user", user1);
            var user1Result = await user1Response.Content.ReadFromJsonAsync<UserGet>();

            UserPost user2 = new UserPost() { Name = "Walter", PasswordHash = "123" };
            var user2Response = await _client.PostAsJsonAsync("/user", user2);
            var user2Result = await user2Response.Content.ReadFromJsonAsync<UserGet>();

            UserPost user3 = new UserPost() { Name = "Mike", PasswordHash = "123" };
            var user3Response = await _client.PostAsJsonAsync("/user", user3);
            var user3Result = await user3Response.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = user1Result.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            // Act
            RatingPost rating1Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user1Result.Id, Score = 4 };
            var ratingResponse1 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating1Post);
            Assert.That(ratingResponse1.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult1 = await ratingResponse1.Content.ReadFromJsonAsync<float>();

            RatingPost rating2Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user2Result.Id, Score = 1 };
            var ratingResponse2 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating2Post);
            Assert.That(ratingResponse2.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult2 = await ratingResponse2.Content.ReadFromJsonAsync<float>();

            RatingPost rating3Post = new RatingPost() { RecipeId = recipeResult.Id, UserId = user3Result.Id, Score = 5 };
            var ratingResponse3 = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/ratings", rating3Post);
            Assert.That(ratingResponse3.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            float ratingResult3 = await ratingResponse3.Content.ReadFromJsonAsync<float>();

            var ratingsResponse = await _client.GetAsync($"/recipe/{recipeResult.Id}/averageRating");
            Assert.That(ratingsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            float ratingsResult = await ratingsResponse.Content.ReadFromJsonAsync<float>();

            //Assert
            Assert.That(ratingsResult, Is.EqualTo((float)Math.Round((4f + 1f + 5f) / 3f, 1)));
        }

        [Test]
        public async Task InsertComment_ReturnComment()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            // Act
            UserCommentPost userComment = new UserCommentPost() { RecipeId = recipeResult.Id, UserId = userResult.Id, Message = "GOOD!" };
            var userCommentResponse = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/comments", userComment);
            Assert.That(userCommentResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var userCommentResult = await userCommentResponse.Content.ReadFromJsonAsync<UserCommentGet>();

            // Assert
            Assert.That(userCommentResult.UserId, Is.EqualTo(userResult.Id));
            Assert.That(userCommentResult.Message, Is.EqualTo("GOOD!"));
        }

        [Test]
        public async Task GetComments_ReturnComments()
        {
            // Arrange
            UserPost user = new UserPost() { Name = "Jesse", PasswordHash = "123" };
            var userResponse = await _client.PostAsJsonAsync("/user", user);
            var userResult = await userResponse.Content.ReadFromJsonAsync<UserGet>();

            CategoryPost category = new CategoryPost() { Name = "Italian" };
            var categoryResponse = await _client.PostAsJsonAsync("/category", category);
            var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CategoryGet>();

            RecipePost recipe = new RecipePost() { Name = "Pizza", Summary = "", Description = "", ImagePath = "", CategoryId = categoryResult.Id, UploaderId = userResult.Id };
            var recipeResponse = await _client.PostAsJsonAsync("/recipe", recipe);
            var recipeResult = await recipeResponse.Content.ReadFromJsonAsync<RecipeGet>();

            // Act
            UserCommentPost userComment1 = new UserCommentPost() { RecipeId = recipeResult.Id, UserId = userResult.Id, Message = "GOOD!" };
            var userComment1Response = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/comments", userComment1);
            Assert.That(userComment1Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var userComment1Result = await userComment1Response.Content.ReadFromJsonAsync<UserCommentGet>();

            UserCommentPost userComment2 = new UserCommentPost() { RecipeId = recipeResult.Id, UserId = userResult.Id, Message = "Amazing!" };
            var userComment2Response = await _client.PostAsJsonAsync($"/recipe/{recipeResult.Id}/comments", userComment2);
            Assert.That(userComment2Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var userComment2Result = await userComment2Response.Content.ReadFromJsonAsync<UserCommentGet>();

            var response = await _client.GetAsync($"/recipe/{recipeResult.Id}/comments");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadFromJsonAsync<List<UserCommentGet>>();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Message == "GOOD!" || result[0].Message == "Amazing");
        }
    }
}
