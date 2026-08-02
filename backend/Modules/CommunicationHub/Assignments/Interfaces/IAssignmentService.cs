using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Assignments.DTOs;

namespace backend.Modules.CommunicationHub.Assignments.Interfaces;

public interface IAssignmentService
{
    Task<IEnumerable<AssignmentDto>> GetAssignmentsAsync(Guid organizationId, AssignmentFilterDto filter);
}
