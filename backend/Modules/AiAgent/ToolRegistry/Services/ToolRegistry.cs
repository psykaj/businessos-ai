using System.Collections.Concurrent;
using backend.Modules.AiAgent.Entities;
using backend.Modules.AiAgent.Repositories;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace backend.Modules.AiAgent.ToolRegistry.Services;

public class ToolRegistry : IToolRegistry
{
    private readonly ConcurrentDictionary<string, ITool> _tools = new(StringComparer.OrdinalIgnoreCase);
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ToolRegistry> _logger;

    public ToolRegistry(
        IEnumerable<ITool> tools,
        IServiceProvider serviceProvider,
        ILogger<ToolRegistry> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        foreach (var tool in tools)
        {
            RegisterTool(tool);
        }
    }

    public void RegisterTool(ITool tool)
    {
        _tools[tool.Name] = tool;
        _logger.LogInformation("Tool registered: {ToolName} ({Category})", tool.Name, tool.Category);
    }

    public ITool? GetTool(string name)
    {
        _tools.TryGetValue(name, out var tool);
        return tool;
    }

    public IReadOnlyCollection<ITool> GetAllTools()
    {
        return _tools.Values.ToList().AsReadOnly();
    }

    public async Task SyncToolDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var toolRepo = scope.ServiceProvider.GetRequiredService<IToolRepository>();

        foreach (var tool in _tools.Values)
        {
            var definition = new ToolDefinition
            {
                Name = tool.Name,
                Description = tool.Description,
                Category = tool.Category,
                RequiredPermissions = string.Join(",", tool.RequiredPermissions),
                Enabled = true,
                IsDestructive = tool.IsDestructive,
                ParametersSchema = tool.ParametersSchema
            };

            await toolRepo.UpsertToolAsync(definition, cancellationToken);
        }

        await toolRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<ToolResult> ExecuteToolAsync(
        string toolName,
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var tool = GetTool(toolName);
        if (tool == null)
        {
            return ToolResult.Fail($"Tool '{toolName}' is not registered in ToolRegistry.");
        }

        try
        {
            return await tool.ExecuteAsync(context, parameters, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing tool {ToolName}", toolName);
            return ToolResult.Fail($"Tool execution error: {ex.Message}");
        }
    }
}
