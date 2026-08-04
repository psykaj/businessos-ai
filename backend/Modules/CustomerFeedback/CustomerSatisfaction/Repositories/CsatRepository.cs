using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.Interfaces;
using backend.Modules.CustomerFeedback.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.CustomerSatisfaction.Repositories;

public class CsatRepository : ICsatRepository
{
    private readonly ApplicationDbContext _context;

    public CsatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerSatisfactionScore?> GetByCustomerIdAsync(Guid organizationId, Guid customerId)
    {
        return await _context.CustomerSatisfactionScores
            .FirstOrDefaultAsync(c => c.OrganizationId == organizationId && c.CustomerId == customerId && !c.IsDeleted);
    }

    public async Task<IEnumerable<CustomerSatisfactionScore>> GetAllAsync(Guid organizationId)
    {
        return await _context.CustomerSatisfactionScores
            .AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted)
            .OrderByDescending(c => c.LastCalculatedAt)
            .ToListAsync();
    }

    public async Task<CustomerSatisfactionScore> AddOrUpdateAsync(CustomerSatisfactionScore entity)
    {
        var existing = await GetByCustomerIdAsync(entity.OrganizationId, entity.CustomerId);
        if (existing == null)
        {
            await _context.CustomerSatisfactionScores.AddAsync(entity);
        }
        else
        {
            existing.CurrentCsat = entity.CurrentCsat;
            existing.CurrentNps = entity.CurrentNps;
            existing.AverageRating = entity.AverageRating;
            existing.TotalFeedbacks = entity.TotalFeedbacks;
            existing.TotalComplaints = entity.TotalComplaints;
            existing.RepeatComplaintRate = entity.RepeatComplaintRate;
            existing.ResolutionSatisfaction = entity.ResolutionSatisfaction;
            existing.ChurnRiskScore = entity.ChurnRiskScore;
            existing.LastCalculatedAt = DateTime.UtcNow;
            _context.CustomerSatisfactionScores.Update(existing);
            entity = existing;
        }
        await _context.SaveChangesAsync();
        return entity;
    }
}
