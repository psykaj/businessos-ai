using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/deals")]
public class DealsController : BaseCrmController
{
    private readonly IDealService _service;

    public DealsController(IDealService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<DealResponseDto>> CreateDeal(CreateDealDto dto)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _service.CreateDealAsync(dto, orgId, userId);
        return CreatedAtAction(nameof(GetDeal), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DealResponseDto>> GetDeal(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetDealAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DealResponseDto>>> GetAllDeals()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllDealsAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DealResponseDto>> UpdateDeal(Guid id, UpdateDealDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateDealAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpPatch("{id}/stage")]
    public async Task<ActionResult<DealResponseDto>> UpdateDealStage(Guid id, UpdateDealStageDto dto)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _service.UpdateDealStageAsync(id, dto.PipelineStage, orgId, userId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeal(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteDealAsync(id, orgId);
        return NoContent();
    }
}
