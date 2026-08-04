using FluentValidation;
using backend.Modules.MarketingROI.DTOs;

namespace backend.Modules.MarketingROI.Validators;

public class CreateMarketingPerformanceRequestValidator : AbstractValidator<CreateMarketingPerformanceRequest>
{
    public CreateMarketingPerformanceRequestValidator()
    {
        RuleFor(x => x.Channel)
            .NotEmpty().WithMessage("Channel is required.")
            .MaximumLength(100);

        RuleFor(x => x.TotalSpend)
            .GreaterThanOrEqualTo(0);
    }
}
