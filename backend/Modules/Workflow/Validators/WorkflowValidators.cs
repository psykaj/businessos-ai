using backend.Modules.Workflow.DTOs;
using FluentValidation;

namespace backend.Modules.Workflow.Validators;

public class CreateWorkflowDtoValidator : AbstractValidator<CreateWorkflowDto>
{
    public CreateWorkflowDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}

public class CreateIntegrationDtoValidator : AbstractValidator<CreateIntegrationDto>
{
    public CreateIntegrationDtoValidator()
    {
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Provider).IsInEnum();
    }
}
