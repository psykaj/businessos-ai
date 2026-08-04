"use client";

import React from "react";
import { useProductAnalytics } from "@/hooks/use-business-performance";
import { KpiActionCard } from "@/components/business-performance/kpi-action-card";
import { MarginRankingTable } from "@/components/business-performance/margin-ranking-table";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";
import { Package, TrendingUp, AlertOctagon, Sparkles, Zap, DollarSign } from "lucide-react";
import { toast } from "sonner";

export default function ProductAnalyticsPage() {
  const { data: products, isLoading } = useProductAnalytics();

  const handleSunsetLossCenter = () => {
    toast.success("Loss Center Sunset Workflow Initiated!", {
      description: "Legacy Custom Scripting Service disabled for new billing. Transitioned accounts to AI Copilot (Saving +$19,200/yr in net losses).",
      duration: 5000,
    });
  };

  const handleReorderAll = () => {
    toast.success("Reserve Capacity Purchase Orders Authorized!", {
      description: "50 Cloud API reserve cluster licenses pre-ordered ahead of Q3 seasonal surge.",
      duration: 5000,
    });
  };

  if (isLoading || !products) {
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

  const topProd = products[0];
  const lossCenter = products.find((p) => p.grossMarginPercentage < 0) || products[products.length - 1];

  return (
    <div className="space-y-8 pb-12 animate-in fade-in-50 duration-300">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between border-b border-border/60 pb-6">
        <div>
          <h1 className="text-2xl font-black tracking-tight text-foreground sm:text-3xl flex items-center gap-2.5">
            <Package className="h-8 w-8 text-primary" />
            Product & Profitability Analytics Engine
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Ranked product performance matrix isolating gross margin leaders from operating loss centers. Take immediate inventory action.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button variant="outline" size="sm" onClick={handleReorderAll} className="font-semibold shadow-xs">
            <Zap className="mr-1.5 h-3.5 w-3.5 text-amber-500" />
            Reorder Reserve Stock
          </Button>
          <Button size="sm" onClick={handleSunsetLossCenter} className="font-bold bg-rose-600 text-white hover:bg-rose-700 shadow-md">
            <AlertOctagon className="mr-1.5 h-3.5 w-3.5" />
            Sunset Loss Center (-$19.2k/yr)
          </Button>
        </div>
      </div>

      {/* KPI Highlights */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
        <KpiActionCard
          title="Top Profit Leader"
          value={topProd ? topProd.sku : "BOS-ENT-AI"}
          subtitle={`Yields $${((topProd?.revenue || 584000) / 1000).toFixed(0)}k at ${topProd?.grossMarginPercentage}% Margin`}
          trendPercentage={34.2}
          trendLabel="YoY profit velocity"
          variant="ai"
          icon={<TrendingUp className="h-5 w-5" />}
          actionLabel="Raise Tier Price 15%"
          onActionClick={() => {
            toast.success("Pricing optimized!", { description: "+15% fee applied to net-new enterprise agreements (+$87.6k/yr lift)." });
          }}
        />

        <KpiActionCard
          title="Loss Center Detected"
          value="-20% Margin"
          subtitle={lossCenter ? lossCenter.name : "Legacy Custom Scripting"}
          businessImpact="+$19,200/yr saved by sunsetting"
          variant="warning"
          icon={<AlertOctagon className="h-5 w-5 text-rose-500" />}
          actionLabel="Sunset Service Now"
          onActionClick={handleSunsetLossCenter}
        />

        <KpiActionCard
          title="Inventory Reserve Risk"
          value="12 Units Left"
          subtitle="Dedicated API Gateway Cluster"
          businessImpact="Q3 surge bottleneck predicted"
          variant="default"
          icon={<Package className="h-5 w-5" />}
          actionLabel="Authorize PO"
          onActionClick={handleReorderAll}
        />
      </div>

      {/* Product Profitability Table */}
      <MarginRankingTable
        products={products}
        title="Comprehensive Product Margin Matrix & Action Hooks"
        description="Filter and apply automated AI strategies to maximize SaaS margin yield above 85%."
      />
    </div>
  );
}
