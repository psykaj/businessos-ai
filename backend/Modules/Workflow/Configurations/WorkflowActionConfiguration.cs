using backend.Modules.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Workflow.Configurations;

public class WorkflowActionConfiguration : IEntityTypeConfiguration<WorkflowAction>
{
    public void Configure(EntityTypeBuilder<WorkflowAction> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.ActionType).HasConversion<string>();
        builder.Property(a => a.Configuration).IsRequired();

        builder.HasMany(a => a.Conditions)
            .WithOne(c => c.WorkflowAction)
            .HasForeignKey(c => c.WorkflowActionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
