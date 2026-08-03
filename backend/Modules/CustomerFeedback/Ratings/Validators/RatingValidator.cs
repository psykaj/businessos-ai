using FluentValidation;
using backend.Modules.CustomerFeedback.Ratings.DTOs;

namespace backend.Modules.CustomerFeedback.Ratings.Validators;

public class SubmitRatingRequestValidator : AbstractValidator<SubmitRatingRequestDto>
{
    public SubmitRatingRequestValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.EntityId).NotEmpty().WithMessage("EntityId is required.");
        RuleFor(x => x.RatingScore)
            .InclusiveBetween(1, 10)
            .WithMessage("Rating score must be between 1 and 10.");
    }
}
