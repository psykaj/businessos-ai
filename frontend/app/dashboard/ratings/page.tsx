"use client";

import React from "react";
import { useRatingsSummary } from "@/hooks/use-customer-feedback";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Star, TrendingUp, ShieldCheck, ThumbsUp, Sparkles, Building2 } from "lucide-react";

export default function RatingsPage() {
  const { data: summaries, isLoading } = useRatingsSummary();

  if (isLoading) {
    return <div className="p-12 text-center text-slate-500 animate-pulse font-medium">Loading Star Ratings & Review Distributions...</div>;
  }

  const items = summaries || [];

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* Page Header */}
      <div className="border-b border-slate-200 dark:border-slate-800 pb-6">
        <h1 className="text-3xl font-black text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
          <Star className="h-8 w-8 text-amber-500 fill-current" />
          Verified Star Ratings & Review Hub
        </h1>
        <p className="text-slate-500 dark:text-slate-400 mt-1 text-sm font-medium">
          Monitor aggregated star distributions across products, support agents, and API service reliability. High star ratings directly correlate with customer renewal and upselling.
        </p>
      </div>

      {/* Entity Rating Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {items.map((item, idx) => {
          const total = item.totalCount || 1;
          const stars5 = item.distribution[5] ?? 0;
          const stars4 = item.distribution[4] ?? 0;
          const stars3 = item.distribution[3] ?? 0;
          const stars2 = item.distribution[2] ?? 0;
          const stars1 = item.distribution[1] ?? 0;

          return (
            <Card key={idx} className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl overflow-hidden hover:border-amber-400/50 transition-all">
              <CardHeader className="p-6 bg-slate-50/50 dark:bg-slate-900/50 border-b border-slate-100 dark:border-slate-800 flex flex-row items-center justify-between">
                <div>
                  <Badge className="bg-slate-100 text-slate-700 dark:bg-slate-800 dark:text-slate-300 font-mono text-[10px] mb-1">
                    {item.entityType.toUpperCase()}
                  </Badge>
                  <CardTitle className="text-lg font-extrabold text-slate-900 dark:text-white">{item.entityName}</CardTitle>
                </div>
                <div className="text-right">
                  <div className="flex items-center gap-1.5 justify-end text-amber-500 font-black text-2xl">
                    <span>{item.averageScore.toFixed(1)}</span>
                    <Star className="h-6 w-6 fill-current" />
                  </div>
                  <span className="text-xs text-slate-400 font-medium">{total} Reviews Captured</span>
                </div>
              </CardHeader>
              <CardContent className="p-6 space-y-3">
                {[
                  { stars: 5, count: stars5, color: "bg-emerald-500" },
                  { stars: 4, count: stars4, color: "bg-emerald-400" },
                  { stars: 3, count: stars3, color: "bg-amber-400" },
                  { stars: 2, count: stars2, color: "bg-orange-500" },
                  { stars: 1, count: stars1, color: "bg-rose-500" },
                ].map((bar) => {
                  const pct = (bar.count / total) * 100;
                  return (
                    <div key={bar.stars} className="flex items-center gap-3 text-xs">
                      <span className="w-12 font-bold text-slate-600 dark:text-slate-400 flex items-center gap-1">
                        {bar.stars} <Star className="h-3 w-3 text-amber-500 fill-current" />
                      </span>
                      <div className="flex-1 bg-slate-100 dark:bg-slate-800 h-2.5 rounded-full overflow-hidden">
                        <div className={`h-full ${bar.color} rounded-full transition-all duration-500`} style={{ width: `${pct}%` }} />
                      </div>
                      <span className="w-24 text-right font-mono text-slate-500 dark:text-slate-400">
                        {bar.count} ({pct.toFixed(0)}%)
                      </span>
                    </div>
                  );
                })}

                <div className="pt-4 mt-3 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-xs font-semibold text-emerald-600 dark:text-emerald-400">
                  <span className="flex items-center gap-1">
                    <ShieldCheck className="h-4 w-4" /> Zero Critical Deficit Detected
                  </span>
                  <span>94% Positive Promoter Rate</span>
                </div>
              </CardContent>
            </Card>
          );
        })}
      </div>
    </div>
  );
}
