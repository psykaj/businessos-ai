using backend.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        if (!await context.Organizations.AnyAsync())
        {
            var defaultOrg = new Organization
            {
                Name = "Default Organization",
                Slug = "default-org",
                Email = "admin@businessos.ai",
                SubscriptionPlan = "Enterprise",
                SubscriptionStatus = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Organizations.AddAsync(defaultOrg);
            await context.SaveChangesAsync();

            if (!await context.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    FullName = "Admin User",
                    Email = "admin@businessos.ai",
                    // Hashed password for 'Admin123!'
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "Admin",
                    OrganizationId = defaultOrg.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }

            if (!await context.Customers.AnyAsync())
            {
                var sampleCustomer = new Customer
                {
                    Name = "Sample Customer",
                    OrganizationId = defaultOrg.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Customers.AddAsync(sampleCustomer);
                await context.SaveChangesAsync();
            }
        }
    }
}
