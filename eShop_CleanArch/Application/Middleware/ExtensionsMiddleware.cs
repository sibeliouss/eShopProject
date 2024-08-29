

using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Application.Middleware;

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
            //userManager User ile alakalı CRUD işlemleri dahil birçok işlemi içinde barındıran identity kütüphanesinden gelen bir service
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