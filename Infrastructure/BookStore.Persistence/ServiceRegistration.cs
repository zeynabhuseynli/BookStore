using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Application.Interfaces.IManagers.Helper;
using BookStore.Persistence.Data;
using BookStore.Persistence.Managers;
using BookStore.Persistence.Managers.Books;
using BookStore.Persistence.Managers.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Persistence;
public static class ServiceRegistration
{
    public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), options =>
            {
                options.MigrationsHistoryTable("__efmigrationshistory", "testing");
                options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), new List<string>());
            });
        });

        services.AddScoped(typeof(IBaseManager<>), typeof(BaseManager<>));
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IClaimManager, ClaimManager>();
        services.AddScoped<IAuthorManager, AuthorManager>();
        services.AddScoped<ICategoryManager, CategoryManager>();
        services.AddScoped<IBookManager, BookManager>();
        services.AddScoped<IBookFileManager, BookFileManager>();
        services.AddScoped<IReviewManager, ReviewManager>();

        services.AddTransient<IEmailManager, EmailManager>();
    }
}

