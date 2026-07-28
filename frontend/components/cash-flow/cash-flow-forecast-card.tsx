"use client";

import { CashFlowForecastDto } from "@/types/finance";
import { Sparkles, TrendingUp, TrendingDown, ArrowRight } from "lucide-react";

interface CashFlowForecastCardProps {
  forecast?: CashFlowForecastDto;
  isLoading?: boolean;
}

export function CashFlowForecastCard({ forecast, isLoading }: CashFlowForecastCardProps) {
  if (isLoading) {
    return <div className="h-40 bg-card/50 animate-pulse rounded-xl border border-border mb-6" />;
  }

  const fmt = (v?: number) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD", maximumFractionDigits: 0 }).format(v || 0);

  return (
    <div className="bg-gradient-to-br from-primary/10 via-card to-card border border-primary/20 rounded-xl p-5 mb-6 shadow-sm">
      <div className="flex items-center gap-2 mb-3">
        <Sparkles className="w-5 h-5 text-primary" />
        <h3 className="text-base font-bold text-foreground">30-Day Forward Cash Flow Projection</h3>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="bg-background/60 p-4 rounded-lg border border-border">
          <div className="text-xs text-muted-foreground mb-1 flex items-center gap-1">
            <TrendingUp className="w-3.5 h-3.5 text-emerald-500" /> Projected Inflows (30 Days)
          </div>
          <div className="text-xl font-bold text-emerald-400">{fmt(forecast?.projectedCashInNext30Days)}</div>
          <div className="text-[11px] text-muted-foreground mt-1">Expected collections from outstanding AR</div>
        </div>

        <div className="bg-background/60 p-4 rounded-lg border border-border">
          <div className="text-xs text-muted-foreground mb-1 flex items-center gap-1">
            <TrendingDown className="w-3.5 h-3.5 text-rose-500" /> Projected Outflows (30 Days)
          </div>
          <div className="text-xl font-bold text-rose-400">{fmt(forecast?.projectedCashOutNext30Days)}</div>
          <div className="text-[11px] text-muted-foreground mt-1">Due supplier bills & recurring overhead</div>
        </div>

        <div className="bg-primary/10 p-4 rounded-lg border border-primary/30">
          <div className="text-xs text-primary font-semibold mb-1 flex items-center gap-1">
            <ArrowRight className="w-3.5 h-3.5" /> Projected Position in 30 Days
          </div>
          <div className="text-2xl font-extrabold text-foreground">{fmt(forecast?.projectedCashPositionIn30Days)}</div>
          <div className="text-[11px] text-muted-foreground mt-1">Estimated bank liquidity end of month</div>
        </div>
      </div>
    </div>
  );
}
