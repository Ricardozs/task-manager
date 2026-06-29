using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Auth;

namespace TaskManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        return services;
    }
}
