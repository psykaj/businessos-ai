# BusinessOS AI - Database Architecture

## 1. Overall Architecture
The database layer is designed using **Clean Architecture** principles and the **Repository Pattern** with a central **Unit of Work**. 
We use **Entity Framework Core** with **PostgreSQL**, leveraging Fluent API for a clean separation of domain concerns from persistence configuration.

## 2. Multi-Tenant Strategy
We implement a **Shared Database, Shared Schema** approach for multi-tenancy.
- **Tenant Root:** The `Organization` entity represents a tenant (business).
- **Tenant Isolation:** Every core business entity (Customer, Invoice, QRCode, etc.) contains an `OrganizationId` foreign key.
- **Data Access:** In the future, this can be securely isolated by applying Global Query Filters on `OrganizationId` within the `ApplicationDbContext` based on the scoped user's context.

## 3. Entity Relationships
- **1-to-Many (Organization -> Users):** An `Organization` has many `Users`. A user strictly belongs to one organization.
- **1-to-Many (Organization -> Core Entities):** Each core business module entity inherits from `BaseEntity` and contains a reference to `Organization`.

## 4. Global Soft Delete Strategy
Physical deletion of records is forbidden to preserve data integrity and historical reporting.
- **Implementation:** When `Delete(entity)` is called on a repository, or `SaveChangesAsync()` intercepts an `EntityState.Deleted`, we change the state to `EntityState.Modified` and flag `IsDeleted = true` along with setting `DeletedAt`.
- **Query Filter:** Every entity configuration applies a global query filter: `builder.HasQueryFilter(e => !e.IsDeleted);`. This ensures soft-deleted records are automatically excluded from all LINQ queries unless explicitly requested.

## 5. Audit System
A standardized audit trail is implemented globally via `BaseEntity`.
- Fields include `CreatedAt`, `UpdatedAt`, `CreatedBy`, and `UpdatedBy`.
- `ApplicationDbContext.SaveChangesAsync` automatically intercepts inserts and updates to stamp `CreatedAt` and `UpdatedAt` centrally. Developers do not need to manually manage these fields.

## 6. Repository Pattern & Unit of Work
- **IGenericRepository<T>:** Provides a standard contract for CRUD operations (`GetByIdAsync`, `GetAllAsync`, `FindAsync`, `AddAsync`, `Update`, `Delete`).
- **Unit of Work (`IUnitOfWork`):** Manages a concurrent dictionary of generic repositories. It provides a single `CompleteAsync()` method which wraps `SaveChangesAsync`, ensuring atomic transactions across multiple repository operations.

## 7. Scalability & Extensibility Considerations
- **RowVersion:** Included in `BaseEntity` for future optimistic concurrency control.
- **Indexes:** Applied strategically to foreign keys (`OrganizationId`), high-frequency lookup fields (`Email`, `Slug`), and sorting fields (`CreatedAt`).
- **Extensibility:** The `BaseEntity` and `GenericRepository` allow any new module (e.g., Dynamic QR Codes, CRM, Reviews) to be integrated in minutes just by inheriting `BaseEntity` and applying `BaseEntityConfiguration<T>`.
