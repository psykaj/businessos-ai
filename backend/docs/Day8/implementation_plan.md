# BusinessOS AI – Organization, Team Management & RBAC Implementation Plan

This document outlines the architecture and steps to implement the Organization, Team Management, and RBAC features for BusinessOS AI.

## User Review Required

> [!WARNING]
> **Database Schema Changes:** The current `User` entity has a direct `OrganizationId` property and a `Role` string. The new requirements introduce a `TeamMember` entity (linking `User` and `Organization` with a `RoleId`) and a `UserRole` entity.
> 
> **Action:** I will update the `User` entity to remove the hardcoded `Role` string (or keep it as fallback) and remove the direct `OrganizationId` (or make it nullable as a "default organization"), relying instead on `TeamMember` for tenant isolation and `UserRole`/`TeamMember.RoleId` for RBAC. I will also update `Organization.cs` to match all your requested fields. 
> 
> **Are you okay with this schema migration strategy for `User` and `Organization`?**

## Open Questions

> [!IMPORTANT]
> 1. Do you want the new entities (e.g., `TeamMember`, `Role`, `Permission`, `ApiKey`) to be placed in the global `backend/Entities/` folder, or inside their respective module folders (e.g., `backend/Modules/Team/Entities/`)? Both patterns seem to exist currently. I will use the `backend/Entities/` folder for consistency with `Organization` and `User` unless you prefer otherwise.
> 2. The requirements mention creating separate modules for `Roles` and `Permissions`. I can combine these into an `RBAC` module or keep them as strictly separate `Modules/Roles` and `Modules/Permissions`. I will keep them separate as requested, but let me know if a unified `RBAC` module is preferred.

## Proposed Changes

---

### Entities & Database Context
Create and update core entities to support RBAC, Teams, API Keys, and Audit Logs.

#### [MODIFY] [Organization.cs](file:///backend/Entities/Organization.cs)
Update properties to match: `Id`, `Name`, `Slug`, `LogoUrl`, `Website`, `Industry`, `Address`, `TimeZone`, `Language`, `Currency`, `SubscriptionId`, `IsActive`, `CreatedAt`, `UpdatedAt`.

#### [MODIFY] [User.cs](file:///backend/Entities/User.cs)
Update relationship properties to support `TeamMember` and `UserRole`.

#### [NEW] [TeamMember.cs](file:///backend/Entities/TeamMember.cs)
#### [NEW] [Role.cs](file:///backend/Entities/Role.cs)
#### [NEW] [Permission.cs](file:///backend/Entities/Permission.cs)
#### [NEW] [UserRole.cs](file:///backend/Entities/UserRole.cs)
#### [NEW] [Invitation.cs](file:///backend/Entities/Invitation.cs)
#### [NEW] [ApiKey.cs](file:///backend/Entities/ApiKey.cs)
#### [NEW] [AuditLog.cs](file:///backend/Entities/AuditLog.cs)

#### [MODIFY] [ApplicationDbContext.cs](file:///backend/Persistence/ApplicationDbContext.cs)
Add new `DbSet`s and apply configurations/relationships via `OnModelCreating`.

---

### Infrastructure & Repositories
Implement the repository layer using Dependency Injection.

#### [NEW] [IOrganizationRepository.cs](file:///backend/Modules/Organization/Repositories/IOrganizationRepository.cs)
#### [NEW] [OrganizationRepository.cs](file:///backend/Modules/Organization/Repositories/OrganizationRepository.cs)
*(Repeat for Team, Role, Permission, ApiKey, AuditLog in their respective module folders)*

---

### Module: Organization
Implement Organization REST APIs and Services.

#### [NEW] [OrganizationController.cs](file:///backend/Modules/Organization/Controllers/OrganizationController.cs)
APIs: Get, Update, Upload Logo, Update Profile, Update Settings.
#### [NEW] [OrganizationService.cs](file:///backend/Modules/Organization/Services/OrganizationService.cs)
#### [NEW] DTOs, Validators, and Interfaces for Organization.

---

### Module: Team
Implement Team Management APIs, including Invitations.

#### [NEW] [TeamController.cs](file:///backend/Modules/Team/Controllers/TeamController.cs)
APIs: Invite, Accept/Reject, List, Update Role, Remove, Deactivate/Reactivate.
#### [NEW] [TeamService.cs](file:///backend/Modules/Team/Services/TeamService.cs)
#### [NEW] DTOs, Validators, and Interfaces for Team Management.

---

### Module: Roles & Permissions (RBAC)
Develop a reusable authorization mechanism.

#### [NEW] [RolesController.cs](file:///backend/Modules/Roles/Controllers/RolesController.cs)
#### [NEW] [PermissionsController.cs](file:///backend/Modules/Permissions/Controllers/PermissionsController.cs)
#### [NEW] [RbacAuthorizationHandler.cs](file:///backend/Middleware/RbacAuthorizationHandler.cs) (or similar policy provider)
#### [NEW] Required System Roles seed logic (`Owner`, `Admin`, `Manager`, `Member`, `Viewer`).

---

### Module: ApiKeys
Implement API Key management.

#### [NEW] [ApiKeysController.cs](file:///backend/Modules/ApiKeys/Controllers/ApiKeysController.cs)
APIs: Generate (hash stored, display once), List, Revoke, Rotate.
#### [NEW] [ApiKeyService.cs](file:///backend/Modules/ApiKeys/Services/ApiKeyService.cs)

---

### Module: AuditLogs
Implement Audit Logging system.

#### [NEW] [AuditLogsController.cs](file:///backend/Modules/AuditLogs/Controllers/AuditLogsController.cs)
#### [NEW] [AuditLogService.cs](file:///backend/Modules/AuditLogs/Services/AuditLogService.cs)
#### [NEW] [AuditLogInterceptor.cs](file:///backend/Middleware/AuditLogInterceptor.cs) (Optional, for automatic EF Core logging, or use explicit service calls).

---

### Documentation
Update documentation as requested.

#### [NEW] [team-management.md](file:///docs/team-management.md)
#### [NEW] [rbac.md](file:///docs/rbac.md)
#### [NEW] [api-keys.md](file:///docs/api-keys.md)
#### [MODIFY] [roadmap.md](file:///docs/roadmap.md)

## Verification Plan

### Automated Checks
- `dotnet build` to ensure no compile errors or EF Core warnings.
- `dotnet ef migrations add Day8_RBAC_Team_ApiKeys` to verify migration creation.
- `dotnet ef database update` to ensure migration applies correctly.

### Manual Verification
- Code review to ensure Clean Architecture, DRY, and SOLID principles are maintained.
- Check dependency injection registrations in `Program.cs`.
- Ensure JWT and Role/Permission-based authorization attributes are applied to controllers.
