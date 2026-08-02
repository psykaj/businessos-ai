using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Assignments.Interfaces;

public interface IAssignmentRepository
{
    Task<IEnumerable<Assignment>> GetByFilterAsync(Guid organizationId, Guid? conversationId, Guid? assignedToUserId);
}
