using backend.Modules.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Workflow.Configurations;

public class WorkflowExecutionLogConfiguration : IEntityTypeConfiguration<WorkflowExecutionLog>
{
    public void Configure(EntityTypeBuilder<WorkflowExecutionLog> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasIndex(l => l.ExecutionId);
        builder.HasIndex(l => l.WorkflowId);
        builder.Property(l => l.Status).HasConversion<string>();
        builder.Property(l => l.StepName).IsRequired().HasMaxLength(150);
    }
}
