using FluentValidation;
using backend.Modules.CustomerFeedback.Feedback.DTOs;

namespace backend.Modules.CustomerFeedback.Feedback.Validators;

public class SubmitFeedbackRequestValidator : AbstractValidator<SubmitFeedbackRequestDto>
{
    public SubmitFeedbackRequestValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment or feedback content cannot be empty.");
        RuleFor(x => x.RatingValue)
            .InclusiveBetween(1, 10)
            .When(x => x.RatingValue.HasValue)
            .WithMessage("Rating value must be between 1 and 10.");
    }
}
