using FluentValidation;
using backend.Modules.CustomerFeedback.ServiceQuality.DTOs;

namespace backend.Modules.CustomerFeedback.ServiceQuality.Validators;

public class RecordServiceMetricRequestValidator : AbstractValidator<RecordServiceMetricRequestDto>
{
    public RecordServiceMetricRequestValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.AverageFirstResponseTimeMinutes).GreaterThanOrEqualTo(0).WithMessage("First response time cannot be negative.");
        RuleFor(x => x.FirstContactResolutionRate).InclusiveBetween(0, 100).WithMessage("FCR rate must be between 0 and 100 percent.");
    }
}
