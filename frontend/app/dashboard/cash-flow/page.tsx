"use client";

import { useCashFlowSummary, useMonthlyCashFlow, useCashFlowForecast } from "@/hooks/use-finance";
import { CashFlowForecastCard } from "@/components/cash-flow/cash-flow-forecast-card";
import { CashFlowChart } from "@/components/finance/cash-flow-chart";
import { TrendingUp, Wallet, ArrowUpRight, ArrowDownLeft, Sparkles } from "lucide-react";

export default function CashFlowPage() {
  const { data: summary, isLoading: isSummaryLoading } = useCashFlowSummary();
  const { data: monthlyData, isLoading: isMonthlyLoading } = useMonthlyCashFlow(6);
  const { data: forecast, isLoading: isForecastLoading } = useCashFlowForecast();

  const fmt = (v?: number) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD", maximumFractionDigits: 0 }).format(v || 0);

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div className="border-b border-border pb-5">
        <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
          <Sparkles className="w-7 h-7 text-primary" /> Cash Flow Analytics & Forecast Engine
        </h1>
        <p className="text-sm text-muted-foreground mt-1">
          Monitor current cash position, historical liquidity trends, and 30-day AI liquidity forecasts
        </p>
      </div>

      {/* 30-Day Forecast Card */}
      <CashFlowForecastCard forecast={forecast} isLoading={isForecastLoading} />

      {/* KPI Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="p-4 rounded-xl bg-card border border-border">
          <div className="text-xs text-muted-foreground mb-1">Total Cash Received (Collections)</div>
          <div className="text-xl font-bold text-emerald-400">{fmt(summary?.totalCashIn)}</div>
        </div>

        <div className="p-4 rounded-xl bg-card border border-border">
          <div className="text-xs text-muted-foreground mb-1">Total Cash Disbursed (Expenses)</div>
          <div className="text-xl font-bold text-rose-400">{fmt(summary?.totalCashOut)}</div>
        </div>

        <div className="p-4 rounded-xl bg-card border border-border">
          <div className="text-xs text-muted-foreground mb-1">Current Bank Balance</div>
          <div className="text-xl font-bold text-foreground">{fmt(summary?.currentCashPosition)}</div>
        </div>

        <div className="p-4 rounded-xl bg-card border border-border">
          <div className="text-xs text-muted-foreground mb-1">Net Cash Flow</div>
          <div className={`text-xl font-bold ${(summary?.netCashFlow || 0) >= 0 ? "text-emerald-400" : "text-rose-400"}`}>
            {fmt(summary?.netCashFlow)}
          </div>
        </div>
      </div>

      {/* Interactive Recharts */}
      <CashFlowChart data={monthlyData} isLoading={isMonthlyLoading} />
    </div>
  );
}
