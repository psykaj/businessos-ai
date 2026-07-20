using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.DTOs;

public class CreateDealDto
{
    public Guid? LeadId { get; set; }
    public Guid? CompanyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public PipelineStage PipelineStage { get; set; }
    public decimal Amount { get; set; }
    public decimal Probability { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public string Status { get; set; } = "Open";
    public Guid? OwnerId { get; set; }
}

public class UpdateDealDto : CreateDealDto
{
}

public class UpdateDealStageDto
{
    public PipelineStage PipelineStage { get; set; }
}

public class DealResponseDto : CreateDealDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
