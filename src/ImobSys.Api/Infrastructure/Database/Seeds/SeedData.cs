using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImobSys.Api.Infrastructure.Database.Seeds;

public static class SeedData
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        SeedPropertiesTypes(modelBuilder);
    }

    private static void SeedPropertiesTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyType>().HasData(
            new PropertyType(Guid.Parse("0366f790-41f6-476a-9780-e648f58b3eaf"), "Casa", "Casa"),
            new PropertyType(Guid.Parse("a10f2f8c-3bce-4e8f-b8bb-69ffb62a2b41"), "Casa duplex", "Casa duplex"),
            new PropertyType(Guid.Parse("5c39a928-2f9c-4d41-95f8-8ef46e9dc15c"), "Casa em condomínio", "Casa em condomínio fechado"),
            new PropertyType(Guid.Parse("4d84993a-4139-4265-a934-8952c20bbf56"), "Apartamento padrão", "Apartamento padrão"),
            new PropertyType(Guid.Parse("3cf4eb65-8052-48f9-9c49-2e09b83f053d"), "Apartamento duplex", "Apartamento duplex"),
            new PropertyType(Guid.Parse("df1b2717-84cd-496a-ae6f-5eaa601cc879"), "Cobertura", "Cobertura")
        );
    }
}
