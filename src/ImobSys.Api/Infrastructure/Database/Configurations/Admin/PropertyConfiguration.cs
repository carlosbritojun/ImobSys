using ImobSys.Api.Modules.Admin.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImobSys.Api.Infrastructure.Database.Configurations.Admin;

internal sealed class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("properties", "admin");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.TypeId)
            .IsRequired();

        builder.HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey(x => x.TypeId);

        builder.HasIndex(x => new { x.Name, x.Description });
    }
}
