using FluentValidation;
using backend.Modules.BusinessPerformance.DTOs;

namespace backend.Modules.BusinessPerformance.Validators;

public class CreateBusinessMetricRequestValidator : AbstractValidator<CreateBusinessMetricRequest>
{
    public CreateBusinessMetricRequestValidator()
    {
        RuleFor(x => x.MetricType)
            .NotEmpty().WithMessage("MetricType is required.")
            .MaximumLength(100).WithMessage("MetricType cannot exceed 100 characters.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(50).WithMessage("Category cannot exceed 50 characters.");

        RuleFor(x => x.Unit)
            .MaximumLength(20).WithMessage("Unit cannot exceed 20 characters.");
    }
}

public class UpdateBusinessMetricRequestValidator : AbstractValidator<UpdateBusinessMetricRequest>
{
    public UpdateBusinessMetricRequestValidator()
    {
        RuleFor(x => x.Unit)
            .MaximumLength(20).WithMessage("Unit cannot exceed 20 characters.");
    }
}
