using ConversationEntity = backend.Modules.AiAgent.Entities.Conversation;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.Repositories;

public class AiConversationRepository : IAiConversationRepository
{
    private readonly ApplicationDbContext _context;

    public AiConversationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ConversationEntity?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted, cancellationToken);
    }

    public async Task<ConversationEntity?> GetByIdWithMessagesAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .Include(c => c.Messages.Where(m => !m.IsDeleted).OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == organizationId && !c.IsDeleted, cancellationToken);
    }

    public async Task<List<ConversationEntity>> GetAllByOrganizationIdAsync(Guid organizationId, string? userId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Conversations.AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(c => c.UserId == userId);
        }

        return await query.OrderByDescending(c => c.UpdatedAt).ToListAsync(cancellationToken);
    }

    public async Task<(List<ConversationEntity> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        string? userId = null,
        int page = 1,
        int pageSize = 20,
        string? search = null,
        string? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Conversations.AsNoTracking()
            .Where(c => c.OrganizationId == organizationId && !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(c => c.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(c => c.Status.ToLower() == status.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search.ToLower()}%";
            query = query.Where(c => EF.Functions.ILike(c.Title, searchPattern));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(c => c.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(ConversationEntity conversation, CancellationToken cancellationToken = default)
    {
        await _context.Conversations.AddAsync(conversation, cancellationToken);
    }

    public void Update(ConversationEntity conversation)
    {
        _context.Conversations.Update(conversation);
    }

    public void Delete(ConversationEntity conversation)
    {
        conversation.IsDeleted = true;
        _context.Conversations.Update(conversation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
