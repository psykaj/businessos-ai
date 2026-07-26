using System.Text.Json;
using backend.Modules.Documents.Approvals.DTOs;
using backend.Modules.Documents.Approvals.Interfaces;
using backend.Modules.Documents.AuditLogs.DTOs;
using backend.Modules.Documents.AuditLogs.Interfaces;
using backend.Modules.Documents.Documents.Interfaces;
using backend.Modules.Documents.Entities;

namespace backend.Modules.Documents.Approvals.Services;

public class ApprovalService : IApprovalService
{
    private readonly IApprovalRepository _repository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentAuditLogService _auditLogService;

    public ApprovalService(
        IApprovalRepository repository,
        IDocumentRepository documentRepository,
        IDocumentAuditLogService auditLogService)
    {
        _repository = repository;
        _documentRepository = documentRepository;
        _auditLogService = auditLogService;
    }

    public async Task<ApprovalRequestDto> CreateAsync(
        Guid organizationId,
        Guid requestedById,
        string requestedByName,
        CreateApprovalRequestDto dto)
    {
        var document = await _documentRepository.GetByIdAsync(organizationId, dto.DocumentId)
            ?? throw new KeyNotFoundException($"Document with ID {dto.DocumentId} not found.");

        var request = new ApprovalRequest
        {
            OrganizationId = organizationId,
            DocumentId = dto.DocumentId,
            Title = dto.Title.Trim(),
            Status = "Pending",
            CurrentStepSequence = 1,
            IsSequential = dto.IsSequential,
            DueDate = dto.DueDate,
            AutoEscalate = dto.AutoEscalate,
            EscalatedToUserId = dto.EscalatedToUserId,
            RequestedById = requestedById,
            RequestedByName = requestedByName
        };

        if (dto.Steps != null && dto.Steps.Count > 0)
        {
            foreach (var s in dto.Steps)
            {
                request.Steps.Add(new ApprovalStep
                {
                    OrganizationId = organizationId,
                    Sequence = s.Sequence,
                    ApproverId = s.ApproverId,
                    ApproverEmail = s.ApproverEmail.Trim(),
                    ApproverName = s.ApproverName,
                    Status = s.Sequence == 1 ? "Pending" : (dto.IsSequential ? "Pending" : "Pending"),
                    IsParallelGroup = s.IsParallelGroup
                });
            }
        }

        document.Status = "InReview";
        await _documentRepository.UpdateAsync(document);
        await _repository.AddAsync(request);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            document.Id, "Approval", "ApprovalRequested", requestedById, requestedByName, null,
            JsonSerializer.Serialize(new { ApprovalRequestId = request.Id, Title = request.Title })
        ));

        return MapToDto(request);
    }

    public async Task<ApprovalRequestDto> GetByIdAsync(Guid organizationId, Guid id)
    {
        var request = await _repository.GetByIdAsync(organizationId, id)
            ?? throw new KeyNotFoundException($"Approval request with ID {id} not found.");
        return MapToDto(request);
    }

    public async Task<List<ApprovalRequestDto>> GetByDocumentIdAsync(Guid organizationId, Guid documentId)
    {
        var requests = await _repository.GetByDocumentIdAsync(organizationId, documentId);
        return requests.Select(MapToDto).ToList();
    }

    public async Task<List<ApprovalRequestDto>> GetPendingForUserAsync(Guid organizationId, Guid userId, string userEmail)
    {
        var requests = await _repository.GetPendingForUserAsync(organizationId, userId, userEmail);
        return requests.Select(MapToDto).ToList();
    }

    public async Task<ApprovalRequestDto> ApproveStepAsync(
        Guid organizationId,
        Guid approvalRequestId,
        Guid userId,
        string userEmail,
        ApproveRejectStepDto dto)
    {
        var request = await _repository.GetByIdAsync(organizationId, approvalRequestId)
            ?? throw new KeyNotFoundException($"Approval request with ID {approvalRequestId} not found.");

        var matchingStep = request.Steps.FirstOrDefault(s =>
            (s.ApproverId == userId || s.ApproverEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase)) &&
            s.Status == "Pending");

        if (matchingStep == null)
        {
            throw new InvalidOperationException("No pending approval step found for this user.");
        }

        matchingStep.Status = "Approved";
        matchingStep.Comments = dto.Comments;
        matchingStep.ActionDate = DateTime.UtcNow;

        // Check if all steps in current sequence are approved
        bool currentSeqDone = request.Steps
            .Where(s => s.Sequence == request.CurrentStepSequence)
            .All(s => s.Status == "Approved");

        if (currentSeqDone)
        {
            int maxSeq = request.Steps.Max(s => s.Sequence);
            if (request.CurrentStepSequence >= maxSeq)
            {
                request.Status = "Approved";
                if (request.Document != null)
                {
                    request.Document.Status = "Active";
                    await _documentRepository.UpdateAsync(request.Document);
                }
            }
            else
            {
                request.CurrentStepSequence++;
            }
        }

        await _repository.UpdateAsync(request);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            request.DocumentId, "Approval", "Approved", userId, userEmail, null,
            JsonSerializer.Serialize(new { StepSequence = matchingStep.Sequence, Comments = dto.Comments })
        ));

        return MapToDto(request);
    }

    public async Task<ApprovalRequestDto> RejectStepAsync(
        Guid organizationId,
        Guid approvalRequestId,
        Guid userId,
        string userEmail,
        ApproveRejectStepDto dto)
    {
        var request = await _repository.GetByIdAsync(organizationId, approvalRequestId)
            ?? throw new KeyNotFoundException($"Approval request with ID {approvalRequestId} not found.");

        var matchingStep = request.Steps.FirstOrDefault(s =>
            (s.ApproverId == userId || s.ApproverEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase)) &&
            s.Status == "Pending");

        if (matchingStep == null)
        {
            throw new InvalidOperationException("No pending approval step found for this user.");
        }

        matchingStep.Status = "Rejected";
        matchingStep.Comments = dto.Comments;
        matchingStep.ActionDate = DateTime.UtcNow;

        request.Status = "Rejected";
        if (request.Document != null)
        {
            request.Document.Status = "Active";
            await _documentRepository.UpdateAsync(request.Document);
        }

        await _repository.UpdateAsync(request);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            request.DocumentId, "Approval", "Rejected", userId, userEmail, null,
            JsonSerializer.Serialize(new { StepSequence = matchingStep.Sequence, Reason = dto.Comments })
        ));

        return MapToDto(request);
    }

    public async Task<ApprovalRequestDto> EscalateAsync(Guid organizationId, Guid approvalRequestId, EscalateApprovalDto dto)
    {
        var request = await _repository.GetByIdAsync(organizationId, approvalRequestId)
            ?? throw new KeyNotFoundException($"Approval request with ID {approvalRequestId} not found.");

        request.EscalatedToUserId = dto.EscalatedToUserId;

        // Reassign pending steps to escalated user
        foreach (var s in request.Steps.Where(s => s.Status == "Pending"))
        {
            s.ApproverId = dto.EscalatedToUserId;
            s.Comments = $"Escalated: {dto.Reason}";
        }

        await _repository.UpdateAsync(request);

        await _auditLogService.LogAsync(organizationId, new CreateAuditLogDto(
            request.DocumentId, "Approval", "Escalated", null, null, null,
            JsonSerializer.Serialize(new { EscalatedTo = dto.EscalatedToUserId, Reason = dto.Reason })
        ));

        return MapToDto(request);
    }

    public async Task ProcessOverdueEscalationsAsync(Guid organizationId)
    {
        var overdue = await _repository.GetOverdueRequestsAsync(organizationId);
        foreach (var req in overdue)
        {
            if (req.AutoEscalate && req.EscalatedToUserId.HasValue)
            {
                await EscalateAsync(organizationId, req.Id, new EscalateApprovalDto(
                    req.EscalatedToUserId.Value,
                    "Automated escalation due to overdue approval date"
                ));
            }
        }
    }

    private static ApprovalRequestDto MapToDto(ApprovalRequest req) => new(
        req.Id,
        req.OrganizationId,
        req.DocumentId,
        req.Document?.Name ?? string.Empty,
        req.Title,
        req.Status,
        req.CurrentStepSequence,
        req.IsSequential,
        req.DueDate,
        req.AutoEscalate,
        req.EscalatedToUserId,
        req.RequestedById,
        req.RequestedByName,
        req.Steps.Select(s => new ApprovalStepDto(
            s.Id,
            s.ApprovalRequestId,
            s.Sequence,
            s.ApproverId,
            s.ApproverEmail,
            s.ApproverName,
            s.Status,
            s.Comments,
            s.ActionDate,
            s.IsParallelGroup
        )).ToList(),
        req.CreatedAt,
        req.UpdatedAt
    );
}
