# Finance & Accounting UI Documentation

The Finance & Accounting UI in BusinessOS AI provides SMEs with complete financial visibility and control, comparable to QuickBooks and Xero while remaining fast and intuitive.

## Key Features & Components
1. **Executive Finance Overview (`/dashboard/finance`)**:
   - Displays real-time KPI cards: Cash Available, Total Revenue, Total Expenses, Net Profit Margin, Outstanding Receivables, and Outstanding Payables.
   - Interactive Recharts Cash Inflow vs Outflow area chart.
   - Recent transaction log showcasing live income & expense activity.

2. **Customer Invoices (`/dashboard/invoices`)**:
   - Create customer invoices with dynamic line items (Quantity, Unit Price, Tax, Discount).
   - Status tracking (Sent, Partial, Paid, Overdue).
   - Record customer payments directly against invoices.

3. **Payments History (`/dashboard/payments`)**:
   - Complete audit trail of incoming collections and outgoing disbursements.

## Architecture
- **State & Data Fetching**: Powered by TanStack React Query (`useFinanceOverview`, `useInvoices`, `usePayments`).
- **Validation**: Zod schema validation & React Hook Form.
- **Styling**: Tailwind CSS, Lucide icons, Dark/Light mode theme integration.
