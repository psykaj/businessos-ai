using System;
using System.Collections.Generic;

namespace backend.Modules.CustomerFeedback.CustomerSatisfaction.DTOs;

public class CustomerCsatDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public decimal CurrentCsat { get; set; }
    public decimal CurrentNps { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalFeedbacks { get; set; }
    public int TotalComplaints { get; set; }
    public decimal RepeatComplaintRate { get; set; }
    public decimal ResolutionSatisfaction { get; set; }
    public string ChurnRiskScore { get; set; } = "Low";
    public DateTime LastCalculatedAt { get; set; }
}

public class CsatDashboardDto
{
    public Guid OrganizationId { get; set; }
    public decimal OverallCsatPercentage { get; set; }
    public decimal NetPromoterScore { get; set; }
    public decimal OverallAverageRating { get; set; }
    public decimal RepeatComplaintRate { get; set; }
    public decimal ResolutionSatisfactionRate { get; set; }
    public int TotalCustomersTracked { get; set; }
    public int HighChurnRiskCount { get; set; }
    public List<string> RecentTrends { get; set; } = new();
}

public class CalculateCsatRequestDto
{
    public Guid OrganizationId { get; set; }
    public Guid CustomerId { get; set; }
}
