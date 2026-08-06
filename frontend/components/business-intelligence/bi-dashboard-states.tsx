"use client";

import React from "react";
import { AlertTriangle, BrainCircuit, RefreshCw, Sparkles, DatabaseZap } from "lucide-react";
import { Button } from "@/components/ui/button";

export const BiDashboardSkeleton: React.FC = () => {
  return (
    <div className="space-y-8 p-1 animate-pulse" aria-label="Loading AI Business Intelligence Dashboard">
      {/* Top Banner & Header Skeleton */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b pb-6 dark:border-slate-800">
        <div className="space-y-2">
          <div className="h-8 w-72 bg-slate-200 dark:bg-slate-800 rounded-lg"></div>
          <div className="h-4 w-96 max-w-full bg-slate-100 dark:bg-slate-800/60 rounded"></div>
        </div>
        <div className="flex gap-3">
          <div className="h-10 w-32 bg-slate-200 dark:bg-slate-800 rounded-lg"></div>
          <div className="h-10 w-36 bg-slate-300 dark:bg-slate-700 rounded-lg"></div>
        </div>
      </div>

      {/* Row 1: Executive Summary Briefing Skeleton (Full Width) */}
      <div className="h-64 rounded-2xl bg-gradient-to-r from-slate-100 to-slate-50 dark:from-slate-900/80 dark:to-slate-900/40 border dark:border-slate-800/80 p-6">
        <div className="flex justify-between items-center mb-6">
          <div className="h-6 w-48 bg-slate-200 dark:bg-slate-800 rounded"></div>
          <div className="h-6 w-24 bg-slate-200 dark:bg-slate-800 rounded-full"></div>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="h-36 rounded-xl bg-slate-200/60 dark:bg-slate-800/50"></div>
          <div className="h-36 rounded-xl bg-slate-200/60 dark:bg-slate-800/50"></div>
          <div className="h-36 rounded-xl bg-slate-200/60 dark:bg-slate-800/50"></div>
        </div>
      </div>

      {/* Row 2: Health Score & Revenue Trends Skeleton */}
      <div className="grid grid-cols-1 lg:grid-cols-12 gap-8">
        {/* Left: Health Score (4 Cols) */}
        <div className="lg:col-span-4 h-96 rounded-2xl bg-slate-100 dark:bg-slate-900/60 border dark:border-slate-800 p-6 flex flex-col items-center justify-center space-y-6">
          <div className="h-40 w-40 rounded-full border-8 border-slate-200 dark:border-slate-800 flex items-center justify-center">
            <div className="h-12 w-16 bg-slate-200 dark:bg-slate-800 rounded"></div>
          </div>
          <div className="h-6 w-40 bg-slate-200 dark:bg-slate-800 rounded"></div>
          <div className="w-full space-y-2">
            <div className="h-3 w-full bg-slate-200 dark:bg-slate-800 rounded"></div>
            <div className="h-3 w-5/6 bg-slate-200 dark:bg-slate-800 rounded"></div>
          </div>
        </div>

        {/* Right: Revenue Analytics Chart (8 Cols) */}
        <div className="lg:col-span-8 h-96 rounded-2xl bg-slate-100 dark:bg-slate-900/60 border dark:border-slate-800 p-6 flex flex-col justify-between">
          <div className="flex justify-between items-center">
            <div className="h-6 w-44 bg-slate-200 dark:bg-slate-800 rounded"></div>
            <div className="flex gap-2">
              <div className="h-8 w-16 bg-slate-200 dark:bg-slate-800 rounded-md"></div>
              <div className="h-8 w-16 bg-slate-200 dark:bg-slate-800 rounded-md"></div>
              <div className="h-8 w-16 bg-slate-200 dark:bg-slate-800 rounded-md"></div>
            </div>
          </div>
          <div className="h-56 w-full bg-slate-200/50 dark:bg-slate-800/40 rounded-xl mt-4"></div>
        </div>
      </div>

      {/* Row 3: AI Recommendations Skeleton Grid */}
      <div className="space-y-4">
        <div className="flex justify-between items-center">
          <div className="h-7 w-60 bg-slate-200 dark:bg-slate-800 rounded"></div>
          <div className="h-6 w-32 bg-slate-200 dark:bg-slate-800 rounded-full"></div>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <div key={i} className="h-64 rounded-2xl bg-slate-100 dark:bg-slate-900/50 border dark:border-slate-800/80 p-5 flex flex-col justify-between">
              <div className="space-y-3">
                <div className="flex justify-between">
                  <div className="h-5 w-20 bg-slate-200 dark:bg-slate-800 rounded"></div>
                  <div className="h-5 w-16 bg-slate-200 dark:bg-slate-800 rounded-full"></div>
                </div>
                <div className="h-6 w-3/4 bg-slate-200 dark:bg-slate-800 rounded"></div>
                <div className="h-12 w-full bg-slate-200/60 dark:bg-slate-800/60 rounded"></div>
              </div>
              <div className="h-10 w-full bg-slate-200 dark:bg-slate-800 rounded-lg"></div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

interface BiErrorStateProps {
  error?: Error | string;
  onRetry: () => void;
}

export const BiErrorState: React.FC<BiErrorStateProps> = ({ error, onRetry }) => {
  const errorMessage = typeof error === "string" ? error : error?.message || "An unexpected network timeout occurred while evaluating enterprise telemetry.";
  return (
    <div className="min-h-[500px] flex items-center justify-center p-6">
      <div className="max-w-md w-full rounded-3xl bg-red-500/5 dark:bg-red-950/20 border border-red-500/20 p-8 text-center backdrop-blur-xl shadow-2xl">
        <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-2xl bg-red-500/10 text-red-500 mb-6 shadow-inner">
          <AlertTriangle className="h-8 w-8 animate-bounce" />
        </div>
        <h3 className="text-xl font-bold tracking-tight text-slate-900 dark:text-slate-100">
          AI Evaluation Engine Paused
        </h3>
        <p className="mt-2 text-sm text-slate-600 dark:text-slate-400 leading-relaxed">
          {errorMessage}
        </p>
        <div className="mt-6 p-3 rounded-xl bg-slate-100 dark:bg-slate-900/80 border border-slate-200 dark:border-slate-800 text-xs text-left text-slate-500 dark:text-slate-400 font-mono overflow-x-auto">
          Diagnostic: Failed to sync multi-layer CQRS specifications and live memory caches.
        </div>
        <Button
          onClick={onRetry}
          className="mt-6 w-full bg-red-600 hover:bg-red-500 text-white font-medium py-3 rounded-xl shadow-lg shadow-red-500/25 transition-all duration-200 flex items-center justify-center gap-2"
        >
          <RefreshCw className="h-4 w-4" />
          Retry AI Data Evaluation
        </Button>
      </div>
    </div>
  );
};

interface BiEmptyStateProps {
  onRunScan: () => void;
  isScanning?: boolean;
}

export const BiEmptyState: React.FC<BiEmptyStateProps> = ({ onRunScan, isScanning = false }) => {
  return (
    <div className="min-h-[550px] flex items-center justify-center p-6">
      <div className="max-w-lg w-full rounded-3xl bg-gradient-to-b from-indigo-500/10 via-slate-900/40 to-slate-900/80 border border-indigo-500/20 p-10 text-center backdrop-blur-2xl shadow-2xl">
        <div className="mx-auto flex h-20 w-20 items-center justify-center rounded-3xl bg-gradient-to-tr from-indigo-600 to-violet-600 text-white mb-6 shadow-xl shadow-indigo-500/30 ring-4 ring-indigo-500/20 animate-pulse">
          <BrainCircuit className="h-10 w-10" />
        </div>
        <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-indigo-500/10 border border-indigo-500/30 text-indigo-400 text-xs font-semibold uppercase tracking-wider mb-3">
          <Sparkles className="h-3.5 w-3.5" /> Ready For First Evaluation
        </span>
        <h3 className="text-2xl font-bold tracking-tight text-slate-900 dark:text-slate-100">
          No Business Intelligence Data Yet
        </h3>
        <p className="mt-3 text-sm text-slate-600 dark:text-slate-300 leading-relaxed">
          BusinessOS AI has connected to your operational database. Trigger your initial deep-dive diagnostic scan to compute your 0–100 Business Health Score, synthesize churn risks, and generate actionable recommendations.
        </p>
        <div className="mt-8">
          <Button
            onClick={onRunScan}
            disabled={isScanning}
            className="w-full bg-gradient-to-r from-indigo-600 via-purple-600 to-indigo-500 hover:opacity-95 text-white font-semibold py-6 rounded-2xl shadow-xl shadow-indigo-500/30 transition-all duration-300 transform hover:-translate-y-0.5 flex items-center justify-center gap-3 text-base"
          >
            {isScanning ? (
              <>
                <RefreshCw className="h-5 w-5 animate-spin" />
                Analyzing 6 Operational Pillars...
              </>
            ) : (
              <>
                <DatabaseZap className="h-5 w-5" />
                Trigger Deep-Dive AI Diagnostic Scan
              </>
            )}
          </Button>
        </div>
      </div>
    </div>
  );
};
