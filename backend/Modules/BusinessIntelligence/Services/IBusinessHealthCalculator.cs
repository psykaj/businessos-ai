using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Services;

/// <summary>
/// Service interface responsible for calculating holistic business health scores and dimension diagnostics.
/// </summary>
public interface IBusinessHealthCalculator
{
    /// <summary>
    /// Asynchronously calculates the aggregate business health score (0-100) and provides diagnostic explanations across key business pillars.
    /// </summary>
    /// <param name="organizationId">The organization identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A comprehensive business health scorecard.</returns>
    Task<BusinessHealthDto> CalculateHealthAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
