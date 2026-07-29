using backend.Modules.Connectors.Interfaces;

namespace backend.Modules.Connectors.Services;

public class ConnectorFactory
{
    private readonly IEnumerable<IConnectorProvider> _providers;

    public ConnectorFactory(IEnumerable<IConnectorProvider> providers)
    {
        _providers = providers;
    }

    public IConnectorProvider GetProvider(string providerName)
    {
        var provider = _providers.FirstOrDefault(p => string.Equals(p.ProviderName, providerName, StringComparison.OrdinalIgnoreCase));
        if (provider == null)
            throw new NotSupportedException($"Connector provider '{providerName}' is not supported.");
            
        return provider;
    }
}
