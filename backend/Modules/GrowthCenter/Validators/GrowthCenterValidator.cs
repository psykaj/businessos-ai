using FluentValidation;
using backend.Modules.GrowthCenter.DTOs;

namespace backend.Modules.GrowthCenter.Validators;

public class GrowthCenterRefreshRequestValidator : AbstractValidator<GrowthCenterRefreshRequest>
{
    public GrowthCenterRefreshRequestValidator()
    {
        // Simple validator to ensure clean pipeline integration
        RuleFor(x => x.ForceRecalculation).NotNull();
    }
}
