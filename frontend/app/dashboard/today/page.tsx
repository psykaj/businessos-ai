"use client";

import { useEffect, useState } from "react";
import { useTodayBriefing, useGenerateBriefing, useRefreshBriefing } from "@/lib/daily-operating-loop-service";
import { TodayHeader } from "@/components/dashboard/today/today-header";
import { TodayHealth } from "@/components/dashboard/today/today-health";
import { TodaySummary } from "@/components/dashboard/today/today-summary";
import { TodayPriorities } from "@/components/dashboard/today/today-priorities";
import { YesterdayReview } from "@/components/dashboard/today/yesterday-review";
import { TodayBusinessValue } from "@/components/dashboard/today/today-business-value";

export default function TodayPage() {
  const { data: briefing, isLoading: isQueryLoading, error, refetch } = useTodayBriefing();
  const generateMutation = useGenerateBriefing();
  const refreshMutation = useRefreshBriefing();
  
  // We determine loading state by combining React Query loading with generation loading
  const isLoading = isQueryLoading || generateMutation.isPending;
  const isRefetching = refreshMutation.isPending;

  // Auto-generate if we receive a 404 (meaning no briefing exists for today)
  useEffect(() => {
    if (error && !briefing && !generateMutation.isPending) {
      // @ts-ignore - Assuming 404 error from Axios/apiClient means not found
      if (error?.response?.status === 404 || error?.message?.includes("404")) {
        generateMutation.mutate(false);
      }
    }
  }, [error, briefing, generateMutation]);

  const handleRefresh = () => {
    if (briefing?.id) {
      refreshMutation.mutate(briefing.id);
    } else {
      generateMutation.mutate(true); // Force regenerate
    }
  };

  return (
    <div className="flex flex-col gap-6 w-full max-w-7xl mx-auto pb-10">
      
      {/* 1. Header */}
      <TodayHeader 
        onRefresh={handleRefresh} 
        isRefetching={isRefetching} 
        generatedAt={briefing?.generatedAt} 
      />

      {/* Main Content Layout */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mt-2">
        
        {/* Left/Main Column: Health, Summary, Priorities */}
        <div className="lg:col-span-2 space-y-8 h-full">
          
          <div className="space-y-4">
            <TodayHealth health={briefing?.businessHealth || "Healthy"} />
            <TodaySummary summary={briefing?.summary} isLoading={isLoading} />
          </div>

          <div className="space-y-4">
            <h2 className="text-lg font-semibold tracking-tight text-foreground">
              Top Priorities
            </h2>
            <TodayPriorities 
              priorities={briefing?.priorities} 
              isLoading={isLoading} 
            />
          </div>

        </div>

        {/* Right Column: Yesterday & Value Context */}
        <div className="lg:col-span-1 space-y-6">
          <YesterdayReview 
            completedCount={briefing?.completedPriorityCount} 
            dismissedCount={0} // Adding default since DTO doesn't include it right now
            isLoading={isLoading} 
          />
          
          <TodayBusinessValue isLoading={isLoading} />
        </div>
      </div>
    </div>
  );
}
