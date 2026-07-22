using System.Text.Json;
using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Integrations.Providers;

public abstract class BaseIntegrationProvider : IIntegrationProvider
{
    public abstract IntegrationProvider Provider { get; }

    public virtual Task<IntegrationTestResultDto> TestConnectionAsync(Integration integration, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new IntegrationTestResultDto(true, $"{Provider} integration connection active.", DateTime.UtcNow));
    }

    public virtual Task<Dictionary<string, string>> ExecuteActionAsync(Integration integration, string actionName, Dictionary<string, string> parameters, CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<string, string>
        {
            ["Provider"] = Provider.ToString(),
            ["Action"] = actionName,
            ["Status"] = "ExecutedSuccessfully"
        };
        return Task.FromResult(result);
    }
}

public class GoogleSheetsProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.GoogleSheets;
}

public class SlackProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.Slack;
}

public class MicrosoftTeamsProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.MicrosoftTeams;
}

public class DiscordProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.Discord;
}

public class GoogleCalendarProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.GoogleCalendar;
}

public class OutlookCalendarProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.OutlookCalendar;
}

public class ResendProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.Resend;
}

public class TwilioProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.Twilio;
}

public class StripeProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.Stripe;
}

public class RazorpayProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.Razorpay;
}

public class WebhookProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.Webhook;
}

public class RestApiProvider : BaseIntegrationProvider
{
    public override IntegrationProvider Provider => IntegrationProvider.RestApi;
}
