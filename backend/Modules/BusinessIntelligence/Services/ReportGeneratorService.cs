using System.Text.Json;
using backend.Modules.BusinessIntelligence.Constants;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Persistence;

namespace backend.Modules.BusinessIntelligence.Services;

public class ReportGeneratorService : IReportGeneratorService
{
    private readonly ApplicationDbContext _context;
    private readonly IReportRepository _reportRepository;
    private readonly IExecutiveDashboardService _dashboardService;

    public ReportGeneratorService(
        ApplicationDbContext context,
        IReportRepository reportRepository,
        IExecutiveDashboardService dashboardService)
    {
        _context = context;
        _reportRepository = reportRepository;
        _dashboardService = dashboardService;
    }

    public async Task<ReportDto> GenerateReportAsync(Guid organizationId, string userId, GenerateReportRequestDto request, CancellationToken cancellationToken = default)
    {
        var reportType = string.IsNullOrWhiteSpace(request.ReportType) ? BIConstants.ReportTypes.Executive : request.ReportType;
        var filterDto = new DashboardFilterDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        object reportPayload = reportType.ToLower() switch
        {
            "sales" => await _dashboardService.GetSalesDashboardAsync(organizationId, filterDto, cancellationToken),
            "marketing" => await _dashboardService.GetMarketingDashboardAsync(organizationId, filterDto, cancellationToken),
            "finance" => await _dashboardService.GetFinanceDashboardAsync(organizationId, filterDto, cancellationToken),
            "operations" or "workflow" => await _dashboardService.GetOperationsDashboardAsync(organizationId, filterDto, cancellationToken),
            "crm" => await _dashboardService.GetSalesDashboardAsync(organizationId, filterDto, cancellationToken),
            "aiusage" => await _dashboardService.GetOperationsDashboardAsync(organizationId, filterDto, cancellationToken),
            _ => await _dashboardService.GetCEODashboardAsync(organizationId, filterDto, cancellationToken)
        };

        var reportEntity = new Report
        {
            OrganizationId = organizationId,
            Name = string.IsNullOrWhiteSpace(request.Name) ? $"{reportType} Business Report - {DateTime.UtcNow:yyyy-MM-dd}" : request.Name,
            ReportType = reportType,
            Filters = JsonSerializer.Serialize(request.FilterParams ?? new Dictionary<string, string>()),
            GeneratedAt = DateTime.UtcNow,
            GeneratedBy = userId,
            Format = string.IsNullOrWhiteSpace(request.Format) ? "JSON" : request.Format
        };

        await _reportRepository.AddAsync(reportEntity, cancellationToken);
        await _reportRepository.SaveChangesAsync(cancellationToken);

        return new ReportDto
        {
            Id = reportEntity.Id,
            OrganizationId = reportEntity.OrganizationId,
            Name = reportEntity.Name,
            ReportType = reportEntity.ReportType,
            Filters = reportEntity.Filters,
            GeneratedAt = reportEntity.GeneratedAt,
            GeneratedBy = reportEntity.GeneratedBy,
            Format = reportEntity.Format,
            FileUrl = reportEntity.FileUrl,
            DataPayload = reportPayload
        };
    }

    public async Task<ReportDto?> GetReportByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var r = await _reportRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (r == null) return null;

        var filterDto = new DashboardFilterDto();
        object reportPayload = r.ReportType.ToLower() switch
        {
            "sales" => await _dashboardService.GetSalesDashboardAsync(organizationId, filterDto, cancellationToken),
            "marketing" => await _dashboardService.GetMarketingDashboardAsync(organizationId, filterDto, cancellationToken),
            "finance" => await _dashboardService.GetFinanceDashboardAsync(organizationId, filterDto, cancellationToken),
            "operations" or "workflow" => await _dashboardService.GetOperationsDashboardAsync(organizationId, filterDto, cancellationToken),
            _ => await _dashboardService.GetCEODashboardAsync(organizationId, filterDto, cancellationToken)
        };

        return new ReportDto
        {
            Id = r.Id,
            OrganizationId = r.OrganizationId,
            Name = r.Name,
            ReportType = r.ReportType,
            Filters = r.Filters,
            GeneratedAt = r.GeneratedAt,
            GeneratedBy = r.GeneratedBy,
            Format = r.Format,
            FileUrl = r.FileUrl,
            DataPayload = reportPayload
        };
    }

    public async Task<List<ReportDto>> GetReportsAsync(Guid organizationId, string? reportType = null, CancellationToken cancellationToken = default)
    {
        var list = await _reportRepository.GetAllByOrganizationIdAsync(organizationId, reportType, cancellationToken);

        return list.Select(r => new ReportDto
        {
            Id = r.Id,
            OrganizationId = r.OrganizationId,
            Name = r.Name,
            ReportType = r.ReportType,
            Filters = r.Filters,
            GeneratedAt = r.GeneratedAt,
            GeneratedBy = r.GeneratedBy,
            Format = r.Format,
            FileUrl = r.FileUrl
        }).ToList();
    }
}
