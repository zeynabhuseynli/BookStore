using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application;
public static class ServiceRegistration
{
    public static void AddApplicationRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        });

    }
}
