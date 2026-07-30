using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.BusinessHealth.DTOs;
using backend.Modules.BusinessHealth.Entities;
using backend.Modules.BusinessHealth.Repositories;
using backend.Modules.BusinessHealth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessHealth.Controllers;

[ApiController]
[Route("api/v1/business-health")]
[Authorize]
public class BusinessHealthController : ControllerBase
{
    private readonly IBusinessHealthRepository _repository;
    private readonly BusinessHealthService _service;

    public BusinessHealthController(IBusinessHealthRepository repository, BusinessHealthService service)
    {
        _repository = repository;
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet("latest")]
    public async Task<ActionResult<BusinessHealthScoreDto>> GetLatest()
    {
        var orgId = GetOrganizationId();
        var score = await _repository.GetLatestAsync(orgId);
        if (score == null) return NotFound();
        return Ok(MapToDto(score));
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<BusinessHealthScoreDto>>> GetHistory([FromQuery] int limit = 12)
    {
        var orgId = GetOrganizationId();
        var history = await _repository.GetHistoryAsync(orgId, limit);
        
        var dtos = new List<BusinessHealthScoreDto>();
        foreach (var h in history) dtos.Add(MapToDto(h));
        return Ok(dtos);
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<BusinessHealthScoreDto>> CalculateHealth()
    {
        var orgId = GetOrganizationId();
        var score = await _service.CalculateHealthAsync(orgId);
        return Ok(MapToDto(score));
    }

    private BusinessHealthScoreDto MapToDto(BusinessHealthScore s)
    {
        return new BusinessHealthScoreDto
        {
            Id = s.Id,
            OrganizationId = s.OrganizationId,
            OverallScore = s.OverallScore,
            FinancialHealth = s.FinancialHealth,
            OperationalHealth = s.OperationalHealth,
            CustomerHealth = s.CustomerHealth,
            CalculatedAt = s.CalculatedAt,
            Status = s.Status
        };
    }
}
