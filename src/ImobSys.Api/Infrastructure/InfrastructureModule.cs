using ImobSys.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ImobSys")
            ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ImobSysDbContext>((serviceProvider, options) =>
        {
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();

            var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            if (environment.IsDevelopment())
            {
                options
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information | LogLevel.Warning | LogLevel.Error);
            }
        });

        return services;
    }
}
