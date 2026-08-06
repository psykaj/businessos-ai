using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Queries.GetBusinessHealth;
using backend.Modules.BusinessIntelligence.Queries.GetCustomerInsights;
using backend.Modules.BusinessIntelligence.Queries.GetDashboardSummary;
using backend.Modules.BusinessIntelligence.Queries.GetInventoryInsights;
using backend.Modules.BusinessIntelligence.Queries.GetRecommendations;
using backend.Modules.BusinessIntelligence.Queries.GetRevenueInsights;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Modules.BusinessIntelligence.Controllers;

/// <summary>
/// REST API endpoints for the AI Business Intelligence Engine, providing executive dashboards, proactive AI recommendations, and business health diagnostics.
/// </summary>
[ApiController]
[Route("api/business-intelligence")]
[Produces("application/json")]
public sealed class BusinessIntelligenceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BusinessIntelligenceController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessIntelligenceController"/> class.
    /// </summary>
    /// <param name="mediator">MediatR query dispatcher.</param>
    /// <param name="logger">Structured Serilog logger instance.</param>
    public BusinessIntelligenceController(IMediator mediator, ILogger<BusinessIntelligenceController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private Guid GetEffectiveOrganizationId(Guid? queryOrgId)
    {
        if (queryOrgId.HasValue && queryOrgId.Value != Guid.Empty)
        {
            return queryOrgId.Value;
        }

        var claim = User.FindFirst("OrganizationId")?.Value;
        if (claim != null && Guid.TryParse(claim, out var orgId) && orgId != Guid.Empty)
        {
            return orgId;
        }

        return Guid.Parse("11111111-1111-1111-1111-111111111111");
    }

    /// <summary>
    /// Retrieves the aggregate AI Business Intelligence executive dashboard summary.
    /// </summary>
    /// <param name="organizationId">Optional target organization identifier. Falls back to user JWT claims or standard tenant.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Comprehensive dashboard overview combining health score, revenue, customer, inventory analytics, and top recommendations.</returns>
    /// <response code="200">Returns the computed dashboard summary successfully.</response>
    /// <response code="400">If validation fails or parameters are invalid.</response>
    /// <response code="500">If an unexpected internal exception occurs.</response>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(DashboardSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DashboardSummaryDto>> GetDashboard([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var orgId = GetEffectiveOrganizationId(organizationId);
        _logger.LogInformation("HTTP GET /api/business-intelligence/dashboard invoked for org {OrgId}", orgId);
        var result = await _mediator.Send(new GetDashboardSummaryQuery(orgId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a prioritized list of AI-generated business operational and financial recommendations.
    /// </summary>
    /// <param name="organizationId">Optional target organization identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Collection of actionable recommendations with priority, confidence score, and expected business impact.</returns>
    /// <response code="200">Returns the list of AI recommendations successfully.</response>
    [HttpGet("recommendations")]
    [ProducesResponseType(typeof(List<RecommendationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetRecommendations([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var orgId = GetEffectiveOrganizationId(organizationId);
        _logger.LogInformation("HTTP GET /api/business-intelligence/recommendations invoked for org {OrgId}", orgId);
        var result = await _mediator.Send(new GetRecommendationsQuery(orgId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the evaluated Business Health Score (0-100) and detailed pillar explanations.
    /// </summary>
    /// <param name="organizationId">Optional target organization identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Business health scorecard analyzing revenue, customer growth, cash flow, inventory, pending invoices, and satisfaction.</returns>
    /// <response code="200">Returns the calculated business health scorecard.</response>
    [HttpGet("health")]
    [ProducesResponseType(typeof(BusinessHealthDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BusinessHealthDto>> GetHealth([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var orgId = GetEffectiveOrganizationId(organizationId);
        _logger.LogInformation("HTTP GET /api/business-intelligence/health invoked for org {OrgId}", orgId);
        var result = await _mediator.Send(new GetBusinessHealthQuery(orgId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves deep intelligence and trend analysis regarding business revenue trajectories and repeat customer attribution.
    /// </summary>
    /// <param name="organizationId">Optional target organization identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Revenue insights including week-over-week variance and revenue primary drivers.</returns>
    /// <response code="200">Returns the revenue intelligence insights.</response>
    [HttpGet("revenue")]
    [ProducesResponseType(typeof(RevenueInsightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RevenueInsightDto>> GetRevenue([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var orgId = GetEffectiveOrganizationId(organizationId);
        _logger.LogInformation("HTTP GET /api/business-intelligence/revenue invoked for org {OrgId}", orgId);
        var result = await _mediator.Send(new GetRevenueInsightsQuery(orgId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves customer retention intelligence, repeat purchasing rates, and AI churn prediction warnings.
    /// </summary>
    /// <param name="organizationId">Optional target organization identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Customer insights including high-risk accounts and retention action recommendations.</returns>
    /// <response code="200">Returns customer retention analytics and churn risk list.</response>
    [HttpGet("customers")]
    [ProducesResponseType(typeof(CustomerInsightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerInsightDto>> GetCustomers([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var orgId = GetEffectiveOrganizationId(organizationId);
        _logger.LogInformation("HTTP GET /api/business-intelligence/customers invoked for org {OrgId}", orgId);
        var result = await _mediator.Send(new GetCustomerInsightsQuery(orgId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves operational supply chain intelligence, stockout warnings, and fastest selling product velocity.
    /// </summary>
    /// <param name="organizationId">Optional target organization identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Inventory intelligence insights including low stock item schedules and sales turnover rates.</returns>
    /// <response code="200">Returns inventory velocity and stock analytics.</response>
    [HttpGet("inventory")]
    [ProducesResponseType(typeof(InventoryInsightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InventoryInsightDto>> GetInventory([FromQuery] Guid? organizationId, CancellationToken cancellationToken)
    {
        var orgId = GetEffectiveOrganizationId(organizationId);
        _logger.LogInformation("HTTP GET /api/business-intelligence/inventory invoked for org {OrgId}", orgId);
        var result = await _mediator.Send(new GetInventoryInsightsQuery(orgId), cancellationToken);
        return Ok(result);
    }
}
