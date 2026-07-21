using backend.Interfaces;
using backend.Modules.Webhooks.DTOs;
using backend.Modules.Webhooks.Entities;
using backend.Modules.Webhooks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Webhooks.Controllers;

[Authorize]
[ApiController]
[Route("api/webhooks/subscriptions")]
public class WebhooksController : ControllerBase
{
    private readonly IWebhookRepository _webhookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WebhooksController(IWebhookRepository webhookRepository, IUnitOfWork unitOfWork)
    {
        _webhookRepository = webhookRepository;
        _unitOfWork = unitOfWork;
    }

    private Guid OrganizationId => Guid.Parse(User.FindFirst("OrganizationId")?.Value ?? Guid.Empty.ToString());

    [HttpGet]
    public async Task<IActionResult> GetSubscriptions(CancellationToken cancellationToken)
    {
        var subs = await _webhookRepository.FindAsync(w => w.OrganizationId == OrganizationId && !w.IsDeleted, cancellationToken);
        var dtos = subs.Select(w => new WebhookSubscriptionDto
        {
            Id = w.Id,
            EventType = w.EventType,
            Url = w.Url,
            IsActive = w.IsActive,
            CreatedAt = w.CreatedAt
        });
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateWebhookSubscriptionDto dto, CancellationToken cancellationToken)
    {
        var sub = new WebhookSubscription
        {
            OrganizationId = OrganizationId,
            EventType = dto.EventType,
            Url = dto.Url,
            Secret = dto.Secret,
            IsActive = true
        };

        await _webhookRepository.AddAsync(sub, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Ok(new WebhookSubscriptionDto
        {
            Id = sub.Id,
            EventType = sub.EventType,
            Url = sub.Url,
            IsActive = sub.IsActive,
            CreatedAt = sub.CreatedAt
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(Guid id, CancellationToken cancellationToken)
    {
        var sub = await _webhookRepository.GetByIdAsync(id, cancellationToken);
        if (sub == null || sub.OrganizationId != OrganizationId) return NotFound();

        _webhookRepository.Delete(sub);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return NoContent();
    }
}
