using backend.Modules.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Workflow.Configurations;

public class WorkflowConditionConfiguration : IEntityTypeConfiguration<WorkflowCondition>
{
    public void Configure(EntityTypeBuilder<WorkflowCondition> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Operator).HasConversion<string>();
        builder.Property(c => c.LogicalOperator).HasConversion<string>();
        builder.Property(c => c.FieldName).IsRequired().HasMaxLength(100);
    }
}
