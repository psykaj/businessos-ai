using FluentValidation;
using backend.Modules.CommunicationHub.Messages.DTOs;

namespace backend.Modules.CommunicationHub.Messages.Validators;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(5000);
    }
}

public class AddInternalNoteRequestValidator : AbstractValidator<AddInternalNoteRequest>
{
    public AddInternalNoteRequestValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(5000);
    }
}
