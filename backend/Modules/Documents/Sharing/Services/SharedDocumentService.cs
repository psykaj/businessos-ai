using System.Text.Json;
using backend.Modules.Documents.AuditLogs.DTOs;
using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Documents.Interfaces;
using backend.Modules.Documents.Entities;
using backend.Modules.Documents.Sharing.DTOs;
using backend.Modules.Documents.Sharing.Interfaces;

namespace backend.Modules.Documents.Sharing.Services;

public class SharedDocumentService : ISharedDocumentService
{
    private readonly ISharedDocumentRepository _repository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentService _documentService;
    private readonly IDocumentAuditLogService _auditLogService;

    public SharedDocumentService(
        ISharedDocumentRepository repository,
        IDocumentRepository documentRepository,
        IDocumentService documentService,
        IDocumentAuditLogService auditLogService)
    {
        _repository = repository;
        _documentRepository = documentRepository;
        _documentService = documentService;
        _auditLogService = auditLogService;
    }

    public async Task<SharedDocumentDto> ShareAsync(Guid organizationId, Guid sharedById, ShareDocumentDto dto)
    {
        var document = await _documentRepository.GetByIdAsync(organizationId, dto.DocumentId)
            ?? throw new KeyNotFoundException($"Document with ID {dto.DocumentId} not found.");

        string? token = dto.PermissionType == "PublicLink" ? Guid.NewGuid().ToString("N") : null;

        var share = new SharedDocument
        {
            OrganizationId = organizationId,
            DocumentId = dto.DocumentId,
            SharedWithUserId = dto.SharedWithUserId,
            SharedWithEmail = dto.SharedWithEmail,
            AccessLevel = dto.AccessLevel,
            PermissionType = dto.PermissionType,
            PublicShareToken = token,
            Passcode = dto.Passcode,
            ExpirationDate = dto.ExpirationDate,
            IsActive = true,
            SharedById = sharedById
        };

        await _repository.AddAsync(share);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Share", "Shared", sharedById, null, null,
            JsonSerializer.Serialize(new { PermissionType = dto.PermissionType, SharedWithEmail = dto.SharedWithEmail })
        ));

        return MapToDto(share);
    }

    public async Task<List<SharedDocumentDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId)
    {
        var shares = await _repository.GetByDocumentIdAsync(organizationId, documentId);
        return shares.Select(MapToDto).ToList();
    }

    public async Task RevokeAsync(Guid organizationId, Guid shareId)
    {
        var share = await _repository.GetByIdAsync(organizationId, shareId)
            ?? throw new KeyNotFoundException($"Share record with ID {shareId} not found.");

        await _repository.DeleteAsync(share);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            share.DocumentId, "Share", "Revoked", null, null, null, null
        ));
    }

    public async Task<DocumentPreviewDto> AccessPublicShareAsync(string publicToken, PublicAccessDto dto)
    {
        var share = await _repository.GetByTokenAsync(publicToken)
            ?? throw new KeyNotFoundException("Invalid or expired public share link.");

        if (share.ExpirationDate.HasValue && share.ExpirationDate.Value < DateTime.UtcNow)
        {
            throw new InvalidOperationException("This share link has expired.");
        }

        if (!string.IsNullOrWhiteSpace(share.Passcode) && share.Passcode != dto.Passcode)
        {
            throw new UnauthorizedAccessException("Incorrect passcode for public document access.");
        }

        share.AccessCount++;
        await _repository.UpdateAsync(share);

        return await _documentService.GetPreviewAsync(share.OrganizationId, share.DocumentId);
    }

    private static SharedDocumentDto MapToDto(SharedDocument s)
    {
        string? shareUrl = !string.IsNullOrWhiteSpace(s.PublicShareToken)
            ? $"/api/documents/public/{s.PublicShareToken}"
            : null;

        return new SharedDocumentDto(
            s.Id,
            s.OrganizationId,
            s.DocumentId,
            s.Document?.Name ?? string.Empty,
            s.SharedWithUserId,
            s.SharedWithEmail,
            s.AccessLevel,
            s.PermissionType,
            s.PublicShareToken,
            shareUrl,
            !string.IsNullOrWhiteSpace(s.Passcode),
            s.ExpirationDate,
            s.AccessCount,
            s.IsActive,
            s.SharedById,
            s.CreatedAt
        );
    }
}
