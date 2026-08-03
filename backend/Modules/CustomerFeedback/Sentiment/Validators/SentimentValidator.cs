using FluentValidation;
using backend.Modules.CustomerFeedback.Sentiment.DTOs;

namespace backend.Modules.CustomerFeedback.Sentiment.Validators;

public class SentimentAnalyzeRequestValidator : AbstractValidator<SentimentAnalyzeRequestDto>
{
    public SentimentAnalyzeRequestValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.TargetEntityId).NotEmpty().WithMessage("TargetEntityId is required.");
        RuleFor(x => x.TextToAnalyze).NotEmpty().WithMessage("TextToAnalyze cannot be empty.");
    }
}
