using ImobSys.Api.Infrastructure.Database.Seeds;
using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Infrastructure.Database;

public sealed class ImobSysDbContext : DbContext
{
    public ImobSysDbContext(DbContextOptions<ImobSysDbContext> options)
        : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyType> PropertiesTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImobSysDbContext).Assembly);
        modelBuilder.Seed();
    }
}

