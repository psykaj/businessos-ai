using backend.Modules.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Workflow.Configurations;

public class WorkflowConfiguration : IEntityTypeConfiguration<Entities.Workflow>
{
    public void Configure(EntityTypeBuilder<Entities.Workflow> builder)
    {
        builder.HasKey(w => w.Id);
        builder.HasIndex(w => w.OrganizationId);
        builder.Property(w => w.Name).IsRequired().HasMaxLength(200);
        builder.Property(w => w.Description).HasMaxLength(1000);
        builder.Property(w => w.Status).HasConversion<string>();

        builder.HasOne(w => w.Trigger)
            .WithOne(t => t.Workflow)
            .HasForeignKey<WorkflowTrigger>(t => t.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.Actions)
            .WithOne(a => a.Workflow)
            .HasForeignKey(a => a.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.Conditions)
            .WithOne(c => c.Workflow)
            .HasForeignKey(c => c.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.Executions)
            .WithOne(e => e.Workflow)
            .HasForeignKey(e => e.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
