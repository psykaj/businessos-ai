using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Entities;
using System.Collections.Generic;

namespace backend.Modules.AutomationStudio.WorkflowTemplates.Controllers;

[ApiController]
[Route("api/automation-studio/templates")]
public class WorkflowTemplatesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public WorkflowTemplatesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkflowTemplate>>> GetTemplates()
    {
        var templates = await _dbContext.WorkflowTemplates
            .AsNoTracking()
            .ToListAsync();
            
        return Ok(templates);
    }

    [HttpPost("{id}/clone")]
    public async Task<ActionResult> CloneTemplate(Guid id, [FromQuery] Guid organizationId)
    {
        var template = await _dbContext.WorkflowTemplates.FindAsync(id);
        if (template == null) return NotFound();

        var newWorkflow = new AutomationWorkflow
        {
            OrganizationId = organizationId,
            Name = $"Clone of {template.Name}",
            Description = template.Description,
            Status = "Draft",
            IsActive = false
        };

        _dbContext.AutomationWorkflows.Add(newWorkflow);
        await _dbContext.SaveChangesAsync();

        return Ok(newWorkflow);
    }
}
