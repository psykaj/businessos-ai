using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Modules.CustomerSuccess.CustomerSegments.DTOs;
using backend.Modules.CustomerSuccess.CustomerSegments.Entities;
using backend.Modules.CustomerSuccess.CustomerSegments.Interfaces;

namespace backend.Modules.CustomerSuccess.CustomerSegments.Services;

public class CustomerSegmentService : ICustomerSegmentService
{
    private readonly ICustomerSegmentRepository _segmentRepo;
    private readonly ICustomerHealthRepository _healthRepo;

    public CustomerSegmentService(
        ICustomerSegmentRepository segmentRepo,
        ICustomerHealthRepository healthRepo)
    {
        _segmentRepo = segmentRepo;
        _healthRepo = healthRepo;
    }

    public async Task<IEnumerable<CustomerSegmentDto>> GetSegmentsAsync(Guid orgId)
    {
        var segments = (await _segmentRepo.GetAllAsync(orgId)).ToList();
        if (!segments.Any())
        {
            await RecalculateSegmentsAsync(orgId);
            segments = (await _segmentRepo.GetAllAsync(orgId)).ToList();
        }

        return segments.Select(MapToDto);
    }

    public async Task<CustomerSegmentDto> GetSegmentByIdAsync(Guid orgId, Guid id)
    {
        var segment = await _segmentRepo.GetByIdAsync(orgId, id)
            ?? throw new KeyNotFoundException("Customer segment not found.");

        return MapToDto(segment);
    }

    public async Task<IEnumerable<CustomerSegmentMemberDto>> GetSegmentMembersAsync(Guid orgId, string segmentName)
    {
        var (healthRecords, _) = await _healthRepo.GetPagedAsync(orgId, null, null, 1, 10000, null, false);
        var filtered = FilterBySegmentName(healthRecords, segmentName);

        return filtered.Select(h => new CustomerSegmentMemberDto(
            h.CustomerId,
            h.Customer?.Name ?? "Customer",
            "customer@example.com",
            segmentName,
            h.LifetimeValue,
            h.HealthScore,
            h.RiskLevel
        ));
    }

    public async Task RecalculateSegmentsAsync(Guid orgId)
    {
        var (healthRecords, _) = await _healthRepo.GetPagedAsync(orgId, null, null, 1, 10000, null, false);
        var healthList = healthRecords.ToList();

        string[] standardSegments = new[]
        {
            "New Customers",
            "Active Customers",
            "VIP Customers",
            "High Spend Customers",
            "Repeat Customers",
            "Inactive Customers",
            "At-Risk Customers"
        };

        foreach (var name in standardSegments)
        {
            var matching = FilterBySegmentName(healthList, name);
            int count = matching.Count();

            var segment = await _segmentRepo.GetByNameAsync(orgId, name);
            if (segment == null)
            {
                segment = new CustomerSegment
                {
                    OrganizationId = orgId,
                    Name = name,
                    SegmentType = "Automated",
                    CustomerCount = count,
                    UpdatedAt = DateTime.UtcNow
                };
                await _segmentRepo.AddAsync(segment);
            }
            else
            {
                segment.CustomerCount = count;
                segment.UpdatedAt = DateTime.UtcNow;
                await _segmentRepo.UpdateAsync(segment);
            }
        }

        await _segmentRepo.SaveChangesAsync();
    }

    private static IEnumerable<CustomerHealth.Entities.CustomerHealth> FilterBySegmentName(
        IEnumerable<CustomerHealth.Entities.CustomerHealth> records, string segmentName)
    {
        var now = DateTime.UtcNow;
        return segmentName.ToLower() switch
        {
            "new customers" => records.Where(r => r.CreatedAt >= now.AddDays(-30)),
            "active customers" => records.Where(r => r.LastInteractionDate.HasValue && r.LastInteractionDate.Value >= now.AddDays(-30)),
            "vip customers" => records.Where(r => r.LifetimeValue >= 1000 || r.HealthScore >= 90),
            "high spend customers" => records.Where(r => r.LifetimeValue >= 500),
            "repeat customers" => records.Where(r => r.PurchaseFrequency >= 2),
            "inactive customers" => records.Where(r => !r.LastInteractionDate.HasValue || r.LastInteractionDate.Value < now.AddDays(-60)),
            "at-risk customers" => records.Where(r => r.HealthScore < 50 || r.RiskLevel == "High Risk" || r.RiskLevel == "Needs Attention"),
            _ => records
        };
    }

    private static CustomerSegmentDto MapToDto(CustomerSegment s)
    {
        return new CustomerSegmentDto(
            s.Id,
            s.Name,
            s.SegmentType,
            s.CriteriaJson,
            s.CustomerCount,
            s.UpdatedAt
        );
    }
}
