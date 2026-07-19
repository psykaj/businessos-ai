using System.Security.Cryptography;
using System.Text;
using backend.Entities;
using backend.Exceptions;
using backend.Interfaces;
using backend.Modules.ApiKeys.DTOs;
using backend.Modules.ApiKeys.Interfaces;
using backend.Modules.ApiKeys.Repositories;

namespace backend.Modules.ApiKeys.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApiKeyService(IApiKeyRepository apiKeyRepository, IUnitOfWork unitOfWork)
    {
        _apiKeyRepository = apiKeyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiKeyResponseDto> GenerateApiKeyAsync(Guid organizationId, string name, CancellationToken cancellationToken = default)
    {
        var rawKey = GenerateSecureKey();
        var keyHash = HashKey(rawKey);

        var apiKey = new ApiKey
        {
            OrganizationId = organizationId,
            Name = name,
            KeyHash = keyHash,
            Status = "Active"
        };

        await _apiKeyRepository.AddAsync(apiKey, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ApiKeyResponseDto
        {
            Id = apiKey.Id,
            Name = apiKey.Name,
            Key = rawKey // Only returned this one time
        };
    }

    public async Task<IReadOnlyList<ApiKeyDto>> ListApiKeysAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var keys = await _apiKeyRepository.GetByOrganizationAsync(organizationId, cancellationToken);
        return keys.Select(k => new ApiKeyDto
        {
            Id = k.Id,
            OrganizationId = k.OrganizationId,
            Name = k.Name,
            Status = k.Status,
            LastUsedAt = k.LastUsedAt,
            CreatedAt = k.CreatedAt
        }).ToList();
    }

    public async Task RevokeApiKeyAsync(Guid organizationId, Guid keyId, CancellationToken cancellationToken = default)
    {
        var key = await _apiKeyRepository.GetByIdAsync(keyId, cancellationToken);
        if (key == null || key.OrganizationId != organizationId)
            throw new NotFoundException("API Key not found.");

        key.Status = "Revoked";
        _apiKeyRepository.Update(key);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<ApiKeyResponseDto> RotateApiKeyAsync(Guid organizationId, Guid keyId, CancellationToken cancellationToken = default)
    {
        var key = await _apiKeyRepository.GetByIdAsync(keyId, cancellationToken);
        if (key == null || key.OrganizationId != organizationId)
            throw new NotFoundException("API Key not found.");

        // Revoke old key
        key.Status = "Revoked";
        _apiKeyRepository.Update(key);

        // Generate new key with same name
        var newRawKey = GenerateSecureKey();
        var newKeyHash = HashKey(newRawKey);

        var newApiKey = new ApiKey
        {
            OrganizationId = organizationId,
            Name = key.Name,
            KeyHash = newKeyHash,
            Status = "Active"
        };

        await _apiKeyRepository.AddAsync(newApiKey, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ApiKeyResponseDto
        {
            Id = newApiKey.Id,
            Name = newApiKey.Name,
            Key = newRawKey
        };
    }

    private static string GenerateSecureKey()
    {
        var keyBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }
        return "bos_" + Convert.ToBase64String(keyBytes).Replace("+", "").Replace("/", "").Replace("=", "");
    }

    private static string HashKey(string key)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        return Convert.ToBase64String(hashedBytes);
    }
}
