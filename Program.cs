using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using BlogApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<BlogUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Contributor" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BlogUser>>();
    string adminEmail = "a@a.a";
    string contributorEmail = "c@c.c";
    string password = "P@$$w0rd";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new BlogUser
        {
            UserName = adminEmail, 
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "Admin"
        };
        
        await userManager.CreateAsync(adminUser, password);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    if (await userManager.FindByEmailAsync(contributorEmail) == null)
    {
        var contributorUser = new BlogUser
        {
            UserName = contributorEmail,
            Email = contributorEmail,
            FirstName = "Contributor",
            LastName = "Contributor"
        };
        
        await userManager.CreateAsync(contributorUser, password);
        await userManager.AddToRoleAsync(contributorUser, "Contributor");
    }
    
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

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
        context.SaveChanges();
    }
    
    
}
    

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
