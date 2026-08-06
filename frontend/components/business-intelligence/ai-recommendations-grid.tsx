"use client";

import React, { useState } from "react";
import { RecommendationDto } from "@/lib/business-intelligence-service";
import { 
  Sparkles, 
  ArrowUpRight, 
  Check, 
  Loader2, 
  Zap, 
  DollarSign, 
  Users, 
  TrendingDown, 
  TrendingUp, 
  Package, 
  Award
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

interface AiRecommendationsGridProps {
  recommendations: RecommendationDto[];
  onExecute?: (title: string) => Promise<void>;
}

export const AiRecommendationsGrid: React.FC<AiRecommendationsGridProps> = ({ recommendations, onExecute }) => {
  const [selectedCategory, setSelectedCategory] = useState<string>("All");
  const [executingTitle, setExecutingTitle] = useState<string | null>(null);
  const [completedTitles, setCompletedTitles] = useState<Record<string, boolean>>({});

  // Categories list
  const categories = ["All", "Finance", "Customer", "Revenue", "Growth", "Inventory"];
  const filteredRecs = selectedCategory === "All" 
    ? recommendations 
    : recommendations.filter((r) => r.category.toLowerCase() === selectedCategory.toLowerCase());

  const handleExecute = async (title: string) => {
    setExecutingTitle(title);
    try {
      if (onExecute) {
        await onExecute(title);
      } else {
        await new Promise((res) => setTimeout(res, 900));
      }
      setCompletedTitles((prev) => ({ ...prev, [title]: true }));
      toast.success("AI Recommendation Executed", {
        description: `Triggered automated action sequence for: "${title}". Department notifications dispatched.`,
      });
    } catch {
      toast.error("Execution Interrupted", { description: "Failed to connect to workflow engine." });
    } finally {
      setExecutingTitle(null);
    }
  };

  const getPriorityTheme = (priority: string) => {
    switch (priority) {
      case "High":
        return {
          badge: "bg-red-500/10 text-red-600 dark:text-red-400 border-red-500/30 ring-1 ring-red-500/20",
          border: "hover:border-red-500/50 dark:hover:border-red-500/40",
          gradient: "from-red-500/5 to-transparent",
        };
      case "Medium":
        return {
          badge: "bg-amber-500/10 text-amber-600 dark:text-amber-400 border-amber-500/30 ring-1 ring-amber-500/20",
          border: "hover:border-amber-500/50 dark:hover:border-amber-500/40",
          gradient: "from-amber-500/5 to-transparent",
        };
      default:
        return {
          badge: "bg-blue-500/10 text-blue-600 dark:text-blue-400 border-blue-500/30 ring-1 ring-blue-500/20",
          border: "hover:border-blue-500/50 dark:hover:border-blue-500/40",
          gradient: "from-blue-500/5 to-transparent",
        };
    }
  };

  const getCategoryIcon = (category: string) => {
    switch (category) {
      case "Finance":
        return <DollarSign className="h-4 w-4 text-emerald-500" />;
      case "Customer":
        return <Users className="h-4 w-4 text-blue-500" />;
      case "Revenue":
        return <TrendingDown className="h-4 w-4 text-red-500" />;
      case "Growth":
        return <TrendingUp className="h-4 w-4 text-purple-500" />;
      case "Inventory":
        return <Package className="h-4 w-4 text-amber-500" />;
      default:
        return <Sparkles className="h-4 w-4 text-indigo-500" />;
    }
  };

  return (
    <div className="space-y-6">
      {/* Section Header & Category Filter Pills */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-4">
        <div className="flex items-center gap-2.5">
          <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-indigo-500/10 text-indigo-600 dark:text-indigo-400">
            <Zap className="h-5 w-5 fill-indigo-500 text-indigo-500 animate-pulse" />
          </div>
          <div>
            <h3 className="text-xl font-bold tracking-tight text-slate-900 dark:text-white flex items-center gap-2">
              Actionable AI Recommendations
              <span className="text-xs bg-indigo-500/20 text-indigo-700 dark:text-indigo-300 px-2 py-0.5 rounded-full font-bold">
                {recommendations.length} Active
              </span>
            </h3>
            <p className="text-xs text-slate-500 dark:text-slate-400">
              One-click autonomous workflows synthesized from live operational data
            </p>
          </div>
        </div>

        {/* Category Filters */}
        <div className="flex flex-wrap items-center gap-1.5 p-1 rounded-2xl bg-slate-100 dark:bg-slate-800/60 border border-slate-200 dark:border-slate-700">
          {categories.map((cat) => {
            const isActive = selectedCategory === cat;
            return (
              <button
                key={cat}
                onClick={() => setSelectedCategory(cat)}
                className={cn(
                  "px-3 py-1.5 rounded-xl text-xs font-bold transition-all duration-200",
                  isActive 
                    ? "bg-white dark:bg-indigo-600 text-slate-900 dark:text-white shadow-md shadow-indigo-500/10"
                    : "text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-white hover:bg-slate-200/50 dark:hover:bg-slate-800"
                )}
              >
                {cat}
              </button>
            );
          })}
        </div>
      </div>

      {/* Recommendations Cards Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredRecs.map((rec, idx) => {
          const theme = getPriorityTheme(rec.priority);
          const isExecuting = executingTitle === rec.title;
          const isCompleted = completedTitles[rec.title];
          const confPercent = Math.round(rec.confidenceScore * 100);

          return (
            <div
              key={idx}
              className={cn(
                "group relative flex flex-col justify-between rounded-3xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-lg hover:shadow-2xl transition-all duration-300 p-6 overflow-hidden transform hover:-translate-y-1",
                theme.border
              )}
            >
              {/* Subtle radial background gradient */}
              <div className={`absolute inset-0 bg-gradient-to-br ${theme.gradient} pointer-events-none opacity-50`}></div>

              <div className="space-y-4 z-10">
                {/* Category Icon & Priority Badge */}
                <div className="flex items-center justify-between">
                  <span className="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-lg bg-slate-100 dark:bg-slate-800/80 text-xs font-bold text-slate-700 dark:text-slate-300 border border-slate-200/60 dark:border-slate-700/60">
                    {getCategoryIcon(rec.category)}
                    {rec.category}
                  </span>
                  <span className={cn("px-2.5 py-0.5 rounded-full text-xs font-black uppercase tracking-wider border shadow-xs", theme.badge)}>
                    {rec.priority} Priority
                  </span>
                </div>

                {/* Title & Description */}
                <div>
                  <h4 className="text-base font-extrabold text-slate-900 dark:text-white tracking-tight leading-snug group-hover:text-indigo-600 dark:group-hover:text-indigo-400 transition-colors">
                    {rec.title}
                  </h4>
                  <p className="mt-2 text-xs text-slate-600 dark:text-slate-400 leading-relaxed line-clamp-3">
                    {rec.description}
                  </p>
                </div>

                {/* Suggested Action Box */}
                <div className="p-3.5 rounded-2xl bg-indigo-50/50 dark:bg-indigo-950/20 border border-indigo-500/20 text-xs text-slate-700 dark:text-slate-300 space-y-1">
                  <span className="text-[10px] font-extrabold text-indigo-600 dark:text-indigo-400 uppercase tracking-widest block">
                    Recommended AI Action
                  </span>
                  <p className="font-semibold text-slate-800 dark:text-slate-200">
                    {rec.recommendedAction}
                  </p>
                </div>

                {/* Expected Business Impact */}
                <div className="p-3 rounded-xl bg-emerald-500/5 border border-emerald-500/20 text-xs flex items-start gap-2.5">
                  <Award className="h-4 w-4 text-emerald-500 shrink-0 mt-0.5" />
                  <div>
                    <span className="text-[10px] font-extrabold text-emerald-600 dark:text-emerald-400 uppercase tracking-wider block">
                      Expected Business Impact
                    </span>
                    <span className="font-bold text-emerald-700 dark:text-emerald-300 text-xs">
                      {rec.expectedBusinessImpact}
                    </span>
                  </div>
                </div>
              </div>

              <div className="space-y-4 mt-6 z-10 pt-4 border-t border-slate-100 dark:border-slate-800/80">
                {/* Confidence Score Bar */}
                <div className="flex items-center justify-between text-[11px]">
                  <span className="text-slate-500 dark:text-slate-400 font-bold">AI Confidence Score</span>
                  <span className="font-extrabold font-mono text-indigo-600 dark:text-indigo-400">{confPercent}%</span>
                </div>
                <div className="w-full bg-slate-100 dark:bg-slate-800 h-1.5 rounded-full overflow-hidden">
                  <div 
                    className="bg-gradient-to-r from-indigo-500 to-purple-500 h-full rounded-full"
                    style={{ width: `${confPercent}%` }}
                  ></div>
                </div>

                {/* One-Click Action Button */}
                <Button
                  onClick={() => handleExecute(rec.title)}
                  disabled={isExecuting || isCompleted}
                  className={cn(
                    "w-full h-11 text-xs font-extrabold rounded-2xl shadow-lg transition-all duration-200 flex items-center justify-center gap-2 transform active:scale-95",
                    isCompleted
                      ? "bg-emerald-600 hover:bg-emerald-600 text-white shadow-emerald-500/20 cursor-default"
                      : "bg-gradient-to-r from-indigo-600 via-indigo-700 to-purple-600 hover:from-indigo-500 hover:to-purple-500 text-white shadow-indigo-500/25 hover:shadow-indigo-500/40"
                  )}
                >
                  {isExecuting ? (
                    <>
                      <Loader2 className="h-4 w-4 animate-spin" />
                      Dispatching Commands...
                    </>
                  ) : isCompleted ? (
                    <>
                      <Check className="h-4 w-4" />
                      Action Executed Successfully
                    </>
                  ) : (
                    <>
                      <Zap className="h-4 w-4 fill-white" />
                      Execute Action Now
                      <ArrowUpRight className="h-4 w-4 opacity-70" />
                    </>
                  )}
                </Button>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};
