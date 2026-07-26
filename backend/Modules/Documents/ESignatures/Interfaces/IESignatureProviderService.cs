using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.ESignatures.Interfaces;

public interface IESignatureProviderService
{
    string ProviderName { get; }

    Task<string> GenerateSecurityHashAsync(SignatureRequest request);

    Task SendSignatureInvitationAsync(SignatureRecipient recipient, string signUrl);

    Task<string> GenerateCompletionCertificateAsync(SignatureRequest request);
}
