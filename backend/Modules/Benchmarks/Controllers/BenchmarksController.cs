using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.Benchmarks.DTOs;
using backend.Modules.Benchmarks.Entities;
using backend.Modules.Benchmarks.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Benchmarks.Controllers;

[ApiController]
[Route("api/v1/benchmarks")]
[Authorize]
public class BenchmarksController : ControllerBase
{
    private readonly IBenchmarkRepository _repository;

    public BenchmarksController(IBenchmarkRepository repository)
    {
        _repository = repository;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BenchmarkDto>>> GetAll()
    {
        var orgId = GetOrganizationId();
        var benchmarks = await _repository.GetAllAsync(orgId);
        
        var dtos = new List<BenchmarkDto>();
        foreach (var b in benchmarks) dtos.Add(MapToDto(b));
        return Ok(dtos);
    }

    [HttpGet("{metricName}")]
    public async Task<ActionResult<BenchmarkDto>> GetByMetric(string metricName)
    {
        var orgId = GetOrganizationId();
        var benchmark = await _repository.GetByMetricAsync(metricName, orgId);
        if (benchmark == null) return NotFound();
        return Ok(MapToDto(benchmark));
    }

    private BenchmarkDto MapToDto(Benchmark b)
    {
        return new BenchmarkDto
        {
            Id = b.Id,
            OrganizationId = b.OrganizationId,
            MetricName = b.MetricName,
            IndustryAverage = b.IndustryAverage,
            TopQuartile = b.TopQuartile,
            Industry = b.Industry,
            ValidFrom = b.ValidFrom,
            ValidTo = b.ValidTo,
            Source = b.Source
        };
    }
}
