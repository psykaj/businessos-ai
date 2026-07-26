namespace backend.Modules.Documents.Approvals.DTOs;

public record ApprovalStepDto(
    Guid Id,
    Guid ApprovalRequestId,
    int Sequence,
    Guid? ApproverId,
    string ApproverEmail,
    string? ApproverName,
    string Status,
    string? Comments,
    DateTime? ActionDate,
    bool IsParallelGroup
);

public record ApprovalRequestDto(
    Guid Id,
    Guid OrganizationId,
    Guid DocumentId,
    string DocumentName,
    string Title,
    string Status,
    int CurrentStepSequence,
    bool IsSequential,
    DateTime? DueDate,
    bool AutoEscalate,
    Guid? EscalatedToUserId,
    Guid RequestedById,
    string? RequestedByName,
    List<ApprovalStepDto> Steps,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateApprovalStepDto(
    int Sequence,
    Guid? ApproverId,
    string ApproverEmail,
    string? ApproverName,
    bool IsParallelGroup = false
);

public record CreateApprovalRequestDto(
    Guid DocumentId,
    string Title,
    bool IsSequential = true,
    DateTime? DueDate = null,
    bool AutoEscalate = false,
    Guid? EscalatedToUserId = null,
    List<CreateApprovalStepDto>? Steps = null
);

public record ApproveRejectStepDto(
    string? Comments
);

public record EscalateApprovalDto(
    Guid EscalatedToUserId,
    string? Reason
);
