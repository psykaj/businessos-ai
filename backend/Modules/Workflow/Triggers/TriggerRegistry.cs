using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Triggers;

public class TriggerRegistry
{
    private readonly Dictionary<TriggerType, ITriggerHandler> _handlers;

    public TriggerRegistry(IEnumerable<ITriggerHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.TriggerType, h => h);
    }

    public ITriggerHandler? GetHandler(TriggerType type)
    {
        return _handlers.TryGetValue(type, out var handler) ? handler : null;
    }
}
