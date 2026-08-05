using FluentValidation;
using backend.Modules.BranchSettings.DTOs;

namespace backend.Modules.BranchSettings.Validators;

public class UpdateBranchConfigurationDtoValidator : AbstractValidator<UpdateBranchConfigurationDto>
{
    public UpdateBranchConfigurationDtoValidator()
    {
        RuleFor(x => x.WorkingHoursJson).NotEmpty();
        RuleFor(x => x.OperationalSettingsJson).NotEmpty();
    }
}
