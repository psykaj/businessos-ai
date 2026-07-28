# Accounting Module Architecture

The Accounting module handles the General Ledger and Chart of Accounts (COA) for SMEs in BusinessOS AI.

## Core Features
- **Chart of Accounts Management**: Pre-configured and customizable hierarchy of accounts across Assets, Liabilities, Equity, Revenue, and Expenses.
- **Auto-Seeding**: Automatically initializes default standard SME accounts upon organization creation.
- **Multi-Currency Support**: Configurable currency per account.
- **Audit & Isolation**: Soft delete support and multi-tenant isolation via `OrganizationId`.

## API Endpoints
- `GET /api/accounting/accounts` - Retrieve Chart of Accounts (supports filtering by `type` and `activeOnly`).
- `GET /api/accounting/accounts/{id}` - Get account by ID.
- `POST /api/accounting/accounts` - Create new GL Account.
- `PUT /api/accounting/accounts/{id}` - Update account properties.
- `DELETE /api/accounting/accounts/{id}` - Soft delete account.

## Account Types
1. **Asset**: Cash on Hand, Operating Bank Account, Accounts Receivable.
2. **Liability**: Accounts Payable, Sales Tax Payable.
3. **Equity**: Owner's Equity.
4. **Revenue**: Sales Revenue, Service Revenue.
5. **Expense**: Cost of Goods Sold, Operating Expenses (Rent, Software, Salaries, Supplies).
