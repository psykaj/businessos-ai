using backend.Common;
using backend.Modules.QRCode.DTOs;
using backend.Modules.QRCode.Enums;

namespace backend.Modules.QRCode.Interfaces;

public interface IQRCodeService
{
    Task<QRCodeDto> CreateAsync(CreateQRCodeDto dto, CancellationToken cancellationToken = default);
    Task<QRCodeDto> UpdateAsync(Guid id, Guid organizationId, UpdateQRCodeDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<QRCodeDto> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<QRCodeDto>> SearchAsync(Guid organizationId, string? searchTerm, string? folder, string? status, QRType? qrType, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateImageAsync(Guid id, Guid organizationId, string format = "png", CancellationToken cancellationToken = default);
}
