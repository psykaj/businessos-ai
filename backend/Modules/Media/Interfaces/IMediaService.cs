using Microsoft.AspNetCore.Http;

namespace backend.Modules.Media.Interfaces;

public interface IMediaService
{
    Task<string> UploadImageAsync(IFormFile file, string folder, Guid organizationId);
    Task DeleteImageAsync(string fileUrl);
}
