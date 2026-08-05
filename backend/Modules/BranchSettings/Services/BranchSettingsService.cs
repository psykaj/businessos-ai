using AutoMapper;
using backend.Exceptions;
using backend.Modules.Branches.Interfaces;
using backend.Modules.BranchSettings.DTOs;
using backend.Modules.BranchSettings.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace backend.Modules.BranchSettings.Services;

public class BranchSettingsService : IBranchSettingsService
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public BranchSettingsService(IBranchRepository branchRepository, IMapper mapper, IDistributedCache cache)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<BranchConfigurationResponseDto> GetConfigurationAsync(Guid branchId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"branch_settings_{branchId}_{organizationId}";
        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(cached))
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<BranchConfigurationResponseDto>(cached);
                if (deserialized != null) return deserialized;
            }
            catch { /* fallback to db */ }
        }

        var branch = await _branchRepository.GetByIdAsync(branchId, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", branchId);
        }

        var dto = _mapper.Map<BranchConfigurationResponseDto>(branch);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
        }, cancellationToken);

        return dto;
    }

    public async Task<BranchConfigurationResponseDto> UpdateConfigurationAsync(Guid branchId, Guid organizationId, UpdateBranchConfigurationDto dto, CancellationToken cancellationToken = default)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, organizationId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException("Branch", branchId);
        }

        branch.WorkingHoursJson = dto.WorkingHoursJson;
        branch.OperationalSettingsJson = dto.OperationalSettingsJson;

        await _branchRepository.UpdateAsync(branch, cancellationToken);

        var response = _mapper.Map<BranchConfigurationResponseDto>(branch);
        var cacheKey = $"branch_settings_{branchId}_{organizationId}";
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
        }, cancellationToken);

        return response;
    }
}
