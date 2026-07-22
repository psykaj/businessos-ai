using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Triggers.Handlers;

public abstract class BaseTriggerHandler : ITriggerHandler
{
    public abstract TriggerType TriggerType { get; }

    public Task<Dictionary<string, string>> ExtractContextAsync(string triggerConfiguration, Dictionary<string, string> rawPayload)
    {
        var context = new Dictionary<string, string>(rawPayload, StringComparer.OrdinalIgnoreCase);
        return Task.FromResult(context);
    }
}

public class LeadCreatedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.LeadCreated;
}

public class LeadUpdatedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.LeadUpdated;
}

public class LeadQualifiedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.LeadQualified;
}

public class QRCodeScannedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.QRCodeScanned;
}

public class CustomerRegisteredTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.CustomerRegistered;
}

public class InvoicePaidTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.InvoicePaid;
}

public class PaymentFailedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.PaymentFailed;
}

public class SubscriptionExpiringTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.SubscriptionExpiring;
}

public class FormSubmittedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.FormSubmitted;
}

public class CampaignCompletedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.CampaignCompleted;
}

public class TeamMemberAddedTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.TeamMemberAdded;
}

public class ScheduledTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.ScheduledTrigger;
}

public class ManualTriggerHandler : BaseTriggerHandler
{
    public override TriggerType TriggerType => TriggerType.ManualTrigger;
}
