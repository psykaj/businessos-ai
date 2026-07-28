"use client";

import { useState } from "react";
import { useProfitAndLoss, useCashFlowStatement, useTaxSummary, useExpenseReport, useRevenueReport } from "@/hooks/use-finance";
import { ProfitAndLossPreview } from "@/components/financial-reports/profit-and-loss-preview";
import { CashFlowStatementPreview } from "@/components/financial-reports/cash-flow-statement-preview";
import { TaxSummaryPreview } from "@/components/financial-reports/tax-summary-preview";
import { FileText, PieChart, TrendingUp, Receipt, ShieldCheck } from "lucide-react";

export default function FinancialReportsPage() {
  const [activeTab, setActiveTab] = useState<"pnl" | "cashflow" | "tax" | "expense" | "revenue">("pnl");

  const { data: pnl, isLoading: isPnlLoading } = useProfitAndLoss();
  const { data: cashFlowStatement, isLoading: isCashFlowLoading } = useCashFlowStatement();
  const { data: taxSummary, isLoading: isTaxLoading } = useTaxSummary();
  const { data: expenseReport, isLoading: isExpenseReportLoading } = useExpenseReport();
  const { data: revenueReport, isLoading: isRevenueReportLoading } = useRevenueReport();

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div className="border-b border-border pb-5">
        <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
          <FileText className="w-7 h-7 text-primary" /> Financial Reports & Statements
        </h1>
        <p className="text-sm text-muted-foreground mt-1">
          Generate, preview, filter, and export Profit & Loss, Cash Flow Statements, and Tax Liability summaries
        </p>
      </div>

      {/* Tabs Header */}
      <div className="flex items-center gap-2 border-b border-border overflow-x-auto pb-2">
        <button
          onClick={() => setActiveTab("pnl")}
          className={`flex items-center gap-2 px-4 py-2 text-xs font-semibold rounded-lg transition-colors whitespace-nowrap ${
            activeTab === "pnl" ? "bg-primary text-primary-foreground" : "bg-card text-muted-foreground hover:text-foreground border border-border"
          }`}
        >
          <TrendingUp className="w-4 h-4" /> Profit & Loss Statement
        </button>

        <button
          onClick={() => setActiveTab("cashflow")}
          className={`flex items-center gap-2 px-4 py-2 text-xs font-semibold rounded-lg transition-colors whitespace-nowrap ${
            activeTab === "cashflow" ? "bg-primary text-primary-foreground" : "bg-card text-muted-foreground hover:text-foreground border border-border"
          }`}
        >
          <PieChart className="w-4 h-4" /> Statement of Cash Flows
        </button>

        <button
          onClick={() => setActiveTab("tax")}
          className={`flex items-center gap-2 px-4 py-2 text-xs font-semibold rounded-lg transition-colors whitespace-nowrap ${
            activeTab === "tax" ? "bg-primary text-primary-foreground" : "bg-card text-muted-foreground hover:text-foreground border border-border"
          }`}
        >
          <Receipt className="w-4 h-4" /> Tax Summary
        </button>

        <button
          onClick={() => setActiveTab("expense")}
          className={`flex items-center gap-2 px-4 py-2 text-xs font-semibold rounded-lg transition-colors whitespace-nowrap ${
            activeTab === "expense" ? "bg-primary text-primary-foreground" : "bg-card text-muted-foreground hover:text-foreground border border-border"
          }`}
        >
          <FileText className="w-4 h-4" /> Expense Breakdown Report
        </button>

        <button
          onClick={() => setActiveTab("revenue")}
          className={`flex items-center gap-2 px-4 py-2 text-xs font-semibold rounded-lg transition-colors whitespace-nowrap ${
            activeTab === "revenue" ? "bg-primary text-primary-foreground" : "bg-card text-muted-foreground hover:text-foreground border border-border"
          }`}
        >
          <ShieldCheck className="w-4 h-4" /> Revenue Report
        </button>
      </div>

      {/* Active Tab Content */}
      {activeTab === "pnl" && <ProfitAndLossPreview report={pnl} isLoading={isPnlLoading} />}
      {activeTab === "cashflow" && <CashFlowStatementPreview statement={cashFlowStatement} isLoading={isCashFlowLoading} />}
      {activeTab === "tax" && <TaxSummaryPreview taxSummary={taxSummary} isLoading={isTaxLoading} />}

      {activeTab === "expense" && (
        <div className="bg-card border border-border rounded-xl p-6 shadow-sm">
          <h2 className="text-xl font-bold text-foreground mb-2">Expense Summary Report</h2>
          <p className="text-xs text-muted-foreground mb-4">Total Amount: ${expenseReport?.totalExpenseAmount.toLocaleString()} ({expenseReport?.totalExpenseCount} expenses)</p>
          <div className="space-y-2">
            {expenseReport?.expensesByCategory.map((c, i) => (
              <div key={i} className="flex justify-between items-center p-3 rounded-lg bg-background/50 border border-border">
                <span className="font-medium text-sm text-foreground">{c.categoryName}</span>
                <div className="text-right">
                  <div className="font-semibold text-sm text-foreground">${c.amount.toLocaleString()}</div>
                  <div className="text-xs text-muted-foreground">{c.percentage}% of total</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {activeTab === "revenue" && (
        <div className="bg-card border border-border rounded-xl p-6 shadow-sm">
          <h2 className="text-xl font-bold text-foreground mb-2">Revenue & Top Customers Report</h2>
          <p className="text-xs text-muted-foreground mb-4">Total Revenue Billed: ${revenueReport?.totalRevenue.toLocaleString()} across {revenueReport?.invoiceCount} invoices</p>
          <div className="space-y-2">
            {revenueReport?.topCustomers.map((cust, i) => (
              <div key={i} className="flex justify-between items-center p-3 rounded-lg bg-background/50 border border-border">
                <span className="font-medium text-sm text-foreground">{cust.customerName}</span>
                <div className="text-right">
                  <div className="font-semibold text-sm text-emerald-400">${cust.totalBilled.toLocaleString()} Billed</div>
                  <div className="text-xs text-muted-foreground">${cust.totalPaid.toLocaleString()} Paid</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
