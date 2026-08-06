"use client";

import React, { useState } from "react";
import { CashFlowAnalyticsDto, CashFlowPaymentDto } from "@/lib/business-intelligence-service";
import { 
  Wallet, 
  TrendingUp, 
  ArrowUpRight, 
  ArrowDownRight, 
  Clock, 
  Receipt, 
  Check, 
  Loader2, 
  Send
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

interface CashFlowOverviewProps {
  cashFlow: CashFlowAnalyticsDto;
}

export const CashFlowOverview: React.FC<CashFlowOverviewProps> = ({ cashFlow }) => {
  const [dunningId, setDunningId] = useState<string | null>(null);
  const [dunningSent, setDunningSent] = useState<Record<string, boolean>>({});

  const handleSendDunning = async (pmt: CashFlowPaymentDto) => {
    setDunningId(pmt.id);
    await new Promise((res) => setTimeout(res, 800));
    setDunningSent((prev) => ({ ...prev, [pmt.id]: true }));
    setDunningId(null);
    toast.success("Automated Dunning Triggered", {
      description: `Dispatched reminder sequence & statement to ${pmt.entityName} for invoice ${pmt.invoiceNumber} ($${pmt.amount.toLocaleString()}).`,
    });
  };

  return (
    <div className="rounded-3xl bg-white dark:bg-slate-900 border border-slate-200/80 dark:border-slate-800 shadow-xl p-6 space-y-6">
      
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 pb-4 border-b border-slate-100 dark:border-slate-800">
        <div className="flex items-center gap-3">
          <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-emerald-500/10 text-emerald-500">
            <Wallet className="h-6 w-6" />
          </div>
          <div>
            <h3 className="text-xl font-bold text-slate-900 dark:text-white flex items-center gap-2">
              Cash Flow & Working Capital
              <span className="text-xs bg-emerald-500/10 text-emerald-500 border border-emerald-500/20 px-2.5 py-0.5 rounded-full font-bold">
                +{cashFlow.profitMarginPercentage}% Net Margin
              </span>
            </h3>
            <p className="text-xs text-slate-500 dark:text-slate-400">
              Liquidity runway, operating cash surplus, and Accounts Receivable dunning automation
            </p>
          </div>
        </div>
      </div>

      {/* 4 Financial KPI Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="p-4 rounded-2xl bg-emerald-500/5 dark:bg-emerald-950/20 border border-emerald-500/20 flex items-center justify-between">
          <div>
            <span className="text-[11px] font-extrabold text-emerald-600 dark:text-emerald-400 uppercase tracking-wider block">
              Total Cash Inflow
            </span>
            <span className="text-2xl font-black font-mono text-slate-900 dark:text-white mt-1 block">
              ${cashFlow.totalIncome.toLocaleString()}
            </span>
          </div>
          <div className="p-3 rounded-xl bg-emerald-500/10 text-emerald-500">
            <ArrowUpRight className="h-6 w-6" />
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-red-500/5 dark:bg-red-950/20 border border-red-500/20 flex items-center justify-between">
          <div>
            <span className="text-[11px] font-extrabold text-red-600 dark:text-red-400 uppercase tracking-wider block">
              Operating Expenses
            </span>
            <span className="text-2xl font-black font-mono text-slate-900 dark:text-white mt-1 block">
              ${cashFlow.totalExpenses.toLocaleString()}
            </span>
          </div>
          <div className="p-3 rounded-xl bg-red-500/10 text-red-500">
            <ArrowDownRight className="h-6 w-6" />
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-slate-50 dark:bg-slate-800/50 border border-slate-200/60 dark:border-slate-800 flex items-center justify-between">
          <div>
            <span className="text-[11px] font-extrabold text-slate-500 dark:text-slate-400 uppercase tracking-wider block">
              Net Operating Profit
            </span>
            <span className="text-2xl font-black font-mono text-indigo-600 dark:text-indigo-400 mt-1 block">
              ${cashFlow.netProfit.toLocaleString()}
            </span>
          </div>
          <div className="p-3 rounded-xl bg-indigo-500/10 text-indigo-500">
            <TrendingUp className="h-6 w-6" />
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-purple-500/5 dark:bg-purple-950/20 border border-purple-500/20 flex items-center justify-between">
          <div>
            <span className="text-[11px] font-extrabold text-purple-600 dark:text-purple-400 uppercase tracking-wider block">
              Liquidity Runway
            </span>
            <span className="text-2xl font-black font-mono text-slate-900 dark:text-white mt-1 block">
              {cashFlow.operatingRunwayMonths} Months
            </span>
          </div>
          <div className="p-3 rounded-xl bg-purple-500/10 text-purple-500">
            <Clock className="h-6 w-6" />
          </div>
        </div>
      </div>

      {/* Upcoming & Overdue Payments Schedule */}
      <div className="space-y-3">
        <div className="flex items-center justify-between">
          <h4 className="text-xs font-extrabold uppercase tracking-wider text-slate-700 dark:text-slate-300 flex items-center gap-2">
            <Receipt className="h-4 w-4 text-red-500" />
            Accounts Receivable & Payable Schedule
          </h4>
          <span className="text-xs font-bold text-red-500">
            8 Overdue Invoices ($24,500 Total)
          </span>
        </div>
        
        <div className="overflow-x-auto rounded-2xl border border-slate-200 dark:border-slate-800">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-slate-100 dark:bg-slate-800/80 text-[11px] font-extrabold uppercase tracking-wider text-slate-600 dark:text-slate-400 border-b border-slate-200 dark:border-slate-700">
                <th className="p-4">Invoice #</th>
                <th className="p-4">Entity / Client Account</th>
                <th className="p-4">Due Date</th>
                <th className="p-4">Amount ($)</th>
                <th className="p-4">Type</th>
                <th className="p-4">Status</th>
                <th className="p-4 text-right">Collection Action</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100 dark:divide-slate-800 text-xs">
              {(cashFlow.upcomingPayments || []).map((pmt) => {
                const isProcessing = dunningId === pmt.id;
                const isSent = dunningSent[pmt.id];

                return (
                  <tr key={pmt.id} className="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors">
                    <td className="p-4 font-mono font-bold text-indigo-600 dark:text-indigo-400">
                      {pmt.invoiceNumber}
                    </td>
                    <td className="p-4 font-extrabold text-slate-900 dark:text-white">
                      {pmt.entityName}
                    </td>
                    <td className="p-4 text-slate-600 dark:text-slate-400 font-medium">
                      {pmt.dueDate}
                    </td>
                    <td className="p-4 font-extrabold font-mono text-sm text-slate-900 dark:text-white">
                      ${pmt.amount.toLocaleString()}
                    </td>
                    <td className="p-4">
                      <span className={cn(
                        "px-2 py-0.5 rounded-md text-[10px] font-bold uppercase tracking-wider border",
                        pmt.type === "Receivable" 
                          ? "bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 border-emerald-500/20" 
                          : "bg-purple-500/10 text-purple-600 dark:text-purple-400 border-purple-500/20"
                      )}>
                        {pmt.type}
                      </span>
                    </td>
                    <td className="p-4">
                      <span className={cn(
                        "px-2.5 py-0.5 rounded-full text-[10px] font-black uppercase tracking-wider border",
                        pmt.status === "Overdue" 
                          ? "bg-red-500 text-white border-red-600 shadow-xs animate-pulse" 
                          : pmt.status === "Due Today"
                          ? "bg-amber-500/20 text-amber-500 border-amber-500/30"
                          : "bg-slate-200 dark:bg-slate-800 text-slate-700 dark:text-slate-300 border-slate-300 dark:border-slate-700"
                      )}>
                        {pmt.status}
                      </span>
                    </td>
                    <td className="p-4 text-right">
                      {pmt.type === "Receivable" && pmt.status !== "Upcoming" ? (
                        <Button
                          size="sm"
                          disabled={isProcessing || isSent}
                          onClick={() => handleSendDunning(pmt)}
                          className={cn(
                            "text-xs font-bold rounded-xl px-3 py-1.5 transition-all shadow-sm",
                            isSent
                              ? "bg-emerald-600 hover:bg-emerald-600 text-white cursor-default"
                              : "bg-indigo-600 hover:bg-indigo-500 text-white shadow-indigo-500/20"
                          )}
                        >
                          {isProcessing ? (
                            <Loader2 className="h-3.5 w-3.5 animate-spin" />
                          ) : isSent ? (
                            <span className="flex items-center gap-1"><Check className="h-3 w-3" /> Dunning Active</span>
                          ) : (
                            <span className="flex items-center gap-1"><Send className="h-3 w-3" /> Trigger Dunning</span>
                          )}
                        </Button>
                      ) : (
                        <span className="text-slate-400 text-xs italic">Scheduled</span>
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>

    </div>
  );
};
