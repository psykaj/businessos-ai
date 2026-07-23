"use client";

import { useState } from "react";
import { useInsights, useGenerateInsights } from "@/hooks/use-bi";
import { 
  Sparkles, 
  AlertTriangle, 
  CheckCircle2, 
  ArrowRight, 
  RefreshCw, 
  ShieldAlert, 
  TrendingUp, 
  Clock, 
  Filter, 
  Check 
} from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { toast } from "sonner";

export default function AIInsightsPage() {
  const [selectedPriority, setSelectedPriority] = useState<string | undefined>(undefined);
  const [resolvedIds, setResolvedIds] = useState<string[]>([]);

  const { data: insights = [], isLoading } = useInsights(undefined, selectedPriority);
  const { mutate: generateInsights, isPending: isGenerating } = useGenerateInsights();

  const handleResolve = (id: string, title: string) => {
    setResolvedIds((prev) => [...prev, id]);
    toast.success("Insight marked as resolved", { description: title });
  };

  const activeInsights = insights.filter((i) => !resolvedIds.includes(i.id));

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-[1600px] mx-auto min-h-screen">
      {/* Header Banner */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between border-b border-border/40 pb-6">
        <div>
          <div className="flex items-center gap-2.5">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary ring-1 ring-primary/20 shadow-inner">
              <Sparkles className="h-5 w-5" />
            </div>
            <div>
              <h1 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
                AI Business Decision Engine
              </h1>
              <p className="text-sm text-muted-foreground mt-0.5">
                Surfacing business risks, revenue opportunities & strategic next actions
              </p>
            </div>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <Button
            onClick={() => generateInsights()}
            disabled={isGenerating}
            className="gap-2 rounded-xl text-xs font-semibold shadow-md"
          >
            <RefreshCw className={`h-3.5 w-3.5 ${isGenerating ? "animate-spin" : ""}`} />
            {isGenerating ? "Analyzing Business Data..." : "Run AI Analysis"}
          </Button>
        </div>
      </div>

      {/* Filter Row */}
      <div className="flex items-center justify-between gap-4 flex-wrap">
        <div className="flex items-center gap-2">
          {["All", "Critical", "High", "Medium", "Low"].map((p) => {
            const isSelected = (p === "All" && !selectedPriority) || selectedPriority === p;
            return (
              <button
                key={p}
                onClick={() => setSelectedPriority(p === "All" ? undefined : p)}
                className={`rounded-xl px-4 py-2 text-xs font-semibold transition-all border ${
                  isSelected
                    ? "bg-primary text-primary-foreground border-primary shadow-sm"
                    : "bg-card/60 text-muted-foreground border-border/50 hover:bg-muted hover:text-foreground"
                }`}
              >
                {p}
              </button>
            );
          })}
        </div>

        <div className="text-xs text-muted-foreground font-medium">
          Active Recommendations: <span className="font-bold text-foreground">{activeInsights.length}</span>
        </div>
      </div>

      {/* Insights List */}
      {isLoading ? (
        <div className="grid gap-4">
          {[1, 2, 3].map((n) => (
            <div key={n} className="h-36 rounded-2xl border border-border/40 bg-card/30 animate-pulse" />
          ))}
        </div>
      ) : activeInsights.length === 0 ? (
        <Card className="rounded-2xl border-border/40 bg-card/40 p-12 text-center">
          <CheckCircle2 className="h-12 w-12 text-emerald-500 mx-auto mb-3 opacity-80" />
          <h3 className="text-lg font-bold text-foreground">All Insights Resolved</h3>
          <p className="text-sm text-muted-foreground max-w-md mx-auto mt-1">
            Great job! Your business operations are running smoothly without any urgent flagged risks.
          </p>
        </Card>
      ) : (
        <div className="grid gap-4">
          {activeInsights.map((insight) => {
            const isCritical = insight.priority === "Critical";
            const isHigh = insight.priority === "High";

            return (
              <Card key={insight.id} className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:border-border p-6">
                <div className="flex flex-col md:flex-row md:items-start md:justify-between gap-4">
                  <div className="space-y-3 flex-1">
                    <div className="flex items-center gap-3">
                      <Badge className={`rounded-lg px-2.5 py-0.5 text-xs font-bold ${
                        isCritical 
                          ? "bg-red-500/20 text-red-400 border-red-500/30" 
                          : isHigh 
                          ? "bg-amber-500/20 text-amber-400 border-amber-500/30" 
                          : "bg-blue-500/20 text-blue-400 border-blue-500/30"
                      }`}>
                        {insight.priority} Priority
                      </Badge>
                      <Badge variant="outline" className="rounded-lg text-xs font-semibold text-muted-foreground border-border/60">
                        {insight.category}
                      </Badge>
                    </div>

                    <h3 className="text-lg font-bold text-foreground tracking-tight">
                      {insight.title}
                    </h3>

                    <p className="text-sm text-muted-foreground leading-relaxed">
                      {insight.description}
                    </p>

                    <div className="grid gap-3 sm:grid-cols-2 pt-2">
                      <div className="rounded-xl border border-border/40 bg-muted/30 p-3">
                        <span className="text-[11px] font-bold text-muted-foreground uppercase tracking-wider block mb-1">
                          Business Impact
                        </span>
                        <span className="text-xs font-medium text-foreground">
                          {insight.businessImpact}
                        </span>
                      </div>
                      <div className="rounded-xl border border-primary/20 bg-primary/5 p-3">
                        <span className="text-[11px] font-bold text-primary uppercase tracking-wider block mb-1">
                          Recommended Action
                        </span>
                        <span className="text-xs font-medium text-foreground">
                          {insight.recommendation}
                        </span>
                      </div>
                    </div>
                  </div>

                  <div className="flex items-center gap-2 md:flex-col shrink-0">
                    <Button
                      size="sm"
                      onClick={() => handleResolve(insight.id, insight.title)}
                      className="gap-2 rounded-xl text-xs font-semibold w-full"
                    >
                      <Check className="h-3.5 w-3.5" />
                      Take Action & Resolve
                    </Button>
                  </div>
                </div>
              </Card>
            );
          })}
        </div>
      )}
    </div>
  );
}
