using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.BusinessMemory.Interfaces;
using backend.Modules.BusinessMemory.DTOs;
using backend.Modules.BusinessMemory.Entities;
using backend.Exceptions;

namespace backend.Modules.BusinessMemory.Services;

public class MemoryService : IMemoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryWriter _memoryWriter;
    private readonly IMemoryRetrievalService _retrievalService;

    public MemoryService(
        ApplicationDbContext context, 
        IMemoryWriter memoryWriter,
        IMemoryRetrievalService retrievalService)
    {
        _context = context;
        _memoryWriter = memoryWriter;
        _retrievalService = retrievalService;
    }

    public async Task<MemoryDto> GetMemoryAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default)
    {
        var memory = await _context.BusinessMemories
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.OrganizationId == organizationId && m.Id == id, cancellationToken);
            
        if (memory == null)
            throw new NotFoundException($"BusinessMemory with ID {id} not found.");

        return MapToDto(memory);
    }

    public async Task<IEnumerable<MemoryDto>> GetCustomerMemoriesAsync(Guid organizationId, string customerId, CancellationToken cancellationToken = default)
    {
        var memories = await _retrievalService.GetByEntityAsync(organizationId, "Customer", customerId, cancellationToken);
        return memories.Select(MapToDto);
    }

    public async Task<IEnumerable<MemoryDto>> GetBusinessMemoriesAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var memories = await _context.BusinessMemories
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId 
                        && m.MemoryType == Enums.MemoryType.BusinessPreference 
                        && m.IsActive)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
            
        return memories.Select(MapToDto);
    }

    public async Task<MemoryDto> CreateMemoryAsync(Guid organizationId, CreateMemoryDto request, string userId, CancellationToken cancellationToken = default)
    {
        var memory = await _memoryWriter.WriteMemoryAsync(organizationId, request, userId, cancellationToken);
        return MapToDto(memory);
    }

    public async Task<MemoryDto> UpdateMemoryAsync(Guid organizationId, Guid id, UpdateMemoryDto request, string userId, CancellationToken cancellationToken = default)
    {
        var memory = await _context.BusinessMemories
            .FirstOrDefaultAsync(m => m.OrganizationId == organizationId && m.Id == id, cancellationToken);
            
        if (memory == null)
            throw new NotFoundException($"BusinessMemory with ID {id} not found.");

        if (request.Title != null) memory.Title = request.Title;
        if (request.Content != null) memory.Content = request.Content;
        if (request.StructuredData != null) memory.StructuredData = request.StructuredData;
        if (request.Importance.HasValue) memory.Importance = request.Importance.Value;
        if (request.Confidence.HasValue) memory.Confidence = request.Confidence.Value;
        if (request.ExpiresAt != null) memory.ExpiresAt = request.ExpiresAt; // can be set to null if clearing expiration
        if (request.IsActive.HasValue) memory.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(memory);
    }

    public async Task DeleteMemoryAsync(Guid organizationId, Guid id, string userId, CancellationToken cancellationToken = default)
    {
        var memory = await _context.BusinessMemories
            .FirstOrDefaultAsync(m => m.OrganizationId == organizationId && m.Id == id, cancellationToken);
            
        if (memory == null)
            throw new NotFoundException($"BusinessMemory with ID {id} not found.");

        // Soft delete via IsActive instead of actually removing it for auditing
        memory.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<MemoryDto>> GetRelevantMemoriesAsync(Guid organizationId, string query, CancellationToken cancellationToken = default)
    {
        var relevant = await _retrievalService.RetrieveRelevantMemoriesAsync(organizationId, query, limit: 20, cancellationToken: cancellationToken);
        return relevant.Select(MapToDto);
    }

    public Task RebuildContextAsync(Guid organizationId, string entityType, string entityId, CancellationToken cancellationToken = default)
    {
        // This is a placeholder for a background job that might trigger the AI Summarizer 
        // to re-read all current data for a customer and overwrite outdated memory.
        return Task.CompletedTask;
    }

    private MemoryDto MapToDto(Entities.BusinessMemory m)
    {
        return new MemoryDto
        {
            Id = m.Id,
            OrganizationId = m.OrganizationId,
            MemoryType = m.MemoryType,
            Title = m.Title,
            Content = m.Content,
            StructuredData = m.StructuredData,
            SourceModule = m.SourceModule,
            SourceEntityId = m.SourceEntityId,
            Importance = m.Importance,
            Confidence = m.Confidence,
            ExpiresAt = m.ExpiresAt,
            LastUsedAt = m.LastUsedAt,
            IsActive = m.IsActive,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        };
    }
}
