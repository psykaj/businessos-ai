using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.CustomerSegments.DTOs;
using backend.Modules.CustomerSuccess.CustomerSegments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.CustomerSegments.Controllers;

[Route("api/customer-success/segments")]
public class CustomerSegmentsController : BaseCustomerSuccessController
{
    private readonly ICustomerSegmentService _segmentService;

    public CustomerSegmentsController(ICustomerSegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerSegmentDto>>> GetSegments()
    {
        var segments = await _segmentService.GetSegmentsAsync(GetOrganizationId());
        return Ok(segments);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerSegmentDto>> GetById(Guid id)
    {
        var segment = await _segmentService.GetSegmentByIdAsync(GetOrganizationId(), id);
        return Ok(segment);
    }

    [HttpGet("members/{segmentName}")]
    public async Task<ActionResult<IEnumerable<CustomerSegmentMemberDto>>> GetMembers(string segmentName)
    {
        var members = await _segmentService.GetSegmentMembersAsync(GetOrganizationId(), segmentName);
        return Ok(members);
    }

    [HttpPost("recalculate")]
    public async Task<ActionResult> Recalculate()
    {
        await _segmentService.RecalculateSegmentsAsync(GetOrganizationId());
        return Ok(new { Message = "Customer segments recalculated successfully." });
    }
}
