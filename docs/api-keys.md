# API Key Management Security Strategy

The BusinessOS AI platform allows organizations to generate API keys for programmatic access.

## Security Guarantees
1. **Hashing at Rest**: API keys are generated using a cryptographically secure RNG. The raw key is returned to the user *only once*. The database only stores a SHA-256 hash (`KeyHash`).
2. **Never Exposed**: Since we only store the hash, it's mathematically impossible to retrieve the original key from the database if compromised.
3. **Tracking**: We track the `LastUsedAt` timestamp for each key to allow organizations to audit active integrations.
4. **Revocation & Rotation**: Keys can be revoked at any time. A key rotation endpoint seamlessly revokes an old key and issues a new one under the same name.

## Endpoints
- `POST /api/v1/organizations/{orgId}/apikeys` (Generate)
- `GET /api/v1/organizations/{orgId}/apikeys` (List)
- `DELETE /api/v1/organizations/{orgId}/apikeys/{keyId}` (Revoke)
- `POST /api/v1/organizations/{orgId}/apikeys/{keyId}/rotate` (Rotate)
