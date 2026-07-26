using backend.Modules.Documents.ESignatures.DTOs;

namespace backend.Modules.Documents.ESignatures.Interfaces;

public interface IESignatureService
{
    Task<SignatureRequestDto> CreateAsync(Guid organizationId, Guid createdById, string createdByName, CreateSignatureRequestDto dto);
    Task<SignatureRequestDto> GetByIdAsync(Guid organizationId, Guid id);
    Task<List<SignatureRequestDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId);
    Task<SignatureRecipientDto> GetRecipientByTokenAsync(string token);
    Task<SignatureRecipientDto> SubmitSignatureAsync(string token, SubmitSignatureDto dto);
    Task<SignatureRequestDto> CancelRequestAsync(Guid organizationId, Guid id, string reason);
    Task<SignatureAuditTrailDto> GetAuditTrailAsync(Guid organizationId, Guid id);
    Task CheckExpirationsAsync(Guid organizationId);
}
