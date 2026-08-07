using System;
using System.Collections.Generic;
using backend.Modules.ActionCenter.DTOs;
using MediatR;

namespace backend.Modules.ActionCenter.Queries;

public class GetActionsQuery : IRequest<IEnumerable<AiActionDto>>
{
    public Guid OrganizationId { get; set; }
}

public class GetPendingActionsQuery : IRequest<IEnumerable<AiActionDto>>
{
    public Guid OrganizationId { get; set; }
}

public class GetActionHistoryQuery : IRequest<IEnumerable<AiActionDto>>
{
    public Guid OrganizationId { get; set; }
}
