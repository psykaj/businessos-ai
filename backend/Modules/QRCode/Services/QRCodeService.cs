using backend.Common;
using backend.Modules.QRCode.DTOs;
using backend.Modules.QRCode.Interfaces;
using BCrypt.Net;

namespace backend.Modules.QRCode.Services;

public class QRCodeService : IQRCodeService
{
    private readonly IQRCodeRepository _repository;
    private readonly IQRGeneratorService _generatorService;

    public QRCodeService(IQRCodeRepository repository, IQRGeneratorService generatorService)
    {
        _repository = repository;
        _generatorService = generatorService;
    }

    public async Task<QRCodeDto> CreateAsync(CreateQRCodeDto dto, CancellationToken cancellationToken = default)
    {
        var shortCode = GenerateShortCode();

        string? passwordHash = null;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        var qrCode = new Models.QRCode
        {
            OrganizationId = dto.OrganizationId,
            Name = dto.Name,
            Description = dto.Description,
            QRType = dto.QRType,
            OriginalValue = dto.OriginalValue,
            ShortCode = shortCode,
            Folder = dto.Folder,
            Tags = dto.Tags ?? new List<string>(),
            ForegroundColor = dto.ForegroundColor,
            BackgroundColor = dto.BackgroundColor,
            LogoUrl = dto.LogoUrl,
            Size = dto.Size,
            Margin = dto.Margin,
            ErrorCorrectionLevel = dto.ErrorCorrectionLevel,
            PasswordProtected = !string.IsNullOrWhiteSpace(dto.Password),
            PasswordHash = passwordHash,
            ExpirationDate = dto.ExpirationDate,
            Status = "Active",
            ScanCount = 0
        };

        var created = await _repository.AddAsync(qrCode, cancellationToken);
        return MapToDto(created);
    }

    public async Task<QRCodeDto> UpdateAsync(Guid id, Guid organizationId, UpdateQRCodeDto dto, CancellationToken cancellationToken = default)
    {
        var qrCode = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (qrCode == null)
            throw new KeyNotFoundException($"QRCode with ID {id} not found.");

        qrCode.Name = dto.Name;
        qrCode.Description = dto.Description;
        qrCode.OriginalValue = dto.OriginalValue;
        qrCode.Folder = dto.Folder;
        qrCode.Tags = dto.Tags ?? new List<string>();
        qrCode.Status = dto.Status;
        qrCode.ForegroundColor = dto.ForegroundColor;
        qrCode.BackgroundColor = dto.BackgroundColor;
        qrCode.LogoUrl = dto.LogoUrl;
        qrCode.Size = dto.Size;
        qrCode.Margin = dto.Margin;
        qrCode.ErrorCorrectionLevel = dto.ErrorCorrectionLevel;
        qrCode.ExpirationDate = dto.ExpirationDate;

        if (dto.Password != null)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                qrCode.PasswordProtected = false;
                qrCode.PasswordHash = null;
            }
            else
            {
                qrCode.PasswordProtected = true;
                qrCode.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }
        }

        await _repository.UpdateAsync(qrCode, cancellationToken);
        return MapToDto(qrCode);
    }

    public async Task DeleteAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var qrCode = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (qrCode == null)
            throw new KeyNotFoundException($"QRCode with ID {id} not found.");

        await _repository.DeleteAsync(qrCode, cancellationToken);
    }

    public async Task<QRCodeDto> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var qrCode = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (qrCode == null)
            throw new KeyNotFoundException($"QRCode with ID {id} not found.");

        return MapToDto(qrCode);
    }

    public async Task<PagedResult<QRCodeDto>> SearchAsync(
        Guid organizationId, 
        string? searchTerm, 
        string? folder, 
        string? status, 
        Enums.QRType? qrType, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.SearchAsync(organizationId, searchTerm, folder, status, qrType, pageNumber, pageSize, cancellationToken);
        
        var dtoList = result.Items.Select(MapToDto).ToList();
        return new PagedResult<QRCodeDto>(dtoList, result.TotalCount, result.PageNumber, result.PageSize);
    }

    public async Task<byte[]> GenerateImageAsync(Guid id, Guid organizationId, string format = "png", CancellationToken cancellationToken = default)
    {
        var qrCode = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (qrCode == null)
            throw new KeyNotFoundException($"QRCode with ID {id} not found.");

        // Normally, the content embedded in the QR code might be a link to our tracking short url.
        // For now, let's embed the original value directly or a simulated short URL.
        string contentToEmbed = qrCode.OriginalValue;

        if (format.ToLower() == "svg")
        {
            string svgString = _generatorService.GenerateSvg(
                contentToEmbed, 
                qrCode.ForegroundColor, 
                qrCode.BackgroundColor, 
                qrCode.Size / 10 > 0 ? qrCode.Size / 10 : 20, 
                qrCode.ErrorCorrectionLevel);
                
            return System.Text.Encoding.UTF8.GetBytes(svgString);
        }
        else
        {
            return _generatorService.GeneratePng(
                contentToEmbed, 
                qrCode.ForegroundColor, 
                qrCode.BackgroundColor, 
                qrCode.Size / 10 > 0 ? qrCode.Size / 10 : 20, 
                qrCode.ErrorCorrectionLevel);
        }
    }

    private string GenerateShortCode()
    {
        // Simple random 8-character string for URL shortcode
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private QRCodeDto MapToDto(Models.QRCode qrCode)
    {
        return new QRCodeDto
        {
            Id = qrCode.Id,
            OrganizationId = qrCode.OrganizationId,
            Name = qrCode.Name,
            Description = qrCode.Description,
            QRType = qrCode.QRType.ToString(),
            OriginalValue = qrCode.OriginalValue,
            ShortCode = qrCode.ShortCode,
            QrImageUrl = qrCode.QrImageUrl,
            Status = qrCode.Status,
            Folder = qrCode.Folder,
            Tags = qrCode.Tags,
            ForegroundColor = qrCode.ForegroundColor,
            BackgroundColor = qrCode.BackgroundColor,
            LogoUrl = qrCode.LogoUrl,
            Size = qrCode.Size,
            Margin = qrCode.Margin,
            ErrorCorrectionLevel = qrCode.ErrorCorrectionLevel,
            PasswordProtected = qrCode.PasswordProtected,
            ExpirationDate = qrCode.ExpirationDate,
            ScanCount = qrCode.ScanCount,
            CreatedAt = qrCode.CreatedAt,
            UpdatedAt = qrCode.UpdatedAt
        };
    }
}
