# Custom Domains Module

The Custom Domains module allows organizations to map their own domains to their Landing Pages and QR Code redirects.

## Features
- **Verification Workflow**: Support for TXT record domain ownership verification.
- **SSL Generation**: Ready to hook into automatic SSL provisions (like Let's Encrypt or Cloudflare).
- **Primary Domains**: Organizations can select a primary domain for their public assets.

## API Endpoints
- `GET /api/domains` - List all domains.
- `POST /api/domains` - Register a new custom domain.
- `PUT /api/domains/{id}` - Set a domain as primary.
- `DELETE /api/domains/{id}` - Remove a domain.
- `POST /api/domains/{id}/verify` - Trigger verification check.
- `GET /api/domains/{id}/dns` - Get DNS instructions for the domain.
