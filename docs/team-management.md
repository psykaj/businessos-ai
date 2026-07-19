# Team Management Architecture

BusinessOS AI uses a robust, scalable Team Management architecture. It supports multi-tenant isolation through the `TeamMember` entity.

## Core Entities
- **Organization**: The top-level tenant.
- **User**: The global user identity.
- **TeamMember**: Maps a `User` to an `Organization` and assigns a specific `Role`.
- **Invitation**: Allows securely adding new users to an organization via a unique token.

## Invitation Workflow
1. An admin invites a user by email and assigns them a role.
2. An `Invitation` record is created with a secure token and a 7-day expiration.
3. The invited user receives the token (via email).
4. The user creates an account (if new) and accepts the invitation using the token.
5. A `TeamMember` record is created, officially adding the user to the organization.

## API Endpoints
- `POST /api/v1/organizations/{orgId}/team/invite`
- `POST /api/v1/invitations/{token}/accept`
- `POST /api/v1/invitations/{token}/reject`
- `GET /api/v1/organizations/{orgId}/team`
- `PUT /api/v1/organizations/{orgId}/team/{memberId}/role`
- `DELETE /api/v1/organizations/{orgId}/team/{memberId}`
- `POST /api/v1/organizations/{orgId}/team/{memberId}/deactivate`
- `POST /api/v1/organizations/{orgId}/team/{memberId}/reactivate`
