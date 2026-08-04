using FluentValidation;
using backend.Modules.GrowthRecommendations.DTOs;

namespace backend.Modules.GrowthRecommendations.Validators;

public class CreateGrowthRecommendationRequestValidator : AbstractValidator<CreateGrowthRecommendationRequest>
{
    public CreateGrowthRecommendationRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.RecommendationType)
            .NotEmpty().WithMessage("RecommendationType is required.")
            .MaximumLength(100);
    }
}

public class UpdateRecommendationStatusRequestValidator : AbstractValidator<UpdateRecommendationStatusRequest>
{
    public UpdateRecommendationStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => s == "Open" || s == "In-Progress" || s == "Actioned" || s == "Dismissed")
            .WithMessage("Status must be one of: Open, In-Progress, Actioned, Dismissed.");
    }
}
