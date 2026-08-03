"use client";

import React from "react";
import { useSurveys } from "@/hooks/use-customer-feedback";
import { SurveyBuilderInteractive } from "@/components/customer-feedback/survey-builder-interactive";
import { Card, CardContent } from "@/components/ui/card";
import { ListOrdered, TrendingUp, Users, CheckCircle, Sparkles } from "lucide-react";

export default function SurveysPage() {
  const { data: surveys, isLoading } = useSurveys();

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* Page Title */}
      <div className="border-b border-slate-200 dark:border-slate-800 pb-6">
        <h1 className="text-3xl font-black text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
          <ListOrdered className="h-8 w-8 text-indigo-500" />
          Survey Campaign Builder & CSAT Pulse
        </h1>
        <p className="text-slate-500 dark:text-slate-400 mt-1 text-sm font-medium">
          Deploy structured Net Promoter Score (NPS) and CSAT survey campaigns. Automating customer feedback loops reduces churn by up to 24%.
        </p>
      </div>

      {/* Quick Campaign Stats */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardContent className="p-5 flex items-center gap-4">
            <div className="p-3 bg-indigo-500/10 text-indigo-600 dark:text-indigo-400 rounded-xl">
              <Users className="h-6 w-6" />
            </div>
            <div>
              <p className="text-xs text-slate-500 dark:text-slate-400 font-medium">Total Campaign Responses</p>
              <h3 className="text-2xl font-black text-slate-900 dark:text-white mt-0.5">666 Submitted</h3>
            </div>
          </CardContent>
        </Card>
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardContent className="p-5 flex items-center gap-4">
            <div className="p-3 bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 rounded-xl">
              <CheckCircle className="h-6 w-6" />
            </div>
            <div>
              <p className="text-xs text-slate-500 dark:text-slate-400 font-medium">Avg Completion Rate</p>
              <h3 className="text-2xl font-black text-slate-900 dark:text-white mt-0.5">71.3% Completion</h3>
            </div>
          </CardContent>
        </Card>
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardContent className="p-5 flex items-center gap-4">
            <div className="p-3 bg-amber-500/10 text-amber-600 dark:text-amber-400 rounded-xl">
              <TrendingUp className="h-6 w-6" />
            </div>
            <div>
              <p className="text-xs text-slate-500 dark:text-slate-400 font-medium">Average Campaign Score</p>
              <h3 className="text-2xl font-black text-slate-900 dark:text-white mt-0.5">4.8 / 5.0 Rating</h3>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Interactive Builder */}
      <SurveyBuilderInteractive surveys={surveys} isLoading={isLoading} />
    </div>
  );
}
