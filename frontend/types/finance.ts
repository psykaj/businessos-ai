export interface AccountDto {
  id: string;
  organizationId: string;
  accountCode: string;
  accountName: string;
  accountType: 'Asset' | 'Liability' | 'Equity' | 'Revenue' | 'Expense' | string;
  subCategory?: string;
  currency: string;
  currentBalance: number;
  isActive: boolean;
  description?: string;
  createdAt: string;
}

export interface CreateAccountDto {
  accountCode: string;
  accountName: string;
  accountType: string;
  subCategory?: string;
  currency?: string;
  initialBalance?: number;
  description?: string;
}

export interface ExpenseCategoryDto {
  id: string;
  organizationId: string;
  name: string;
  code: string;
  description?: string;
  color: string;
  isActive: boolean;
}

export interface CreateExpenseCategoryDto {
  name: string;
  code: string;
  description?: string;
  color?: string;
}

export interface ExpenseDto {
  id: string;
  organizationId: string;
  title: string;
  description?: string;
  amount: number;
  taxAmount: number;
  taxRate: number;
  currency: string;
  expenseCategoryId: string;
  categoryName: string;
  expenseDate: string;
  vendorName?: string;
  paymentMethod: string;
  status: 'Draft' | 'PendingApproval' | 'Approved' | 'Rejected' | 'Paid' | string;
  receiptUrl?: string;
  isRecurring: boolean;
  recurringInterval?: string;
  nextRecurringDate?: string;
  approvedBy?: string;
  approvedAt?: string;
  createdAt: string;
}

export interface CreateExpenseDto {
  title: string;
  description?: string;
  amount: number;
  taxAmount: number;
  taxRate: number;
  currency?: string;
  expenseCategoryId: string;
  expenseDate: string;
  vendorName?: string;
  paymentMethod: string;
  receiptUrl?: string;
  isRecurring: boolean;
  recurringInterval?: string;
}

export interface FinanceInvoiceItemDto {
  id: string;
  productId?: string;
  description: string;
  quantity: number;
  unitPrice: number;
  discountAmount: number;
  taxRate: number;
  taxAmount: number;
  totalAmount: number;
}

export interface FinanceInvoiceDto {
  id: string;
  organizationId: string;
  invoiceNumber: string;
  customerId?: string;
  customerName: string;
  customerEmail?: string;
  issueDate: string;
  dueDate: string;
  status: 'Draft' | 'Sent' | 'Partial' | 'Paid' | 'Overdue' | 'Cancelled' | string;
  subTotal: number;
  taxTotal: number;
  discountTotal: number;
  totalAmount: number;
  amountPaid: number;
  balanceDue: number;
  currency: string;
  notes?: string;
  terms?: string;
  pdfUrl?: string;
  items: FinanceInvoiceItemDto[];
  createdAt: string;
}

export interface CreateFinanceInvoiceDto {
  invoiceNumber?: string;
  customerId?: string;
  customerName: string;
  customerEmail?: string;
  issueDate: string;
  dueDate: string;
  currency?: string;
  notes?: string;
  terms?: string;
  items?: {
    productId?: string;
    description: string;
    quantity: number;
    unitPrice: number;
    discountAmount?: number;
    taxRate?: number;
  }[];
}

export interface InvoiceSummaryDto {
  totalInvoiced: number;
  totalPaid: number;
  totalOverdue: number;
  totalCount: number;
  pendingCount: number;
  overdueCount: number;
}

export interface FinancePaymentDto {
  id: string;
  organizationId: string;
  paymentNumber: string;
  invoiceId?: string;
  billId?: string;
  accountId?: string;
  amount: number;
  paymentDate: string;
  paymentMethod: string;
  paymentType: 'Incoming' | 'Outgoing' | string;
  referenceNumber?: string;
  notes?: string;
  status: string;
  createdAt: string;
}

export interface CreateFinancePaymentDto {
  paymentNumber?: string;
  invoiceId?: string;
  billId?: string;
  accountId?: string;
  amount: number;
  paymentDate: string;
  paymentMethod?: string;
  paymentType?: string;
  referenceNumber?: string;
  notes?: string;
}

export interface AccountsReceivableDto {
  id: string;
  organizationId: string;
  invoiceId: string;
  customerId?: string;
  customerName: string;
  totalAmount: number;
  amountPaid: number;
  balanceDue: number;
  dueDate: string;
  status: string;
  daysOverdue: number;
  lastReminderSentAt?: string;
  createdAt: string;
}

