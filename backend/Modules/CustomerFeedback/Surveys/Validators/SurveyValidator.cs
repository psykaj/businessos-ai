using FluentValidation;
using backend.Modules.CustomerFeedback.Surveys.DTOs;

namespace backend.Modules.CustomerFeedback.Surveys.Validators;

public class CreateSurveyRequestValidator : AbstractValidator<CreateSurveyRequestDto>
{
    public CreateSurveyRequestValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Survey Title cannot be empty.");
        RuleFor(x => x.Questions).NotEmpty().WithMessage("A survey must contain at least one question.");
    }
}

public class SubmitSurveyResponseValidator : AbstractValidator<SubmitSurveyResponseDto>
{
    public SubmitSurveyResponseValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.AnswersJson).NotEmpty().WithMessage("AnswersJson is required.");
    }
}
