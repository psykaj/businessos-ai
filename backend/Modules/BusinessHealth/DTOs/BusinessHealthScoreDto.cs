using System;

namespace backend.Modules.BusinessHealth.DTOs;

public class BusinessHealthScoreDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public decimal OverallScore { get; set; }
    public decimal FinancialHealth { get; set; }
    public decimal OperationalHealth { get; set; }
    public decimal CustomerHealth { get; set; }
    public DateTime CalculatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
