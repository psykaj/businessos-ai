# BusinessOS AI - Day 17: Document Management System (DMS), E-Signature & Approval Workflow Architecture

## Executive Summary

The **Document Management System (DMS)** in BusinessOS AI provides a enterprise-grade, multi-tenant centralized document engine comparable to DocuSign, Dropbox Business, Google Drive, and PandaDoc.

It enables businesses to store, manage, search, approve, sign, and share critical documents with full audit compliance, multi-level approvals, provider-independent e-signatures, and storage layer abstractions.

---

## Architecture & Sub-Modules

Located in `backend/Modules/Documents`, the system adheres strictly to Clean Architecture, Repository Pattern, CQRS, and SOLID principles.

```
backend/Modules/Documents/
├── Approvals/               # Multi-level approval workflows (Sequential/Parallel/Escalation)
├── AuditLogs/               # Immutable activity audit trail
├── Configurations/          # EF Core entity type configurations & indexes
├── Controllers/             # Base and domain controllers
├── Documents/               # Core file CRUD, upload stream, versioning, search, tagging, favorites
├── DocumentTemplates/       # Contract/Document templates & variable rendering engine
├── Entities/                # Database entities with tenant isolation
├── ESignatures/             # E-signature request, token verification, SHA256 checksums, provider abstraction
├── Extensions/              # Service collection DI registration
├── Folders/                 # Hierarchical folder structures & tree traversal
├── Sharing/                 # Internal RBAC sharing & passcode-protected public link sharing
├── Storage/                 # Unified IStorageService abstraction (Local Storage + Azure Blob Storage)
└── Tests/                   # Automated integration test suite
```

---

## 1. Storage Abstraction Layer (`IStorageService`)

Supports pluggable cloud-native storage options:
- **LocalStorageService**: High-performance local storage with HMAC-SHA256 signed download tokens.
- **AzureBlobStorageService**: Enterprise Azure Blob Storage provider integration with seamless fallback.

### Presigned URL Security
Downloader endpoints use HMAC-SHA256 token validation with configurable expiration times (e.g. 30 minutes) to prevent unauthorized hotlinking or unauthorized direct file access.

---

## 2. Document & Version Control System

- **Multi-Versioning**: Every file upload creates a `DocumentVersion` entity. Reversion to any past version updates the pointer without destroying historical binaries.
- **Tags & Metadata**: Support for tags and JSON key-value metadata for full-text search filtering.
- **Hierarchical Folders**: Full parent-child folder trees with dynamic path building (`/Invoices/2026/Q1`).
- **Soft Delete & Restore**: Soft deletion with `IsDeleted` flags and `DeletedAt` timestamps, preventing accidental data loss.

---

## 3. Approval Workflow Engine

- **Multi-Level Approvals**: Supports multi-step approval pipelines.
- **Sequential & Parallel Approvals**: Configure steps to execute sequentially (Step 1 -> Step 2) or in parallel groups.
- **Comments & Audit**: Comprehensive approval/rejection commentary recorded in audit trails.
- **Escalation Engine**: Automated escalation for overdue approvals (`AutoEscalate`, `DueDate`).

---

## 4. E-Signature Foundation

- **Provider-Independent Abstraction**: Abstract `IESignatureProviderService` allows plugging in self-hosted native engine or DocuSign / Adobe Sign APIs.
- **Multi-Signer Workflow**: Multi-recipient ordering, custom roles (`Signer`, `Viewer`, `Approver`, `CC`).
- **Security Checksum**: SHA-256 cryptographic hash of document payload.
- **Public Sign Tokens & Passcodes**: Tokenized landing links with optional PIN access codes.
- **Immutable Audit Trail**: Tracks IP address, User Agent, viewed timestamp, and signed timestamp for legal compliance.

---

## 5. Security & Multi-Tenant Scoping

- **Tenant Isolation**: Every database query filters automatically on `OrganizationId`. Cross-tenant data leakage is strictly prevented.
- **Role-Based Access Control (RBAC)**: Integrated JWT claims validation for organization and user contexts.

---

## 6. API Endpoint Summary

| Category | Method | Endpoint | Description |
| :--- | :--- | :--- | :--- |
| **Documents** | `POST` | `/api/documents/upload` | Upload document file stream |
| | `POST` | `/api/documents/{id}/versions` | Upload new version |
| | `GET` | `/api/documents/{id}` | Get document metadata |
| | `GET` | `/api/documents/{id}/download` | Stream file or signed URL download |
| | `GET` | `/api/documents/{id}/preview` | Get presigned preview link & metadata |
| | `PUT` | `/api/documents/{id}/rename` | Rename document |
| | `PUT` | `/api/documents/{id}/move` | Move document to target folder |
| | `POST` | `/api/documents/{id}/copy` | Duplicate document |
| | `DELETE`| `/api/documents/{id}` | Soft delete document |
| | `POST` | `/api/documents/{id}/restore` | Restore soft deleted document |
| | `GET` | `/api/documents/{id}/versions` | List version history |
| | `POST` | `/api/documents/{id}/versions/{versionId}/revert` | Revert document to version |
| | `GET` | `/api/documents/search` | Search documents (query, tag, folder, date) |
| | `PUT` | `/api/documents/{id}/tags` | Update document tags |
| | `POST` | `/api/documents/{id}/favorite` | Toggle favorite flag |
| **Folders** | `POST` | `/api/folders` | Create folder |
| | `GET` | `/api/folders` | List folders in parent |
| | `GET` | `/api/folders/tree` | Get nested folder tree |
| | `PUT` | `/api/folders/{id}/move` | Move folder to target parent |
| **Templates**| `POST` | `/api/document-templates` | Create template |
| | `POST` | `/api/document-templates/{id}/render` | Generate document from template |
| **Approvals**| `POST` | `/api/approvals` | Create multi-level approval request |
| | `POST` | `/api/approvals/{id}/approve` | Approve step with comments |
| | `POST` | `/api/approvals/{id}/reject` | Reject step with reason |
| **E-Sign** | `POST` | `/api/esignatures` | Create multi-signer signature request |
| | `GET` | `/api/esignatures/public/sign/{token}` | Public signer view link |
| | `POST` | `/api/esignatures/public/sign/{token}` | Submit base64 signature & IP log |
| | `GET` | `/api/esignatures/{id}/audit-trail` | Download complete audit trail & hash |
| **Sharing** | `POST` | `/api/documents/share` | Create internal permission or public link |
| | `POST` | `/api/documents/public/{token}` | Access public document with passcode |
