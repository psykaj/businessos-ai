using Microsoft.AspNetCore.Http;

namespace backend.Modules.Expenses.Interfaces;

public interface IReceiptStorageService
{
    Task<string> UploadReceiptAsync(IFormFile file, Guid organizationId);
}
