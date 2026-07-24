using backend.Modules.AiAgent.CommandEngine.Models;
using backend.Modules.AiAgent.Context.Models;

namespace backend.Modules.AiAgent.CommandEngine.Interfaces;

public interface IAiCommandEngine
{
    Task<CommandParseResult> ParseCommandAsync(
        string userPrompt,
        AiContext context,
        CancellationToken cancellationToken = default);
}
