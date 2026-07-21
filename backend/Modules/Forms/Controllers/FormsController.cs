using backend.Modules.Forms.DTOs;
using backend.Modules.Forms.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Forms.Controllers;

[Authorize]
[ApiController]
[Route("api/forms")]
public class FormsController : ControllerBase
{
    private readonly IFormService _formService;

    public FormsController(IFormService formService)
    {
        _formService = formService;
    }

    private Guid OrganizationId => Guid.Parse(User.FindFirst("OrganizationId")?.Value ?? Guid.Empty.ToString());

    [HttpGet]
    public async Task<IActionResult> GetForms([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] string? status = null, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _formService.GetFormsPagedAsync(OrganizationId, page, pageSize, search, status, cancellationToken);
        return Ok(new { items, totalCount, page, pageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetForm(Guid id, CancellationToken cancellationToken)
    {
        var form = await _formService.GetFormAsync(OrganizationId, id, cancellationToken);
        return Ok(form);
    }

    [HttpPost]
    public async Task<IActionResult> CreateForm([FromBody] CreateFormDto dto, CancellationToken cancellationToken)
    {
        var form = await _formService.CreateFormAsync(OrganizationId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateForm(Guid id, [FromBody] UpdateFormDto dto, CancellationToken cancellationToken)
    {
        var form = await _formService.UpdateFormAsync(OrganizationId, id, dto, cancellationToken);
        return Ok(form);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteForm(Guid id, CancellationToken cancellationToken)
    {
        await _formService.DeleteFormAsync(OrganizationId, id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id}/publish")]
    public async Task<IActionResult> PublishForm(Guid id, CancellationToken cancellationToken)
    {
        var form = await _formService.PublishFormAsync(OrganizationId, id, cancellationToken);
        return Ok(form);
    }

    [HttpPost("{id}/duplicate")]
    public async Task<IActionResult> DuplicateForm(Guid id, CancellationToken cancellationToken)
    {
        var form = await _formService.DuplicateFormAsync(OrganizationId, id, cancellationToken);
        return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
    }
}
