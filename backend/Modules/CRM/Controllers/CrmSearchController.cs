using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/search")]
public class CrmSearchController : BaseCrmController
{
    private readonly ICrmSearchService _service;

    public CrmSearchController(ICrmSearchService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GlobalSearchResultDto>>> Search([FromQuery] string query)
    {
        var orgId = GetOrganizationId();
        var result = await _service.SearchAsync(query, orgId);
        return Ok(result);
    }
}
