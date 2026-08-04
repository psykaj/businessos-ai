using FluentValidation;
using backend.Modules.ProductAnalytics.DTOs;

namespace backend.Modules.ProductAnalytics.Validators;

public class CreateProductPerformanceRequestValidator : AbstractValidator<CreateProductPerformanceRequest>
{
    public CreateProductPerformanceRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("ProductName is required.")
            .MaximumLength(200);

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0);
    }
}