export interface AccountsReceivableAgingDto {
  totalOutstanding: number;
  current: number;
  days1To30: number;
  days31To60: number;
  days61To90: number;
  days90Plus: number;
  totalOverdueInvoices: number;
}

export interface AccountsPayableDto {
  id: string;
  organizationId: string;
  billNumber: string;
  supplierId?: string;
  supplierName: string;
  purchaseOrderId?: string;
  totalAmount: number;
  amountPaid: number;
  balanceDue: number;
  dueDate: string;
  status: string;
  daysOverdue: number;
  createdAt: string;
}

export interface CreateAccountsPayableDto {
  billNumber: string;
  supplierId?: string;
  supplierName: string;
  purchaseOrderId?: string;
  totalAmount: number;
  dueDate: string;
}

export interface AccountsPayableAgingDto {
  totalOutstanding: number;
  current: number;
  days1To30: number;
  days31To60: number;
  days61To90: number;
  days90Plus: number;
  totalOverdueBills: number;
}

export interface CashFlowSummaryDto {
  totalCashIn: number;
  totalCashOut: number;
  currentCashPosition: number;
  netCashFlow: number;
  outstandingReceivables: number;
  outstandingPayables: number;
  estimatedNetProfit: number;
}

export interface MonthlyCashFlowPointDto {
  month: string;
  cashIn: number;
  cashOut: number;
  netFlow: number;
}

export interface CashFlowForecastDto {
  projectedCashInNext30Days: number;
  projectedCashOutNext30Days: number;
  projectedCashPositionIn30Days: number;
  forecastDetails: MonthlyCashFlowPointDto[];
}

export interface CreateCashFlowEntryDto {
  entryDate: string;
  type: 'CashIn' | 'CashOut' | string;
  category: string;
  amount: number;
  accountId?: string;
  referenceType?: string;
  referenceId?: string;
  description?: string;
}

export interface TaxRecordDto {
  id: string;
  organizationId: string;
  taxName: string;
  taxCode: string;
  rate: number;
  taxType: string;
  countryCode: string;
  isActive: boolean;
  description?: string;
  createdAt: string;
}

export interface CreateTaxRecordDto {
  taxName: string;
  taxCode: string;
  rate: number;
  taxType?: string;
  countryCode?: string;
  description?: string;
}

export interface CalculateTaxResultDto {
  baseAmount: number;
  taxAmount: number;
  totalAmount: number;
  taxRate: number;
  taxCode: string;
  taxName: string;
}

export interface TaxSummaryDto {
  totalTaxCollected: number;
  totalTaxPaid: number;
  netTaxLiability: number;
  taxBreakdowns: {
    taxCode: string;
    taxName: string;
    rate: number;
    taxCollected: number;
    taxPaid: number;
  }[];
}

export interface CategoryBreakdownDto {
  categoryName: string;
  amount: number;
  percentage: number;
}

export interface ProfitAndLossReportDto {
  periodStart: string;
  periodEnd: string;
  operatingRevenue: number;
  costOfGoodsSold: number;
  grossProfit: number;
  operatingExpenses: number;
  netIncome: number;
  revenueBreakdown: CategoryBreakdownDto[];
  expenseBreakdown: CategoryBreakdownDto[];
}

export interface CashFlowStatementReportDto {
  periodStart: string;
  periodEnd: string;
  cashFromOperatingActivities: number;
  cashFromInvestingActivities: number;
  cashFromFinancingActivities: number;
  netIncreaseInCash: number;
  beginningCashBalance: number;
  endingCashBalance: number;
}

export interface ExpenseReportDto {
  periodStart: string;
  periodEnd: string;
  totalExpenseAmount: number;
  totalExpenseCount: number;
  expensesByCategory: CategoryBreakdownDto[];
}

export interface CustomerRevenueDto {
  customerName: string;
  totalBilled: number;
  totalPaid: number;
}

export interface RevenueReportDto {
  periodStart: string;
  periodEnd: string;
  totalRevenue: number;
  invoiceCount: number;
  topCustomers: CustomerRevenueDto[];
}

export interface FinanceOverviewDto {
  totalRevenue: number;
  totalExpenses: number;
  netIncome: number;
  cashPosition: number;
  totalReceivables: number;
  totalPayables: number;
  pendingInvoicesCount: number;
  pendingBillsCount: number;
  profitMarginPercentage: number;
}
