using FluentValidation;
using backend.Modules.Profitability.DTOs;

namespace backend.Modules.Profitability.Validators;

public class CreateProfitSnapshotRequestValidator : AbstractValidator<CreateProfitSnapshotRequest>
{
    public CreateProfitSnapshotRequestValidator()
    {
        RuleFor(x => x.Period)
            .MaximumLength(30);
    }
}
