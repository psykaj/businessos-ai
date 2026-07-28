using backend.Modules.Expenses.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace backend.Modules.Expenses.Services;

public class LocalReceiptStorageService : IReceiptStorageService
{
    private readonly IWebHostEnvironment _environment;

    public LocalReceiptStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadReceiptAsync(IFormFile file, Guid organizationId)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file.");
        }

        var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads", "receipts", organizationId.ToString());
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/receipts/{organizationId}/{fileName}";
    }
}
