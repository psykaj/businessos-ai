using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Assignments.Interfaces;
using backend.Modules.CommunicationHub.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CommunicationHub.Assignments.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Assignment>> GetByFilterAsync(Guid organizationId, Guid? conversationId, Guid? assignedToUserId)
    {
        var query = _context.CommAssignments
            .Where(a => a.OrganizationId == organizationId && !a.IsDeleted);

        if (conversationId.HasValue)
        {
            query = query.Where(a => a.ConversationId == conversationId.Value);
        }

        if (assignedToUserId.HasValue)
        {
            query = query.Where(a => a.AssignedToUserId == assignedToUserId.Value);
        }

        return await query.OrderByDescending(a => a.AssignedAt).ToListAsync();
    }
}
