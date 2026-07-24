using backend.Modules.AiAgent.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.Repositories;

public class ToolRepository : IToolRepository
{
    private readonly ApplicationDbContext _context;

    public ToolRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ToolDefinition?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.ToolDefinitions
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower() && !t.IsDeleted, cancellationToken);
    }

    public async Task<List<ToolDefinition>> GetAllAsync(bool enabledOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _context.ToolDefinitions.AsNoTracking().Where(t => !t.IsDeleted);

        if (enabledOnly)
        {
            query = query.Where(t => t.Enabled);
        }

        return await query.OrderBy(t => t.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ToolDefinition tool, CancellationToken cancellationToken = default)
    {
        await _context.ToolDefinitions.AddAsync(tool, cancellationToken);
    }

    public void Update(ToolDefinition tool)
    {
        _context.ToolDefinitions.Update(tool);
    }

    public async Task UpsertToolAsync(ToolDefinition tool, CancellationToken cancellationToken = default)
    {
        var existing = await _context.ToolDefinitions
            .FirstOrDefaultAsync(t => t.Name.ToLower() == tool.Name.ToLower(), cancellationToken);

        if (existing == null)
        {
            await _context.ToolDefinitions.AddAsync(tool, cancellationToken);
        }
        else
        {
            existing.Description = tool.Description;
            existing.Category = tool.Category;
            existing.RequiredPermissions = tool.RequiredPermissions;
            existing.Enabled = tool.Enabled;
            existing.IsDestructive = tool.IsDestructive;
            existing.ParametersSchema = tool.ParametersSchema;
            _context.ToolDefinitions.Update(existing);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
