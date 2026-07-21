namespace backend.Modules.CustomerJourney.DTOs;

public class CustomerJourneyDto
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public string CurrentStage { get; set; } = string.Empty;
    public string PreviousStage { get; set; } = string.Empty;
    public DateTime EnteredAt { get; set; }
}
