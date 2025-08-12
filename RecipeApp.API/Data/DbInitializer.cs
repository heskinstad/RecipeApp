using RecipeApp.API.Models;
using System.Runtime.Intrinsics.X86;

namespace RecipeApp.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(RecipeContext context)
        {
            context.Database.EnsureCreated();

            User user1 = new User() { Username = "Carl", PasswordHash = "string" };
            User user2 = new User() { Username = "Chris", PasswordHash = "string" };

            Unit unit1 = new Unit() { Name = "kg" };
            Unit unit2 = new Unit() { Name = "l" };

            Ingredient ingredient1 = new Ingredient() { Name = "Tomato" };
            Ingredient ingredient2 = new Ingredient() { Name = "Cheese" };

            Category cat1 = new Category() { Name = "Italian" };
            Category cat2 = new Category() { Name = "Mexican" };
            Category cat3 = new Category() { Name = "Indian" };

            if (context.Users.Any())
            {
                return;
            }

            if (!context.Users.Any())
            {
                context.Users.Add(user1);
                context.Users.Add(user2);
            }

            if (!context.Units.Any())
            {
                context.Units.Add(unit1);
                context.Units.Add(unit2);
            }

            if (!context.Ingredients.Any())
            {
                context.Ingredients.Add(ingredient1);
                context.Ingredients.Add(ingredient2);
            }

            if (!context.Categories.Any())
            {
                context.Categories.Add(cat1);
                context.Categories.Add(cat2);
                context.Categories.Add(cat3);
            }

            context.SaveChanges();

            Recipe recipe1 = new Recipe() { Name = "Spaghetti", Description = "  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum eget mi quis risus imperdiet scelerisque vitae at velit. Aenean aliquam tincidunt enim id viverra. Nullam non varius sem. Donec vestibulum lobortis massa sed consequat. Mauris placerat pretium sapien, nec ultrices erat elementum sit amet. Ut et risus erat. Duis vel risus nec eros dapibus tincidunt. Nunc laoreet blandit nulla, vehicula sagittis turpis ornare vel. Aliquam sit amet lectus aliquam, facilisis ex eu, sagittis est. Integer commodo mollis dui, vel egestas justo sodales nec. Proin rhoncus massa pharetra libero finibus tristique. Pellentesque non massa mattis, facilisis libero ut, lacinia odio. Ut maximus nisl ante, et lobortis nulla tempor condimentum. Etiam et ex eu tellus feugiat dapibus in at elit. Sed luctus feugiat blandit. Maecenas dictum, purus non pretium viverra, velit eros finibus elit, sed rhoncus tortor elit a est.  Donec vel placerat risus. Praesent sodales, est eget feugiat pretium, lorem tortor maximus eros, eu ullamcorper sem ipsum ut urna. Suspendisse a porta mauris. Etiam id mauris in felis fermentum convallis. Cras eleifend eu augue a lobortis. Donec posuere lectus non gravida bibendum. Nullam enim arcu, venenatis mattis erat semper, tincidunt laoreet ligula. In mauris nisi, ultricies sit amet commodo id, pulvinar vitae nunc. Quisque cursus nunc neque, sed ultrices sem semper convallis.  Praesent sit amet elementum risus. Morbi accumsan ullamcorper sapien vitae dapibus. Nunc facilisis nulla vel leo porta mattis. Nam lacinia dolor eget leo tempor pellentesque. Sed cursus finibus nisi, ac egestas dui molestie sit amet. Curabitur malesuada volutpat nulla, eu vestibulum mauris lobortis in. Etiam finibus dignissim est, in aliquet dolor. Integer ultrices ultricies nibh sed maximus. Nunc convallis arcu scelerisque, finibus lectus a, lacinia ante. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Morbi venenatis massa leo, sed sodales odio malesuada et. Maecenas ac urna in dui bibendum porta in non ante.  Quisque sit amet leo porta, auctor velit pulvinar, feugiat erat. Nam eros odio, molestie sit amet eros sit amet, posuere tempus augue. Curabitur sit amet fermentum diam. Nullam in nunc ac mauris luctus scelerisque. Cras at placerat sapien, ultricies accumsan orci. Etiam at purus vel ipsum eleifend dictum ac at lacus. Maecenas mi tellus, tristique eu tortor tincidunt, malesuada suscipit augue. Ut a est vel nisl semper pretium id quis metus. Aliquam in erat bibendum, condimentum ligula id, ultricies ligula. Donec sit amet nunc eget ante porta aliquam. Mauris mattis semper turpis, eget tempor lacus suscipit non. Quisque vitae bibendum ex, rutrum ultricies metus. Mauris ullamcorper eu lorem eget gravida.  Integer molestie ipsum turpis, sit amet ultrices nulla pellentesque at. Morbi vel gravida velit, et vulputate velit. Nullam lacinia nulla tellus, at fringilla arcu tempor id. Nunc arcu mauris, sodales eget sollicitudin eget, ullamcorper eget nibh. Proin lobortis purus ut nulla elementum tristique. Quisque sit amet felis ac enim ultricies convallis. Nam vulputate pulvinar massa ac ultrices. Phasellus non libero hendrerit, consequat lorem accumsan, dapibus urna. Maecenas justo nunc, semper vel felis sit amet, tincidunt aliquet enim. Donec venenatis pretium lacus non pulvinar. ", CategoryId = cat1.Id, ImagePath = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fstatic.vecteezy.com%2Fsystem%2Fresources%2Fpreviews%2F029%2F889%2F981%2Fnon_2x%2Fmissing-image-flat-illustration-no-image-available-concept-vector.jpg&f=1&nofb=1&ipt=15692cce88c575b281208084be2a9b4308b8e4544a62a2736734275349545bf9", UploaderId = user1.Id, Summary = "Donec vel placerat risus. Praesent sodales, est eget feugiat pretium, lorem tortor maximus eros, eu ullamcorper sem ipsum ut urna. Suspendisse a porta mauris. Etiam id mauris in felis fermentum convallis. Cras eleifend eu augue a lobortis. Donec posuere lectus non gravida bibendum." };
            Recipe recipe2 = new Recipe() { Name = "Pizza", Description = "  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum eget mi quis risus imperdiet scelerisque vitae at velit. Aenean aliquam tincidunt enim id viverra. Nullam non varius sem. Donec vestibulum lobortis massa sed consequat. Mauris placerat pretium sapien, nec ultrices erat elementum sit amet. Ut et risus erat. Duis vel risus nec eros dapibus tincidunt. Nunc laoreet blandit nulla, vehicula sagittis turpis ornare vel. Aliquam sit amet lectus aliquam, facilisis ex eu, sagittis est. Integer commodo mollis dui, vel egestas justo sodales nec. Proin rhoncus massa pharetra libero finibus tristique. Pellentesque non massa mattis, facilisis libero ut, lacinia odio. Ut maximus nisl ante, et lobortis nulla tempor condimentum. Etiam et ex eu tellus feugiat dapibus in at elit. Sed luctus feugiat blandit. Maecenas dictum, purus non pretium viverra, velit eros finibus elit, sed rhoncus tortor elit a est.  Donec vel placerat risus. Praesent sodales, est eget feugiat pretium, lorem tortor maximus eros, eu ullamcorper sem ipsum ut urna. Suspendisse a porta mauris. Etiam id mauris in felis fermentum convallis. Cras eleifend eu augue a lobortis. Donec posuere lectus non gravida bibendum. Nullam enim arcu, venenatis mattis erat semper, tincidunt laoreet ligula. In mauris nisi, ultricies sit amet commodo id, pulvinar vitae nunc. Quisque cursus nunc neque, sed ultrices sem semper convallis.  Praesent sit amet elementum risus. Morbi accumsan ullamcorper sapien vitae dapibus. Nunc facilisis nulla vel leo porta mattis. Nam lacinia dolor eget leo tempor pellentesque. Sed cursus finibus nisi, ac egestas dui molestie sit amet. Curabitur malesuada volutpat nulla, eu vestibulum mauris lobortis in. Etiam finibus dignissim est, in aliquet dolor. Integer ultrices ultricies nibh sed maximus. Nunc convallis arcu scelerisque, finibus lectus a, lacinia ante. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Morbi venenatis massa leo, sed sodales odio malesuada et. Maecenas ac urna in dui bibendum porta in non ante.  Quisque sit amet leo porta, auctor velit pulvinar, feugiat erat. Nam eros odio, molestie sit amet eros sit amet, posuere tempus augue. Curabitur sit amet fermentum diam. Nullam in nunc ac mauris luctus scelerisque. Cras at placerat sapien, ultricies accumsan orci. Etiam at purus vel ipsum eleifend dictum ac at lacus. Maecenas mi tellus, tristique eu tortor tincidunt, malesuada suscipit augue. Ut a est vel nisl semper pretium id quis metus. Aliquam in erat bibendum, condimentum ligula id, ultricies ligula. Donec sit amet nunc eget ante porta aliquam. Mauris mattis semper turpis, eget tempor lacus suscipit non. Quisque vitae bibendum ex, rutrum ultricies metus. Mauris ullamcorper eu lorem eget gravida.  Integer molestie ipsum turpis, sit amet ultrices nulla pellentesque at. Morbi vel gravida velit, et vulputate velit. Nullam lacinia nulla tellus, at fringilla arcu tempor id. Nunc arcu mauris, sodales eget sollicitudin eget, ullamcorper eget nibh. Proin lobortis purus ut nulla elementum tristique. Quisque sit amet felis ac enim ultricies convallis. Nam vulputate pulvinar massa ac ultrices. Phasellus non libero hendrerit, consequat lorem accumsan, dapibus urna. Maecenas justo nunc, semper vel felis sit amet, tincidunt aliquet enim. Donec venenatis pretium lacus non pulvinar. ", CategoryId = cat1.Id, ImagePath = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fstatic.vecteezy.com%2Fsystem%2Fresources%2Fpreviews%2F029%2F889%2F981%2Fnon_2x%2Fmissing-image-flat-illustration-no-image-available-concept-vector.jpg&f=1&nofb=1&ipt=15692cce88c575b281208084be2a9b4308b8e4544a62a2736734275349545bf9", UploaderId = user2.Id, Summary = "Donec vel placerat risus. Praesent sodales, est eget feugiat pretium, lorem tortor maximus eros, eu ullamcorper sem ipsum ut urna. Suspendisse a porta mauris. Etiam id mauris in felis fermentum convallis. Cras eleifend eu augue a lobortis. Donec posuere lectus non gravida bibendum." };
            Recipe recipe3 = new Recipe() { Name = "Taco", Description = "  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum eget mi quis risus imperdiet scelerisque vitae at velit. Aenean aliquam tincidunt enim id viverra. Nullam non varius sem. Donec vestibulum lobortis massa sed consequat. Mauris placerat pretium sapien, nec ultrices erat elementum sit amet. Ut et risus erat. Duis vel risus nec eros dapibus tincidunt. Nunc laoreet blandit nulla, vehicula sagittis turpis ornare vel. Aliquam sit amet lectus aliquam, facilisis ex eu, sagittis est. Integer commodo mollis dui, vel egestas justo sodales nec. Proin rhoncus massa pharetra libero finibus tristique. Pellentesque non massa mattis, facilisis libero ut, lacinia odio. Ut maximus nisl ante, et lobortis nulla tempor condimentum. Etiam et ex eu tellus feugiat dapibus in at elit. Sed luctus feugiat blandit. Maecenas dictum, purus non pretium viverra, velit eros finibus elit, sed rhoncus tortor elit a est.  Donec vel placerat risus. Praesent sodales, est eget feugiat pretium, lorem tortor maximus eros, eu ullamcorper sem ipsum ut urna. Suspendisse a porta mauris. Etiam id mauris in felis fermentum convallis. Cras eleifend eu augue a lobortis. Donec posuere lectus non gravida bibendum. Nullam enim arcu, venenatis mattis erat semper, tincidunt laoreet ligula. In mauris nisi, ultricies sit amet commodo id, pulvinar vitae nunc. Quisque cursus nunc neque, sed ultrices sem semper convallis.  Praesent sit amet elementum risus. Morbi accumsan ullamcorper sapien vitae dapibus. Nunc facilisis nulla vel leo porta mattis. Nam lacinia dolor eget leo tempor pellentesque. Sed cursus finibus nisi, ac egestas dui molestie sit amet. Curabitur malesuada volutpat nulla, eu vestibulum mauris lobortis in. Etiam finibus dignissim est, in aliquet dolor. Integer ultrices ultricies nibh sed maximus. Nunc convallis arcu scelerisque, finibus lectus a, lacinia ante. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Morbi venenatis massa leo, sed sodales odio malesuada et. Maecenas ac urna in dui bibendum porta in non ante.  Quisque sit amet leo porta, auctor velit pulvinar, feugiat erat. Nam eros odio, molestie sit amet eros sit amet, posuere tempus augue. Curabitur sit amet fermentum diam. Nullam in nunc ac mauris luctus scelerisque. Cras at placerat sapien, ultricies accumsan orci. Etiam at purus vel ipsum eleifend dictum ac at lacus. Maecenas mi tellus, tristique eu tortor tincidunt, malesuada suscipit augue. Ut a est vel nisl semper pretium id quis metus. Aliquam in erat bibendum, condimentum ligula id, ultricies ligula. Donec sit amet nunc eget ante porta aliquam. Mauris mattis semper turpis, eget tempor lacus suscipit non. Quisque vitae bibendum ex, rutrum ultricies metus. Mauris ullamcorper eu lorem eget gravida.  Integer molestie ipsum turpis, sit amet ultrices nulla pellentesque at. Morbi vel gravida velit, et vulputate velit. Nullam lacinia nulla tellus, at fringilla arcu tempor id. Nunc arcu mauris, sodales eget sollicitudin eget, ullamcorper eget nibh. Proin lobortis purus ut nulla elementum tristique. Quisque sit amet felis ac enim ultricies convallis. Nam vulputate pulvinar massa ac ultrices. Phasellus non libero hendrerit, consequat lorem accumsan, dapibus urna. Maecenas justo nunc, semper vel felis sit amet, tincidunt aliquet enim. Donec venenatis pretium lacus non pulvinar. ", CategoryId = cat2.Id, ImagePath = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fstatic.vecteezy.com%2Fsystem%2Fresources%2Fpreviews%2F029%2F889%2F981%2Fnon_2x%2Fmissing-image-flat-illustration-no-image-available-concept-vector.jpg&f=1&nofb=1&ipt=15692cce88c575b281208084be2a9b4308b8e4544a62a2736734275349545bf9", UploaderId = user1.Id, Summary = "Donec vel placerat risus. Praesent sodales, est eget feugiat pretium, lorem tortor maximus eros, eu ullamcorper sem ipsum ut urna. Suspendisse a porta mauris. Etiam id mauris in felis fermentum convallis. Cras eleifend eu augue a lobortis. Donec posuere lectus non gravida bibendum." };

            if (!context.Recipes.Any())
            {
                context.Recipes.Add(recipe1);
                context.Recipes.Add(recipe2);
                context.Recipes.Add(recipe3);
            }

            context.SaveChanges();

            RecipeIngredients ri1 = new RecipeIngredients() { RecipeId = recipe1.Id, IngredientId = ingredient1.Id, Amount = 4, UnitId = unit1.Id };
            RecipeIngredients ri2 = new RecipeIngredients() { RecipeId = recipe1.Id, IngredientId = ingredient1.Id, Amount = 2, UnitId = unit2.Id };

            Favorites fav1 = new Favorites() { UserId = user1.Id, RecipeId = recipe1.Id };

            Rating rating1 = new Rating() { UserId = user1.Id, RecipeId = recipe1.Id, Score = 2 };
            Rating rating2 = new Rating() { UserId = user2.Id, RecipeId = recipe1.Id, Score = 5 };
            Rating rating3 = new Rating() { UserId = user1.Id, RecipeId = recipe2.Id, Score = 1 };

            UserComment userComment1 = new UserComment() { UserId = user1.Id, RecipeId = recipe1.Id, message = "Mamma mia this is better than mama's spaghetti-a!" };

            if (!context.RecipeIngredients.Any())
            {
                context.RecipeIngredients.Add(ri1);
                context.RecipeIngredients.Add(ri2);
            }

            if (!context.Favorites.Any())
            {
                context.Favorites.Add(fav1);
            }

            if (!context.Ratings.Any())
            {
                context.Ratings.Add(rating1);
                context.Ratings.Add(rating2);
                context.Ratings.Add(rating3);
            }

            if (!context.UserComments.Any())
            {
                context.UserComments.Add(userComment1);
            }

            context.SaveChanges();
        }
    }
}
