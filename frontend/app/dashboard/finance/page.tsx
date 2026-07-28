"use client";

import { useFinanceOverview, useMonthlyCashFlow, useExpenses, usePayments } from "@/hooks/use-finance";
import { FinanceKpiCards } from "@/components/finance/finance-kpi-cards";
import { CashFlowChart } from "@/components/finance/cash-flow-chart";
import { RecentTransactions } from "@/components/finance/recent-transactions";
import { DollarSign, ArrowUpRight, Plus, FileText, CreditCard } from "lucide-react";
import Link from "next/link";
import { Button } from "@/components/ui/button";

export default function FinanceDashboardPage() {
  const { data: overview, isLoading: isOverviewLoading } = useFinanceOverview();
  const { data: monthlyCashFlow, isLoading: isMonthlyLoading } = useMonthlyCashFlow(6);
  const { data: expenses, isLoading: isExpensesLoading } = useExpenses();
  const { data: payments, isLoading: isPaymentsLoading } = usePayments();

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* Top Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border pb-5">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
            <DollarSign className="w-7 h-7 text-primary" /> Finance & Accounting Center
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Real-time financial visibility, cash flow performance, and ledger management for SMEs
          </p>
        </div>

        <div className="flex items-center gap-3">
          <Link href="/dashboard/expenses">
            <Button variant="outline" size="sm" className="gap-2 text-xs font-semibold">
              <Plus className="w-4 h-4" /> Add Expense
            </Button>
          </Link>
          <Link href="/dashboard/invoices">
            <Button size="sm" className="gap-2 text-xs font-semibold">
              <FileText className="w-4 h-4" /> Create Invoice
            </Button>
          </Link>
        </div>
      </div>

      {/* KPI Cards */}
      <FinanceKpiCards overview={overview} isLoading={isOverviewLoading} />

      {/* Main Charts & Activity Row */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2">
          <CashFlowChart data={monthlyCashFlow} isLoading={isMonthlyLoading} />
        </div>
        <div>
          <RecentTransactions expenses={expenses} payments={payments} isLoading={isExpensesLoading || isPaymentsLoading} />
        </div>
      </div>

      {/* Quick Navigation Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 pt-4">
        <Link href="/dashboard/cash-flow" className="p-4 rounded-xl bg-card border border-border hover:border-primary transition-all group">
          <div className="text-xs text-primary font-semibold uppercase mb-1">Cash Flow Engine</div>
          <div className="font-bold text-foreground group-hover:text-primary transition-colors flex items-center justify-between">
            Forecast & Analytics <ArrowUpRight className="w-4 h-4" />
          </div>
          <p className="text-xs text-muted-foreground mt-1">30-day liquidity projections and historical breakdown</p>
        </Link>

        <Link href="/dashboard/accounts-receivable" className="p-4 rounded-xl bg-card border border-border hover:border-primary transition-all group">
          <div className="text-xs text-sky-400 font-semibold uppercase mb-1">Accounts Receivable</div>
          <div className="font-bold text-foreground group-hover:text-primary transition-colors flex items-center justify-between">
            Customer Invoices & Aging <ArrowUpRight className="w-4 h-4" />
          </div>
          <p className="text-xs text-muted-foreground mt-1">Track outstanding payments & send reminders</p>
        </Link>

        <Link href="/dashboard/accounts-payable" className="p-4 rounded-xl bg-card border border-border hover:border-primary transition-all group">
          <div className="text-xs text-amber-400 font-semibold uppercase mb-1">Accounts Payable</div>
          <div className="font-bold text-foreground group-hover:text-primary transition-colors flex items-center justify-between">
            Supplier Bills & Aging <ArrowUpRight className="w-4 h-4" />
          </div>
          <p className="text-xs text-muted-foreground mt-1">Manage vendor bills and scheduled payments</p>
        </Link>

        <Link href="/dashboard/financial-reports" className="p-4 rounded-xl bg-card border border-border hover:border-primary transition-all group">
          <div className="text-xs text-emerald-400 font-semibold uppercase mb-1">Financial Statements</div>
          <div className="font-bold text-foreground group-hover:text-primary transition-colors flex items-center justify-between">
            Profit & Loss / Reports <ArrowUpRight className="w-4 h-4" />
          </div>
          <p className="text-xs text-muted-foreground mt-1">Generate P&L, Cash Flow & Tax statements</p>
        </Link>
      </div>
    </div>
  );
}
