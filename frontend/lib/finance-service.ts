import apiClient from "./api-client";
import {
  AccountDto,
  CreateAccountDto,
  ExpenseCategoryDto,
  CreateExpenseCategoryDto,
  ExpenseDto,
  CreateExpenseDto,
  FinanceInvoiceDto,
  CreateFinanceInvoiceDto,
  InvoiceSummaryDto,
  FinancePaymentDto,
  CreateFinancePaymentDto,
  AccountsReceivableDto,
  AccountsReceivableAgingDto,
  AccountsPayableDto,
  CreateAccountsPayableDto,
  AccountsPayableAgingDto,
  CashFlowSummaryDto,
  MonthlyCashFlowPointDto,
  CashFlowForecastDto,
  CreateCashFlowEntryDto,
  TaxRecordDto,
  CreateTaxRecordDto,
  CalculateTaxResultDto,
  TaxSummaryDto,
  ProfitAndLossReportDto,
  CashFlowStatementReportDto,
  ExpenseReportDto,
  RevenueReportDto,
  FinanceOverviewDto,
} from "@/types/finance";

export const financeService = {
  // ── 1. Executive Finance Overview ───────────────────────────────────────────
  async getOverview(): Promise<FinanceOverviewDto> {
    const res = await apiClient.get<FinanceOverviewDto>("/api/finance/overview");
    return res.data;
  },

  // ── 2. Chart of Accounts ───────────────────────────────────────────────────
  async getAccounts(type?: string, activeOnly = false): Promise<AccountDto[]> {
    const res = await apiClient.get<AccountDto[]>("/api/accounting/accounts", {
      params: { type, activeOnly },
    });
    return res.data;
  },

  async createAccount(dto: CreateAccountDto): Promise<AccountDto> {
    const res = await apiClient.post<AccountDto>("/api/accounting/accounts", dto);
    return res.data;
  },

  // ── 3. Expense Management ──────────────────────────────────────────────────
  async getExpenses(params?: { categoryId?: string; status?: string; startDate?: string; endDate?: string }): Promise<ExpenseDto[]> {
    const res = await apiClient.get<ExpenseDto[]>("/api/expenses", { params });
    return res.data;
  },

  async getExpenseById(id: string): Promise<ExpenseDto> {
    const res = await apiClient.get<ExpenseDto>(`/api/expenses/${id}`);
    return res.data;
  },

  async createExpense(dto: CreateExpenseDto): Promise<ExpenseDto> {
    const res = await apiClient.post<ExpenseDto>("/api/expenses", dto);
    return res.data;
  },

  async approveExpense(id: string): Promise<ExpenseDto> {
    const res = await apiClient.post<ExpenseDto>(`/api/expenses/${id}/approve`);
    return res.data;
  },

  async rejectExpense(id: string): Promise<ExpenseDto> {
    const res = await apiClient.post<ExpenseDto>(`/api/expenses/${id}/reject`);
    return res.data;
  },

  async uploadReceipt(file: File): Promise<{ url: string }> {
    const formData = new FormData();
    formData.append("file", file);
    const res = await apiClient.post<{ url: string }>("/api/expenses/upload-receipt", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return res.data;
  },

  async getExpenseCategories(): Promise<ExpenseCategoryDto[]> {
    const res = await apiClient.get<ExpenseCategoryDto[]>("/api/expenses/categories");
    return res.data;
  },

  async createExpenseCategory(dto: CreateExpenseCategoryDto): Promise<ExpenseCategoryDto> {
    const res = await apiClient.post<ExpenseCategoryDto>("/api/expenses/categories", dto);
    return res.data;
  },

  // ── 4. Customer Invoices ───────────────────────────────────────────────────
  async getInvoices(status?: string, customerId?: string): Promise<FinanceInvoiceDto[]> {
    const res = await apiClient.get<FinanceInvoiceDto[]>("/api/invoices", {
      params: { status, customerId },
    });
    return res.data;
  },

  async getInvoiceSummary(): Promise<InvoiceSummaryDto> {
    const res = await apiClient.get<InvoiceSummaryDto>("/api/invoices/summary");
    return res.data;
  },

  async createInvoice(dto: CreateFinanceInvoiceDto): Promise<FinanceInvoiceDto> {
    const res = await apiClient.post<FinanceInvoiceDto>("/api/invoices", dto);
    return res.data;
  },

  async recordInvoicePayment(id: string, amount: number): Promise<FinanceInvoiceDto> {
    const res = await apiClient.post<FinanceInvoiceDto>(`/api/invoices/${id}/pay`, { amount });
    return res.data;
  },

  // ── 5. Payment Transactions ────────────────────────────────────────────────
  async getPayments(paymentType?: string): Promise<FinancePaymentDto[]> {
    const res = await apiClient.get<FinancePaymentDto[]>("/api/finance/payments", {
      params: { paymentType },
    });
    return res.data;
  },

  async recordPayment(dto: CreateFinancePaymentDto): Promise<FinancePaymentDto> {
    const res = await apiClient.post<FinancePaymentDto>("/api/finance/payments", dto);
    return res.data;
  },

  // ── 6. Accounts Receivable ─────────────────────────────────────────────────
  async getAccountsReceivable(status?: string): Promise<AccountsReceivableDto[]> {
    const res = await apiClient.get<AccountsReceivableDto[]>("/api/accounts-receivable", {
      params: { status },
    });
    return res.data;
  },

  async getARAging(): Promise<AccountsReceivableAgingDto> {
    const res = await apiClient.get<AccountsReceivableAgingDto>("/api/accounts-receivable/aging");
    return res.data;
  },

  async sendAROverdueReminder(id: string, customNote?: string): Promise<{ message: string }> {
    const res = await apiClient.post<{ message: string }>(`/api/accounts-receivable/${id}/remind-overdue`, { customNote });
    return res.data;
  },

  // ── 7. Accounts Payable ────────────────────────────────────────────────────
  async getAccountsPayable(status?: string): Promise<AccountsPayableDto[]> {
    const res = await apiClient.get<AccountsPayableDto[]>("/api/accounts-payable", {
      params: { status },
    });
    return res.data;
  },

  async createBill(dto: CreateAccountsPayableDto): Promise<AccountsPayableDto> {
    const res = await apiClient.post<AccountsPayableDto>("/api/accounts-payable", dto);
    return res.data;
  },

  async recordBillPayment(id: string, amount: number): Promise<AccountsPayableDto> {
    const res = await apiClient.post<AccountsPayableDto>(`/api/accounts-payable/${id}/pay`, { amount });
    return res.data;
  },

  async getAPAging(): Promise<AccountsPayableAgingDto> {
    const res = await apiClient.get<AccountsPayableAgingDto>("/api/accounts-payable/aging");
    return res.data;
  },

  // ── 8. Cash Flow Engine ────────────────────────────────────────────────────
  async getCashFlowSummary(startDate?: string, endDate?: string): Promise<CashFlowSummaryDto> {
    const res = await apiClient.get<CashFlowSummaryDto>("/api/cashflow/summary", {
      params: { startDate, endDate },
    });
    return res.data;
  },

  async getMonthlyCashFlow(months = 6): Promise<MonthlyCashFlowPointDto[]> {
    const res = await apiClient.get<MonthlyCashFlowPointDto[]>("/api/cashflow/monthly-breakdown", {
      params: { months },
    });
    return res.data;
  },

  async getCashFlowForecast(): Promise<CashFlowForecastDto> {
    const res = await apiClient.get<CashFlowForecastDto>("/api/cashflow/forecast");
    return res.data;
  },

  async addCashFlowEntry(dto: CreateCashFlowEntryDto) {
    const res = await apiClient.post("/api/cashflow/entries", dto);
    return res.data;
  },

  // ── 9. Tax Foundation ──────────────────────────────────────────────────────
  async getTaxes(): Promise<TaxRecordDto[]> {
    const res = await apiClient.get<TaxRecordDto[]>("/api/taxes");
    return res.data;
  },

  async createTax(dto: CreateTaxRecordDto): Promise<TaxRecordDto> {
    const res = await apiClient.post<TaxRecordDto>("/api/taxes", dto);
    return res.data;
  },

  async calculateTax(amount: number, taxCode: string, isInclusive = false): Promise<CalculateTaxResultDto> {
    const res = await apiClient.post<CalculateTaxResultDto>("/api/taxes/calculate", { amount, taxCode, isInclusive });
    return res.data;
  },

  async getTaxSummary(startDate?: string, endDate?: string): Promise<TaxSummaryDto> {
    const res = await apiClient.get<TaxSummaryDto>("/api/taxes/summary", { params: { startDate, endDate } });
    return res.data;
  },

  // ── 10. Financial Reports ──────────────────────────────────────────────────
  async getProfitAndLoss(startDate?: string, endDate?: string): Promise<ProfitAndLossReportDto> {
    const res = await apiClient.get<ProfitAndLossReportDto>("/api/financial-reports/profit-and-loss", {
      params: { startDate, endDate },
    });
    return res.data;
  },

  async getCashFlowStatement(startDate?: string, endDate?: string): Promise<CashFlowStatementReportDto> {
    const res = await apiClient.get<CashFlowStatementReportDto>("/api/financial-reports/cash-flow", {
      params: { startDate, endDate },
    });
    return res.data;
  },

  async getExpenseReport(startDate?: string, endDate?: string): Promise<ExpenseReportDto> {
    const res = await apiClient.get<ExpenseReportDto>("/api/financial-reports/expense-summary", {
      params: { startDate, endDate },
    });
    return res.data;
  },

  async getRevenueReport(startDate?: string, endDate?: string): Promise<RevenueReportDto> {
    const res = await apiClient.get<RevenueReportDto>("/api/financial-reports/revenue-summary", {
      params: { startDate, endDate },
    });
    return res.data;
  },
};
