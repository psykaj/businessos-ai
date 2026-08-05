using Microsoft.EntityFrameworkCore;
using backend.Modules.Branches.Entities;
using backend.Modules.Branches.Interfaces;
using backend.Persistence;

namespace backend.Modules.Branches.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly ApplicationDbContext _context;

    public BranchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Branch?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .Where(x => x.Id == id && x.OrganizationId == organizationId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Branch>> GetAllByOrgAsync(Guid organizationId, Guid? locationId = null, BranchStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Branches
            .Where(x => x.OrganizationId == organizationId && !x.IsDeleted);

        if (locationId.HasValue)
        {
            query = query.Where(x => x.LocationId == locationId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task<Branch> AddAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        await _context.Branches.AddAsync(branch, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return branch;
    }

    public async Task UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Remove(branch);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CodeExistsAsync(string code, Guid organizationId, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Branches
            .Where(x => x.OrganizationId == organizationId && x.Code.ToLower() == code.ToLower() && !x.IsDeleted);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<BranchManager> AddManagerAsync(BranchManager manager, CancellationToken cancellationToken = default)
    {
        await _context.BranchManagers.AddAsync(manager, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return manager;
    }

    public async Task<IEnumerable<BranchManager>> GetManagersByBranchAsync(Guid branchId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.BranchManagers
            .Where(x => x.BranchId == branchId && x.OrganizationId == organizationId && !x.IsDeleted && x.IsActive)
            .OrderBy(x => x.ManagerName)
            .ToListAsync(cancellationToken);
    }

    public async Task<BranchManager?> GetManagerAsync(Guid branchId, Guid userId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.BranchManagers
            .Where(x => x.BranchId == branchId && x.UserId == userId && x.OrganizationId == organizationId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task RemoveManagerAsync(BranchManager manager, CancellationToken cancellationToken = default)
    {
        _context.BranchManagers.Remove(manager);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
