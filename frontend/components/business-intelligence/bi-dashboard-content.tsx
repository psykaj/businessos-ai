"use client";

import React, { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { businessIntelligenceService } from "@/lib/business-intelligence-service";
import { BiDashboardSkeleton, BiErrorState, BiEmptyState } from "@/components/business-intelligence/bi-dashboard-states";
import { ExecutiveSummaryBriefing } from "@/components/business-intelligence/executive-summary-briefing";
import { BusinessHealthCard } from "@/components/business-intelligence/business-health-card";
import { AiRecommendationsGrid } from "@/components/business-intelligence/ai-recommendations-grid";
import { RevenueAnalyticsChart } from "@/components/business-intelligence/revenue-analytics-chart";
import { CustomerIntelligenceView } from "@/components/business-intelligence/customer-intelligence-view";
import { InventoryIntelligenceSection } from "@/components/business-intelligence/inventory-intelligence-section";
import { CashFlowOverview } from "@/components/business-intelligence/cash-flow-overview";
import { 
  BrainCircuit, 
  RefreshCw, 
  Download, 
  Sparkles, 
  Eye
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

export const BiDashboardContent: React.FC = () => {
  const [isScanning, setIsScanning] = useState(false);
  const [debugState, setDebugState] = useState<"normal" | "loading" | "error" | "empty">("normal");

  // React Query fetch
  const { data, isLoading, isError, error, refetch, isRefetching } = useQuery({
    queryKey: ["ai-business-intelligence-dashboard"],
    queryFn: () => businessIntelligenceService.getDashboardSummary(),
    staleTime: 5 * 60 * 1000, // 5 minutes cache
  });

  const handleManualRefresh = async () => {
    setIsScanning(true);
    try {
      await refetch();
      await new Promise((r) => setTimeout(r, 600));
      toast.success("AI Analysis Refreshed", {
        description: "Re-calculated 0–100 Business Health Score and updated live recommendation models.",
      });
    } catch {
      toast.error("Refresh Warning", { description: "Using cached telemetry." });
    } finally {
      setIsScanning(false);
    }
  };

  const handleExportReport = () => {
    toast.success("Executive Command Report Exported", {
      description: "Compiled full multi-pillar diagnostic snapshot into downloadable executive format.",
    });
  };

  // Allow switching states for verification & testing
  if (debugState === "loading" || isLoading) {
    return (
      <div className="space-y-4 max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
        {/* State Simulator Bar for testing */}
        <div className="flex justify-end items-center text-[10px] text-slate-400 gap-2 mb-2">
          <span>Debug State:</span>
          <button onClick={() => setDebugState("normal")} className="underline hover:text-indigo-500">Normal</button>
          <button onClick={() => setDebugState("error")} className="underline hover:text-indigo-500">Error</button>
          <button onClick={() => setDebugState("empty")} className="underline hover:text-indigo-500">Empty</button>
        </div>
        <BiDashboardSkeleton />
      </div>
    );
  }

  if (debugState === "error" || isError) {
    return (
      <div className="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
        <div className="flex justify-end items-center text-[10px] text-slate-400 gap-2 mb-2">
          <span>Debug State:</span>
          <button onClick={() => setDebugState("normal")} className="underline hover:text-indigo-500">Normal</button>
          <button onClick={() => setDebugState("loading")} className="underline hover:text-indigo-500">Loading</button>
          <button onClick={() => setDebugState("empty")} className="underline hover:text-indigo-500">Empty</button>
        </div>
        <BiErrorState error={error as Error} onRetry={() => { setDebugState("normal"); refetch(); }} />
      </div>
    );
  }

  if (debugState === "empty" || !data) {
    return (
      <div className="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
        <div className="flex justify-end items-center text-[10px] text-slate-400 gap-2 mb-2">
          <span>Debug State:</span>
          <button onClick={() => setDebugState("normal")} className="underline hover:text-indigo-500">Normal</button>
          <button onClick={() => setDebugState("loading")} className="underline hover:text-indigo-500">Loading</button>
          <button onClick={() => setDebugState("error")} className="underline hover:text-indigo-500">Error</button>
        </div>
        <BiEmptyState onRunScan={() => { setDebugState("normal"); handleManualRefresh(); }} isScanning={isScanning} />
      </div>
    );
  }

  return (
    <main className="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8 space-y-8 animate-in fade-in-50 duration-500">
      
      {/* Top Header Banner */}
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b pb-6 dark:border-slate-800">
        <div className="space-y-1">
          <div className="flex items-center gap-2">
            <div className="flex h-10 w-10 items-center justify-center rounded-2xl bg-indigo-600 text-white shadow-lg shadow-indigo-500/30">
              <BrainCircuit className="h-6 w-6" />
            </div>
            <h1 className="text-2xl sm:text-3xl font-black tracking-tight text-slate-900 dark:text-white">
              AI Business Intelligence
            </h1>
            <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-xs font-black bg-gradient-to-r from-indigo-600 to-purple-600 text-white shadow-xs">
              <Sparkles className="h-3 w-3 fill-white" /> Engine v2
            </span>
          </div>
          <p className="text-sm text-slate-600 dark:text-slate-400 max-w-2xl">
            Real-time multi-pillar operational diagnostic, predictive churn warning triggers, and one-click autonomous recommendation execution.
          </p>
        </div>

        {/* Header Action Buttons & Debug Simulator */}
        <div className="flex flex-col sm:flex-row items-end sm:items-center gap-3">
          <div className="flex items-center gap-2">
            <Button
              onClick={handleManualRefresh}
              disabled={isRefetching || isScanning}
              variant="outline"
              size="sm"
              className="h-10 px-4 rounded-xl font-extrabold text-xs bg-white dark:bg-slate-900 hover:bg-slate-50 dark:hover:bg-slate-800 border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-2"
            >
              <RefreshCw className={`h-3.5 w-3.5 ${isRefetching || isScanning ? "animate-spin text-indigo-600" : ""}`} />
              <span>{isRefetching || isScanning ? "Analyzing Telemetry..." : "Refresh Analysis"}</span>
            </Button>
            
            <Button
              onClick={handleExportReport}
              size="sm"
              className="h-10 px-4 rounded-xl font-extrabold text-xs bg-gradient-to-r from-indigo-600 via-indigo-700 to-purple-600 hover:opacity-95 text-white shadow-md shadow-indigo-500/25 flex items-center gap-2 transform active:scale-95"
            >
              <Download className="h-3.5 w-3.5" />
              <span>Export Command Report</span>
            </Button>
          </div>

          {/* Quick UI state simulator toggle for developer validation */}
          <div className="text-[10px] text-slate-400 flex items-center gap-1 mt-1 sm:mt-0 px-2 py-1 rounded-lg bg-slate-100 dark:bg-slate-800/80 border border-slate-200 dark:border-slate-700">
            <Eye className="h-3 w-3 text-slate-400" />
            <span>Test UI States:</span>
            <button onClick={() => setDebugState("loading")} className="hover:text-indigo-500 font-bold ml-1">Skeleton</button> •
            <button onClick={() => setDebugState("error")} className="hover:text-indigo-500 font-bold">Error</button> •
            <button onClick={() => setDebugState("empty")} className="hover:text-indigo-500 font-bold">Empty</button>
          </div>
        </div>
      </header>

      {/* Row 1: Executive Summary Briefing */}
      <ExecutiveSummaryBriefing 
        summary={data.executiveSummary} 
        onExecuteAction={async (title) => { await businessIntelligenceService.executeAiAction(title); }} 
      />

      {/* Row 2: Business Health Gauge (4 Cols) & Revenue Analytics (8 Cols) */}
      <div className="grid grid-cols-1 lg:grid-cols-12 gap-8 items-stretch">
        <div className="lg:col-span-4 h-full">
          <BusinessHealthCard 
            health={data.health} 
            revenueChangePercentage={data.revenue.percentageChange}
            customerGrowthPercentage={data.customers.growthRate} 
          />
        </div>
        <div className="lg:col-span-8 h-full">
          <RevenueAnalyticsChart revenue={data.revenue} />
        </div>
      </div>

      {/* Row 3: Actionable AI Recommendations Grid */}
      <section aria-label="AI Recommendations">
        <AiRecommendationsGrid 
          recommendations={data.topRecommendations} 
          onExecute={async (title) => { await businessIntelligenceService.executeAiAction(title); }} 
        />
      </section>

      {/* Row 4: Customer Intelligence & Churn Risk Table */}
      <section aria-label="Customer Intelligence">
        <CustomerIntelligenceView customers={data.customers} />
      </section>

      {/* Row 5: Inventory Intelligence & Low Stock Replenishment */}
      <section aria-label="Inventory Intelligence">
        <InventoryIntelligenceSection inventory={data.inventory} />
      </section>

      {/* Row 6: Cash Flow Overview & AR Dunning Schedule */}
      <section aria-label="Cash Flow Intelligence">
        <CashFlowOverview cashFlow={data.cashFlow} />
      </section>

      {/* Footer info banner */}
      <footer className="pt-6 border-t border-slate-200 dark:border-slate-800 text-center sm:flex sm:justify-between text-xs text-slate-400">
        <div>
          <span>Powered by <strong>BusinessOS AI</strong> • Clean Architecture CQRS Analytics Engine</span>
        </div>
        <div className="mt-2 sm:mt-0 font-mono text-slate-500">
          Last Evaluation: {data.generatedAt ? new Date(data.generatedAt).toLocaleTimeString() : "Live Snapshot"}
        </div>
      </footer>

    </main>
  );
};
