using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CommunicationHub.Assignments.DTOs;
using backend.Modules.CommunicationHub.Assignments.Interfaces;

namespace backend.Modules.CommunicationHub.Assignments.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _repository;
    private readonly IMapper _mapper;

    public AssignmentService(IAssignmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AssignmentDto>> GetAssignmentsAsync(Guid organizationId, AssignmentFilterDto filter)
    {
        var items = await _repository.GetByFilterAsync(organizationId, filter.ConversationId, filter.AssignedToUserId);
        return _mapper.Map<IEnumerable<AssignmentDto>>(items);
    }
}
