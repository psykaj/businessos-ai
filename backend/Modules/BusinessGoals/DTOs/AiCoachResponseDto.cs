using System;
using System.Collections.Generic;
using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessGoals.DTOs;

public class AiCoachResponseDto
{
    public string Summary { get; set; } = string.Empty;
    public string StatusExplanation { get; set; } = string.Empty;
    public List<string> KeyDrivers { get; set; } = new List<string>();
    public List<string> Risks { get; set; } = new List<string>();
    public List<string> Opportunities { get; set; } = new List<string>();
    public List<RecommendationDto> RecommendedActions { get; set; } = new List<RecommendationDto>();
    public List<string> RelevantOutcomes { get; set; } = new List<string>();
    public decimal Confidence { get; set; }
}
