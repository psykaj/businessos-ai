using backend.Common;
using backend.Modules.QRCode.Enums;
using backend.Modules.QRCode.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.QRCode.Repositories;

public class QRCodeRepository : IQRCodeRepository
{
    private readonly ApplicationDbContext _context;

    public QRCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Models.QRCode?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var query = _context.QRCodes.Where(x => x.Id == id && !x.IsDeleted);
        if (organizationId != Guid.Empty)
        {
            query = query.Where(x => x.OrganizationId == organizationId);
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Models.QRCode?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return await _context.QRCodes
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode && !x.IsDeleted, cancellationToken);
    }

    public async Task<PagedResult<Models.QRCode>> SearchAsync(
        Guid organizationId, 
        string? searchTerm, 
        string? folder, 
        string? status, 
        QRType? qrType, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.QRCodes.Where(x => x.OrganizationId == organizationId && !x.IsDeleted).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Name.Contains(searchTerm) || (x.Description != null && x.Description.Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(folder))
        {
            query = query.Where(x => x.Folder == folder);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.Status == status);
        }

        if (qrType.HasValue)
        {
            query = query.Where(x => x.QRType == qrType.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Models.QRCode>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<Models.QRCode> AddAsync(Models.QRCode qrCode, CancellationToken cancellationToken = default)
    {
        _context.QRCodes.Add(qrCode);
        await _context.SaveChangesAsync(cancellationToken);
        return qrCode;
    }

    public async Task UpdateAsync(Models.QRCode qrCode, CancellationToken cancellationToken = default)
    {
        _context.QRCodes.Update(qrCode);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Models.QRCode qrCode, CancellationToken cancellationToken = default)
    {
        // Relying on ApplicationDbContext's HandleSaveChanges for soft delete
        _context.QRCodes.Remove(qrCode);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
