using backend.Modules.Analytics.DTOs;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Analytics.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AnalyticsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AnalyticsOverviewDto> GetOverviewAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var scansQuery = _dbContext.QRScans.AsNoTracking().Where(s => s.OrganizationId == organizationId);
        
        if (startDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime <= endDate.Value);

        var totalScans = await scansQuery.CountAsync();
        
        // Count unique IP addresses as unique scans for simplicity in this implementation
        var uniqueScans = await scansQuery.Select(s => s.IPAddress).Distinct().CountAsync();
        
        var today = DateTime.UtcNow.Date;
        var scansToday = await scansQuery.Where(s => s.ScanDateTime >= today).CountAsync();
        
        var activeQRCodes = await _dbContext.QRCodes
            .AsNoTracking()
            .Where(q => q.OrganizationId == organizationId && q.Status == "Active")
            .CountAsync();

        return new AnalyticsOverviewDto
        {
            TotalScans = totalScans,
            UniqueScans = uniqueScans,
            ScansToday = scansToday,
            ActiveQRCodes = activeQRCodes
        };
    }

    public async Task<QRPerformanceDto> GetQRPerformanceAsync(Guid organizationId, Guid qrCodeId, DateTime? startDate, DateTime? endDate)
    {
        var qrCode = await _dbContext.QRCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == qrCodeId && q.OrganizationId == organizationId);
            
        if (qrCode == null) return null!;

        var scansQuery = _dbContext.QRScans.AsNoTracking().Where(s => s.QRCodeId == qrCodeId);
        
        if (startDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime <= endDate.Value);

        var totalScans = await scansQuery.CountAsync();
        var uniqueScans = await scansQuery.Select(s => s.IPAddress).Distinct().CountAsync();
        var lastScanDate = await scansQuery.OrderByDescending(s => s.ScanDateTime).Select(s => s.ScanDateTime).FirstOrDefaultAsync();

        return new QRPerformanceDto
        {
            QRCodeId = qrCode.Id,
            QRCodeName = qrCode.Name,
            TotalScans = totalScans,
            UniqueScans = uniqueScans,
            LastScanDate = lastScanDate == default ? null : lastScanDate
        };
    }

    public async Task<IEnumerable<ScanTimelineDto>> GetTimelineAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var scansQuery = _dbContext.QRScans.AsNoTracking().Where(s => s.OrganizationId == organizationId);
        
        if (startDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime <= endDate.Value);

        // Group by Date
        var timeline = await scansQuery
            .GroupBy(s => s.ScanDateTime.Date)
            .Select(g => new ScanTimelineDto
            {
                Date = g.Key,
                ScanCount = g.Count()
            })
            .OrderBy(t => t.Date)
            .ToListAsync();

        return timeline;
    }

    public async Task<IEnumerable<DeviceAnalyticsDto>> GetDeviceAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var scansQuery = _dbContext.QRScans.AsNoTracking().Where(s => s.OrganizationId == organizationId);
        
        if (startDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime <= endDate.Value);

        var totalCount = await scansQuery.CountAsync();
        if (totalCount == 0) return new List<DeviceAnalyticsDto>();

        var devices = await scansQuery
            .Where(s => s.DeviceType != null)
            .GroupBy(s => s.DeviceType)
            .Select(g => new DeviceAnalyticsDto
            {
                DeviceType = g.Key!,
                Count = g.Count(),
                Percentage = (double)g.Count() / totalCount * 100
            })
            .OrderByDescending(d => d.Count)
            .ToListAsync();

        return devices;
    }

    public async Task<IEnumerable<BrowserAnalyticsDto>> GetBrowserAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
         var scansQuery = _dbContext.QRScans.AsNoTracking().Where(s => s.OrganizationId == organizationId);
        
        if (startDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime <= endDate.Value);

        var totalCount = await scansQuery.CountAsync();
        if (totalCount == 0) return new List<BrowserAnalyticsDto>();

        var browsers = await scansQuery
            .Where(s => s.Browser != null)
            .GroupBy(s => s.Browser)
            .Select(g => new BrowserAnalyticsDto
            {
                Browser = g.Key!,
                Count = g.Count(),
                Percentage = (double)g.Count() / totalCount * 100
            })
            .OrderByDescending(b => b.Count)
            .ToListAsync();

        return browsers;
    }

    public async Task<IEnumerable<CountryAnalyticsDto>> GetCountryAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var scansQuery = _dbContext.QRScans.AsNoTracking().Where(s => s.OrganizationId == organizationId);
        
        if (startDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime <= endDate.Value);

        var totalCount = await scansQuery.CountAsync();
        if (totalCount == 0) return new List<CountryAnalyticsDto>();

        var countries = await scansQuery
            .Where(s => s.Country != null)
            .GroupBy(s => s.Country)
            .Select(g => new CountryAnalyticsDto
            {
                Country = g.Key!,
                Count = g.Count(),
                Percentage = (double)g.Count() / totalCount * 100
            })
            .OrderByDescending(c => c.Count)
            .ToListAsync();

        return countries;
    }

    public async Task<IEnumerable<ReferrerAnalyticsDto>> GetReferrerAnalyticsAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var scansQuery = _dbContext.QRScans.AsNoTracking().Where(s => s.OrganizationId == organizationId);
        
        if (startDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            scansQuery = scansQuery.Where(s => s.ScanDateTime <= endDate.Value);

        var totalCount = await scansQuery.CountAsync();
        if (totalCount == 0) return new List<ReferrerAnalyticsDto>();

        var referrers = await scansQuery
            .Where(s => !string.IsNullOrEmpty(s.Referrer))
            .GroupBy(s => s.Referrer)
            .Select(g => new ReferrerAnalyticsDto
            {
                Referrer = g.Key!,
                Count = g.Count(),
                Percentage = (double)g.Count() / totalCount * 100
            })
            .OrderByDescending(r => r.Count)
            .ToListAsync();

        return referrers;
    }

    public async Task<backend.Common.PagedResult<ScanHistoryDto>> GetHistoryAsync(Guid organizationId, Guid? qrCodeId, string? search, int page, int pageSize, DateTime? startDate, DateTime? endDate)
    {
        var query = _dbContext.QRScans
            .Include(s => s.QRCode)
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId);

        if (qrCodeId.HasValue)
        {
            query = query.Where(s => s.QRCodeId == qrCodeId.Value);
        }

        if (startDate.HasValue)
            query = query.Where(s => s.ScanDateTime >= startDate.Value);
        
        if (endDate.HasValue)
            query = query.Where(s => s.ScanDateTime <= endDate.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(s => 
                (s.QRCode != null && s.QRCode.Name.ToLower().Contains(searchLower)) ||
                (s.DeviceType != null && s.DeviceType.ToLower().Contains(searchLower)) ||
                (s.Browser != null && s.Browser.ToLower().Contains(searchLower)) ||
                (s.Country != null && s.Country.ToLower().Contains(searchLower))
            );
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.ScanDateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ScanHistoryDto
            {
                Id = s.Id,
                QRCodeId = s.QRCodeId,
                QRCodeName = s.QRCode != null ? s.QRCode.Name : "Unknown",
                ScanDateTime = s.ScanDateTime,
                IPAddress = s.IPAddress,
                DeviceType = s.DeviceType,
                Browser = s.Browser,
                OperatingSystem = s.OperatingSystem,
                Country = s.Country,
                City = s.City,
                Referrer = s.Referrer,
                UTMCampaign = s.UTMCampaign
            })
            .ToListAsync();

        return new backend.Common.PagedResult<ScanHistoryDto>(items, totalItems, page, pageSize);
    }
}
