# BusinessOS AI - Day 17: Document Management System (DMS) Frontend Architecture

## Overview

The Document Center Frontend provides a modern, responsive, Google Drive / DocuSign-tier document platform built using **Next.js 15 App Router**, **React 19**, **TypeScript**, **Tailwind CSS**, **Lucide Icons**, and **TanStack React Query**.

---

## 1. Page Routes & Navigation Structure

All routes are nested under `/dashboard` with integrated sidebar navigation:

| Route | Title | Description |
| :--- | :--- | :--- |
| `/dashboard/documents` | **Document Center** | Main file explorer, drag-and-drop upload zone, folder navigation, grid/list toggle, search, favorites, tagging, and upload progress. |
| `/dashboard/documents/[id]` | **Document Viewer & Detail** | Dual-tab view: interactive document preview/download stream, historical version control timeline with reversion, and activity audit logs. |
| `/dashboard/documents/templates` | **Document Templates** | Template library categorized by Contract, NDA, Proposal, Invoice, and HR with variable placeholder substitution (`{{VariableName}}`) and 1-click document generator. |
| `/dashboard/approvals` | **Approval Workspace** | Multi-level approval requests pipeline (Pending, Approved, Rejected) with review modal, comments, and escalation actions. |
| `/dashboard/signatures` | **E-Signature Workspace** | Signature request creator (multi-signer, roles, PIN access code), progress badges, SHA-256 cryptographic hash verification, and completion certificates. |
| `/dashboard/shared` | **Shared Files & Links** | Team permissions & passcode-encrypted public link sharing with expiration dates and view count tracking. |

---

## 2. Component & Custom Hook Architecture

Located in:
- `frontend/types/document.ts`: Comprehensive TypeScript DTO interfaces.
- `frontend/lib/documents-service.ts`: Centralized API client service wrapping all backend DMS REST endpoints.
- `frontend/hooks/use-documents.ts`: React Query hooks with automatic cache invalidation (`useDocuments`, `useFolders`, `useUploadDocument`, `useDocumentVersions`, `useApprovals`, `useCreateSignatureRequest`, etc.).

---

## 3. Key UX Capabilities

- **Drag-and-Drop Uploads**: File dropzone with real-time percentage progress bar.
- **Hierarchical Breadcrumbs**: Deep folder navigation with path history (`Root > Contracts > 2026`).
- **Responsive Layout**: Designed for mobile, tablet, and desktop screens with dark mode support.
- **Audit & Cryptographic Checksums**: Built-in visual indicators for SHA-256 hashes and timestamped audit logs.
