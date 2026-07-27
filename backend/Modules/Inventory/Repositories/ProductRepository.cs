using backend.Common;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Inventory.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.StockLevels)
                .ThenInclude(s => s.Warehouse)
            .FirstOrDefaultAsync(p => p.Id == id && p.OrganizationId == organizationId && !p.IsDeleted, cancellationToken);
    }

    public async Task<Product?> GetBySKUAsync(string sku, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.StockLevels)
            .FirstOrDefaultAsync(p => p.SKU.ToLower() == sku.ToLower() && p.OrganizationId == organizationId && !p.IsDeleted, cancellationToken);
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.StockLevels)
            .FirstOrDefaultAsync(p => (p.Barcode == barcode || p.QRCode == barcode) && p.OrganizationId == organizationId && !p.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<Product>> GetPagedAsync(Guid organizationId, ProductSearchFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.StockLevels)
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            var searchTerm = filter.Query.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm) ||
                                     p.SKU.ToLower().Contains(searchTerm) ||
                                     (p.Barcode != null && p.Barcode.ToLower().Contains(searchTerm)));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.IsArchived.HasValue)
        {
            query = query.Where(p => p.IsArchived == filter.IsArchived.Value);
        }

        if (filter.LowStockOnly == true)
        {
            query = query.Where(p => p.StockLevels.Sum(s => s.QuantityOnHand - s.QuantityReserved) <= p.ReorderPoint);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = filter.SortBy?.ToLower() switch
        {
            "sku" => filter.SortDescending ? query.OrderByDescending(p => p.SKU) : query.OrderBy(p => p.SKU),
            "price" => filter.SortDescending ? query.OrderByDescending(p => p.SellingPrice) : query.OrderBy(p => p.SellingPrice),
            "createdat" => filter.SortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            _ => filter.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>(items, totalCount, filter.PageNumber, filter.PageSize);
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.StockLevels)
            .Include(p => p.Category)
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted && !p.IsArchived && p.IsActive)
            .Where(p => p.StockLevels.Sum(s => s.QuantityOnHand - s.QuantityReserved) <= p.ReorderPoint)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.StockLevels)
            .Include(p => p.Category)
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted && !p.IsArchived && p.IsActive)
            .Where(p => p.StockLevels.Sum(s => s.QuantityOnHand - s.QuantityReserved) <= 0)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetOverstockProductsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.StockLevels)
            .Include(p => p.Category)
            .Where(p => p.OrganizationId == organizationId && !p.IsDeleted && !p.IsArchived && p.IsActive)
            .Where(p => p.StockLevels.Sum(s => s.QuantityOnHand) > p.MaxStockLevel)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
