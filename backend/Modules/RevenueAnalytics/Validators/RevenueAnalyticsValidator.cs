using FluentValidation;
using backend.Modules.RevenueAnalytics.DTOs;

namespace backend.Modules.RevenueAnalytics.Validators;

public class CreateRevenueSnapshotRequestValidator : AbstractValidator<CreateRevenueSnapshotRequest>
{
    public CreateRevenueSnapshotRequestValidator()
    {
        RuleFor(x => x.PeriodType)
            .NotEmpty().WithMessage("PeriodType is required.")
            .MaximumLength(20);

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .MaximumLength(10);
    }
}
