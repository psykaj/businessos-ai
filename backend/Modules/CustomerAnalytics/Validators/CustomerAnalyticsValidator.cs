using FluentValidation;
using backend.Modules.CustomerAnalytics.DTOs;

namespace backend.Modules.CustomerAnalytics.Validators;

public class CreateCustomerPerformanceRequestValidator : AbstractValidator<CreateCustomerPerformanceRequest>
{
    public CreateCustomerPerformanceRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName is required.")
            .MaximumLength(200);
    }
}
