using FluentValidation;
using backend.Modules.Transfers.DTOs;

namespace backend.Modules.Transfers.Validators;

public class CreateTransferRequestDtoValidator : AbstractValidator<CreateTransferRequestDto>
{
    public CreateTransferRequestDtoValidator()
    {
        RuleFor(x => x.SourceWarehouseId).NotEmpty();
        RuleFor(x => x.DestinationWarehouseId).NotEmpty().NotEqual(x => x.SourceWarehouseId).WithMessage("Destination warehouse must be different from source warehouse.");
        RuleFor(x => x.RequestedByName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one transfer item must be included.");
        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.Sku).NotEmpty();
            items.RuleFor(i => i.Quantity).GreaterThan(0);
            items.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
        });
    }
}

public class TransferApprovalDtoValidator : AbstractValidator<TransferApprovalDto>
{
    public TransferApprovalDtoValidator()
    {
        RuleFor(x => x.ApprovedByName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.RejectionReason).NotEmpty().When(x => !x.IsApproved).WithMessage("Rejection reason is required when rejecting a transfer.");
    }
}
