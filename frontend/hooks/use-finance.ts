import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { financeService } from "@/lib/finance-service";
import {
  CreateAccountDto,
  CreateExpenseDto,
  CreateExpenseCategoryDto,
  CreateFinanceInvoiceDto,
  CreateFinancePaymentDto,
  CreateAccountsPayableDto,
  CreateCashFlowEntryDto,
  CreateTaxRecordDto,
} from "@/types/finance";

// ── Keys ───────────────────────────────────────────────────────────────────
export const FINANCE_KEYS = {
  overview: ["finance", "overview"] as const,
  accounts: (type?: string, activeOnly?: boolean) => ["finance", "accounts", type, activeOnly] as const,
  expenses: (params?: Record<string, unknown>) => ["finance", "expenses", params] as const,
  expenseCategories: ["finance", "expense-categories"] as const,
  invoices: (status?: string, customerId?: string) => ["finance", "invoices", status, customerId] as const,
  invoiceSummary: ["finance", "invoice-summary"] as const,
  payments: (type?: string) => ["finance", "payments", type] as const,
  ar: (status?: string) => ["finance", "ar", status] as const,
  arAging: ["finance", "ar-aging"] as const,
  ap: (status?: string) => ["finance", "ap", status] as const,
  apAging: ["finance", "ap-aging"] as const,
  cashFlowSummary: (start?: string, end?: string) => ["finance", "cashflow-summary", start, end] as const,
  cashFlowMonthly: (months?: number) => ["finance", "cashflow-monthly", months] as const,
  cashFlowForecast: ["finance", "cashflow-forecast"] as const,
  taxes: ["finance", "taxes"] as const,
  taxSummary: (start?: string, end?: string) => ["finance", "tax-summary", start, end] as const,
  pnl: (start?: string, end?: string) => ["finance", "pnl", start, end] as const,
  cashFlowStatement: (start?: string, end?: string) => ["finance", "cashflow-statement", start, end] as const,
  expenseReport: (start?: string, end?: string) => ["finance", "expense-report", start, end] as const,
  revenueReport: (start?: string, end?: string) => ["finance", "revenue-report", start, end] as const,
};

// ── Hooks ──────────────────────────────────────────────────────────────────
export function useFinanceOverview() {
  return useQuery({
    queryKey: FINANCE_KEYS.overview,
    queryFn: () => financeService.getOverview(),
  });
}

export function useAccounts(type?: string, activeOnly = false) {
  return useQuery({
    queryKey: FINANCE_KEYS.accounts(type, activeOnly),
    queryFn: () => financeService.getAccounts(type, activeOnly),
  });
}

export function useCreateAccount() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateAccountDto) => financeService.createAccount(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useExpenses(params?: { categoryId?: string; status?: string; startDate?: string; endDate?: string }) {
  return useQuery({
    queryKey: FINANCE_KEYS.expenses(params),
    queryFn: () => financeService.getExpenses(params),
  });
}

export function useExpenseCategories() {
  return useQuery({
    queryKey: FINANCE_KEYS.expenseCategories,
    queryFn: () => financeService.getExpenseCategories(),
  });
}

export function useCreateExpense() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateExpenseDto) => financeService.createExpense(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useApproveExpense() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => financeService.approveExpense(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useInvoices(status?: string, customerId?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.invoices(status, customerId),
    queryFn: () => financeService.getInvoices(status, customerId),
  });
}

export function useInvoiceSummary() {
  return useQuery({
    queryKey: FINANCE_KEYS.invoiceSummary,
    queryFn: () => financeService.getInvoiceSummary(),
  });
}

export function useCreateInvoice() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateFinanceInvoiceDto) => financeService.createInvoice(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useRecordInvoicePayment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, amount }: { id: string; amount: number }) => financeService.recordInvoicePayment(id, amount),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function usePayments(paymentType?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.payments(paymentType),
    queryFn: () => financeService.getPayments(paymentType),
  });
}

export function useRecordPayment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateFinancePaymentDto) => financeService.recordPayment(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useAccountsReceivable(status?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.ar(status),
    queryFn: () => financeService.getAccountsReceivable(status),
  });
}

export function useARAging() {
  return useQuery({
    queryKey: FINANCE_KEYS.arAging,
    queryFn: () => financeService.getARAging(),
  });
}

export function useSendAROverdueReminder() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, customNote }: { id: string; customNote?: string }) => financeService.sendAROverdueReminder(id, customNote),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useAccountsPayable(status?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.ap(status),
    queryFn: () => financeService.getAccountsPayable(status),
  });
}

export function useCreateBill() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateAccountsPayableDto) => financeService.createBill(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useRecordBillPayment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, amount }: { id: string; amount: number }) => financeService.recordBillPayment(id, amount),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useAPAging() {
  return useQuery({
    queryKey: FINANCE_KEYS.apAging,
    queryFn: () => financeService.getAPAging(),
  });
}

export function useCashFlowSummary(startDate?: string, endDate?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.cashFlowSummary(startDate, endDate),
    queryFn: () => financeService.getCashFlowSummary(startDate, endDate),
  });
}

export function useMonthlyCashFlow(months = 6) {
  return useQuery({
    queryKey: FINANCE_KEYS.cashFlowMonthly(months),
    queryFn: () => financeService.getMonthlyCashFlow(months),
  });
}

export function useCashFlowForecast() {
  return useQuery({
    queryKey: FINANCE_KEYS.cashFlowForecast,
    queryFn: () => financeService.getCashFlowForecast(),
  });
}

export function useAddCashFlowEntry() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateCashFlowEntryDto) => financeService.addCashFlowEntry(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finance"] });
    },
  });
}

export function useTaxes() {
  return useQuery({
    queryKey: FINANCE_KEYS.taxes,
    queryFn: () => financeService.getTaxes(),
  });
}

export function useTaxSummary(startDate?: string, endDate?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.taxSummary(startDate, endDate),
    queryFn: () => financeService.getTaxSummary(startDate, endDate),
  });
}

export function useProfitAndLoss(startDate?: string, endDate?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.pnl(startDate, endDate),
    queryFn: () => financeService.getProfitAndLoss(startDate, endDate),
  });
}

export function useCashFlowStatement(startDate?: string, endDate?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.cashFlowStatement(startDate, endDate),
    queryFn: () => financeService.getCashFlowStatement(startDate, endDate),
  });
}

export function useExpenseReport(startDate?: string, endDate?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.expenseReport(startDate, endDate),
    queryFn: () => financeService.getExpenseReport(startDate, endDate),
  });
}

export function useRevenueReport(startDate?: string, endDate?: string) {
  return useQuery({
    queryKey: FINANCE_KEYS.revenueReport(startDate, endDate),
    queryFn: () => financeService.getRevenueReport(startDate, endDate),
  });
}
