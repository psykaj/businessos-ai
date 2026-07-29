using System.Security.Claims;
using System.Text.Encodings.Web;
using backend.Modules.ApiKeys.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace backend.Modules.ApiPlatform.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ApiKey";
    public string HeaderName { get; set; } = "X-Api-Key";
}

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IServiceProvider _serviceProvider;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IServiceProvider serviceProvider) 
        : base(options, logger, encoder)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues))
        {
            return AuthenticateResult.NoResult();
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            return AuthenticateResult.NoResult();
        }

        // Ideally, we'd hash the provided key and check the database.
        // For performance, we'd cache this in Redis.
        // For this foundation, we'll use a scoped service to validate it (pseudo-code logic).
        
        using var scope = _serviceProvider.CreateScope();
        // var apiKeyService = scope.ServiceProvider.GetRequiredService<IApiKeyService>();
        // var isValid = await apiKeyService.ValidateKeyAsync(providedApiKey);
        
        // This is simplified. In a real system, we look up the key hash, verify it's active,
        // and fetch the OrganizationId and Scopes.
        
        // Assuming validation passed:
        var organizationId = Guid.NewGuid().ToString(); // Mocked
        
        var claims = new[] 
        {
            new Claim(ClaimTypes.NameIdentifier, providedApiKey),
            new Claim("OrganizationId", organizationId)
        };
        var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.DefaultScheme);
        var identities = new List<ClaimsIdentity> { identity };
        var principal = new ClaimsPrincipal(identities);
        var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.DefaultScheme);

        return AuthenticateResult.Success(ticket);
    }
}
