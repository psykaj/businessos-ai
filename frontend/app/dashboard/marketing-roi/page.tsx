"use client";

import React from "react";
import { useMarketingRoi, useBusinessHealthSummary } from "@/hooks/use-business-performance";
import { KpiActionCard } from "@/components/business-performance/kpi-action-card";
import { RoasChannelOptimizer } from "@/components/business-performance/roas-channel-optimizer";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";
import { Sparkles, TrendingUp, Zap, Target, ArrowRightLeft, ShieldAlert } from "lucide-react";
import { toast } from "sonner";

export default function MarketingRoiPage() {
  const { data: channels, isLoading } = useMarketingRoi();
  const { data: health } = useBusinessHealthSummary();

  const handleOptimizeAll = () => {
    toast.success("Marketing Budget Reassigned!", {
      description: "Shifted $15,500/mo out of Broad Display Ads directly into LinkedIn B2B Executive Targeting. Projected ARR lift: +$53,040/yr.",
      duration: 6000,
    });
  };

  if (isLoading || !channels || !health) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-12 w-1/3" />
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {[1, 2, 3].map((i) => (
            <Skeleton key={i} className="h-36 w-full rounded-xl" />
          ))}
        </div>
        <Skeleton className="h-96 w-full rounded-xl" />
      </div>
    );
  }

  const topChan = channels.reduce((prev, curr) => (curr.roas > prev.roas ? curr : prev), channels[0]);
  const worstChan = channels.reduce((prev, curr) => (curr.roas < prev.roas ? curr : prev), channels[0]);

  return (
    <div className="space-y-8 pb-12 animate-in fade-in-50 duration-300">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between border-b border-border/60 pb-6">
        <div>
          <h1 className="text-2xl font-black tracking-tight text-foreground sm:text-3xl flex items-center gap-2.5">
            <Zap className="h-8 w-8 text-amber-500 fill-amber-500 animate-pulse" />
            Marketing ROI & Attribution Optimizer
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Evaluate Customer Acquisition Cost (CAC) by channel and shift budget dollars instantly into high-converting conversion leaders.
          </p>
        </div>
        <Button
          size="lg"
          onClick={handleOptimizeAll}
          className="font-bold bg-gradient-to-r from-emerald-600 to-primary text-white shadow-lg transition-transform hover:scale-105 active:scale-95 px-6"
        >
          <ArrowRightLeft className="mr-2 h-4 w-4" />
          Apply Complete AI Budget Shift
        </Button>
      </div>

      {/* KPI Highlights */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
        <KpiActionCard
          title="Top Converting Channel"
          value={`${topChan.roas}x ROAS`}
          subtitle={topChan.channelName}
          trendPercentage={42.8}
          trendLabel="YoY return efficiency"
          variant="ai"
          icon={<Sparkles className="h-5 w-5" />}
          actionLabel={`Scale +$${topChan.suggestedBudgetShift.toLocaleString()}`}
          onActionClick={handleOptimizeAll}
        />

        <KpiActionCard
          title="Budget Leakage Flag"
          value={`${worstChan.roas}x ROAS`}
          subtitle={worstChan.channelName}
          businessImpact="-$15,500 wasted monthly spend"
          variant="warning"
          icon={<ShieldAlert className="h-5 w-5 text-rose-500" />}
          actionLabel="Cut Channel Budget"
          onActionClick={handleOptimizeAll}
        />

        <KpiActionCard
          title="Aggregate Acquisition Cost"
          value="$4,200 CAC"
          subtitle="LinkedIn Executive B2B average"
          businessImpact="4.6x Overall LTV:CAC Ratio"
          variant="default"
          icon={<Target className="h-5 w-5" />}
          actionLabel="View Attribution"
          onActionClick={() => {
            const el = document.getElementById("optimizer-section");
            el?.scrollIntoView({ behavior: "smooth" });
          }}
        />
      </div>

      {/* Channel Optimizer Visualization & Actions */}
      <div id="optimizer-section">
        <RoasChannelOptimizer
          channels={channels}
          title="Channel Spend vs ROAS Attribution Matrix"
          description="Click any action button to simulate instant automated ad network API adjustments."
        />
      </div>
    </div>
  );
}
