# Leads Management

Leads represent potential customers or business opportunities before they are qualified.

## Workflow

1. **Creation**: Leads are captured via API or UI and saved in the CRM.
2. **Assignment**: Leads can be assigned to a specific user/team member (`AssignedUserId`).
3. **Qualification**: Leads progress through statuses (New -> Contacted -> Qualified).
4. **Conversion**: Once qualified, a lead is typically converted into a Contact, Company, and Deal.

## Endpoints

- `GET /api/crm/leads`
- `GET /api/crm/leads/{id}`
- `POST /api/crm/leads`
- `PUT /api/crm/leads/{id}`
- `DELETE /api/crm/leads/{id}`
