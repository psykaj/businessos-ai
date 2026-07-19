# Role-Based Access Control (RBAC)

The RBAC implementation provides fine-grained access control to organization resources.

## System Roles
The system includes predefined roles out of the box:
- **Owner**: Full access to the organization, including billing and API Keys.
- **Admin**: Full access to standard features (QR, Analytics, Team Management) but typically restricted from billing destruction or org deletion.
- **Manager**: Can manage content and view analytics.
- **Member**: Can create content but cannot manage team.
- **Viewer**: Read-only access to specific modules.

## Permissions System
Permissions are grouped logically using `Constants.AppPermissions`.
Example: `Dashboard.View`, `QRCodes.Create`, `TeamManagement.ManageRoles`.

## Authorization
We use ASP.NET Core Policy-based Authorization:
- `[RequirePermission(AppPermissions.OrganizationSettings.Update)]` Attribute protects controllers/actions.
- `PermissionAuthorizationHandler` evaluates if the user's current role allows this action.

In the future, this can be extended to allow custom tenant-specific roles (where tenants create their own roles and assign permissions).
