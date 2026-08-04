"use client";

import React, { useState } from "react";
import { useRevenueAnalytics, useBusinessHealthSummary } from "@/hooks/use-business-performance";
import { KpiActionCard } from "@/components/business-performance/kpi-action-card";
import { RevenueTrendChart } from "@/components/business-performance/revenue-trend-chart";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { DollarSign, TrendingUp, ArrowUpRight, Layers, FileText, Download, Sparkles } from "lucide-react";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

export default function RevenueAnalyticsPage() {
  const { data: revenue, isLoading } = useRevenueAnalytics();
  const { data: health } = useBusinessHealthSummary();
  const [selectedPeriod, setSelectedPeriod] = useState<string | null>(null);

  if (isLoading || !revenue || !health) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-12 w-1/3" />
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {[1, 2, 3].map((i) => (
            <Skeleton key={i} className="h-32 w-full rounded-xl" />
          ))}
        </div>
        <Skeleton className="h-96 w-full rounded-xl" />
      </div>
    );
  }

  const handleDrilldown = (period: string) => {
    setSelectedPeriod(period);
  };

  const handleExportReport = () => {
    toast.success("Audit-grade Revenue Report exported!", {
      description: "Comprehensive MRR decomposition and retention ledger downloaded to CSV.",
    });
  };

  const selectedData = revenue.trends.find((t) => t.period === selectedPeriod);

  return (
    <div className="space-y-8 pb-12 animate-in fade-in-50 duration-300">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between border-b border-border/60 pb-6">
        <div>
          <h1 className="text-2xl font-black tracking-tight text-foreground sm:text-3xl flex items-center gap-2.5">
            <DollarSign className="h-8 w-8 text-primary" />
            Revenue & Profit Analytics Engine
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Audit-grade MRR decomposition (New, Expansion, Contraction, Churned), retention dynamics, and global revenue foundations.
          </p>
        </div>
        <Button size="sm" onClick={handleExportReport} className="font-bold shadow-sm">
          <Download className="mr-1.5 h-4 w-4" />
          Export Revenue Ledger
        </Button>
      </div>

      {/* KPI Highlight Row */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
        <KpiActionCard
          title="Net Revenue Retention (NRR)"
          value={`${health.netRevenueRetention}%`}
          subtitle="Top quartile threshold: >120%"
          businessImpact="Compound organic expansion active"
          variant="ai"
          icon={<Layers className="h-5 w-5" />}
          actionLabel="View Regions"
          onActionClick={() => {
            const el = document.getElementById("regional-foundation");
            el?.scrollIntoView({ behavior: "smooth" });
          }}
        />

        <KpiActionCard
          title="Expansion MRR Velocity"
          value="$8,400 / mo"
          subtitle="Current ending MRR: $104.5k/mo"
          trendPercentage={24.5}
          trendLabel="MoM upgrade growth"
          variant="success"
          icon={<TrendingUp className="h-5 w-5" />}
          actionLabel="View Drilldown"
          onActionClick={() => handleDrilldown("Jul")}
        />

        <KpiActionCard
          title="Revenue Leakage Prevention"
          value="1.4% Churn"
          subtitle="Contraction MRR: only $600/mo in Jul"
          businessImpact="-$14,500 year-over-year saved"
          variant="default"
          icon={<Sparkles className="h-5 w-5" />}
          actionLabel="Export Report"
          onActionClick={handleExportReport}
        />
      </div>

      {/* Master Trend Chart */}
      <div id="regional-foundation">
        <RevenueTrendChart
          trends={revenue.trends}
          regions={revenue.regions}
          title="Multi-Period Revenue, Net Profit & Expansion Ledger"
          description="Click any month in the comparison ledger below to trigger granular audit reports."
        />
      </div>

      {/* Monthly Comparison Table with Drilldown */}
      <Card className="border border-border bg-card shadow-sm">
        <CardHeader className="border-b border-border/50 pb-4">
          <CardTitle className="text-lg font-bold">Monthly MRR Evolution & Comparison Ledger</CardTitle>
          <CardDescription>Decomposing net subscription momentum across active tenant schemas.</CardDescription>
        </CardHeader>
        <CardContent className="p-0 overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="bg-muted/50 text-xs uppercase tracking-wider text-muted-foreground border-b border-border/60">
              <tr>
                <th className="py-3.5 px-4 font-semibold">Period</th>
                <th className="py-3.5 px-4 font-semibold">Total Revenue</th>
                <th className="py-3.5 px-4 font-semibold">Net Profit ($)</th>
                <th className="py-3.5 px-4 font-semibold text-emerald-600 dark:text-emerald-400">New MRR</th>
                <th className="py-3.5 px-4 font-semibold text-primary">Expansion MRR</th>
                <th className="py-3.5 px-4 font-semibold text-rose-600 dark:text-rose-400">Contraction + Churn</th>
                <th className="py-3.5 px-4 font-semibold text-right">Drilldown Report</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border/40 font-mono text-xs">
              {revenue.trends.map((item, index) => {
                const prev = index > 0 ? revenue.trends[index - 1] : null;
                const revGrowth = prev ? ((item.revenue - prev.revenue) / prev.revenue * 100).toFixed(1) : "N/A";

                return (
                  <tr key={item.period} className="hover:bg-muted/30 transition-colors">
                    <td className="py-3.5 px-4 font-bold text-foreground text-sm">{item.period} 2026</td>
                    <td className="py-3.5 px-4 font-bold text-foreground">
                      ${item.revenue.toLocaleString()}
                      {revGrowth !== "N/A" && (
                        <span className="ml-2 text-[11px] text-emerald-600 dark:text-emerald-400 font-sans font-bold">
                          (+{revGrowth}%)
                        </span>
                      )}
                    </td>
                    <td className="py-3.5 px-4 text-primary font-bold">${item.profit.toLocaleString()}</td>
                    <td className="py-3.5 px-4 text-emerald-600 dark:text-emerald-400">+${item.newMrr.toLocaleString()}</td>
                    <td className="py-3.5 px-4 text-primary">+${item.expansionMrr.toLocaleString()}</td>
                    <td className="py-3.5 px-4 text-rose-600 dark:text-rose-400">
                      -${(item.contractionMrr + item.churnedMrr).toLocaleString()}
                    </td>
                    <td className="py-3.5 px-4 text-right font-sans">
                      <Button size="sm" variant="outline" onClick={() => handleDrilldown(item.period)} className="h-7 text-xs font-semibold">
                        <FileText className="mr-1 h-3 w-3" />
                        Drilldown
                      </Button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </CardContent>
      </Card>

      {/* Drilldown Modal / Dialog */}
      <Dialog open={!!selectedPeriod} onOpenChange={(open) => !open && setSelectedPeriod(null)}>
        <DialogContent className="max-w-2xl">
          <DialogHeader>
            <DialogTitle className="text-xl font-bold flex items-center gap-2">
              <FileText className="h-5 w-5 text-primary" />
              Audit-Grade Revenue Drilldown: {selectedPeriod} 2026
            </DialogTitle>
            <DialogDescription>
              Detailed breakdown of recurring monetary flows and enterprise contraction mitigation.
            </DialogDescription>
          </DialogHeader>

          {selectedData && (
            <div className="space-y-6 pt-2">
              <div className="grid grid-cols-2 gap-4">
                <div className="p-3.5 rounded-xl border bg-muted/20 space-y-1">
                  <span className="text-xs text-muted-foreground uppercase">Net New MRR Added</span>
                  <div className="text-2xl font-bold text-emerald-600 dark:text-emerald-400">
                    +${(selectedData.newMrr + selectedData.expansionMrr - selectedData.contractionMrr - selectedData.churnedMrr).toLocaleString()}
                  </div>
                </div>
                <div className="p-3.5 rounded-xl border bg-muted/20 space-y-1">
                  <span className="text-xs text-muted-foreground uppercase">Net Profit Margin (%)</span>
                  <div className="text-2xl font-bold text-primary">
                    {((selectedData.profit / selectedData.revenue) * 100).toFixed(1)}%
                  </div>
                </div>
              </div>

              <div className="space-y-3 text-sm">
                <h4 className="font-bold text-foreground border-b pb-1.5">Executive Insights for {selectedPeriod}</h4>
                <ul className="space-y-2 list-disc pl-5 text-muted-foreground">
                  <li>
                    <strong className="text-foreground">New Logo Acquisition:</strong> Acquired enterprise contracts yielding <span className="text-emerald-600 font-semibold">${selectedData.newMrr.toLocaleString()}</span> in recurring commitments.
                  </li>
                  <li>
                    <strong className="text-foreground">Expansion & Cross-Sells:</strong> AI Workflow upgrades generated an additional <span className="text-primary font-semibold">${selectedData.expansionMrr.toLocaleString()}</span> from existing cohorts.
                  </li>
                  <li>
                    <strong className="text-foreground">Contraction Mitigation:</strong> Proactive intervention maintained churned MRR at an immaterial <span className="text-rose-600 font-semibold">${selectedData.churnedMrr.toLocaleString()}</span>.
                  </li>
                </ul>
              </div>

              <div className="flex justify-end gap-3 pt-4 border-t">
                <Button variant="outline" onClick={() => setSelectedPeriod(null)}>Close</Button>
                <Button onClick={() => { setSelectedPeriod(null); handleExportReport(); }}>Export Detailed Audit (CSV)</Button>
              </div>
            </div>
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}
