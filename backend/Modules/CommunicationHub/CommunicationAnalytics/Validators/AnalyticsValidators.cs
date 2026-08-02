using FluentValidation;
using backend.Modules.CommunicationHub.CommunicationAnalytics.DTOs;

namespace backend.Modules.CommunicationHub.CommunicationAnalytics.Validators;

public class SubmitCsatRequestValidator : AbstractValidator<SubmitCsatRequest>
{
    public SubmitCsatRequestValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Feedback).MaximumLength(2000);
    }
}
