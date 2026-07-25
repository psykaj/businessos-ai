using backend.Modules.CustomerSuccess.CustomerSegments.DTOs;

namespace backend.Modules.CustomerSuccess.CustomerSegments.Interfaces;

public interface ICustomerSegmentService
{
    Task<IEnumerable<CustomerSegmentDto>> GetSegmentsAsync(Guid orgId);
    Task<CustomerSegmentDto> GetSegmentByIdAsync(Guid orgId, Guid id);
    Task<IEnumerable<CustomerSegmentMemberDto>> GetSegmentMembersAsync(Guid orgId, string segmentName);
    Task RecalculateSegmentsAsync(Guid orgId);
}
