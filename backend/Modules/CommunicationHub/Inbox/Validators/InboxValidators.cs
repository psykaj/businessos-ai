using FluentValidation;
using backend.Modules.CommunicationHub.Inbox.DTOs;

namespace backend.Modules.CommunicationHub.Inbox.Validators;

public class InboxSummaryValidator : AbstractValidator<InboxSummaryDto>
{
    public InboxSummaryValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
    }
}
