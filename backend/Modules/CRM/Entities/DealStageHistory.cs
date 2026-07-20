using backend.Common;
using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.Entities;

public class DealStageHistory : BaseEntity
{
    public Guid DealId { get; set; }
    public Deal? Deal { get; set; }

    public PipelineStage OldStage { get; set; }
    public PipelineStage NewStage { get; set; }
    
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public Guid? ChangedById { get; set; }
}
