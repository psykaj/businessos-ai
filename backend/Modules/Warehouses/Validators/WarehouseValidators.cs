using FluentValidation;
using backend.Modules.Warehouses.DTOs;

namespace backend.Modules.Warehouses.Validators;

public class CreateWarehouseDtoValidator : AbstractValidator<CreateWarehouseDto>
{
    public CreateWarehouseDtoValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.StorageCapacitySqFt).GreaterThan(0);
    }
}

public class UpdateWarehouseDtoValidator : AbstractValidator<UpdateWarehouseDto>
{
    public UpdateWarehouseDtoValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.StorageCapacitySqFt).GreaterThan(0);
        RuleFor(x => x.CurrentUtilizationPercentage).InclusiveBetween(0, 100);
    }
}
