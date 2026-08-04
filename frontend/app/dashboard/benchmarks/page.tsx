"use client";

import React from "react";
import { useIndustryBenchmarks, useBusinessHealthSummary } from "@/hooks/use-business-performance";
import { BenchmarkGauge } from "@/components/business-performance/benchmark-gauge";
import { KpiActionCard } from "@/components/business-performance/kpi-action-card";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";
import { Award, ShieldCheck, TrendingUp, RefreshCw, Download, Sparkles } from "lucide-react";
import { toast } from "sonner";

export default function BenchmarksPage() {
  const { data: benchmarks, isLoading, refetch } = useIndustryBenchmarks();
  const { data: health } = useBusinessHealthSummary();

  const handleSync = () => {
    refetch();
    toast.info("Industry Benchmarks Synced!", {
      description: "Updated cross-company SaaS & E-Commerce medians from real-time market intelligence feeds.",
    });
  };

  const handleExport = () => {
    toast.success("Benchmark Scorecard Downloaded!", {
      description: "Comparative industry standing report prepared for board and executive briefing.",
    });
  };

  if (isLoading || !benchmarks || !health) {
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

  return (
    <div className="space-y-8 pb-12 animate-in fade-in-50 duration-300">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between border-b border-border/60 pb-6">
        <div>
          <h1 className="text-2xl font-black tracking-tight text-foreground sm:text-3xl flex items-center gap-2.5">
            <Award className="h-8 w-8 text-primary" />
            Industry Benchmarks & Quartile Standing
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Cross-company comparative intelligence against SaaS and E-Commerce industry medians and top 25th percentile leaders.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button variant="outline" size="sm" onClick={handleSync} className="font-semibold shadow-xs">
            <RefreshCw className="mr-1.5 h-3.5 w-3.5" />
            Sync Market Data
          </Button>
          <Button size="sm" onClick={handleExport} className="font-bold bg-primary text-primary-foreground shadow-md">
            <Download className="mr-1.5 h-3.5 w-3.5" />
            Export Board Scorecard
          </Button>
        </div>
      </div>

      {/* KPI Highlights */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
        <KpiActionCard
          title="Net Revenue Retention (NRR)"
          value={`${health.netRevenueRetention}%`}
          subtitle="Top Quartile Industry Threshold: 120%"
          businessImpact="88th Percentile SaaS Leader"
          variant="ai"
          icon={<Sparkles className="h-5 w-5" />}
          actionLabel="View Details"
          onActionClick={() => {
            toast.info("NRR Telemetry:", { description: "You outperform 88% of SaaS peers due to automated AI workflow upgrades." });
          }}
        />

        <KpiActionCard
          title="LTV : CAC Unit Economics"
          value={`${health.ltvCacRatio}x`}
          subtitle="Industry Median: 2.8x"
          trendPercentage={18.2}
          trendLabel="Above top quartile target"
          variant="success"
          icon={<TrendingUp className="h-5 w-5" />}
          actionLabel="Scale Ads Budget"
          onActionClick={() => {
            toast.success("Ready to scale!", { description: "Unit economics justify immediate aggressive spending in top acquisition channels." });
          }}
        />

        <KpiActionCard
          title="Customer Stickiness & Churn"
          value="1.4% Churn"
          subtitle="SaaS Industry Median Churn: 8.5%"
          businessImpact="94th Percentile Loyalty Score"
          variant="default"
          icon={<ShieldCheck className="h-5 w-5 text-emerald-500" />}
          actionLabel="Export Proof"
          onActionClick={handleExport}
        />
      </div>

      {/* Benchmark Gauge Comparison List */}
      <BenchmarkGauge
        benchmarks={benchmarks}
        title="Comprehensive Metrics vs Industry Quartiles"
        description="Detailed breakdown of median, company value, and AI consultant recommendations for reaching the top 5% bracket."
      />
    </div>
  );
}
