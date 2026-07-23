namespace backend.Modules.BusinessIntelligence.Constants;

public static class BIConstants
{
    public static class KPINames
    {
        public const string TotalRevenue = "Total Revenue";
        public const string MonthlyRevenue = "Monthly Revenue";
        public const string NewCustomers = "New Customers";
        public const string ActiveCustomers = "Active Customers";
        public const string CustomerRetention = "Customer Retention Rate";
        public const string ChurnRate = "Churn Rate";
        public const string TotalLeads = "Total Leads";
        public const string ConversionRate = "Conversion Rate";
        public const string AverageDealSize = "Average Deal Size";
        public const string AverageSalesCycle = "Average Sales Cycle";
        public const string ActiveWorkflows = "Active Workflows";
        public const string AIUsage = "AI Usage";
        public const string QRScans = "QR Scans";
        public const string CampaignROI = "Campaign ROI";
    }

    public static class KPICategories
    {
        public const string Finance = "Finance";
        public const string Sales = "Sales";
        public const string Marketing = "Marketing";
        public const string Customers = "Customers";
        public const string Operations = "Operations";
        public const string AI = "AI";
    }

    public static class ReportTypes
    {
        public const string Executive = "Executive";
        public const string Sales = "Sales";
        public const string Marketing = "Marketing";
        public const string CRM = "CRM";
        public const string Workflow = "Workflow";
        public const string AIUsage = "AIUsage";
    }

    public static class ForecastTypes
    {
        public const string Revenue = "Revenue";
        public const string Sales = "Sales";
        public const string Lead = "Lead";
        public const string CustomerGrowth = "CustomerGrowth";
        public const string Subscription = "Subscription";
    }

    public static class GoalStatuses
    {
        public const string NotStarted = "NotStarted";
        public const string InProgress = "InProgress";
        public const string Achieved = "Achieved";
        public const string Behind = "Behind";
        public const string Failed = "Failed";
    }

    public static class InsightPriorities
    {
        public const string Low = "Low";
        public const string Medium = "Medium";
        public const string High = "High";
        public const string Critical = "Critical";
    }
}
