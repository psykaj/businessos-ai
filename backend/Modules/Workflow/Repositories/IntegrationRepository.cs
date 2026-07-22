using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Workflow.Repositories;

public class IntegrationRepository : IIntegrationRepository
{
    private readonly ApplicationDbContext _context;

    public IntegrationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Integration?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Integrations
            .FirstOrDefaultAsync(i => i.Id == id && i.OrganizationId == organizationId && !i.IsDeleted, cancellationToken);
    }

    public async Task<Integration?> GetByProviderAsync(Guid organizationId, IntegrationProvider provider, CancellationToken cancellationToken = default)
    {
        return await _context.Integrations
            .FirstOrDefaultAsync(i => i.OrganizationId == organizationId && i.Provider == provider && !i.IsDeleted, cancellationToken);
    }

    public async Task<List<Integration>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Integrations
            .Where(i => i.OrganizationId == organizationId && !i.IsDeleted)
            .OrderBy(i => i.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Integration> AddAsync(Integration integration, CancellationToken cancellationToken = default)
    {
        await _context.Integrations.AddAsync(integration, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return integration;
    }

    public async Task UpdateAsync(Integration integration, CancellationToken cancellationToken = default)
    {
        integration.UpdatedAt = DateTime.UtcNow;
        _context.Integrations.Update(integration);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Integration integration, CancellationToken cancellationToken = default)
    {
        integration.IsDeleted = true;
        integration.DeletedAt = DateTime.UtcNow;
        _context.Integrations.Update(integration);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
