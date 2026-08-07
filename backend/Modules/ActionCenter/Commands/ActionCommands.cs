using System;
using MediatR;

namespace backend.Modules.ActionCenter.Commands;

public class ExecuteActionCommand : IRequest<bool>
{
    public Guid OrganizationId { get; set; }
    public Guid ActionId { get; set; }
    public Guid UserId { get; set; }
}

public class ApproveActionCommand : IRequest<bool>
{
    public Guid OrganizationId { get; set; }
    public Guid ActionId { get; set; }
    public Guid UserId { get; set; }
}

public class RejectActionCommand : IRequest<bool>
{
    public Guid OrganizationId { get; set; }
    public Guid ActionId { get; set; }
    public Guid UserId { get; set; }
}
