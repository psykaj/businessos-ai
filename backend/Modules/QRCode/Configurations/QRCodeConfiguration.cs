using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.QRCode.Configurations;

public class QRCodeConfiguration : IEntityTypeConfiguration<Models.QRCode>
{
    public void Configure(EntityTypeBuilder<Models.QRCode> builder)
    {
        builder.ToTable("QRCodes");

        builder.HasKey(q => q.Id);
        
        builder.HasQueryFilter(q => !q.IsDeleted);

        builder.Property(q => q.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(q => q.Description)
            .HasMaxLength(500);

        builder.Property(q => q.OriginalValue)
            .IsRequired();

        builder.Property(q => q.ShortCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(q => q.ForegroundColor)
            .HasMaxLength(7); // #FFFFFF

        builder.Property(q => q.BackgroundColor)
            .HasMaxLength(7);

        builder.Property(q => q.ErrorCorrectionLevel)
            .HasMaxLength(1);

        builder.HasIndex(q => q.OrganizationId);
        
        builder.HasIndex(q => q.ShortCode)
            .IsUnique();

        builder.HasOne(q => q.Organization)
            .WithMany()
            .HasForeignKey(q => q.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
