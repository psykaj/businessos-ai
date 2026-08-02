using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.RealTime;

[Authorize]
public class CommunicationHubSignalR : Hub
{
    private readonly ILogger<CommunicationHubSignalR> _logger;

    public CommunicationHubSignalR(ILogger<CommunicationHubSignalR> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var organizationId = Context.User?.FindFirst("OrganizationId")?.Value ?? Context.User?.FindFirst("organizationId")?.Value;
        var userId = Context.UserIdentifier ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Context.User?.FindFirst("sub")?.Value;

        if (!string.IsNullOrEmpty(organizationId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"CommOrg_{organizationId}");
            _logger.LogInformation("Client {ConnectionId} joined communication organization group {OrgId}", Context.ConnectionId, organizationId);
        }
        
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"CommUser_{userId}");
            _logger.LogInformation("Client {ConnectionId} joined communication user group {UserId}", Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var organizationId = Context.User?.FindFirst("OrganizationId")?.Value ?? Context.User?.FindFirst("organizationId")?.Value;
        var userId = Context.UserIdentifier ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Context.User?.FindFirst("sub")?.Value;

        if (!string.IsNullOrEmpty(organizationId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"CommOrg_{organizationId}");
        }
        
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"CommUser_{userId}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Allow agents to subscribe to real-time events for a specific conversation
    public async Task JoinConversationGroup(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"CommConversation_{conversationId}");
        _logger.LogInformation("Client {ConnectionId} joined conversation channel {ConversationId}", Context.ConnectionId, conversationId);
    }

    public async Task LeaveConversationGroup(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"CommConversation_{conversationId}");
    }
}
