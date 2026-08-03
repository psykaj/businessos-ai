using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.CustomerFeedback.Feedback.DTOs;
using backend.Modules.CustomerFeedback.Feedback.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerFeedback.Feedback.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly ApplicationDbContext _context;

    public FeedbackRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entities.Feedback?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.Feedbacks
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id && f.OrganizationId == organizationId && !f.IsDeleted);
    }

    public async Task<(IEnumerable<Entities.Feedback> Items, int TotalCount)> SearchAsync(FeedbackSearchFilterDto filter)
    {
        var query = _context.Feedbacks
            .AsNoTracking()
            .Where(f => f.OrganizationId == filter.OrganizationId && !f.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            var q = filter.Query.ToLower();
            query = query.Where(f => f.Comment.ToLower().Contains(q) || (f.CustomerName != null && f.CustomerName.ToLower().Contains(q)));
        }

        if (filter.RatingValue.HasValue)
        {
            query = query.Where(f => f.RatingValue == filter.RatingValue.Value);
        }

        if (filter.CustomerId.HasValue)
        {
            query = query.Where(f => f.CustomerId == filter.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Status) && Enum.TryParse<FeedbackStatus>(filter.Status, true, out var status))
        {
            query = query.Where(f => f.Status == status);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(f => f.CreatedAt >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(f => f.CreatedAt <= filter.EndDate.Value);
        }

        int total = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<IEnumerable<Entities.Feedback>> GetForExportAsync(Guid organizationId, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Feedbacks
            .AsNoTracking()
            .Where(f => f.OrganizationId == organizationId && !f.IsDeleted);

        if (startDate.HasValue) query = query.Where(f => f.CreatedAt >= startDate.Value);
        if (endDate.HasValue) query = query.Where(f => f.CreatedAt <= endDate.Value);

        return await query.OrderByDescending(f => f.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Entities.Feedback>> GetUnassignedUrgentAsync(int batchSize)
    {
        return await _context.Feedbacks
            .Where(f => !f.IsDeleted && f.IsUrgent && f.Status == FeedbackStatus.New && f.AssignedUserId == null)
            .Take(batchSize)
            .ToListAsync();
    }

    public async Task<Entities.Feedback> AddAsync(Entities.Feedback entity)
    {
        await _context.Feedbacks.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Entities.Feedback entity)
    {
        _context.Feedbacks.Update(entity);
        await _context.SaveChangesAsync();
    }
}
