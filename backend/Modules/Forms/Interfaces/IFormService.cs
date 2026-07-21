using backend.Modules.Forms.DTOs;

namespace backend.Modules.Forms.Interfaces;

public interface IFormService
{
    Task<(IReadOnlyList<FormDto> Items, int TotalCount)> GetFormsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default);
    Task<FormDto> GetFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default);
    Task<FormDto> GetFormBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<FormDto> CreateFormAsync(Guid organizationId, CreateFormDto dto, CancellationToken cancellationToken = default);
    Task<FormDto> UpdateFormAsync(Guid organizationId, Guid id, UpdateFormDto dto, CancellationToken cancellationToken = default);
    Task DeleteFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default);
    Task<FormDto> PublishFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default);
    Task<FormDto> DuplicateFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default);
    Task SubmitFormAsync(Guid organizationId, Guid formId, SubmitFormDto dto, string device, string browser, CancellationToken cancellationToken = default);
    Task PublicSubmitFormAsync(Guid formId, SubmitFormDto dto, string device, string browser, CancellationToken cancellationToken = default);
}
