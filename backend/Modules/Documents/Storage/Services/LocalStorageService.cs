using System.Security.Cryptography;
using System.Text;
using backend.Modules.Documents.Storage.Interfaces;
using Microsoft.Extensions.Configuration;

namespace backend.Modules.Documents.Storage.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _baseStoragePath;
    private readonly string _secretKey;

    public string StorageProviderName => "LocalStorage";

    public LocalStorageService(IConfiguration configuration)
    {
        var configuredPath = configuration["Storage:LocalPath"];
        _baseStoragePath = !string.IsNullOrWhiteSpace(configuredPath)
            ? configuredPath
            : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");

        _secretKey = configuration["Storage:SecretKey"] ?? "BusinessOS_DMS_Secret_Key_2026_Secured!";

        if (!Directory.Exists(_baseStoragePath))
        {
            Directory.CreateDirectory(_baseStoragePath);
        }
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, Guid organizationId, CancellationToken ct = default)
    {
        var orgDir = Path.Combine(_baseStoragePath, organizationId.ToString());
        if (!Directory.Exists(orgDir))
        {
            Directory.CreateDirectory(orgDir);
        }

        var uniqueFileName = $"{Guid.NewGuid():N}_{Path.GetFileName(fileName)}";
        var fullPath = Path.Combine(orgDir, uniqueFileName);
        var relativePath = Path.Combine(organizationId.ToString(), uniqueFileName).Replace('\\', '/');

        using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await stream.CopyToAsync(fileStream, ct);

        return relativePath;
    }

    public Task<Stream> DownloadAsync(string filePath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_baseStoragePath, filePath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File not found at path: {filePath}");
        }

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        return Task.FromResult(stream);
    }

    public Task<string> GetPresignedUrlAsync(string filePath, TimeSpan expiry, CancellationToken ct = default)
    {
        var expiresAt = DateTimeOffset.UtcNow.Add(expiry).ToUnixTimeSeconds();
        var rawData = $"{filePath}:{expiresAt}:{_secretKey}";

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData)));

        var encodedPath = Uri.EscapeDataString(filePath);
        var url = $"/api/documents/secure-download?path={encodedPath}&expires={expiresAt}&token={Uri.EscapeDataString(hash)}";
        return Task.FromResult(url);
    }

    public Task<bool> DeleteAsync(string filePath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_baseStoragePath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public async Task<string> CopyAsync(string sourcePath, string destinationPath, CancellationToken ct = default)
    {
        var fullSource = Path.Combine(_baseStoragePath, sourcePath);
        var fullDest = Path.Combine(_baseStoragePath, destinationPath);

        var destDir = Path.GetDirectoryName(fullDest);
        if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        using var sourceStream = new FileStream(fullSource, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        using var destStream = new FileStream(fullDest, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await sourceStream.CopyToAsync(destStream, ct);

        return destinationPath;
    }
}
