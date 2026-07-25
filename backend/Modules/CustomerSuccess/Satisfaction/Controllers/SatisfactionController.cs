using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.Satisfaction.DTOs;
using backend.Modules.CustomerSuccess.Satisfaction.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.Satisfaction.Controllers;

[Route("api/customer-success/satisfaction")]
public class SatisfactionController : BaseCustomerSuccessController
{
    private readonly ISatisfactionService _satisfactionService;

    public SatisfactionController(ISatisfactionService satisfactionService)
    {
        _satisfactionService = satisfactionService;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerFeedbackDto>> Submit([FromBody] SubmitFeedbackDto dto)
    {
        var feedback = await _satisfactionService.SubmitFeedbackAsync(GetOrganizationId(), dto);
        return Ok(feedback);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<SatisfactionSummaryDto>> GetSummary([FromQuery] string? feedbackType)
    {
        var summary = await _satisfactionService.GetSummaryAsync(GetOrganizationId(), feedbackType);
        return Ok(summary);
    }

    [HttpGet("feedback")]
    public async Task<ActionResult> GetPaged(
        [FromQuery] Guid? customerId,
        [FromQuery] int? minRating,
        [FromQuery] int? maxRating,
        [FromQuery] string? feedbackType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _satisfactionService.GetPagedAsync(
            GetOrganizationId(), customerId, minRating, maxRating, feedbackType, page, pageSize);

        return Ok(new { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize });
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<IEnumerable<CustomerFeedbackDto>>> GetByCustomer(Guid customerId)
    {
        var items = await _satisfactionService.GetByCustomerAsync(GetOrganizationId(), customerId);
        return Ok(items);
    }
}
