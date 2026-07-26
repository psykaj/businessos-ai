namespace backend.Modules.Documents.ESignatures.DTOs;

public record SignatureRecipientDto(
    Guid Id,
    Guid SignatureRequestId,
    string SignerName,
    string SignerEmail,
    Guid? SignerUserId,
    string Role,
    int SigningOrder,
    string Status,
    DateTime? ViewedAt,
    DateTime? SignedAt,
    string SecurityToken,
    string? AccessCode,
    string? IpAddress
);

public record SignatureRequestDto(
    Guid Id,
    Guid OrganizationId,
    Guid DocumentId,
    string DocumentName,
    string Title,
    string? Message,
    string Status,
    DateTime? ExpiresAt,
    DateTime? CompletedAt,
    Guid CreatedById,
    string? CreatedByName,
    string SecurityHash,
    string? SignatureCertificateUrl,
    List<SignatureRecipientDto> Recipients,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateSignatureRecipientDto(
    string SignerName,
    string SignerEmail,
    Guid? SignerUserId = null,
    string Role = "Signer",
    int SigningOrder = 1,
    string? AccessCode = null
);

public record CreateSignatureRequestDto(
    Guid DocumentId,
    string Title,
    string? Message = null,
    DateTime? ExpiresAt = null,
    List<CreateSignatureRecipientDto>? Recipients = null
);

public record SubmitSignatureDto(
    string SignatureData, // Base64 PNG / Vector SVG signature string
    string? AccessCode = null,
    string? IpAddress = null,
    string? UserAgent = null
);

public record SignatureAuditTrailDto(
    Guid SignatureRequestId,
    string Title,
    string DocumentName,
    string SecurityHash,
    string Status,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    List<SignatureRecipientDto> AuditLogRecipients
);
