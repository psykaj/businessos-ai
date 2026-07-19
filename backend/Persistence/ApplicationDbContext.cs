using backend.Common;
using backend.Entities;
using backend.Modules.QRCode.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<QRCode> QRCodes => Set<QRCode>();
    public DbSet<backend.Modules.Analytics.Models.QRScan> QRScans => Set<backend.Modules.Analytics.Models.QRScan>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<LandingPage> LandingPages => Set<LandingPage>();
    public DbSet<AIConversation> AIConversations => Set<AIConversation>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<BusinessCard> BusinessCards => Set<BusinessCard>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure composite key for UserRole
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        HandleSaveChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleSaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    // CreatedBy could be populated from a scoped service holding the CurrentUser context
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    // Soft delete logic
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
