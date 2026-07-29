# API Keys UI

The API Keys UI is responsible for secure generation, rotation, and revocation of API credentials.

## Features
- **Key Generation Modal**: Allows users to specify a name, optional expiration, and scope limits (e.g., `read:customers`).
- **Secure Display**: Employs a one-time display pattern. The `Client Secret` is shown exactly once upon creation, mimicking standard security best practices (Stripe, GitHub).
- **Data Table**: Shows key prefix, status, scopes, creation dates, and last used timestamps.
- **Actions**: Rotate or Revoke keys with explicit confirmation prompts.
