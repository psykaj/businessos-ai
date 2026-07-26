using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.ESignatures.Interfaces;

public interface IESignatureRepository
{
    Task<SignatureRequest?> GetByIdAsync(Guid organizationId, Guid id);
    Task<SignatureRecipient?> GetRecipientByTokenAsync(string securityToken);
    Task<List<SignatureRequest>> GetByDocumentIdAsync(Guid organizationId, Guid documentId);
    Task<List<SignatureRequest>> GetExpiredRequestsAsync(Guid organizationId);
    Task AddAsync(SignatureRequest request);
    Task UpdateAsync(SignatureRequest request);
    Task UpdateRecipientAsync(SignatureRecipient recipient);
}
