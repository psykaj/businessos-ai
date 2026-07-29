using System.Security.Cryptography;
using System.Text;
using backend.Interfaces;
using backend.Modules.OAuth.Entities;
using backend.Modules.OAuth.Interfaces;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;

namespace backend.Modules.OAuth.Services;

public class OAuthService : IOAuthService
{
    private readonly ApplicationDbContext _dbContext;

    public OAuthService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OAuthApplication> RegisterApplicationAsync(Guid organizationId, string name, string description, string homepageUrl, string redirectUris, string allowedScopes, CancellationToken cancellationToken = default)
    {
        var clientId = GenerateClientId();
        var clientSecret = GenerateClientSecret();
        var clientSecretHash = HashSecret(clientSecret);

        var app = new OAuthApplication
        {
            OrganizationId = organizationId,
            Name = name,
            Description = description,
            HomepageUrl = homepageUrl,
            ClientId = clientId,
            ClientSecretHash = clientSecretHash,
            RedirectUris = redirectUris,
            AllowedScopes = allowedScopes,
            Status = "Active"
        };

        _dbContext.OAuthApplications.Add(app);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // For the response, we would typically return the plain text secret only once.
        // Returning the entity here; the controller should map it to a DTO and include the plain secret.
        // For simplicity in this foundation, we're returning the entity.
        // A real implementation would use a DTO.
        return app; 
    }

    public async Task<OAuthApplication?> GetApplicationByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.OAuthApplications
            .FirstOrDefaultAsync(a => a.ClientId == clientId, cancellationToken);
    }

    public async Task<bool> ValidateClientCredentialsAsync(string clientId, string clientSecret, CancellationToken cancellationToken = default)
    {
        var app = await GetApplicationByClientIdAsync(clientId, cancellationToken);
        if (app == null || app.Status != "Active")
            return false;

        var hashedSecret = HashSecret(clientSecret);
        return app.ClientSecretHash == hashedSecret;
    }

    private static string GenerateClientId()
    {
        return "client_" + Guid.NewGuid().ToString("N");
    }

    private static string GenerateClientSecret()
    {
        var secretBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(secretBytes);
        }
        return "secret_" + Convert.ToBase64String(secretBytes).Replace("+", "").Replace("/", "").Replace("=", "");
    }

    private static string HashSecret(string secret)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(secret));
        return Convert.ToBase64String(hashedBytes);
    }
}
