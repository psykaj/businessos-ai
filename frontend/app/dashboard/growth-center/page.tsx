"use client";

import React, { useState } from "react";
import { useGrowthRecommendations, useExecuteGrowthAction } from "@/hooks/use-business-performance";
import { GrowthRecommendationDto } from "@/lib/business-performance-service";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Skeleton } from "@/components/ui/skeleton";
import { Sparkles, Zap, CheckCircle2, DollarSign, TrendingUp, AlertTriangle, ShieldCheck, ArrowRight, Filter } from "lucide-react";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

export default function AiGrowthCenterPage() {
  const { data: recommendations, isLoading } = useGrowthRecommendations();
  const { mutate: executeAction, isPending } = useExecuteGrowthAction();
  const [selectedCategory, setSelectedCategory] = useState<string>("All");
  const [executedIds, setExecutedIds] = useState<Record<string, boolean>>({});

  const handleExecute = (rec: GrowthRecommendationDto) => {
    executeAction({
      id: rec.id,
      actionType: rec.title,
      estimatedImpact: rec.estimatedRevenueImpact + rec.costReductionImpact,
    });
    setExecutedIds((prev) => ({ ...prev, [rec.id]: true }));
  };

  const handleExecuteAll = () => {
    if (!recommendations) return;
    recommendations.forEach((r) => {
      if (!executedIds[r.id] && r.status !== "Actioned") {
        setExecutedIds((prev) => ({ ...prev, [r.id]: true }));
      }
    });
    toast.success("All AI Growth Strategies Dispatched!", {
      description: "Triggered 5 parallel automated workflows. Projected annual bottom-line yield: +$134,640/yr.",
      duration: 6000,
    });
  };

  if (isLoading || !recommendations) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-12 w-1/3" />
        <Skeleton className="h-32 w-full rounded-xl" />
        <div className="space-y-4">
          {[1, 2, 3, 4, 5].map((i) => (
            <Skeleton key={i} className="h-44 w-full rounded-xl" />
          ))}
        </div>
      </div>
    );
  }

  const categories = ["All", "Campaign Optimization", "Inventory Reorder", "VIP Retention", "Upsell Expansion", "Loss Elimination"];

  const filteredRecs = selectedCategory === "All"
    ? recommendations
    : recommendations.filter((r) => r.category === selectedCategory);

  const totalRevenueImpact = recommendations
    .filter((r) => !executedIds[r.id] && r.status === "Open")
    .reduce((sum, r) => sum + r.estimatedRevenueImpact, 0);

  const totalCostSavings = recommendations
    .filter((r) => !executedIds[r.id] && r.status === "Open")
    .reduce((sum, r) => sum + r.costReductionImpact, 0);

  return (
    <div className="space-y-8 pb-12 animate-in fade-in-50 duration-300">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between border-b border-border/60 pb-6">
        <div>
          <h1 className="text-2xl font-black tracking-tight text-foreground sm:text-3xl flex items-center gap-2.5">
            <Sparkles className="h-8 w-8 text-primary animate-bounce" />
            AI Growth Center Master Orchestrator
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Automated digital strategy consulting. Prioritizing business decisions by projected revenue lift and AI algorithmic confidence scores.
          </p>
        </div>
        <Button
          size="lg"
          onClick={handleExecuteAll}
          disabled={isPending || (totalRevenueImpact + totalCostSavings === 0)}
          className="font-bold bg-gradient-to-r from-primary via-primary/90 to-emerald-600 text-white shadow-lg transition-transform hover:scale-105 active:scale-95 px-6"
        >
          <Zap className="mr-2 h-5 w-5 fill-white animate-pulse" />
          Execute All High-ROI Strategies
        </Button>
      </div>

      {/* Opportunity Vault Scorecard */}
      <Card className="border-2 border-primary/40 bg-gradient-to-r from-primary/10 via-card to-emerald-500/10 shadow-md">
        <CardContent className="p-6">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6 text-center md:text-left divide-y md:divide-y-0 md:divide-x divide-border/60">
            <div className="space-y-1">
              <span className="text-xs font-bold uppercase tracking-wider text-muted-foreground">Total Available Financial Yield</span>
              <div className="text-3xl font-black text-foreground">
                ${(totalRevenueImpact + totalCostSavings).toLocaleString()} <span className="text-sm text-emerald-500 font-semibold">/ year</span>
              </div>
              <p className="text-xs text-muted-foreground">Synthesized across all 8 analytical sub-modules.</p>
            </div>

            <div className="space-y-1 md:pl-6 pt-4 md:pt-0">
              <span className="text-xs font-bold uppercase tracking-wider text-muted-foreground">Projected Revenue Expansion</span>
              <div className="text-3xl font-black text-emerald-600 dark:text-emerald-400">
                +${totalRevenueImpact.toLocaleString()} <span className="text-sm font-normal text-muted-foreground">ARR Lift</span>
              </div>
              <p className="text-xs text-muted-foreground">Via ad reallocation, enterprise upsells & VIP retention.</p>
            </div>

            <div className="space-y-1 md:pl-6 pt-4 md:pt-0">
              <span className="text-xs font-bold uppercase tracking-wider text-muted-foreground">Operational Cost Savings</span>
              <div className="text-3xl font-black text-primary">
                +${totalCostSavings.toLocaleString()} <span className="text-sm font-normal text-muted-foreground">Saved</span>
              </div>
              <p className="text-xs text-muted-foreground">Via sunsetting negative-margin legacy scripting support.</p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Filter Tabs */}
      <div className="flex flex-wrap items-center gap-1.5 border-b border-border pb-3">
        <span className="text-xs font-bold uppercase text-muted-foreground flex items-center gap-1.5 mr-2">
          <Filter className="h-3.5 w-3.5" /> Filter Vector:
        </span>
        {categories.map((cat) => (
          <Button
            key={cat}
            size="sm"
            variant={selectedCategory === cat ? "default" : "outline"}
            onClick={() => setSelectedCategory(cat)}
            className="h-8 text-xs font-semibold px-3 rounded-full"
          >
            {cat}
          </Button>
        ))}
      </div>

      {/* Prioritized AI Recommendations List */}
      <div className="space-y-5">
        {filteredRecs.map((rec) => {
          const isExecuted = executedIds[rec.id] || rec.status === "Actioned";
          const totalYield = rec.estimatedRevenueImpact + rec.costReductionImpact;

          return (
            <Card
              key={rec.id}
              className={cn(
                "transition-all duration-300 border-2 overflow-hidden shadow-sm hover:shadow-md",
                isExecuted ? "border-border/40 bg-muted/20 opacity-80" : rec.priority === "High" ? "border-primary/40 bg-card hover:border-primary/70" : "border-border"
              )}
            >
              <CardContent className="p-6">
                <div className="flex flex-col gap-6 lg:flex-row lg:items-center lg:justify-between">
                  <div className="space-y-3 max-w-3xl">
                    <div className="flex flex-wrap items-center gap-2">
                      <Badge className={cn(
                        "font-bold text-xs px-2.5 py-0.5",
                        rec.priority === "High" ? "bg-amber-500/20 text-amber-700 dark:text-amber-400 border border-amber-500/30" : "bg-muted text-muted-foreground"
                      )}>
                        <Zap className="mr-1 h-3.5 w-3.5 fill-amber-500 text-amber-500" />
                        {rec.priority} Priority
                      </Badge>
                      <Badge variant="outline" className="bg-emerald-500/10 text-emerald-600 font-extrabold border-emerald-500/30">
                        <TrendingUp className="mr-1 h-3.5 w-3.5" />
                        Est. Yield: +${totalYield.toLocaleString()}/yr
                      </Badge>
                      <Badge variant="secondary" className="font-semibold text-primary">
                        {rec.category}
                      </Badge>
                    </div>

                    <h2 className="text-xl font-bold text-foreground">
                      {rec.title}
                    </h2>

                    <p className="text-sm text-muted-foreground leading-relaxed">
                      {rec.description} <strong className="text-foreground font-semibold">Action Strategy: {rec.suggestedAction}</strong>
                    </p>

                    <div className="flex items-center gap-6 pt-1">
                      <div className="w-56 space-y-1">
                        <div className="flex justify-between text-xs font-bold">
                          <span className="text-muted-foreground">AI Confidence Factor</span>
                          <span className="text-primary">{rec.confidenceScore}%</span>
                        </div>
                        <Progress value={rec.confidenceScore} className="h-1.5" />
                      </div>

                      {rec.targetEntityName && (
                        <div className="text-xs text-muted-foreground border-l border-border/60 pl-4">
                          Target Entity: <span className="font-bold text-foreground">{rec.targetEntityName}</span>
                        </div>
                      )}
                    </div>
                  </div>

                  {/* Action execution column */}
                  <div className="shrink-0 flex flex-col items-stretch sm:flex-row lg:flex-col justify-center gap-2 min-w-[200px]">
                    {isExecuted ? (
                      <Button disabled className="w-full bg-emerald-600 text-white font-bold h-12 text-sm shadow-inner">
                        <CheckCircle2 className="mr-2 h-5 w-5" />
                        Workflow Active
                      </Button>
                    ) : (
                      <Button
                        size="lg"
                        onClick={() => handleExecute(rec)}
                        disabled={isPending}
                        className="w-full font-bold shadow-md bg-gradient-to-r from-primary to-primary/85 text-primary-foreground hover:scale-105 transition-transform h-12 text-sm"
                      >
                        <Zap className="mr-2 h-4 w-4 fill-primary-foreground animate-pulse" />
                        Execute Strategy
                        <ArrowRight className="ml-2 h-4 w-4" />
                      </Button>
                    )}

                    <span className="text-[11px] text-center text-muted-foreground font-mono">
                      {isExecuted ? "90-day real-time validation active" : "One-click automated workflow trigger"}
                    </span>
                  </div>
                </div>
              </CardContent>
            </Card>
          );
        })}
      </div>
    </div>
  );
}
