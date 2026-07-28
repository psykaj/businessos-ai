# Expense Management UI Documentation

The Expense Management workspace (`/dashboard/expenses`) allows SME business owners and finance teams to record, categorize, approve, and track expenses.

## Core UI Components
- `ExpenseListTable`: Searchable, filterable expense list with category color badges, vendor details, payment method, recurring schedule tags, and approval actions.
- `CreateExpenseModal`: React Hook Form powered modal supporting expense creation, tax rate calculations, category selection, recurring frequency, and receipt attachment.
- `Receipt Upload`: Integrated file upload supporting image and PDF receipt attachments via `IReceiptStorageService`.

## Features
- Search by expense title or vendor name.
- Filter by category & status (Paid, Approved, PendingApproval, Rejected).
- One-click expense approval workflow for managers.
