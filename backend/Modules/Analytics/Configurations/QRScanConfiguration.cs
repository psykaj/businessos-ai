using backend.Modules.Analytics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Analytics.Configurations;

public class QRScanConfiguration : IEntityTypeConfiguration<QRScan>
{
    public void Configure(EntityTypeBuilder<QRScan> builder)
    {
        builder.HasKey(q => q.Id);

        builder.HasOne(q => q.Organization)
            .WithMany()
            .HasForeignKey(q => q.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.QRCode)
            .WithMany()
            .HasForeignKey(q => q.QRCodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(q => q.IPAddress).HasMaxLength(45); // IPv6 max length
        builder.Property(q => q.UserAgent).HasMaxLength(1000);
        builder.Property(q => q.DeviceType).HasMaxLength(50);
        builder.Property(q => q.Browser).HasMaxLength(100);
        builder.Property(q => q.BrowserVersion).HasMaxLength(50);
        builder.Property(q => q.OperatingSystem).HasMaxLength(100);
        builder.Property(q => q.OperatingSystemVersion).HasMaxLength(50);
        builder.Property(q => q.Country).HasMaxLength(100);
        builder.Property(q => q.State).HasMaxLength(100);
        builder.Property(q => q.City).HasMaxLength(100);
        builder.Property(q => q.TimeZone).HasMaxLength(100);
        builder.Property(q => q.Referrer).HasMaxLength(2000);
        builder.Property(q => q.UTMSource).HasMaxLength(200);
        builder.Property(q => q.UTMMedium).HasMaxLength(200);
        builder.Property(q => q.UTMCampaign).HasMaxLength(200);
        builder.Property(q => q.UTMTerm).HasMaxLength(200);
        builder.Property(q => q.UTMContent).HasMaxLength(200);
        builder.Property(q => q.Language).HasMaxLength(50);

        // Indexes for analytics workloads
        builder.HasIndex(q => q.OrganizationId);
        builder.HasIndex(q => q.QRCodeId);
        builder.HasIndex(q => q.ScanDateTime);
        builder.HasIndex(q => q.Country);
        builder.HasIndex(q => q.DeviceType);
        builder.HasIndex(q => q.Browser);
    }
}
