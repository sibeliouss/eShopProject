using System.Reflection;
using System.Text;
using Application.Features.Auth.Rules;
using Application.Features.Customers.Rules;
using Application.Services.Auth;
using Application.Services.Customers;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<JwtService>();
        var assembly = typeof(ApplicationServiceRegistration).Assembly;
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(typeof(ApplicationServiceRegistration).Assembly);
        services.AddScoped<AuthBusinessRules>();
        services.AddScoped<CustomerBusinessRules>();

        services.AddScoped<ICustomerService, CustomerService>();
        
        
        #region JWT
        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true, //tokeni gönderen kişi bilgisi
                ValidateAudience = true, // tokeni kullanacak site ya da kişi bilgisi
                ValidateIssuerSigningKey = true, //güvenlik anahtarı üretmesini sağlayan güvenlik sözcüğü
                ValidateLifetime = true, // tokenin yaşam süresini kontrol edilmesi
                ValidIssuer = "Sibel Öztürk",
                ValidAudience = "eShop Project",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKey"))
            };
        });
        #endregion
      
        
    }
}