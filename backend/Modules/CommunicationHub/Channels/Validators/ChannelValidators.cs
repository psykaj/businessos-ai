using FluentValidation;
using backend.Modules.CommunicationHub.Channels.DTOs;

namespace backend.Modules.CommunicationHub.Channels.Validators;

public class CreateChannelRequestValidator : AbstractValidator<CreateChannelRequest>
{
    public CreateChannelRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ChannelType).IsInEnum();
        RuleFor(x => x.ProviderIdentifier).MaximumLength(255);
    }
}

public class UpdateChannelRequestValidator : AbstractValidator<UpdateChannelRequest>
{
    public UpdateChannelRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ProviderIdentifier).MaximumLength(255);
    }
}
