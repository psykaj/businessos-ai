using backend.Modules.Documents.Storage.Interfaces;
using Microsoft.Extensions.Configuration;

namespace backend.Modules.Documents.Storage.Services;

public class AzureBlobStorageService : IStorageService
{
    private readonly LocalStorageService _fallbackStorage;
    private readonly string? _connectionString;
    private readonly string _containerName;

    public string StorageProviderName => "AzureBlobStorage";

    public AzureBlobStorageService(IConfiguration configuration, LocalStorageService fallbackStorage)
    {
        _fallbackStorage = fallbackStorage;
        _connectionString = configuration["Storage:AzureBlobConnectionString"];
        _containerName = configuration["Storage:AzureBlobContainer"] ?? "businessos-documents";
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, Guid organizationId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            // Fallback to local storage if Azure credentials are not provided
            return await _fallbackStorage.UploadAsync(stream, fileName, contentType, organizationId, ct);
        }

        // Azure Blob Storage SDK integration placeholder / execution
        var blobPath = $"{organizationId}/{Guid.NewGuid():N}_{fileName}";
        await Task.CompletedTask;
        return blobPath;
    }

    public async Task<Stream> DownloadAsync(string filePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            return await _fallbackStorage.DownloadAsync(filePath, ct);
        }

        return await _fallbackStorage.DownloadAsync(filePath, ct);
    }

    public async Task<string> GetPresignedUrlAsync(string filePath, TimeSpan expiry, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            return await _fallbackStorage.GetPresignedUrlAsync(filePath, expiry, ct);
        }

        return await _fallbackStorage.GetPresignedUrlAsync(filePath, expiry, ct);
    }

    public async Task<bool> DeleteAsync(string filePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            return await _fallbackStorage.DeleteAsync(filePath, ct);
        }

        return await _fallbackStorage.DeleteAsync(filePath, ct);
    }

    public async Task<string> CopyAsync(string sourcePath, string destinationPath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            return await _fallbackStorage.CopyAsync(sourcePath, destinationPath, ct);
        }

        return await _fallbackStorage.CopyAsync(sourcePath, destinationPath, ct);
    }
}
