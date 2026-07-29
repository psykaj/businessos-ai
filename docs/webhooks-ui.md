# Webhooks UI

The Webhooks UI manages real-time HTTP callbacks to external endpoints.

## Features
- **Endpoint Registration**: Users can register `https://` URLs to receive specific event payloads (e.g., `customer.created`).
- **Data Table**: Displays configured URLs, event types, status, and the result of the last delivery attempt (Success/Failed).
- **Testing Utility**: Allows users to manually trigger a test payload from the UI to verify endpoint health without generating real domain events.
- **Validation**: Enforces strict `https://` URLs through Zod schema validation to ensure secure deliveries.
