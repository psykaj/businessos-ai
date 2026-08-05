using FluentValidation;
using backend.Modules.BranchAnalytics.DTOs;

namespace backend.Modules.BranchAnalytics.Validators;

public class CalculatePerformanceRequestDtoValidator : AbstractValidator<CalculatePerformanceRequestDto>
{
    public CalculatePerformanceRequestDtoValidator()
    {
        RuleFor(x => x.Year).InclusiveBetween(2020, 2100);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
    }
}
