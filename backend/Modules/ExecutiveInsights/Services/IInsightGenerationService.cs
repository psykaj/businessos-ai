using System;
using System.Threading.Tasks;

namespace backend.Modules.ExecutiveInsights.Services;

public interface IInsightGenerationService
{
    Task GenerateInsightsAsync(Guid organizationId);
}
