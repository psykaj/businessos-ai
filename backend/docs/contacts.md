# Contacts Management

Contacts are individual people associated with an Organization, and optionally tied to a specific Company.

## Features

- **Consolidated Information**: Stores phone, email, address, job title, and social links.
- **Relationships**: A Contact can belong to a Company and be associated with multiple Deals.
- **Activity Tracking**: Any calls, meetings, or emails with a contact are tracked via the CrmActivities table.

## Endpoints

- `GET /api/crm/contacts`
- `POST /api/crm/contacts`
