"use client";

import React from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { IndustryBenchmarkItem } from "@/lib/business-performance-service";
import { ShieldCheck, Award, TrendingUp, Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface BenchmarkGaugeProps {
  benchmarks: IndustryBenchmarkItem[];
  title?: string;
  description?: string;
}

export function BenchmarkGauge({
  benchmarks,
  title = "Industry Benchmark & Quartile Standing",
  description = "Cross-company comparison against SaaS & E-Commerce industry medians and top 25th percentile performers.",
}: BenchmarkGaugeProps) {
  const averagePercentile = Math.round(
    benchmarks.reduce((acc, curr) => acc + curr.percentileRank, 0) / (benchmarks.length || 1)
  );

  return (
    <Card className="border border-border bg-card shadow-sm">
      <CardHeader className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between border-b border-border/50 pb-4">
        <div>
          <CardTitle className="text-lg font-bold tracking-tight flex items-center gap-2">
            <Award className="h-5 w-5 text-primary" />
            {title}
          </CardTitle>
          <CardDescription className="text-xs text-muted-foreground mt-1">
            {description}
          </CardDescription>
        </div>

        <div className="flex items-center gap-3">
          <Badge className="bg-gradient-to-r from-primary to-emerald-600 text-white font-bold px-3 py-1 text-xs shadow-sm">
            <Sparkles className="mr-1.5 h-3.5 w-3.5" />
            Top {100 - averagePercentile}% Industry Leader (82nd Percentile Avg)
          </Badge>
        </div>
      </CardHeader>

      <CardContent className="p-6">
        <div className="space-y-6">
          {benchmarks.map((bm, idx) => {
            const isTop = bm.percentileRank >= 75;

            return (
              <div key={idx} className="space-y-2 border-b border-border/40 pb-5 last:border-0 last:pb-0">
                <div className="flex flex-wrap items-center justify-between gap-2">
                  <div className="flex items-center gap-2">
                    <span className="font-bold text-sm text-foreground">{bm.metricName}</span>
                    <Badge
                      variant="outline"
                      className={cn(
                        "text-xs font-semibold px-2 py-0.5",
                        isTop ? "bg-emerald-500/10 text-emerald-600 border-emerald-500/30" : "bg-blue-500/10 text-blue-600 border-blue-500/30"
                      )}
                    >
                      {bm.status}
                    </Badge>
                  </div>
                  <div className="text-xs text-muted-foreground font-medium">
                    Percentile Standing: <span className="font-bold text-primary text-sm">{bm.percentileRank}th</span>
                  </div>
                </div>

                {/* Progress Visualizer */}
                <div className="space-y-1 pt-1">
                  <Progress value={bm.percentileRank} className={cn("h-2", isTop ? "bg-emerald-500/20" : "bg-muted")} />
                  <div className="flex justify-between text-[11px] font-mono text-muted-foreground pt-1">
                    <span>Median: {bm.industryMedian}{bm.unit}</span>
                    <span className="font-bold text-foreground">Our Company: {bm.companyValue}{bm.unit}</span>
                    <span>Top Quartile Target: {bm.topQuartileValue}{bm.unit}</span>
                  </div>
                </div>

                {/* AI Consultant Guidance Note */}
                <p className="text-xs text-muted-foreground bg-muted/30 p-2.5 rounded-lg border border-border/40 flex items-start gap-2 mt-2">
                  <Sparkles className="h-4 w-4 text-primary shrink-0 mt-0.5" />
                  <span>
                    <strong className="text-foreground font-semibold">AI Consultant Guidance:</strong> {bm.aiAdvice}
                  </span>
                </p>
              </div>
            );
          })}
        </div>
      </CardContent>
    </Card>
  );
}
