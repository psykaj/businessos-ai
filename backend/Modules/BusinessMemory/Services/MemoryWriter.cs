using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.BusinessMemory.Interfaces;
using backend.Modules.BusinessMemory.DTOs;
using backend.Modules.BusinessMemory.Entities;
using backend.Modules.BusinessMemory.Enums;

namespace backend.Modules.BusinessMemory.Services;

public class MemoryWriter : IMemoryWriter
{
    private readonly ApplicationDbContext _context;

    public MemoryWriter(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entities.BusinessMemory> WriteMemoryAsync(
        Guid organizationId, 
        CreateMemoryDto request, 
        string userId, 
        CancellationToken cancellationToken = default)
    {
        // Deterministic deduplication/contradiction check can happen here or via DeactivateContradictoryMemoriesAsync before calling this
        var memory = new Entities.BusinessMemory
        {
            OrganizationId = organizationId,
            MemoryType = request.MemoryType,
            Title = request.Title,
            Content = request.Content,
            StructuredData = request.StructuredData,
            SourceModule = request.SourceModule,
            SourceEntityId = request.SourceEntityId,
            Importance = request.Importance,
            Confidence = request.Confidence,
            ExpiresAt = request.ExpiresAt,
            IsActive = true
        };

        _context.BusinessMemories.Add(memory);
        await _context.SaveChangesAsync(cancellationToken);

        return memory;
    }

    public async Task DeactivateContradictoryMemoriesAsync(
        Guid organizationId, 
        string sourceModule, 
        string sourceEntityId, 
        string newContext,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var existingMemories = await _context.BusinessMemories
            .Where(m => m.OrganizationId == organizationId 
                        && m.SourceModule == sourceModule 
                        && m.SourceEntityId == sourceEntityId
                        && m.IsActive)
            .ToListAsync(cancellationToken);

        // Very basic logic: if we are writing new context for a specific entity/module, 
        // we might deactivate older ones that are directly related to prevent contradiction.
        // A more advanced system would use AI to detect true contradiction.
        foreach (var memory in existingMemories)
        {
            // Just an example of how we handle updates.
            // In a real system, we might check if 'newContext' contradicts 'memory.Content'
            memory.IsActive = false;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
