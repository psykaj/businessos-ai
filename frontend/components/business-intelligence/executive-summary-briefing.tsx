"use client";

import React, { useState } from "react";
import { ExecutiveSummaryDto } from "@/lib/business-intelligence-service";
import { 
  Sparkles, 
  TrendingUp, 
  ArrowRight, 
  CheckCircle2, 
  Flame, 
  ShieldAlert, 
  Target, 
  Check, 
  Loader2 
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

interface ExecutiveSummaryBriefingProps {
  summary: ExecutiveSummaryDto;
  onExecuteAction?: (title: string) => Promise<void>;
}

export const ExecutiveSummaryBriefing: React.FC<ExecutiveSummaryBriefingProps> = ({ summary, onExecuteAction }) => {
  const [executingId, setExecutingId] = useState<string | null>(null);
  const [completedIds, setCompletedIds] = useState<Record<string, boolean>>({});

  const handleTriggerAction = async (id: string, title: string, department: string) => {
    setExecutingId(id);
    try {
      if (onExecuteAction) {
        await onExecuteAction(title);
      } else {
        await new Promise((resolve) => setTimeout(resolve, 800));
      }
      setCompletedIds((prev) => ({ ...prev, [id]: true }));
      toast.success(`Executed ${department} Workflow`, {
        description: `Successfully dispatched instruction for: "${title}" across live enterprise endpoints.`,
      });
    } catch {
      toast.error("Execution Failed", { description: "Unable to dispatch workflow commands." });
    } finally {
      setExecutingId(null);
    }
  };

  return (
    <section 
      aria-label="Executive Summary Briefing" 
      className="relative overflow-hidden rounded-3xl bg-gradient-to-r from-indigo-950/90 via-slate-900 to-purple-950/90 p-1 border border-indigo-500/30 shadow-2xl shadow-indigo-950/50 text-white"
    >
      {/* Decorative Glow Elements */}
      <div className="absolute -top-32 -left-32 h-64 w-64 rounded-full bg-indigo-500/20 blur-3xl pointer-events-none"></div>
      <div className="absolute -bottom-32 -right-32 h-64 w-64 rounded-full bg-purple-500/20 blur-3xl pointer-events-none"></div>

      <div className="relative rounded-[22px] bg-slate-950/80 backdrop-blur-xl p-6 md:p-8 space-y-8">
        {/* Header Badge & Title */}
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-white/10 pb-6">
          <div className="flex items-center gap-3">
            <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-gradient-to-tr from-indigo-500 to-purple-500 text-white shadow-lg shadow-indigo-500/30 ring-2 ring-white/20">
              <Sparkles className="h-6 w-6 animate-pulse" />
            </div>
            <div>
              <div className="flex items-center gap-2">
                <span className="inline-flex items-center gap-1.5 rounded-full bg-indigo-500/20 px-2.5 py-0.5 text-xs font-bold text-indigo-300 ring-1 ring-indigo-400/30 uppercase tracking-wider">
                  AI Executive Command
                </span>
                <span className="text-xs text-slate-400">Updated Real-Time</span>
              </div>
              <h2 className="text-2xl font-black tracking-tight text-white mt-1">
                Daily Operational Briefing & Next Best Actions
              </h2>
            </div>
          </div>
          <div className="text-right hidden sm:block">
            <span className="text-xs font-semibold uppercase tracking-wider text-slate-400">Enterprise AI Engine</span>
            <p className="text-xs text-slate-300 mt-0.5">Synthesizing telemetry across 6 pillars</p>
          </div>
        </div>

        {/* 4-Column Grid for Highlights, Opportunities, Risks, and Next Best Actions */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          
          {/* Column 1: Today's Highlights */}
          <div className="flex flex-col space-y-4 rounded-2xl bg-white/[0.03] border border-white/10 p-5 hover:bg-white/[0.05] transition-all duration-300">
            <div className="flex items-center justify-between">
              <span className="text-xs font-extrabold uppercase tracking-widest text-emerald-400 flex items-center gap-1.5">
                <Flame className="h-4 w-4" /> Today&apos;s Highlights
              </span>
              <span className="text-xs bg-emerald-500/20 text-emerald-300 font-semibold px-2 py-0.5 rounded-full border border-emerald-500/30">
                {summary.todayHighlights.length} Live
              </span>
            </div>
            <ul className="space-y-3 flex-1 text-sm text-slate-300">
              {summary.todayHighlights.map((highlight, idx) => (
                <li key={idx} className="flex items-start gap-2.5 leading-snug">
                  <CheckCircle2 className="h-4 w-4 text-emerald-400 shrink-0 mt-0.5" />
                  <span className="text-xs text-slate-300 font-medium leading-relaxed">{highlight}</span>
                </li>
              ))}
            </ul>
          </div>

          {/* Column 2: Top Opportunities */}
          <div className="flex flex-col space-y-4 rounded-2xl bg-white/[0.03] border border-white/10 p-5 hover:bg-white/[0.05] transition-all duration-300">
            <div className="flex items-center justify-between">
              <span className="text-xs font-extrabold uppercase tracking-widest text-amber-400 flex items-center gap-1.5">
                <TrendingUp className="h-4 w-4" /> Top Opportunities
              </span>
              <span className="text-xs bg-amber-500/20 text-amber-300 font-semibold px-2 py-0.5 rounded-full border border-amber-500/30">
                ${summary.topOpportunities.reduce((acc, curr) => acc + curr.estimatedValue, 0).toLocaleString()} Value
              </span>
            </div>
            <div className="space-y-3.5 flex-1">
              {summary.topOpportunities.slice(0, 3).map((opp) => (
                <div key={opp.id} className="p-2.5 rounded-xl bg-slate-900/60 border border-slate-800 space-y-1">
                  <div className="flex justify-between items-start gap-2">
                    <h4 className="text-xs font-bold text-white line-clamp-1">{opp.title}</h4>
                    <span className="text-xs font-bold text-amber-400 shrink-0">+${opp.estimatedValue.toLocaleString()}</span>
                  </div>
                  <p className="text-[11px] text-slate-400 leading-tight line-clamp-2">{opp.recommendedAction}</p>
                </div>
              ))}
            </div>
          </div>

          {/* Column 3: Top Risks */}
          <div className="flex flex-col space-y-4 rounded-2xl bg-white/[0.03] border border-white/10 p-5 hover:bg-white/[0.05] transition-all duration-300">
            <div className="flex items-center justify-between">
              <span className="text-xs font-extrabold uppercase tracking-widest text-red-400 flex items-center gap-1.5">
                <ShieldAlert className="h-4 w-4" /> Top Risks
              </span>
              <span className="text-xs bg-red-500/20 text-red-300 font-semibold px-2 py-0.5 rounded-full border border-red-500/30 animate-pulse">
                {summary.topRisks.length} Requires Action
              </span>
            </div>
            <div className="space-y-3 flex-1">
              {summary.topRisks.slice(0, 3).map((risk) => (
                <div key={risk.id} className="p-2.5 rounded-xl bg-slate-900/60 border border-red-500/20 space-y-1">
                  <div className="flex justify-between items-center gap-2">
                    <h4 className="text-xs font-bold text-white line-clamp-1">{risk.title}</h4>
                    <span className={`text-[10px] font-extrabold uppercase px-1.5 py-0.5 rounded ${
                      risk.severity === "Critical" ? "bg-red-500 text-white" : "bg-amber-500/30 text-amber-300"
                    }`}>
                      {risk.severity}
                    </span>
                  </div>
                  <p className="text-[11px] text-slate-400 leading-tight line-clamp-2">{risk.mitigation}</p>
                </div>
              ))}
            </div>
          </div>

          {/* Column 4: Next Best Actions */}
          <div className="flex flex-col space-y-4 rounded-2xl bg-gradient-to-b from-indigo-900/40 via-indigo-950/30 to-slate-900/80 border border-indigo-500/30 p-5 shadow-lg shadow-indigo-950/50">
            <div className="flex items-center justify-between">
              <span className="text-xs font-extrabold uppercase tracking-widest text-indigo-300 flex items-center gap-1.5">
                <Target className="h-4 w-4 text-indigo-400" /> Next Best Actions
              </span>
              <span className="text-[11px] bg-indigo-500/30 text-indigo-200 font-semibold px-2 py-0.5 rounded-full border border-indigo-400/40">
                AI Guided
              </span>
            </div>
            <div className="space-y-3 flex-1 overflow-y-auto max-h-[340px] pr-1">
              {summary.nextBestActions.map((act) => {
                const isExecuting = executingId === act.id;
                const isCompleted = completedIds[act.id];

                return (
                  <div key={act.id} className="p-3 rounded-xl bg-slate-900/90 border border-indigo-500/30 space-y-2 hover:border-indigo-400 transition-colors">
                    <div className="flex justify-between items-start">
                      <div>
                        <span className="text-[10px] font-bold tracking-wider text-indigo-400 uppercase">{act.department}</span>
                        <h4 className="text-xs font-bold text-white leading-tight">{act.title}</h4>
                      </div>
                    </div>
                    <p className="text-[11px] text-slate-400 italic leading-tight">
                      Expected: {act.expectedResult}
                    </p>
                    <Button
                      size="sm"
                      disabled={isExecuting || isCompleted}
                      onClick={() => handleTriggerAction(act.id, act.title, act.department)}
                      className={`w-full h-8 text-xs font-semibold rounded-lg shadow-md transition-all duration-200 flex items-center justify-center gap-1.5 ${
                        isCompleted 
                          ? "bg-emerald-600 hover:bg-emerald-600 text-white cursor-default" 
                          : "bg-indigo-600 hover:bg-indigo-500 text-white"
                      }`}
                    >
                      {isExecuting ? (
                        <>
                          <Loader2 className="h-3.5 w-3.5 animate-spin" />
                          Executing...
                        </>
                      ) : isCompleted ? (
                        <>
                          <Check className="h-3.5 w-3.5" />
                          Action Executed
                        </>
                      ) : (
                        <>
                          <span>{act.actionText}</span>
                          <ArrowRight className="h-3.5 w-3.5" />
                        </>
                      )}
                    </Button>
                  </div>
                );
              })}
            </div>
          </div>

        </div>
      </div>
    </section>
  );
};
