using Application.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<JwtService>();
        var assembly = typeof(ApplicationServiceRegistration).Assembly;
        
    }
}