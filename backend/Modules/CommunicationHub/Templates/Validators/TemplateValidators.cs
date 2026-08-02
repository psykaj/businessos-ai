using FluentValidation;
using backend.Modules.CommunicationHub.Templates.DTOs;

namespace backend.Modules.CommunicationHub.Templates.Validators;

public class CreateTemplateRequestValidator : AbstractValidator<CreateTemplateRequest>
{
    public CreateTemplateRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.ShortcutCode).MaximumLength(50);
    }
}

public class UpdateTemplateRequestValidator : AbstractValidator<UpdateTemplateRequest>
{
    public UpdateTemplateRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Content).NotEmpty();
    }
}
