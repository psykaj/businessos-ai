using System.Text.Json;
using backend.Modules.Documents.AuditLogs.DTOs;
using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.Documents.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Modules.Documents.ESignatures.DTOs;
using backend.Modules.Documents.ESignatures.Interfaces;

namespace backend.Modules.Documents.ESignatures.Services;

public class ESignatureService : IESignatureService
{
    private readonly IESignatureRepository _repository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IESignatureProviderService _providerService;
    private readonly IDocumentAuditLogService _auditLogService;

    public ESignatureService(
        IESignatureRepository repository,
        IDocumentRepository documentRepository,
        IESignatureProviderService providerService,
        IDocumentAuditLogService auditLogService)
    {
        _repository = repository;
        _documentRepository = documentRepository;
        _providerService = providerService;
        _auditLogService = auditLogService;
    }

    public async Task<SignatureRequestDto> CreateAsync(
        Guid organizationId,
        Guid createdById,
        string createdByName,
        CreateSignatureRequestDto dto)
    {
        var document = await _documentRepository.GetByIdAsync(organizationId, dto.DocumentId)
            ?? throw new KeyNotFoundException($"Document with ID {dto.DocumentId} not found.");

        var request = new SignatureRequest
        {
            OrganizationId = organizationId,
            DocumentId = dto.DocumentId,
            Title = dto.Title.Trim(),
            Message = dto.Message,
            Status = "Pending",
            ExpiresAt = dto.ExpiresAt ?? DateTime.UtcNow.AddDays(14),
            CreatedById = createdById,
            CreatedByName = createdByName
        };

        request.SecurityHash = await _providerService.GenerateSecurityHashAsync(request);

        if (dto.Recipients != null && dto.Recipients.Count > 0)
        {
            foreach (var r in dto.Recipients)
            {
                var recipient = new SignatureRecipient
                {
                    OrganizationId = organizationId,
                    SignerName = r.SignerName.Trim(),
                    SignerEmail = r.SignerEmail.Trim(),
                    SignerUserId = r.SignerUserId,
                    Role = r.Role,
                    SigningOrder = r.SigningOrder,
                    Status = "Pending",
                    AccessCode = r.AccessCode
                };
                request.Recipients.Add(recipient);
            }
        }

        document.Status = "PendingSignature";
        await _documentRepository.UpdateAsync(document);
        await _repository.AddAsync(request);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Signature", "SignatureRequested", createdById, createdByName, null,
            JsonSerializer.Serialize(new { SignatureRequestId = request.Id, Title = request.Title, RecipientsCount = request.Recipients.Count })
        ));

        return MapToDto(request);
    }

    public async Task<SignatureRequestDto> GetByIdAsync(Guid organizationId, Guid id)
    {
        var request = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Signature request with ID {id} not found.");
        return MapToDto(request);
    }

    public async Task<List<SignatureRequestDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId)
    {
        var requests = await _repository.GetByDocumentIdAsync(organizationId, documentId);
        return requests.Select(MapToDto).ToList();
    }

    public async Task<SignatureRecipientDto> GetRecipientByTokenAsync(string token)
    {
        var recipient = await _repository.GetRecipientByTokenAsync(token)
            ?? throw new KeyNotFoundException("Invalid or expired security token.");

        if (recipient.Status == "Pending")
        {
            recipient.Status = "Viewed";
            recipient.ViewedAt = DateTime.UtcNow;
            await _repository.UpdateRecipientAsync(recipient);
        }

        return MapRecipientToDto(recipient);
    }

    public async Task<SignatureRecipientDto> SubmitSignatureAsync(string token, SubmitSignatureDto dto)
    {
        var recipient = await _repository.GetRecipientByTokenAsync(token)
            ?? throw new KeyNotFoundException("Invalid or expired security token.");

        if (recipient.SignatureRequest == null)
        {
            throw new InvalidOperationException("Signature request context is missing.");
        }

        if (recipient.SignatureRequest.Status is "Completed" or "Cancelled" or "Expired")
        {
            throw new InvalidOperationException($"Signature request is currently {recipient.SignatureRequest.Status}.");
        }

        if (!string.IsNullOrWhiteSpace(recipient.AccessCode) && recipient.AccessCode != dto.AccessCode)
        {
            throw new UnauthorizedAccessException("Incorrect security access code.");
        }

        recipient.Status = "Signed";
        recipient.SignedAt = DateTime.UtcNow;
        recipient.SignatureData = dto.SignatureData;
        recipient.IpAddress = dto.IpAddress;
        recipient.UserAgent = dto.UserAgent;

        await _repository.UpdateRecipientAsync(recipient);

        var request = recipient.SignatureRequest;
        bool allSigned = request.Recipients.All(r => r.Status == "Signed");
        if (allSigned)
        {
            request.Status = "Completed";
            request.CompletedAt = DateTime.UtcNow;
            request.SignatureCertificateUrl = await _providerService.GenerateCompletionCertificateAsync(request);

            if (request.Document != null)
            {
                request.Document.Status = "Signed";
                await _documentRepository.UpdateAsync(request.Document);
            }

            await _repository.UpdateAsync(request);
        }

        await _auditLogService.LogAsync(request.OrganizationId, new CreateAuditLogDto(
            request.DocumentId, "Signature", "Signed", recipient.SignerUserId, recipient.SignerName, dto.IpAddress,
            JsonSerializer.Serialize(new { RecipientEmail = recipient.SignerEmail, Status = recipient.Status })
        ));

        return MapRecipientToDto(recipient);
    }

    public async Task<SignatureRequestDto> CancelRequestAsync(Guid organizationId, Guid id, string reason)
    {
        var request = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Signature request with ID {id} not found.");

        request.Status = "Cancelled";
        if (request.Document != null)
        {
            request.Document.Status = "Active";
            await _documentRepository.UpdateAsync(request.Document);
        }

        await _repository.UpdateAsync(request);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            request.DocumentId, "Signature", "Cancelled", null, null, null,
            JsonSerializer.Serialize(new { Reason = reason })
        ));

        return MapToDto(request);
    }

    public async Task<SignatureAuditTrailDto> GetAuditTrailAsync(Guid organizationId, Guid id)
    {
        var request = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Signature request with ID {id} not found.");

        return new SignatureAuditTrailDto(
            request.Id,
            request.Title,
            request.Document?.Name ?? string.Empty,
            request.SecurityHash,
            request.Status,
            request.CreatedAt,
            request.CompletedAt,
            request.Recipients.Select(MapRecipientToDto).ToList()
        );
    }

    public async Task CheckExpirationsAsync(Guid organizationId)
    {
        var expiredRequests = await _repository.GetExpiredRequestsAsync(organizationId);
        foreach (var req in expiredRequests)
        {
            req.Status = "Expired";
            await _repository.UpdateAsync(req);
        }
    }

    private static SignatureRequestDto MapToDto(SignatureRequest r) => new(
        r.Id,
        r.OrganizationId,
        r.DocumentId,
        r.Document?.Name ?? string.Empty,
        r.Title,
        r.Message,
        r.Status,
        r.ExpiresAt,
        r.CompletedAt,
        r.CreatedById,
        r.CreatedByName,
        r.SecurityHash,
        r.SignatureCertificateUrl,
        r.Recipients.Select(MapRecipientToDto).ToList(),
        r.CreatedAt,
        r.UpdatedAt
    );

    private static SignatureRecipientDto MapRecipientToDto(SignatureRecipient r) => new(
        r.Id,
        r.SignatureRequestId,
        r.SignerName,
        r.SignerEmail,
        r.SignerUserId,
        r.Role,
        r.SigningOrder,
        r.Status,
        r.ViewedAt,
        r.SignedAt,
        r.SecurityToken,
        r.AccessCode,
        r.IpAddress
    );
}
