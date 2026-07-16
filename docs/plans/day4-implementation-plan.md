# Day 4: Database Foundation & Multi-Tenant Architecture

This document outlines the architectural changes and additions to establish a robust, multi-tenant database foundation for BusinessOS AI, following Clean Architecture principles within a single monolithic backend project structure.

## User Review Required
> [!IMPORTANT]
> The database structure is significantly changing. The `User` entity will now be tied to an `Organization`. Seed data will automatically populate a default organization and admin user during development.
> Please review the core entities and their properties before execution to ensure they meet your long-term requirements. Let me know if any properties need adjustment.

## Open Questions
> [!NOTE]
> 1. Should we link `CreatedBy` and `UpdatedBy` in `BaseEntity` as `Guid?` (referencing `User.Id`) or keep them as `string?` (for simpler audit logs)? The plan currently assumes `string?` to store the User ID as a string, preventing complex circular relationships in some contexts, but `Guid?` is strictly typed.
> 2. Will users be able to belong to multiple organizations in the future? The current requirement is "One Organization → Many Users". We will stick to a strict 1-to-Many relationship (User has one `OrganizationId`), but this is a critical decision.

## Proposed Changes

### Clean Architecture Folders
We will organize the `backend/` project with the following folder structure to align with the Clean Architecture and enterprise patterns requested:
- `Entities/` (Domain Entities)
- `Common/` (Base classes like `BaseEntity`)
- `Configurations/` (Entity Framework Fluent API Configurations)
- `Persistence/` (DbContext and related data access configurations)
- `Repositories/` (Implementations of Repositories and Unit of Work)
- `Interfaces/` (Contracts for Repositories, Unit of Work, and Services)
- `Services/` (Business logic)
- `Seed/` (Database seed data)
- `Migrations/` (EF Core Migrations)
- `Extensions/` (Extension methods like DI registration)
- `Exceptions/` (Custom exceptions)
- `Constants/` (Static values)

---

### Common
#### [NEW] [BaseEntity.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Common/BaseEntity.cs)
A reusable base class containing:
- `Id` (Guid)
- `CreatedAt`, `UpdatedAt` (DateTime)
- `CreatedBy`, `UpdatedBy` (string?)
- `IsDeleted` (bool)
- `DeletedAt` (DateTime?)
- `RowVersion` (byte[])

---

### Entities
#### [MODIFY] [User.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Entities/User.cs)
Update to inherit from `BaseEntity`. Add `OrganizationId` and `Organization` navigation property.

#### [NEW] [Organization.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Entities/Organization.cs)
The multi-tenant root entity representing a business. Contains `Name`, `Slug`, `Industry`, `Email`, `Phone`, `LogoUrl`, `Website`, `Address`, `City`, `State`, `Country`, `PostalCode`, `TimeZone`, `Currency`, `SubscriptionPlan`, `SubscriptionStatus`, `IsActive`, and a collection of `Users`.

#### [NEW] [Business Entities](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Entities)
Create production-ready skeleton classes inheriting from `BaseEntity` with an `OrganizationId` for:
- `Customer.cs`
- `QRCode.cs`
- `Review.cs`
- `Invoice.cs`
- `Campaign.cs`
- `Appointment.cs`
- `Contact.cs`
- `LandingPage.cs`
- `AIConversation.cs`
- `Notification.cs`
- `ActivityLog.cs`
- `BusinessCard.cs`
- `Subscription.cs`

---

### Interfaces
#### [NEW] [IGenericRepository.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Interfaces/IGenericRepository.cs)
Contract for the generic repository pattern.
#### [NEW] [IUnitOfWork.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Interfaces/IUnitOfWork.cs)
Contract for the Unit of Work.

---

### Repositories
#### [NEW] [GenericRepository.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Repositories/GenericRepository.cs)
Implementation of `IGenericRepository` using `ApplicationDbContext`.
#### [NEW] [UnitOfWork.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Repositories/UnitOfWork.cs)
Implementation of `IUnitOfWork` orchestrating repositories and `SaveChangesAsync`.

---

### Configurations
#### [NEW] [EntityConfigurations](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Configurations)
Implement `IEntityTypeConfiguration<T>` for each entity to configure:
- Table names, constraints, maximum lengths.
- Soft Delete query filter (`e => !e.IsDeleted`).
- Global multi-tenant indexing (e.g., `HasIndex(e => e.OrganizationId)`).

---

### Persistence
#### [MODIFY] [ApplicationDbContext.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Data/ApplicationDbContext.cs) -> Move to `Persistence/`
- Add `DbSet` properties for all new entities.
- Override `SaveChangesAsync` to automatically set `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`, and handle Soft Delete logic (`IsDeleted = true`, `DeletedAt = DateTime.UtcNow` instead of physical deletion).
- Apply configurations via `modelBuilder.ApplyConfigurationsFromAssembly()`.

---

### Seed
#### [NEW] [SeedData.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Seed/SeedData.cs)
Implement a database initializer to seed a default Organization, an Admin User, roles, subscription plan, and sample customer.

---

### Extensions
#### [NEW] [ServiceCollectionExtensions.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Extensions/ServiceCollectionExtensions.cs)
Extension methods to register `DbContext`, `UnitOfWork`, and generic repositories cleanly in `Program.cs`.

---

### Documentation
#### [NEW] [database-architecture.md](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/docs/database-architecture.md)
A comprehensive document explaining the architecture, multi-tenancy, soft delete, and repositories.

---

## Verification Plan

### Automated Tests
- Build the solution using `dotnet build`.
- Add and apply EF Core migrations using `dotnet ef migrations add` and `dotnet ef database update`.

### Manual Verification
- Review the generated PostgreSQL schema via `psql` or logs to ensure tables and relationships are created accurately.
- Verify the seed data is correctly populated in the database on startup.
