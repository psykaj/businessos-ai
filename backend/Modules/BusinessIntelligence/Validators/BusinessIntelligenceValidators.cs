using backend.Modules.BusinessIntelligence.DTOs;
using FluentValidation;

namespace backend.Modules.BusinessIntelligence.Validators;

public class CreateGoalDtoValidator : AbstractValidator<CreateGoalDto>
{
    public CreateGoalDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TargetValue).GreaterThan(0);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
    }
}

public class GenerateReportRequestDtoValidator : AbstractValidator<GenerateReportRequestDto>
{
    public GenerateReportRequestDtoValidator()
    {
        RuleFor(x => x.ReportType).NotEmpty();
        RuleFor(x => x.Format).Must(f => string.IsNullOrEmpty(f) || new[] { "JSON", "PDF", "EXCEL", "CSV" }.Contains(f.ToUpper()))
            .WithMessage("Format must be JSON, PDF, Excel, or CSV.");
    }
}

public class DashboardFilterDtoValidator : AbstractValidator<DashboardFilterDto>
{
    public DashboardFilterDtoValidator()
    {
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}
