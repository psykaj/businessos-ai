using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Sharing.DTOs;

namespace backend.Modules.Documents.Sharing.Interfaces;

public interface ISharedDocumentService
{
    Task<SharedDocumentDto> ShareAsync(Guid organizationId, Guid sharedById, ShareDocumentDto dto);
    Task<List<SharedDocumentDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId);
    Task RevokeAsync(Guid organizationId, Guid shareId);
    Task<DocumentPreviewDto> AccessPublicShareAsync(string publicToken, PublicAccessDto dto);
}
