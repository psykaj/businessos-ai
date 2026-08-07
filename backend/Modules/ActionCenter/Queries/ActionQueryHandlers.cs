using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.ActionCenter.DTOs;
using backend.Modules.ActionCenter.Repositories;
using MediatR;

namespace backend.Modules.ActionCenter.Queries;

public class ActionQueryHandlers :
    IRequestHandler<GetActionsQuery, IEnumerable<AiActionDto>>,
    IRequestHandler<GetPendingActionsQuery, IEnumerable<AiActionDto>>,
    IRequestHandler<GetActionHistoryQuery, IEnumerable<AiActionDto>>
{
    private readonly IAiActionRepository _repository;
    private readonly IMapper _mapper;

    public ActionQueryHandlers(IAiActionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AiActionDto>> Handle(GetActionsQuery request, CancellationToken cancellationToken)
    {
        var actions = await _repository.GetAllAsync(request.OrganizationId);
        return _mapper.Map<IEnumerable<AiActionDto>>(actions);
    }

    public async Task<IEnumerable<AiActionDto>> Handle(GetPendingActionsQuery request, CancellationToken cancellationToken)
    {
        var actions = await _repository.GetPendingAsync(request.OrganizationId);
        return _mapper.Map<IEnumerable<AiActionDto>>(actions);
    }

    public async Task<IEnumerable<AiActionDto>> Handle(GetActionHistoryQuery request, CancellationToken cancellationToken)
    {
        var actions = await _repository.GetHistoryAsync(request.OrganizationId);
        return _mapper.Map<IEnumerable<AiActionDto>>(actions);
    }
}
