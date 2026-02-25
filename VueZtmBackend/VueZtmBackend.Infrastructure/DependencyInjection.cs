using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VueZtmBackend.Application.Common.Interfaces;
using VueZtmBackend.Domain.Interfaces;
using VueZtmBackend.Infrastructure.Persistence;
using VueZtmBackend.Infrastructure.Persistence.Repositories;
using VueZtmBackend.Infrastructure.Services;

namespace VueZtmBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=vueztm.db";
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserStopRepository, UserStopRepository>();

        // Services
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();

        // ZTM API Service with HttpClient
        services.AddHttpClient<IZtmApiService, ZtmApiService>(client =>
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Memory Cache
        services.AddMemoryCache();

        return services;
    }
}

