using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Interfaces;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/inventory/adjustments")]
public class StockAdjustmentsController : BaseInventoryController
{
    private readonly StockMovementService _stockMovementService;
    private readonly IStockAdjustmentRepository _adjustmentRepository;
    private readonly AutoMapper.IMapper _mapper;

    public StockAdjustmentsController(
        StockMovementService stockMovementService,
        IStockAdjustmentRepository adjustmentRepository,
        AutoMapper.IMapper mapper)
    {
        _stockMovementService = stockMovementService;
        _adjustmentRepository = adjustmentRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAdjustments([FromQuery] Guid? warehouseId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var paged = await _adjustmentRepository.GetPagedAsync(orgId, warehouseId, pageNumber, pageSize, cancellationToken);
        var dto = new Common.PagedResult<StockAdjustmentResponseDto>(
            _mapper.Map<List<StockAdjustmentResponseDto>>(paged.Items),
            paged.TotalCount,
            paged.PageNumber,
            paged.PageSize
        );
        return Ok(dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAdjustmentById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var adjustment = await _adjustmentRepository.GetByIdAsync(id, orgId, cancellationToken);
        if (adjustment == null) return NotFound();
        return Ok(_mapper.Map<StockAdjustmentResponseDto>(adjustment));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdjustment([FromBody] CreateStockAdjustmentDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _stockMovementService.CreateAdjustmentAsync(orgId, dto, userId, cancellationToken);
        return CreatedAtAction(nameof(GetAdjustmentById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApproveAdjustment(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId();
        var result = await _stockMovementService.ApproveAdjustmentAsync(id, orgId, userId, cancellationToken);
        return Ok(result);
    }
}
