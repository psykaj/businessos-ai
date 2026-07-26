using System.Security.Cryptography;
using System.Text;
using backend.Modules.Documents.Entities;
using backend.Modules.Documents.ESignatures.Interfaces;

namespace backend.Modules.Documents.ESignatures.Services;

public class NativeESignatureProviderService : IESignatureProviderService
{
    public string ProviderName => "NativeEngine";

    public Task<string> GenerateSecurityHashAsync(SignatureRequest request)
    {
        var payload = $"{request.Id}:{request.OrganizationId}:{request.DocumentId}:{request.CreatedAt.Ticks}";
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var hash = Convert.ToHexString(bytes).ToLower();
        return Task.FromResult(hash);
    }

    public Task SendSignatureInvitationAsync(SignatureRecipient recipient, string signUrl)
    {
        // Integration hook to Email/SMS notification service
        return Task.CompletedTask;
    }

    public Task<string> GenerateCompletionCertificateAsync(SignatureRequest request)
    {
        var certificateSummary = $"Certificate of Completion\nTitle: {request.Title}\nHash: {request.SecurityHash}\nCompletedAt: {request.CompletedAt}\nSigners Count: {request.Recipients.Count}";
        var certificateUrl = $"/api/esignatures/{request.Id}/certificate";
        return Task.FromResult(certificateUrl);
    }
}
