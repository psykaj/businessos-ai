using backend.Interfaces;
using backend.Modules.Forms.Entities;

namespace backend.Modules.Forms.Interfaces;

public interface ISubmissionRepository : IGenericRepository<FormSubmission>
{
    Task<(IReadOnlyList<FormSubmission> Items, int TotalCount)> GetSubmissionsPagedAsync(Guid organizationId, Guid formId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetSubmissionCountAsync(Guid organizationId, Guid formId, CancellationToken cancellationToken = default);
}
