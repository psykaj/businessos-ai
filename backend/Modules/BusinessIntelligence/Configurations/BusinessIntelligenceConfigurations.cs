using backend.Configurations;
using backend.Modules.BusinessIntelligence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.BusinessIntelligence.Configurations;

public class KPIConfiguration : BaseEntityConfiguration<KPI>
{
    public override void Configure(EntityTypeBuilder<KPI> builder)
    {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
        builder.HasIndex(e => new { e.OrganizationId, e.Name });
        builder.Property(e => e.Name).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Category).IsRequired().HasMaxLength(100);
        builder.Property(e => e.CurrentValue).HasPrecision(18, 2);
        builder.Property(e => e.TargetValue).HasPrecision(18, 2);
    }
}

public class GoalConfiguration : BaseEntityConfiguration<Goal>
{
    public override void Configure(EntityTypeBuilder<Goal> builder)
    {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.TargetValue).HasPrecision(18, 2);
        builder.Property(e => e.CurrentValue).HasPrecision(18, 2);
        builder.Property(e => e.Status).IsRequired().HasMaxLength(50);
    }
}

public class ReportConfiguration : BaseEntityConfiguration<Report>
{
    public override void Configure(EntityTypeBuilder<Report> builder)
    {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.ReportType).IsRequired().HasMaxLength(100);
    }
}

public class InsightConfiguration : BaseEntityConfiguration<Insight>
{
    public override void Configure(EntityTypeBuilder<Insight> builder)
    {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
        builder.Property(e => e.Category).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(250);
        builder.Property(e => e.Priority).IsRequired().HasMaxLength(50);
    }
}

public class ForecastConfiguration : BaseEntityConfiguration<Forecast>
{
    public override void Configure(EntityTypeBuilder<Forecast> builder)
    {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
        builder.HasIndex(e => new { e.OrganizationId, e.ForecastType, e.ForecastDate });
        builder.Property(e => e.ForecastType).IsRequired().HasMaxLength(100);
        builder.Property(e => e.PredictedValue).HasPrecision(18, 2);
    }
}
