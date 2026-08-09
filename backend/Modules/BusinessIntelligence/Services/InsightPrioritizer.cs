using System.Collections.Generic;
using System.Linq;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;

namespace backend.Modules.BusinessIntelligence.Services;

public class InsightPrioritizer : IInsightPrioritizer
{
    public List<BriefingItem> Prioritize(List<BriefingItem> items, int maxItems = 15)
    {
        // Sort based on Priority (which is computed by the aggregator based on financial impact, risk, etc.)
        // and ConfidenceScore
        var sorted = items
            .OrderByDescending(i => i.Priority)
            .ThenByDescending(i => i.ConfidenceScore)
            .Take(maxItems)
            .ToList();
            
        return sorted;
    }
}
