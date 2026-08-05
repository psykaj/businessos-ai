using FluentValidation;
using backend.Modules.Branches.DTOs;

namespace backend.Modules.Branches.Validators;

public class CreateBranchDtoValidator : AbstractValidator<CreateBranchDto>
{
    public CreateBranchDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.ContactEmail).EmailAddress().When(x => !string.IsNullOrEmpty(x.ContactEmail));
    }
}

public class UpdateBranchDtoValidator : AbstractValidator<UpdateBranchDto>
{
    public UpdateBranchDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.ContactEmail).EmailAddress().When(x => !string.IsNullOrEmpty(x.ContactEmail));
    }
}

public class AssignManagerDtoValidator : AbstractValidator<AssignManagerDto>
{
    public AssignManagerDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ManagerName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ManagerEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.MaxTransferApprovalLimit).GreaterThanOrEqualTo(0);
    }
}
