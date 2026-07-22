using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Actions;

public class ActionRegistry
{
    private readonly Dictionary<ActionType, IActionHandler> _handlers;

    public ActionRegistry(IEnumerable<IActionHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.ActionType, h => h);
    }

    public IActionHandler? GetHandler(ActionType actionType)
    {
        return _handlers.TryGetValue(actionType, out var handler) ? handler : null;
    }
}
