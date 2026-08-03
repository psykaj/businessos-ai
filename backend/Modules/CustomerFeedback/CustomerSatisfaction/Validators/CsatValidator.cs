using FluentValidation;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.DTOs;

namespace backend.Modules.CustomerFeedback.CustomerSatisfaction.Validators;

public class CalculateCsatRequestValidator : AbstractValidator<CalculateCsatRequestDto>
{
    public CalculateCsatRequestValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
    }
}
