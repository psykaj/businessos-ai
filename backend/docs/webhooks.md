# Webhooks Module

The Webhook Engine securely dispatches system events (e.g., `FormSubmitted`, `LeadCreated`) to external integrations like n8n, Zapier, Make, and Power Automate.

## Architecture
- **In-Memory Queue**: Uses `System.Threading.Channels` for robust, high-performance in-memory queuing of webhook deliveries, ensuring HTTP request threads are never blocked.
- **Background Worker**: `WebhookBackgroundService` consumes the queue and attempts HTTP POST deliveries.
- **Retry Mechanism**: Implements an exponential backoff strategy, retrying failed payloads up to 3 times.
- **Security**: Supports cryptographic payload signing via HMAC-SHA256 (`X-Hub-Signature-256`) when a secret is provided.

## Data Models
- `WebhookSubscription`: Tracks endpoints, event types, and secrets.
- `WebhookDelivery`: Audit log of each payload, HTTP status code, and attempt count.
