# Cash Flow Engine Architecture

The Cash Flow Engine calculates real-time liquidity, cash position, historical trends, and 30-day forward projections.

## Core Features
- **Cash Position Calculation**: Bank balance + cumulative Cash In - Cash Out.
- **Monthly Breakdown**: Multi-month historical breakdown of cash inflows vs outflows.
- **Cash Flow Forecasting**: 30-day projected cash inflows from Accounts Receivable vs projected outflows from Accounts Payable and recurring expenses.
- **Ledger Entries**: Structured categorization (Sales, ARCollection, Expense, APPayment, Investment, Loan).

## API Endpoints
- `GET /api/cashflow/summary` - Get total cash in/out, net cash flow, cash position, AR & AP outstanding.
- `GET /api/cashflow/monthly-breakdown` - Monthly historical cash flow breakdown.
- `GET /api/cashflow/forecast` - 30-day cash flow projection engine.
- `POST /api/cashflow/entries` - Add manual cash flow ledger entry.
