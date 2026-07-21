using backend.Interfaces;
using backend.Modules.Forms.Entities;

namespace backend.Modules.Forms.Interfaces;

public interface IFormRepository : IGenericRepository<Form>
{
    Task<(IReadOnlyList<Form> Items, int TotalCount)> GetFormsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default);
    Task<Form?> GetFormBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Form?> GetFormWithFieldsAsync(Guid id, CancellationToken cancellationToken = default);
}
