# Expense Management Architecture

The Expense Management module allows SMEs to record, categorize, approve, and track recurring expenses and receipt documentation.

## Core Features
- **Categorization & Color Coding**: Categorized expenses with color tags for UI visualization.
- **Receipt Upload Abstraction**: `IReceiptStorageService` abstraction supporting local file storage and cloud storage options.
- **Approval Workflow**: Foundation endpoints for expense submission, approval, and rejection.
- **Recurring Expenses**: Automatically calculates next recurring dates (Monthly, Quarterly, Yearly).

## API Endpoints
- `GET /api/expenses` - Retrieve expenses (supports filtering by category, status, date range).
- `GET /api/expenses/{id}` - Get expense details.
- `POST /api/expenses` - Create expense.
- `PUT /api/expenses/{id}` - Update expense.
- `DELETE /api/expenses/{id}` - Delete expense.
- `POST /api/expenses/{id}/approve` - Approve expense.
- `POST /api/expenses/{id}/reject` - Reject expense.
- `POST /api/expenses/upload-receipt` - Upload receipt image/PDF.
- `GET /api/expenses/categories` - List categories.
- `POST /api/expenses/categories` - Add expense category.
