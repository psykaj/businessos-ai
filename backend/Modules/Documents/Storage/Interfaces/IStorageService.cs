namespace backend.Modules.Documents.Storage.Interfaces;

public interface IStorageService
{
    string StorageProviderName { get; }

    Task<string> UploadAsync(Stream stream, string fileName, string contentType, Guid organizationId, CancellationToken ct = default);

    Task<Stream> DownloadAsync(string filePath, CancellationToken ct = default);

    Task<string> GetPresignedUrlAsync(string filePath, TimeSpan expiry, CancellationToken ct = default);

    Task<bool> DeleteAsync(string filePath, CancellationToken ct = default);

    Task<string> CopyAsync(string sourcePath, string destinationPath, CancellationToken ct = default);
}
