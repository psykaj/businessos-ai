"use client";

import { useCommandCenterSummary } from "@/lib/command-center-service";
import { CommandCenterHealth } from "@/components/dashboard/command-center/command-center-health";
import { CommandCenterAiSummary } from "@/components/dashboard/command-center/command-center-ai-summary";
import { CommandCenterNeedsAttention } from "@/components/dashboard/command-center/command-center-needs-attention";
import { CommandCenterOpportunities } from "@/components/dashboard/command-center/command-center-opportunities";
import { CommandCenterMetrics } from "@/components/dashboard/command-center/command-center-metrics";
import { CommandCenterAutomations } from "@/components/dashboard/command-center/command-center-automations";
import { CommandCenterActivity } from "@/components/dashboard/command-center/command-center-activity";
import { CommandCenterAskAi } from "@/components/dashboard/command-center/command-center-ask-ai";
import { Button } from "@/components/ui/button";
import { RefreshCcw } from "lucide-react";
import { format } from "date-fns";
import { useEffect, useState } from "react";

export default function DashboardPage() {
  const { data: summary, isLoading, refetch, isRefetching } = useCommandCenterSummary();
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  // Prevent hydration mismatch on date
  const today = mounted ? format(new Date(), "EEEE, MMMM d, yyyy") : "";

  return (
    <div className="flex flex-col gap-6 w-full max-w-7xl mx-auto pb-10">
      {/* Header */}
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight text-foreground">
            Business Command Center
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            {today}. Know what matters and what to do next.
          </p>
        </div>
        <div className="flex items-center gap-2">
          <Button 
            variant="outline" 
            size="sm" 
            onClick={() => refetch()} 
            disabled={isLoading || isRefetching}
            className="h-8"
          >
            <RefreshCcw className={`w-3.5 h-3.5 mr-2 ${isRefetching ? "animate-spin" : ""}`} />
            Refresh
          </Button>
        </div>
      </div>

      {/* 1. Ask AI */}
      <CommandCenterAskAi />

      {/* 2. Top Row: Health & AI Summary */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-1">
          <CommandCenterHealth health={summary?.businessHealth} isLoading={isLoading} />
        </div>
        <div className="lg:col-span-2 h-full">
          <CommandCenterAiSummary summary={summary?.executiveSummary} isLoading={isLoading} />
        </div>
      </div>

      {/* 3. Metrics */}
      <div className="space-y-3">
        <h3 className="text-sm font-medium text-muted-foreground uppercase tracking-wider">Key Metrics</h3>
        <CommandCenterMetrics metrics={summary?.metrics} isLoading={isLoading} />
      </div>

      {/* 4. Actionable Sections: Needs Attention & Opportunities */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <CommandCenterNeedsAttention alerts={summary?.priorityAlerts} actions={summary?.recommendedActions} isLoading={isLoading} />
        <CommandCenterOpportunities opportunities={summary?.opportunities} isLoading={isLoading} />
      </div>

      {/* 5. Lower Row: Automations & Activity */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-1">
          <CommandCenterAutomations summary={summary?.automationSummary} isLoading={isLoading} />
        </div>
        <div className="lg:col-span-2">
          <CommandCenterActivity activities={summary?.recentActivity} isLoading={isLoading} />
        </div>
      </div>
    </div>
  );
}
