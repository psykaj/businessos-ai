using backend.Modules.QRCode.DTOs;
using backend.Modules.QRCode.Interfaces;
using backend.Modules.QRCode.Repositories;
using backend.Modules.QRCode.Services;
using backend.Modules.QRCode.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.QRCode.Extensions;

public static class QRCodeModuleExtensions
{
    public static IServiceCollection AddQRCodeModule(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IQRCodeRepository, QRCodeRepository>();
        
        // Services
        services.AddScoped<IQRCodeService, QRCodeService>();
        services.AddScoped<IQRGeneratorService, QRGeneratorService>();
        
        // Validators
        services.AddScoped<IValidator<CreateQRCodeDto>, CreateQRCodeDtoValidator>();
        services.AddScoped<IValidator<UpdateQRCodeDto>, UpdateQRCodeDtoValidator>();
        
        return services;
    }
}
