using backend.Modules.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Workflow.Configurations;

public class IntegrationConfiguration : IEntityTypeConfiguration<Integration>
{
    public void Configure(EntityTypeBuilder<Integration> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasIndex(i => i.OrganizationId);
        builder.Property(i => i.Provider).HasConversion<string>();
        builder.Property(i => i.Status).HasConversion<string>();
        builder.Property(i => i.DisplayName).IsRequired().HasMaxLength(150);
    }
}
