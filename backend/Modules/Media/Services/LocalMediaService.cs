using backend.Exceptions;
using backend.Modules.Media.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace backend.Modules.Media.Services;

public class LocalMediaService : IMediaService
{
    private readonly IWebHostEnvironment _env;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".svg", ".ico" };
    private const int MaxFileSizeMB = 5;

    public LocalMediaService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder, Guid organizationId)
    {
        if (file == null || file.Length == 0)
            throw new BadRequestException("File is empty or missing.");

        if (file.Length > MaxFileSizeMB * 1024 * 1024)
            throw new BadRequestException($"File size exceeds {MaxFileSizeMB} MB limit.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext))
            throw new BadRequestException("Invalid file extension.");

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", organizationId.ToString(), folder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative URL path
        return $"/uploads/{organizationId}/{folder}/{uniqueFileName}";
    }

    public Task DeleteImageAsync(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return Task.CompletedTask;

        var relativePath = fileUrl.TrimStart('/');
        var fullPath = Path.Combine(_env.WebRootPath, relativePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}
