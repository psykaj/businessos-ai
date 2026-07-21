namespace backend.Modules.Campaigns.DTOs;

public class CampaignDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CampaignType { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Medium { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateCampaignDto
{
    public string Name { get; set; } = string.Empty;
    public string CampaignType { get; set; } = "Email";
    public string Source { get; set; } = string.Empty;
    public string Medium { get; set; } = string.Empty;
    public decimal Budget { get; set; }
}

public class UpdateCampaignDto
{
    public string Name { get; set; } = string.Empty;
    public string CampaignType { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Medium { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string Status { get; set; } = string.Empty;
}
