using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Integrations;

public class IntegrationRegistry
{
    private readonly Dictionary<IntegrationProvider, IIntegrationProvider> _providers;

    public IntegrationRegistry(IEnumerable<IIntegrationProvider> providers)
    {
        _providers = providers.ToDictionary(p => p.Provider, p => p);
    }

    public IIntegrationProvider? GetProvider(IntegrationProvider provider)
    {
        return _providers.TryGetValue(provider, out var impl) ? impl : null;
    }
}
