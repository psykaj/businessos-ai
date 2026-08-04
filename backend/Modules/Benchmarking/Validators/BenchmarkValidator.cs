using FluentValidation;
using backend.Modules.Benchmarking.DTOs;

namespace backend.Modules.Benchmarking.Validators;

public class CreateBenchmarkMetricRequestValidator : AbstractValidator<CreateBenchmarkMetricRequest>
{
    public CreateBenchmarkMetricRequestValidator()
    {
        RuleFor(x => x.MetricName)
            .NotEmpty().WithMessage("MetricName is required.")
            .MaximumLength(150);

        RuleFor(x => x.ComparisonType)
            .NotEmpty().WithMessage("ComparisonType is required.")
            .MaximumLength(100);
    }
}
