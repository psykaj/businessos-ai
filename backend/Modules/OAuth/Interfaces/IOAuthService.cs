using backend.Modules.OAuth.Entities;

namespace backend.Modules.OAuth.Interfaces;

public interface IOAuthService
{
    Task<OAuthApplication> RegisterApplicationAsync(Guid organizationId, string name, string description, string homepageUrl, string redirectUris, string allowedScopes, CancellationToken cancellationToken = default);
    Task<OAuthApplication?> GetApplicationByClientIdAsync(string clientId, CancellationToken cancellationToken = default);
    Task<bool> ValidateClientCredentialsAsync(string clientId, string clientSecret, CancellationToken cancellationToken = default);
}
