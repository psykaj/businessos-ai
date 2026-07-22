using backend.Modules.Workflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Workflow.Configurations;

public class WorkflowTriggerConfiguration : IEntityTypeConfiguration<WorkflowTrigger>
{
    public void Configure(EntityTypeBuilder<WorkflowTrigger> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.TriggerType).HasConversion<string>();
        builder.Property(t => t.TriggerConfiguration).IsRequired();
    }
}
