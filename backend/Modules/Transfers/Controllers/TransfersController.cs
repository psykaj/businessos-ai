using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Modules.Transfers.DTOs;
using backend.Modules.Transfers.Entities;
using backend.Modules.Transfers.Interfaces;
using System.Security.Claims;

namespace backend.Modules.Transfers.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class TransfersController : ControllerBase
{
    private readonly ITransferService _service;

    public TransfersController(ITransferService service)
    {
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var orgClaim = User.FindFirst("OrganizationId")?.Value ?? User.FindFirst("org_id")?.Value;
        if (Guid.TryParse(orgClaim, out var organizationId))
        {
            return organizationId;
        }
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }

    private Guid GetUserId()
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (Guid.TryParse(userClaim, out var userId))
        {
            return userId;
        }
        return Guid.Parse("11111111-1111-1111-1111-111111111111");
    }

    [HttpPost]
    [ProducesResponseType(typeof(WarehouseTransferResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestTransfer([FromBody] CreateTransferRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.RequestTransferAsync(GetOrganizationId(), GetUserId(), dto, cancellationToken);
        return CreatedAtAction(nameof(GetTransferById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WarehouseTransferResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransferById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetTransferByIdAsync(id, GetOrganizationId(), cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id:guid}/approval")]
    [ProducesResponseType(typeof(WarehouseTransferResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessApproval(Guid id, [FromBody] TransferApprovalDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.ProcessApprovalAsync(id, GetOrganizationId(), GetUserId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/tracking")]
    [ProducesResponseType(typeof(WarehouseTransferResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTracking(Guid id, [FromBody] UpdateTransferTrackingDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateTrackingAsync(id, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/receive")]
    [ProducesResponseType(typeof(WarehouseTransferResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReceiveTransfer(Guid id, [FromBody] ReceiveTransferDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.ReceiveTransferAsync(id, GetOrganizationId(), dto, cancellationToken);
        return Ok(result);
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(IEnumerable<WarehouseTransferResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransferHistory([FromQuery] Guid? warehouseId, [FromQuery] TransferStatus? status, CancellationToken cancellationToken)
    {
        var result = await _service.GetHistoryAsync(GetOrganizationId(), warehouseId, status, cancellationToken);
        return Ok(result);
    }
}
