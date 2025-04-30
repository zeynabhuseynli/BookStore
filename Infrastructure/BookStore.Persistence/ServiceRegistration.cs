using BookStore.Application.Interfaces.IManagers;
using BookStore.Persistence.Data;
using BookStore.Persistence.Managers;
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
        services.AddScoped<IEmailManager, EmailManager>();
    }
}

