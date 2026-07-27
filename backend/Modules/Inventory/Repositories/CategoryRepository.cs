using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductCategory?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Include(c => c.ParentCategory)
            .Include(c => c.Products)
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        await _context.ProductCategories.AddAsync(category, cancellationToken);
    }

    public void Update(ProductCategory category)
    {
        _context.ProductCategories.Update(category);
    }

    public void Delete(ProductCategory category)
    {
        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        _context.ProductCategories.Update(category);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
