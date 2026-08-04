"use client";

import React from "react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Sparkles, Zap, ArrowRight, TrendingUp, DollarSign, ShieldAlert, CheckCircle2 } from "lucide-react";
import { useExecuteGrowthAction } from "@/hooks/use-business-performance";
import { GrowthRecommendationDto } from "@/lib/business-performance-service";
import { cn } from "@/lib/utils";

interface GrowthOpportunityBannerProps {
  recommendations: GrowthRecommendationDto[];
  onExploreAll?: () => void;
  className?: string;
}

export function GrowthOpportunityBanner({
  recommendations,
  onExploreAll,
  className,
}: GrowthOpportunityBannerProps) {
  const { mutate: executeAction, isPending } = useExecuteGrowthAction();
  
  // Find top priority open recommendation with highest revenue impact
  const topRec = recommendations.find((r) => r.status === "Open" && r.priority === "High") ?? recommendations[0];

  if (!topRec) {
    return null;
  }

  const handleExecute = () => {
    executeAction({
      id: topRec.id,
      actionType: topRec.title,
      estimatedImpact: topRec.estimatedRevenueImpact + topRec.costReductionImpact,
    });
  };

  const totalOpportunityValue = recommendations
    .filter((r) => r.status === "Open")
    .reduce((acc, curr) => acc + curr.estimatedRevenueImpact + curr.costReductionImpact, 0);

  return (
    <Card
      className={cn(
        "relative overflow-hidden border-2 border-primary/30 bg-gradient-to-r from-primary/15 via-background to-background p-0 shadow-lg transition-all duration-300 hover:border-primary/60",
        className
      )}
    >
      {/* Decorative ambient background glow */}
      <div className="pointer-events-none absolute -left-20 -top-20 h-64 w-64 rounded-full bg-primary/20 blur-3xl" />
      <div className="pointer-events-none absolute right-10 top-0 h-40 w-40 rounded-full bg-emerald-500/10 blur-2xl" />

      <CardContent className="p-6">
        <div className="flex flex-col gap-6 lg:flex-row lg:items-center lg:justify-between">
          {/* Left info column */}
          <div className="space-y-3 max-w-3xl">
            <div className="flex flex-wrap items-center gap-2">
              <Badge variant="default" className="bg-primary px-3 py-1 font-semibold text-primary-foreground shadow-sm">
                <Sparkles className="mr-1.5 h-3.5 w-3.5 animate-bounce" />
                AI Growth Center Priority #1
              </Badge>
              <Badge variant="outline" className="border-emerald-500/40 bg-emerald-500/10 font-bold text-emerald-600 dark:text-emerald-400">
                <TrendingUp className="mr-1 h-3.5 w-3.5" />
                Est. Lift: +${(topRec.estimatedRevenueImpact + topRec.costReductionImpact).toLocaleString()}/yr ARR
              </Badge>
              {topRec.priority === "High" && (
                <Badge variant="secondary" className="bg-amber-500/15 text-amber-700 dark:text-amber-400 font-semibold">
                  <Zap className="mr-1 h-3.5 w-3.5 fill-amber-500 text-amber-500" />
                  Immediate ROI Action
                </Badge>
              )}
            </div>

            <h2 className="text-xl font-bold tracking-tight text-foreground sm:text-2xl">
              {topRec.title}
            </h2>

            <p className="text-sm text-muted-foreground leading-relaxed">
              {topRec.description} <strong className="text-foreground font-semibold">{topRec.suggestedAction}</strong>
            </p>

            <div className="flex items-center gap-4 pt-1">
              <div className="w-48 space-y-1">
                <div className="flex justify-between text-xs font-medium">
                  <span className="text-muted-foreground">AI Confidence Score</span>
                  <span className="text-primary font-bold">{topRec.confidenceScore}%</span>
                </div>
                <Progress value={topRec.confidenceScore} className="h-1.5 bg-muted" />
              </div>
              
              <div className="text-xs text-muted-foreground pl-4 border-l border-border/60">
                Total Open Opportunity Vault: <span className="font-bold text-foreground text-sm">${totalOpportunityValue.toLocaleString()}</span>
              </div>
            </div>
          </div>

          {/* Right actions column */}
          <div className="flex shrink-0 flex-col gap-3 sm:flex-row lg:flex-col justify-end items-stretch">
            {topRec.status === "Actioned" ? (
              <Button disabled className="w-full bg-emerald-600 text-white font-semibold">
                <CheckCircle2 className="mr-2 h-4 w-4" />
                Action In-Flight
              </Button>
            ) : (
              <Button
                size="lg"
                onClick={handleExecute}
                disabled={isPending}
                className="w-full font-bold shadow-md shadow-primary/30 transition-all hover:scale-105 active:scale-95 bg-gradient-to-r from-primary to-primary/80 text-primary-foreground py-6 text-base"
              >
                <Zap className="mr-2 h-5 w-5 fill-primary-foreground animate-pulse" />
                {isPending ? "Executing..." : "Execute AI Strategy"}
              </Button>
            )}

            {onExploreAll && (
              <Button
                variant="outline"
                size="sm"
                onClick={onExploreAll}
                className="w-full font-semibold border-border hover:bg-muted/60"
              >
                Explore All {recommendations.length} AI Recommendations
                <ArrowRight className="ml-1.5 h-3.5 w-3.5" />
              </Button>
            )}
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
