using backend.Modules.CustomerSuccess.Satisfaction.Entities;
using CustomerFeedback = backend.Modules.CustomerSuccess.Satisfaction.Entities.CustomerFeedback;
using backend.Modules.CustomerSuccess.Satisfaction.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerSuccess.Satisfaction.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly ApplicationDbContext _context;

    public FeedbackRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entities.CustomerFeedback?> GetByIdAsync(Guid orgId, Guid id)
    {
        return await _context.CustomerFeedbacks
            .Include(f => f.Customer)
            .FirstOrDefaultAsync(f => f.OrganizationId == orgId && f.Id == id);
    }

    public async Task<IEnumerable<Entities.CustomerFeedback>> GetByCustomerIdAsync(Guid orgId, Guid customerId)
    {
        return await _context.CustomerFeedbacks
            .Where(f => f.OrganizationId == orgId && f.CustomerId == customerId)
            .OrderByDescending(f => f.SubmittedAt)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Entities.CustomerFeedback> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, int? minRating, int? maxRating, string? feedbackType, int page, int pageSize)
    {
        var query = _context.CustomerFeedbacks
            .Include(f => f.Customer)
            .Where(f => f.OrganizationId == orgId)
            .AsNoTracking();

        if (customerId.HasValue)
        {
            query = query.Where(f => f.CustomerId == customerId.Value);
        }

        if (minRating.HasValue)
        {
            query = query.Where(f => f.Rating >= minRating.Value);
        }

        if (maxRating.HasValue)
        {
            query = query.Where(f => f.Rating <= maxRating.Value);
        }

        if (!string.IsNullOrWhiteSpace(feedbackType))
        {
            query = query.Where(f => f.FeedbackType.ToLower() == feedbackType.ToLower());
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(f => f.SubmittedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<double> GetAverageRatingAsync(Guid orgId, string? feedbackType)
    {
        var query = _context.CustomerFeedbacks.Where(f => f.OrganizationId == orgId);
        if (!string.IsNullOrWhiteSpace(feedbackType))
        {
            query = query.Where(f => f.FeedbackType.ToLower() == feedbackType.ToLower());
        }

        if (!await query.AnyAsync()) return 0.0;
        return await query.AverageAsync(f => f.Rating);
    }

    public async Task<Dictionary<int, int>> GetRatingDistributionAsync(Guid orgId, string? feedbackType)
    {
        var query = _context.CustomerFeedbacks.Where(f => f.OrganizationId == orgId);
        if (!string.IsNullOrWhiteSpace(feedbackType))
        {
            query = query.Where(f => f.FeedbackType.ToLower() == feedbackType.ToLower());
        }

        var groups = await query
            .GroupBy(f => f.Rating)
            .Select(g => new { Rating = g.Key, Count = g.Count() })
            .ToListAsync();

        var result = new Dictionary<int, int>();
        for (int r = 1; r <= 5; r++)
        {
            result[r] = groups.FirstOrDefault(g => g.Rating == r)?.Count ?? 0;
        }

        return result;
    }

    public async Task AddAsync(Entities.CustomerFeedback feedback)
    {
        await _context.CustomerFeedbacks.AddAsync(feedback);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
