using backend.Modules.QRCode.DTOs;
using backend.Modules.QRCode.Enums;
using FluentValidation;

namespace backend.Modules.QRCode.Validators;

public class CreateQRCodeDtoValidator : AbstractValidator<CreateQRCodeDto>
{
    public CreateQRCodeDtoValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("OrganizationId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.QRType)
            .IsInEnum().WithMessage("Invalid QR Type.");

        RuleFor(x => x.OriginalValue)
            .NotEmpty().WithMessage("Original Value is required.");
            
        // Conditional validations based on QRType
        RuleFor(x => x.OriginalValue)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => x.QRType == QRType.Website)
            .WithMessage("Original Value must be a valid URL for Website QR Type.");
            
        RuleFor(x => x.OriginalValue)
            .EmailAddress()
            .When(x => x.QRType == QRType.Email)
            .WithMessage("Original Value must be a valid Email Address for Email QR Type.");

        RuleFor(x => x.ForegroundColor)
            .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$").WithMessage("Foreground Color must be a valid HEX code.");

        RuleFor(x => x.BackgroundColor)
            .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$").WithMessage("Background Color must be a valid HEX code.");

        RuleFor(x => x.Size)
            .InclusiveBetween(100, 2000).WithMessage("Size must be between 100 and 2000 pixels.");

        RuleFor(x => x.Margin)
            .InclusiveBetween(0, 10).WithMessage("Margin must be between 0 and 10.");

        RuleFor(x => x.ErrorCorrectionLevel)
            .Must(e => new[] { "L", "M", "Q", "H" }.Contains(e))
            .WithMessage("Error Correction Level must be one of: L, M, Q, H.");
            
        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiration Date must be in the future.")
            .When(x => x.ExpirationDate.HasValue);
    }
}
