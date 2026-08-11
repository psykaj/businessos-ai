using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.BusinessMemory.DTOs;
using backend.Modules.BusinessMemory.Interfaces;

namespace backend.Modules.BusinessMemory.Controllers;

[ApiController]
[Route("api/memory")]
[Authorize]
public class MemoryController : ControllerBase
{
    private readonly IMemoryService _memoryService;

    public MemoryController(IMemoryService memoryService)
    {
        _memoryService = memoryService;
    }

    private Guid GetOrganizationId()
    {
        var orgIdClaim = User.FindFirst("organizationId")?.Value;
        if (Guid.TryParse(orgIdClaim, out var orgId)) return orgId;

        // Fallback for dev/test
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(subClaim)) return Guid.Parse("00000000-0000-0000-0000-000000000001");

        throw new UnauthorizedAccessException("Organization ID not found in token context.");
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
               ?? User.FindFirst("sub")?.Value 
               ?? "system_user";
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemoryDto>>> GetBusinessMemories(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var memories = await _memoryService.GetBusinessMemoriesAsync(orgId, cancellationToken);
        return Ok(memories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemoryDto>> GetMemory(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var memory = await _memoryService.GetMemoryAsync(orgId, id, cancellationToken);
        return Ok(memory);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<MemoryDto>>> GetCustomerMemories(string customerId, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var memories = await _memoryService.GetCustomerMemoriesAsync(orgId, customerId, cancellationToken);
        return Ok(memories);
    }

    [HttpGet("relevant")]
    public async Task<ActionResult<IEnumerable<MemoryDto>>> GetRelevantMemories([FromQuery] string query, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var memories = await _memoryService.GetRelevantMemoriesAsync(orgId, query, cancellationToken);
        return Ok(memories);
    }

    [HttpPost]
    public async Task<ActionResult<MemoryDto>> CreateMemory([FromBody] CreateMemoryDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var memory = await _memoryService.CreateMemoryAsync(orgId, request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetMemory), new { id = memory.Id }, memory);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MemoryDto>> UpdateMemory(Guid id, [FromBody] UpdateMemoryDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        var memory = await _memoryService.UpdateMemoryAsync(orgId, id, request, userId, cancellationToken);
        return Ok(memory);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMemory(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetUserId();
        await _memoryService.DeleteMemoryAsync(orgId, id, userId, cancellationToken);
        return NoContent();
    }

    [HttpPost("rebuild-context/{entityType}/{entityId}")]
    public async Task<ActionResult> RebuildContext(string entityType, string entityId, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        await _memoryService.RebuildContextAsync(orgId, entityType, entityId, cancellationToken);
        return Accepted(); // Processed asynchronously typically
    }
}
