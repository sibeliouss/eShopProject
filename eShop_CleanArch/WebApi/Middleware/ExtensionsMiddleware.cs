using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace WebApi.Middleware;

public static class ExtensionsMiddleware
{
    public static void AutoMigration(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var context = scoped.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
    }

    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
          
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>();
            if (!userManager.Users.Any())
            {
                userManager.CreateAsync(new()
                {
                    Email = "admin@admin.com",
                    UserName = "admin",
                    FirstName = "Sibel",
                    LastName = "Öztürk",
                    EmailConfirmed = true,
                }, "Passw0rd*").Wait();
            }
        }
    }
}