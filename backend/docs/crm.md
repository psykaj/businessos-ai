# CRM Platform

The BusinessOS AI CRM platform is a highly scalable, multi-tenant solution built using Clean Architecture principles. It allows organizations to manage their sales pipeline, track leads, organize contacts and companies, and log activities and tasks seamlessly.

## Key Features

- **Multi-Tenant Isolation**: All CRM records are scoped strictly to their `OrganizationId`.
- **Sales Pipeline Management**: Visual and data-driven deal stages.
- **Activity Timeline**: Automatic event logging for lead creation, deal updates, and task completions.
- **Global Search**: Search across Leads, Contacts, Companies, and Deals simultaneously.
- **Reporting Foundation**: Built-in endpoints for calculating total leads, revenue forecasts, and conversion rates.

## Architecture

The CRM is built as a separate module under `backend/Modules/CRM/`. It utilizes:
- **Controllers** for API routing.
- **Services** for business logic and activity logging.
- **Repositories** inheriting from `GenericRepository` for DB access.
- **AutoMapper** for DTO mappings.
