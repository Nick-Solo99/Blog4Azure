using BlogApp.Models;

namespace BlogApp.Data;

public class ArticleSeedData {
    public static async Task Initialize(ApplicationDbContext context) {
        await context.Database.EnsureCreatedAsync();
        
        if (!context.Articles.Any())
        {
            context.Articles.AddRange(
                new Article
                {
                    Title = "First Article",
                    Body = "This is the Body of the First Article. The Body will be larger than 100 characters to display the requirements.",
                    CreateDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    ContributorEmail = "c@c.c"
                },
                new Article
                {
                    Title = "Second Article",
                    Body = "This is the Body of the Second Article. The Body will be larger than 100 characters to display the requirements.",
                    CreateDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    ContributorEmail = "c@c.c"
                });
            await context.SaveChangesAsync();
        }

        
    }
}