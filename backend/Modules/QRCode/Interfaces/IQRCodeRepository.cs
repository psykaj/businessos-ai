using backend.Common;
using backend.Modules.QRCode.Enums;

namespace backend.Modules.QRCode.Interfaces;

public interface IQRCodeRepository
{
    Task<Models.QRCode?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<Models.QRCode?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);
    
    Task<PagedResult<Models.QRCode>> SearchAsync(
        Guid organizationId, 
        string? searchTerm, 
        string? folder, 
        string? status, 
        QRType? qrType, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
        
    Task<Models.QRCode> AddAsync(Models.QRCode qrCode, CancellationToken cancellationToken = default);
    Task UpdateAsync(Models.QRCode qrCode, CancellationToken cancellationToken = default);
    Task DeleteAsync(Models.QRCode qrCode, CancellationToken cancellationToken = default);
}
