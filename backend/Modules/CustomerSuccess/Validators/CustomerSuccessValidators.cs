using backend.Modules.CustomerSuccess.Loyalty.DTOs;
using backend.Modules.CustomerSuccess.Referrals.DTOs;
using backend.Modules.CustomerSuccess.Satisfaction.DTOs;
using backend.Modules.CustomerSuccess.SuccessTasks.DTOs;
using FluentValidation;

namespace backend.Modules.CustomerSuccess.Validators;

public class CreateLoyaltyProgramValidator : AbstractValidator<CreateLoyaltyProgramDto>
{
    public CreateLoyaltyProgramValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.PointsPerPurchase).GreaterThan(0);
        RuleFor(x => x.MinimumRedemptionPoints).GreaterThan(0);
    }
}

public class EarnPointsValidator : AbstractValidator<EarnPointsDto>
{
    public EarnPointsValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.PurchaseAmount).GreaterThan(0);
    }
}

public class RedeemPointsValidator : AbstractValidator<RedeemPointsDto>
{
    public RedeemPointsValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.PointsToRedeem).GreaterThan(0);
    }
}

public class CreateReferralValidator : AbstractValidator<CreateReferralDto>
{
    public CreateReferralValidator()
    {
        RuleFor(x => x.ReferrerCustomerId).NotEmpty();
    }
}

public class SubmitFeedbackValidator : AbstractValidator<SubmitFeedbackDto>
{
    public SubmitFeedbackValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
    }
}

public class CreateSuccessTaskValidator : AbstractValidator<CreateSuccessTaskDto>
{
    public CreateSuccessTaskValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TaskType).NotEmpty();
    }
}
