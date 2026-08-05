using Microsoft.EntityFrameworkCore;
using backend.Modules.Transfers.Entities;
using backend.Modules.Transfers.Interfaces;
using backend.Persistence;

namespace backend.Modules.Transfers.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly ApplicationDbContext _context;

    public TransferRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WarehouseTransfer?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.WarehouseTransfers
            .Where(x => x.Id == id && x.OrganizationId == organizationId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<WarehouseTransfer>> GetHistoryAsync(Guid organizationId, Guid? warehouseId = null, TransferStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.WarehouseTransfers
            .Where(x => x.OrganizationId == organizationId && !x.IsDeleted);

        if (warehouseId.HasValue)
        {
            query = query.Where(x => x.SourceWarehouseId == warehouseId.Value || x.DestinationWarehouseId == warehouseId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query.OrderByDescending(x => x.RequestedAt).ToListAsync(cancellationToken);
    }

    public async Task<WarehouseTransfer> AddAsync(WarehouseTransfer transfer, CancellationToken cancellationToken = default)
    {
        await _context.WarehouseTransfers.AddAsync(transfer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return transfer;
    }

    public async Task UpdateAsync(WarehouseTransfer transfer, CancellationToken cancellationToken = default)
    {
        _context.WarehouseTransfers.Update(transfer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetCountByOrgAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.WarehouseTransfers
            .CountAsync(x => x.OrganizationId == organizationId && !x.IsDeleted, cancellationToken);
    }
}
