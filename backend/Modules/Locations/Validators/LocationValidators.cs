using FluentValidation;
using backend.Modules.Locations.DTOs;

namespace backend.Modules.Locations.Validators;

public class CreateLocationDtoValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Region).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
        RuleFor(x => x.TimeZone).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}

public class UpdateLocationDtoValidator : AbstractValidator<UpdateLocationDto>
{
    public UpdateLocationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Region).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
        RuleFor(x => x.TimeZone).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}
