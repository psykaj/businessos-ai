using backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasQueryFilter(o => !o.IsDeleted);
        builder.HasIndex(o => o.Slug).IsUnique();
        builder.HasIndex(o => o.Email);
        builder.Property(o => o.Name).IsRequired().HasMaxLength(200);
        builder.Property(o => o.Slug).IsRequired().HasMaxLength(200);
        
        // One-to-Many Organization -> Users
        builder.HasMany(o => o.Users)
               .WithOne(u => u.Organization)
               .HasForeignKey(u => u.OrganizationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasQueryFilter(u => !u.IsDeleted);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.OrganizationId);
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(320);
        builder.Property(u => u.PasswordHash).IsRequired();
    }
}

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : backend.Common.BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

// Applying basic configurations to all core entities
public class CustomerConfiguration : BaseEntityConfiguration<Customer> {
    public override void Configure(EntityTypeBuilder<Customer> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class QRCodeConfiguration : BaseEntityConfiguration<QRCode> {
    public override void Configure(EntityTypeBuilder<QRCode> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class ReviewConfiguration : BaseEntityConfiguration<Review> {
    public override void Configure(EntityTypeBuilder<Review> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class InvoiceConfiguration : BaseEntityConfiguration<Invoice> {
    public override void Configure(EntityTypeBuilder<Invoice> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class CampaignConfiguration : BaseEntityConfiguration<Campaign> {
    public override void Configure(EntityTypeBuilder<Campaign> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class AppointmentConfiguration : BaseEntityConfiguration<Appointment> {
    public override void Configure(EntityTypeBuilder<Appointment> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class ContactConfiguration : BaseEntityConfiguration<Contact> {
    public override void Configure(EntityTypeBuilder<Contact> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class LandingPageConfiguration : BaseEntityConfiguration<LandingPage> {
    public override void Configure(EntityTypeBuilder<LandingPage> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class AIConversationConfiguration : BaseEntityConfiguration<AIConversation> {
    public override void Configure(EntityTypeBuilder<AIConversation> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class NotificationConfiguration : BaseEntityConfiguration<Notification> {
    public override void Configure(EntityTypeBuilder<Notification> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class ActivityLogConfiguration : BaseEntityConfiguration<ActivityLog> {
    public override void Configure(EntityTypeBuilder<ActivityLog> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
        builder.HasIndex(e => e.CreatedAt);
    }
}
public class BusinessCardConfiguration : BaseEntityConfiguration<BusinessCard> {
    public override void Configure(EntityTypeBuilder<BusinessCard> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
public class SubscriptionConfiguration : BaseEntityConfiguration<Subscription> {
    public override void Configure(EntityTypeBuilder<Subscription> builder) {
        base.Configure(builder);
        builder.HasIndex(e => e.OrganizationId);
    }
}
