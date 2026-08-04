using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Ratings.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.Ratings.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly ApplicationDbContext _context;

    public RatingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rating>> GetByEntityAsync(Guid organizationId, RatingEntityType entityType, string entityId)
    {
        return await _context.Ratings
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId && r.EntityType == entityType && r.EntityId == entityId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rating>> GetRecentAsync(Guid organizationId, int limit)
    {
        return await _context.Ratings
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Rating> AddAsync(Rating entity)
    {
        await _context.Ratings.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
