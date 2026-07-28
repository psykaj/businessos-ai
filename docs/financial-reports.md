# Financial Reporting Architecture

The Financial Reports module generates standardized financial statements and analytical reports designed for JSON output and PDF/Excel export integration.

## Generated Reports
1. **Profit & Loss (P&L)**: Operating Revenue vs Cost of Goods Sold vs Operating Expenses = Net Income.
2. **Cash Flow Statement**: Cash flows from Operating, Investing, and Financing activities.
3. **Expense Summary Report**: Categorized expense totals with percentage breakdowns.
4. **Revenue Summary Report**: Revenue breakdown by customer and invoice volume.

## API Endpoints
- `GET /api/financial-reports/profit-and-loss` - Generate Profit & Loss statement.
- `GET /api/financial-reports/cash-flow` - Generate Cash Flow Statement.
- `GET /api/financial-reports/expense-summary` - Generate Expense Summary report.
- `GET /api/financial-reports/revenue-summary` - Generate Revenue Summary report.
