using backend.Modules.ActionCenter.Commands;
using FluentValidation;

namespace backend.Modules.ActionCenter.Validators;

public class ExecuteActionCommandValidator : AbstractValidator<ExecuteActionCommand>
{
    public ExecuteActionCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("Organization ID is required.");
        RuleFor(x => x.ActionId).NotEmpty().WithMessage("Action ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}

public class ApproveActionCommandValidator : AbstractValidator<ApproveActionCommand>
{
    public ApproveActionCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("Organization ID is required.");
        RuleFor(x => x.ActionId).NotEmpty().WithMessage("Action ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}

public class RejectActionCommandValidator : AbstractValidator<RejectActionCommand>
{
    public RejectActionCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("Organization ID is required.");
        RuleFor(x => x.ActionId).NotEmpty().WithMessage("Action ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}
