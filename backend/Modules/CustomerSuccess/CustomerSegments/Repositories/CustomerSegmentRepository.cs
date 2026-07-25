using backend.Modules.CustomerSuccess.CustomerSegments.Entities;
using backend.Modules.CustomerSuccess.CustomerSegments.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerSuccess.CustomerSegments.Repositories;

public class CustomerSegmentRepository : ICustomerSegmentRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerSegmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerSegment?> GetByIdAsync(Guid orgId, Guid id)
    {
        return await _context.CustomerSegments
            .FirstOrDefaultAsync(s => s.OrganizationId == orgId && s.Id == id);
    }

    public async Task<CustomerSegment?> GetByNameAsync(Guid orgId, string name)
    {
        return await _context.CustomerSegments
            .FirstOrDefaultAsync(s => s.OrganizationId == orgId && s.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<CustomerSegment>> GetAllAsync(Guid orgId)
    {
        return await _context.CustomerSegments
            .Where(s => s.OrganizationId == orgId)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task AddAsync(CustomerSegment segment)
    {
        await _context.CustomerSegments.AddAsync(segment);
    }

    public async Task UpdateAsync(CustomerSegment segment)
    {
        _context.CustomerSegments.Update(segment);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
