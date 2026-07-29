namespace backend.Modules.Connectors.Interfaces;

public interface IConnectorProvider
{
    string ProviderName { get; }
    
    Task<bool> AuthenticateAsync(string credentials, CancellationToken cancellationToken = default);
    Task<bool> PushDataAsync(string settings, object payload, CancellationToken cancellationToken = default);
    Task<object> PullDataAsync(string settings, CancellationToken cancellationToken = default);
}
