namespace backend.Modules.Roles.Constants;

public static class AppPermissions
{
    public static class Dashboard
    {
        public const string View = "Dashboard.View";
    }
    
    public static class QRCodes
    {
        public const string View = "QRCodes.View";
        public const string Create = "QRCodes.Create";
        public const string Update = "QRCodes.Update";
        public const string Delete = "QRCodes.Delete";
    }
    
    public static class Analytics
    {
        public const string View = "Analytics.View";
        public const string Export = "Analytics.Export";
    }

    public static class Billing
    {
        public const string View = "Billing.View";
        public const string Manage = "Billing.Manage";
    }

    public static class TeamManagement
    {
        public const string View = "TeamManagement.View";
        public const string Invite = "TeamManagement.Invite";
        public const string ManageRoles = "TeamManagement.ManageRoles";
        public const string Remove = "TeamManagement.Remove";
    }

    public static class OrganizationSettings
    {
        public const string View = "OrganizationSettings.View";
        public const string Update = "OrganizationSettings.Update";
    }

    public static class ApiKeys
    {
        public const string View = "ApiKeys.View";
        public const string Manage = "ApiKeys.Manage";
    }
}
