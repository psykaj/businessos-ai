using FluentValidation;
using backend.Modules.CustomerFeedback.ImprovementActions.DTOs;

namespace backend.Modules.CustomerFeedback.ImprovementActions.Validators;

public class GenerateRecommendationsRequestValidator : AbstractValidator<GenerateRecommendationsRequestDto>
{
    public GenerateRecommendationsRequestValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
    }
}
