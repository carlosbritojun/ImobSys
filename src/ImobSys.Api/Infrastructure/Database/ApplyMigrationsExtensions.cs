using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Infrastructure.Database;

public static class ApplyMigrationsExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app, bool dropDatabase = false)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ImobSysDbContext>();

        if (dropDatabase)
        {
            db.Database.EnsureDeleted();
        }

        db.Database.Migrate();
    }
}
