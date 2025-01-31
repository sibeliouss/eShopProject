using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("eShopProject"));
        });
        #region Identity
//AddIdentity: Identity kütüphanesinin DbContext ile bağlı olduğunu bildirmek için:
        services.AddIdentity<User, AppRole>(opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.SignIn.RequireConfirmedEmail = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Sonra 20 dk olarak düzelt.
            opt.Lockout.MaxFailedAccessAttempts = 3;
            opt.Lockout.AllowedForNewUsers = true;

        }).AddEntityFrameworkStores<AppDbContext>();
        #endregion
        
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IBillingAddressRepository, BillingAddressRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
        services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWishListRepository, WishListRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

    } 
}