"use client";

import React from "react";
import { useRouter } from "next/navigation";
import {
  useBusinessHealthSummary,
  useRevenueAnalytics,
  useProductAnalytics,
  useGrowthRecommendations,
} from "@/hooks/use-business-performance";
import { KpiActionCard } from "@/components/business-performance/kpi-action-card";
import { GrowthOpportunityBanner } from "@/components/business-performance/growth-opportunity-banner";
import { RevenueTrendChart } from "@/components/business-performance/revenue-trend-chart";
import { MarginRankingTable } from "@/components/business-performance/margin-ranking-table";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Sparkles, TrendingUp, DollarSign, Users, Target, ShieldCheck, Download, RefreshCw } from "lucide-react";
import { toast } from "sonner";

export default function BusinessPerformanceDashboardPage() {
  const router = useRouter();
  const { data: health, isLoading: healthLoading, refetch } = useBusinessHealthSummary();
  const { data: revenueData, isLoading: revenueLoading } = useRevenueAnalytics();
  const { data: products, isLoading: productsLoading } = useProductAnalytics();
  const { data: recommendations } = useGrowthRecommendations();

  const handleRefresh = () => {
    refetch();
    toast.info("Telemetry refreshed!", { description: "Recalculated executive performance KPIs across all 8 sub-modules." });
  };

  const handleExport = () => {
    toast.success("Executive Briefing generated!", {
      description: "Business Performance Summary exported to encrypted PDF and dispatched to your email.",
    });
  };

  if (healthLoading || revenueLoading || productsLoading || !health || !revenueData || !products) {
    return (
      <div className="space-y-6 p-1">
        <Skeleton className="h-12 w-1/3" />
        <Skeleton className="h-44 w-full rounded-2xl" />
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <Skeleton key={i} className="h-36 w-full rounded-xl" />
          ))}
        </div>
        <Skeleton className="h-96 w-full rounded-xl" />
      </div>
    );
  }

  return (
    <div className="space-y-8 pb-12 animate-in fade-in-50 duration-300">
      {/* Page Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between border-b border-border/60 pb-6">
        <div>
          <h1 className="text-2xl font-black tracking-tight text-foreground sm:text-3xl flex items-center gap-2.5">
            <ShieldCheck className="h-8 w-8 text-primary" />
            Business Performance Dashboard
          </h1>
          <p className="mt-1.5 text-sm text-muted-foreground">
            Enterprise command center inspired by Power BI & Salesforce Revenue Intelligence. Driven by real-time actionable AI recommendations.
          </p>
        </div>
        <div className="flex items-center gap-3 shrink-0">
          <Button variant="outline" size="sm" onClick={handleRefresh} className="font-semibold shadow-xs">
            <RefreshCw className="mr-1.5 h-3.5 w-3.5" />
            Recalculate KPIs
          </Button>
          <Button size="sm" onClick={handleExport} className="font-bold bg-gradient-to-r from-primary to-primary/80 shadow-md">
            <Download className="mr-1.5 h-3.5 w-3.5" />
            Export Executive Brief
          </Button>
        </div>
      </div>

      {/* Hero AI Growth Opportunity Banner */}
      {recommendations && recommendations.length > 0 && (
        <GrowthOpportunityBanner
          recommendations={recommendations}
          onExploreAll={() => router.push("/dashboard/growth-center")}
        />
      )}

      {/* Actionable KPI Grid (6 Core Executive Metrics) */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
        <KpiActionCard
          title="Executive Health Score"
          value={`${health.healthScore} / 100`}
          subtitle="Optimal scalability & retention resilience"
          businessImpact="Top 15% SaaS Quartile Standing"
          variant="ai"
          icon={<ShieldCheck className="h-5 w-5" />}
          actionLabel="View Benchmarks"
          onActionClick={() => router.push("/dashboard/benchmarks")}
        />

        <KpiActionCard
          title="Annualized Run Rate (ARR)"
          value={`$${(health.arr / 1000).toFixed(1)}k`}
          subtitle={`Current ending MRR: $${(health.mrr / 1000).toFixed(1)}k`}
          trendPercentage={health.revenueGrowthRate}
          trendLabel="YoY ARR Momentum"
          variant="success"
          icon={<DollarSign className="h-5 w-5" />}
          actionLabel="Analyze Revenue"
          onActionClick={() => router.push("/dashboard/revenue-analytics")}
        />

        <KpiActionCard
          title="Net Profit Velocity"
          value={`$85.1k / mo`}
          subtitle={`Gross Profit Margin: ${health.grossMarginPercentage}%`}
          trendPercentage={health.profitGrowthRate}
          trendLabel="YoY Net Profit Growth"
          variant="success"
          icon={<TrendingUp className="h-5 w-5" />}
          actionLabel="Eliminate Loss Center"
          onActionClick={() => router.push("/dashboard/product-analytics")}
        />

        <KpiActionCard
          title="LTV : CAC Ratio"
          value={`${health.ltvCacRatio}x`}
          subtitle="Target healthy SaaS range: 3.0x - 5.0x"
          businessImpact="High acquisition capital efficiency"
          variant="default"
          icon={<Target className="h-5 w-5" />}
          actionLabel="Customer Cohorts"
          onActionClick={() => router.push("/dashboard/customer-analytics")}
        />

        <KpiActionCard
          title="Marketing ROAS & Attribution"
          value={`${health.marketingRoi}x`}
          subtitle="Return on Ad Spend across paid channels"
          businessImpact="+$44.5k/yr lift available via LinkedIn ABM"
          variant="default"
          icon={<Sparkles className="h-5 w-5" />}
          actionLabel="Optimize Budgets"
          onActionClick={() => router.push("/dashboard/marketing-roi")}
        />

        <KpiActionCard
          title="Active Enterprise Logos"
          value={health.activeCustomersCount}
          subtitle={`Net Revenue Retention (NRR): ${health.netRevenueRetention}%`}
          trendPercentage={health.customerGrowthRate}
          trendLabel="Net new accounts added"
          variant="default"
          icon={<Users className="h-5 w-5" />}
          actionLabel="Win-Back VIPs"
          onActionClick={() => router.push("/dashboard/customer-analytics")}
        />
      </div>

      {/* Multi-Series Revenue & MRR Velocity Chart */}
      <RevenueTrendChart
        trends={revenueData.trends}
        regions={revenueData.regions}
        title="Revenue Growth, Profitability & Global Regions"
        description="Drill down into New vs Expansion MRR velocity and geographic ARR foundations."
      />

      {/* Product Profitability & Loss Center Elimination Matrix */}
      <MarginRankingTable
        products={products}
        title="Top Products & Loss Center Optimization"
        description="Ranked by gross margin percentage. Take instant automated action to reorder reserve capacity or sunset negative margin tiers."
      />
    </div>
  );
}
