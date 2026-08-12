namespace backend.Modules.Outcomes.Entities;

public enum OutcomeType
{
    RevenueIncrease,
    RevenueRecovered,
    CostReduction,
    TimeSaved,
    CustomerRetained,
    CustomerConverted,
    LeadConverted,
    InvoicePaid,
    InventoryStockoutAvoided,
    AutomationCompleted,
    ActionCompleted,
    RiskReduced,
    OperationalImprovement
}

public enum OutcomeSourceType
{
    AiRecommendation,
    ActionCenter,
    Automation,
    BusinessBriefing,
    Alert,
    CRM,
    Finance,
    Inventory,
    Customer360,
    BusinessIntelligence,
    UserReported,
    Unknown
}

public enum OutcomeConfidence
{
    Unknown,
    Low,
    Medium,
    High
}

public enum AttributionLevel
{
    Unknown,
    Weak,
    Moderate,
    Strong,
    Direct
}

public enum OutcomeStatus
{
    Draft,
    Active,
    Voided,
    Archived
}
