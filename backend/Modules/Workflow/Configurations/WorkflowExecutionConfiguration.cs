using backend.Modules.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Workflow.Configurations;

public class WorkflowExecutionConfiguration : IEntityTypeConfiguration<WorkflowExecution>
{
    public void Configure(EntityTypeBuilder<WorkflowExecution> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.OrganizationId);
        builder.HasIndex(e => e.WorkflowId);
        builder.Property(e => e.Status).HasConversion<string>();

        builder.HasMany(e => e.Logs)
            .WithOne(l => l.Execution)
            .HasForeignKey(l => l.ExecutionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
