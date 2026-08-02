using FluentValidation;
using backend.Modules.CommunicationHub.Conversations.DTOs;

namespace backend.Modules.CommunicationHub.Conversations.Validators;

public class CreateConversationRequestValidator : AbstractValidator<CreateConversationRequest>
{
    public CreateConversationRequestValidator()
    {
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.ChannelType).IsInEnum();
    }
}

public class UpdateConversationStatusRequestValidator : AbstractValidator<UpdateConversationStatusRequest>
{
    public UpdateConversationStatusRequestValidator()
    {
        RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
    }
}

public class AssignConversationRequestValidator : AbstractValidator<AssignConversationRequest>
{
    public AssignConversationRequestValidator()
    {
        RuleFor(x => x.AssignedToUserId).NotEmpty();
        RuleFor(x => x.AssignedToUserName).NotEmpty().MaximumLength(150);
    }
}
