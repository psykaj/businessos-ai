# Financial Reports UI Documentation

The Financial Reports workspace (`/dashboard/financial-reports`) allows SMEs to preview, filter, and print/export standard financial statements.

## Statements & Views
1. **Profit & Loss Statement (`ProfitAndLossPreview`)**:
   - Operating Revenue vs Cost of Goods Sold = Gross Profit.
   - Operating Expenses breakdown = Net Income.
2. **Statement of Cash Flows (`CashFlowStatementPreview`)**:
   - Operating, Investing, and Financing activities summary.
3. **Tax Summary (`TaxSummaryPreview`)**:
   - Output tax collected vs Input tax paid = Net tax liability.
   - Tax code breakdown table (GST, VAT, Sales Tax).
4. **Expense & Revenue Breakdown Reports**:
   - Categorized expense totals and customer billing revenue rankings.

## Export & Print Integration
- Built-in browser print engine formatted for PDF/paper exports (`window.print()`).
