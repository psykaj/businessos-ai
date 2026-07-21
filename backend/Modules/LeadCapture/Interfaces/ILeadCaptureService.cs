using backend.Modules.Forms.DTOs;

namespace backend.Modules.LeadCapture.Interfaces;

public interface ILeadCaptureService
{
    Task HandleFormSubmissionAsync(Guid organizationId, Guid formId, SubmitFormDto dto, string device, string browser, CancellationToken cancellationToken = default);
}
