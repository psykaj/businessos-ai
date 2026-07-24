"use client";

import { useState } from "react";
import { CopilotNav } from "@/components/copilot/CopilotNav";
import { useCopilot } from "@/hooks/use-copilot";
import {
  Lightbulb,
  Sparkles,
  CheckCircle2,
  AlertTriangle,
  ArrowRight,
  TrendingUp,
  Filter,
  RefreshCw,
  Zap,
} from "lucide-react";
import { Recommendation } from "@/lib/copilot-service";
import { cn } from "@/lib/utils";

export default function RecommendationsPage() {
  const [categoryFilter, setCategoryFilter] = useState<string | undefined>(undefined);
  const [priorityFilter, setPriorityFilter] = useState<string | undefined>(undefined);

  const {
    useRecommendations,
    analyzeRecommendations,
    applyRecommendation,
    isAnalyzing,
  } = useCopilot();

  const { data: recommendationsData, isLoading } = useRecommendations({
    category: categoryFilter,
    priority: priorityFilter,
  });

  const recommendations: Recommendation[] = recommendationsData?.items || [];

  return (
    <div className="flex flex-col space-y-6 pb-12">
      <CopilotNav />

      <div className="px-6 space-y-6">
        {/* Header & Scan Button */}
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h2 className="text-xl font-bold text-foreground flex items-center gap-2">
              AI Recommendations Dashboard
              <Sparkles className="h-5 w-5 text-indigo-500" />
            </h2>
            <p className="text-xs text-muted-foreground">
              Proactive revenue-increasing and cost-reducing business suggestions.
            </p>
          </div>

          <div className="flex items-center gap-3">
            <select
              value={priorityFilter || ""}
              onChange={(e) => setPriorityFilter(e.target.value || undefined)}
              className="rounded-xl border border-border/60 bg-card px-3 py-2 text-xs text-foreground focus:outline-none"
            >
              <option value="">All Priorities</option>
              <option value="High">High Priority</option>
              <option value="Medium">Medium Priority</option>
              <option value="Low">Low Priority</option>
            </select>

            <button
              onClick={() => analyzeRecommendations()}
              disabled={isAnalyzing}
              className="flex items-center gap-2 rounded-xl bg-gradient-to-tr from-indigo-600 to-purple-600 px-4 py-2 text-xs font-semibold text-white shadow-md shadow-indigo-500/20 hover:opacity-90 transition-all disabled:opacity-50"
            >
              <RefreshCw className={cn("h-4 w-4", isAnalyzing && "animate-spin")} />
              {isAnalyzing ? "Analyzing Business Data..." : "Run AI Analysis Scan"}
            </button>
          </div>
        </div>

        {/* Recommendations List */}
        {isLoading ? (
          <div className="rounded-2xl border border-border/60 bg-card p-12 text-center text-xs text-muted-foreground animate-pulse">
            Scanning CRM, Billing, and Marketing metrics for recommendations...
          </div>
        ) : recommendations.length === 0 ? (
          <div className="rounded-2xl border border-border/60 bg-card p-12 text-center space-y-3">
            <CheckCircle2 className="h-10 w-10 text-emerald-500 mx-auto" />
            <h3 className="text-sm font-semibold text-foreground">All metrics optimal</h3>
            <p className="text-xs text-muted-foreground">
              Click "Run AI Analysis Scan" to analyze your latest business performance.
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {recommendations.map((rec) => {
              const isHigh = rec.priority === "High";
              return (
                <div
                  key={rec.id}
                  className={cn(
                    "flex flex-col justify-between rounded-2xl border p-5 shadow-sm transition-all hover:shadow-md",
                    isHigh
                      ? "border-amber-500/30 bg-amber-500/5 dark:bg-amber-500/10"
                      : "border-border/60 bg-card"
                  )}
                >
                  <div className="space-y-3">
                    <div className="flex items-center justify-between">
                      <span className="rounded-md bg-indigo-500/10 px-2 py-0.5 text-[10px] font-semibold text-indigo-500 uppercase tracking-wider">
                        {rec.category}
                      </span>
                      <span
                        className={cn(
                          "rounded-full px-2.5 py-0.5 text-[10px] font-bold uppercase",
                          isHigh
                            ? "bg-amber-500/20 text-amber-600 dark:text-amber-400"
                            : "bg-blue-500/20 text-blue-600 dark:text-blue-400"
                        )}
                      >
                        {rec.priority} Priority
                      </span>
                    </div>

                    <div>
                      <h3 className="text-sm font-bold text-foreground">{rec.title}</h3>
                      <p className="text-xs text-muted-foreground mt-1 leading-relaxed">
                        {rec.description}
                      </p>
                    </div>

                    {rec.reason && (
                      <div className="rounded-xl border border-border/40 bg-background/50 p-3 text-xs text-muted-foreground">
                        <span className="font-semibold text-foreground block mb-0.5">
                          Impact & Reason:
                        </span>
                        {rec.reason}
                      </div>
                    )}

                    <div className="rounded-xl border border-indigo-500/20 bg-indigo-500/5 p-3 text-xs text-indigo-700 dark:text-indigo-300">
                      <span className="font-semibold block mb-0.5">Suggested Action:</span>
                      {rec.suggestedAction}
                    </div>
                  </div>

                  <div className="flex items-center justify-between pt-4 mt-4 border-t border-border/40">
                    <span className="text-[11px] text-muted-foreground">
                      Created: {new Date(rec.createdAt).toLocaleDateString()}
                    </span>

                    {rec.isApplied ? (
                      <span className="flex items-center gap-1 text-xs font-semibold text-emerald-500">
                        <CheckCircle2 className="h-4 w-4" />
                        Applied
                      </span>
                    ) : (
                      <button
                        onClick={() => applyRecommendation(rec.id)}
                        className="flex items-center gap-1.5 rounded-xl bg-indigo-600 px-3.5 py-1.5 text-xs font-semibold text-white shadow-sm hover:bg-indigo-700 transition-colors"
                      >
                        <Zap className="h-3.5 w-3.5" />
                        Apply Action
                      </button>
                    )}
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
}
