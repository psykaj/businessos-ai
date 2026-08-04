"use client";

import React, { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  ResponsiveContainer,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
} from "recharts";
import { MarketingRoiChannel } from "@/lib/business-performance-service";
import { useReallocateMarketingBudget } from "@/hooks/use-business-performance";
import { Zap, DollarSign, TrendingUp, CheckCircle2, ArrowRightLeft } from "lucide-react";
import { cn } from "@/lib/utils";

interface RoasChannelOptimizerProps {
  channels: MarketingRoiChannel[];
  title?: string;
  description?: string;
}

export function RoasChannelOptimizer({
  channels,
  title = "Marketing ROI & Customer Acquisition Optimizer",
  description = "Channel ROAS analysis with automated budget shifting from underperforming broad networks directly into high-converting leaders.",
}: RoasChannelOptimizerProps) {
  const { mutate: shiftBudget, isPending } = useReallocateMarketingBudget();
  const [shiftedIds, setShiftedIds] = useState<Record<string, boolean>>({});

  const handleShift = (chan: MarketingRoiChannel) => {
    shiftBudget({
      channelId: chan.id,
      channelName: chan.channelName,
      shiftAmount: chan.suggestedBudgetShift,
      estimatedLift: chan.estimatedArrImpact,
    });
    setShiftedIds((prev) => ({ ...prev, [chan.id]: true }));
  };

  const totalSpend = channels.reduce((sum, c) => sum + c.spend, 0);
  const totalRev = channels.reduce((sum, c) => sum + c.revenueGenerated, 0);
  const aggregateRoas = (totalRev / (totalSpend || 1)).toFixed(2);

  // Format data for chart
  const chartData = channels.map((c) => ({
    name: c.channelName.split(" ")[0] + " " + (c.channelName.split(" ")[1] || ""),
    Spend: c.spend,
    Revenue: c.revenueGenerated,
    roas: c.roas,
  }));

  return (
    <Card className="border border-border bg-card shadow-sm">
      <CardHeader className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between border-b border-border/50 pb-4">
        <div>
          <CardTitle className="text-lg font-bold tracking-tight flex items-center gap-2">
            <Zap className="h-5 w-5 text-amber-500 fill-amber-500" />
            {title}
          </CardTitle>
          <CardDescription className="text-xs text-muted-foreground mt-1">
            {description}
          </CardDescription>
        </div>
        <div className="flex flex-wrap items-center gap-2">
          <Badge variant="outline" className="bg-primary/10 text-primary border-primary/30 font-bold px-3 py-1">
            Aggregate ROAS: {aggregateRoas}x
          </Badge>
          <Badge variant="outline" className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 font-semibold px-3 py-1">
            Est. Optimization Yield: +$53.0k/yr
          </Badge>
        </div>
      </CardHeader>

      <CardContent className="p-6">
        <div className="grid grid-cols-1 lg:grid-cols-12 gap-8 items-center">
          {/* Chart Section */}
          <div className="lg:col-span-6 space-y-3">
            <h4 className="text-xs font-bold uppercase tracking-wider text-muted-foreground flex items-center justify-between">
              <span>Channel Spend vs Revenue Yield</span>
              <span>Target ROAS: &gt;3.0x</span>
            </h4>
            <div className="h-[320px] w-full pt-2">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={chartData} margin={{ top: 10, right: 10, left: 0, bottom: 20 }}>
                  <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--muted-foreground)/0.15)" />
                  <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{ fontSize: 11, fill: "hsl(var(--muted-foreground))" }} interval={0} angle={-15} textAnchor="end" />
                  <YAxis axisLine={false} tickLine={false} tickFormatter={(v) => `$${v / 1000}k`} tick={{ fontSize: 11, fill: "hsl(var(--muted-foreground))" }} />
                  <Tooltip
                    contentStyle={{ backgroundColor: "hsl(var(--card))", borderColor: "hsl(var(--border))", borderRadius: "8px" }}
                    formatter={(val: any, name: any) => [`$${Number(val ?? 0).toLocaleString()}`, String(name ?? "")]}
                  />
                  <Legend verticalAlign="top" height={36} />
                  <Bar dataKey="Spend" name="Ad Spend ($)" fill="hsl(var(--muted-foreground)/0.5)" radius={[4, 4, 0, 0]} />
                  <Bar dataKey="Revenue" name="Revenue Generated ($)" fill="#10b981" radius={[4, 4, 0, 0]} />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>

          {/* Actionable Recommendations List */}
          <div className="lg:col-span-6 space-y-3">
            <h4 className="text-xs font-bold uppercase tracking-wider text-muted-foreground border-b border-border pb-2">
              AI Budget Reallocation Actions
            </h4>
            <div className="space-y-3 max-h-[340px] overflow-y-auto pr-1">
              {channels.map((chan) => {
                const isShifted = shiftedIds[chan.id];
                const isPositive = chan.suggestedBudgetShift > 0;

                return (
                  <div
                    key={chan.id}
                    className={cn(
                      "p-3.5 rounded-xl border transition-all flex flex-col gap-2.5",
                      chan.trend === "Underperforming" ? "border-rose-500/30 bg-rose-500/5" : "border-border/60 bg-muted/20 hover:bg-muted/40"
                    )}
                  >
                    <div className="flex items-start justify-between gap-2">
                      <div>
                        <span className="font-bold text-sm text-foreground flex items-center gap-1.5">
                          {chan.channelName}
                          <Badge
                            className={cn(
                              "text-[10px] font-extrabold px-1.5 py-0.5",
                              chan.roas >= 4 ? "bg-emerald-500/15 text-emerald-600 border-emerald-500/30" : chan.roas < 2 ? "bg-rose-500/15 text-rose-600 border-rose-500/30" : "bg-blue-500/15 text-blue-600"
                            )}
                          >
                            {chan.roas}x ROAS
                          </Badge>
                        </span>
                        <div className="text-xs text-muted-foreground mt-0.5">
                          Spend: ${chan.spend.toLocaleString()} | CAC: ${chan.cac.toLocaleString()} ({chan.conversions} enterprise deals)
                        </div>
                      </div>

                      {isShifted ? (
                        <Button size="sm" variant="outline" disabled className="h-7 text-xs bg-emerald-500/10 text-emerald-600 border-emerald-500/30 shrink-0">
                          <CheckCircle2 className="mr-1 h-3.5 w-3.5" />
                          Budget Shifted
                        </Button>
                      ) : (
                        <Button
                          size="sm"
                          onClick={() => handleShift(chan)}
                          disabled={isPending || chan.suggestedBudgetShift === 0}
                          className={cn(
                            "h-7 text-xs font-semibold shrink-0 shadow-sm transition-transform active:scale-95",
                            isPositive
                              ? "bg-emerald-600 text-white hover:bg-emerald-700"
                              : "bg-rose-600 text-white hover:bg-rose-700"
                          )}
                        >
                          <ArrowRightLeft className="mr-1 h-3.5 w-3.5" />
                          {isPositive ? `Add +$${chan.suggestedBudgetShift.toLocaleString()}` : `Cut -$${Math.abs(chan.suggestedBudgetShift).toLocaleString()}`}
                        </Button>
                      )}
                    </div>

                    <p className="text-xs text-muted-foreground border-t border-border/40 pt-2">
                      <span className="font-semibold text-foreground">AI Rationale:</span> {isPositive ? "Scale ad spending in this channel immediately." : "Stop budget leakage from sub-median conversions."}{" "}
                      <span className="text-emerald-600 dark:text-emerald-400 font-bold">
                        (Est. Impact: +${chan.estimatedArrImpact.toLocaleString()}/yr)
                      </span>
                    </p>
                  </div>
                );
              })}
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
