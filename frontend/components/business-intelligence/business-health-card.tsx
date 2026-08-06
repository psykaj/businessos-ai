"use client";

import React from "react";
import { BusinessHealthDto } from "@/lib/business-intelligence-service";
import { 
  ShieldCheck, 
  TrendingUp, 
  AlertCircle, 
  HelpCircle, 
  CheckCircle, 
  AlertTriangle, 
  DollarSign,
  Users,
  Wallet,
  Package,
  Receipt,
  HeartHandshake
} from "lucide-react";
import { cn } from "@/lib/utils";

interface BusinessHealthCardProps {
  health: BusinessHealthDto;
  revenueChangePercentage?: number;
  customerGrowthPercentage?: number;
}

export const BusinessHealthCard: React.FC<BusinessHealthCardProps> = ({
  health,
  revenueChangePercentage = 12.5,
  customerGrowthPercentage = 8.4,
}) => {
  const { score, status, explanation, dimensionScores, dimensionExplanations } = health;

  // Determine vibrant color theme based on score threshold
  const getStatusTheme = (scoreVal: number) => {
    if (scoreVal >= 90) {
      return {
        strokeColor: "#10B981", // Emerald-500
        gradientFrom: "from-emerald-500/20",
        badgeBg: "bg-emerald-500/10 text-emerald-400 border-emerald-500/30",
        glow: "shadow-emerald-500/30",
        textColor: "text-emerald-500",
        icon: <CheckCircle className="h-4 w-4 text-emerald-400" />,
      };
    } else if (scoreVal >= 75) {
      return {
        strokeColor: "#3B82F6", // Blue-500 / Indigo
        gradientFrom: "from-blue-500/20",
        badgeBg: "bg-blue-500/10 text-blue-400 border-blue-500/30",
        glow: "shadow-blue-500/30",
        textColor: "text-blue-500",
        icon: <ShieldCheck className="h-4 w-4 text-blue-400" />,
      };
    } else if (scoreVal >= 60) {
      return {
        strokeColor: "#F59E0B", // Amber-500
        gradientFrom: "from-amber-500/20",
        badgeBg: "bg-amber-500/10 text-amber-400 border-amber-500/30",
        glow: "shadow-amber-500/30",
        textColor: "text-amber-500",
        icon: <AlertTriangle className="h-4 w-4 text-amber-400" />,
      };
    } else {
      return {
        strokeColor: "#EF4444", // Red-500
        gradientFrom: "from-red-500/20",
        badgeBg: "bg-red-500/10 text-red-400 border-red-500/30",
        glow: "shadow-red-500/30",
        textColor: "text-red-500",
        icon: <AlertCircle className="h-4 w-4 text-red-400" />,
      };
    }
  };

  const theme = getStatusTheme(score);

  // SVG Circular progress mathematics (viewBox 0 0 160 160)
  const radius = 64;
  const circumference = 2 * Math.PI * radius;
  const strokeDashoffset = circumference - (score / 100) * circumference;

  // Icons mapping for the 6 core pillars
  const pillarIcons: Record<string, React.ReactNode> = {
    "Revenue": <DollarSign className="h-4 w-4 text-emerald-400" />,
    "Customer Growth": <Users className="h-4 w-4 text-blue-400" />,
    "Cash Flow": <Wallet className="h-4 w-4 text-purple-400" />,
    "Inventory": <Package className="h-4 w-4 text-amber-400" />,
    "Pending Invoices": <Receipt className="h-4 w-4 text-red-400" />,
    "Customer Satisfaction": <HeartHandshake className="h-4 w-4 text-indigo-400" />,
  };

  return (
    <div className="h-full rounded-3xl bg-white dark:bg-slate-900 border border-slate-200/80 dark:border-slate-800 shadow-xl p-6 flex flex-col justify-between transition-all duration-300">
      {/* Title & Status Badge */}
      <div className="flex items-center justify-between pb-4 border-b border-slate-100 dark:border-slate-800/80">
        <div>
          <h3 className="text-lg font-bold text-slate-900 dark:text-slate-100 flex items-center gap-2">
            <ShieldCheck className="h-5 w-5 text-indigo-500" />
            Business Health Score
          </h3>
          <p className="text-xs text-slate-500 dark:text-slate-400">0–100 Multi-Pillar Index</p>
        </div>
        <span className={cn("inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold border shadow-sm uppercase tracking-wide", theme.badgeBg)}>
          {theme.icon}
          {status}
        </span>
      </div>

      {/* Large Circular Gauge & Core Trends */}
      <div className="my-6 flex flex-col items-center justify-center relative">
        <div className="relative flex items-center justify-center">
          <svg className="w-48 h-48 -rotate-90 transform drop-shadow-xl" viewBox="0 0 160 160">
            {/* Background circle */}
            <circle
              cx="80"
              cy="80"
              r={radius}
              className="text-slate-100 dark:text-slate-800"
              strokeWidth="12"
              stroke="currentColor"
              fill="transparent"
            />
            {/* Animated progress gauge */}
            <circle
              cx="80"
              cy="80"
              r={radius}
              strokeWidth="12"
              strokeDasharray={circumference}
              strokeDashoffset={strokeDashoffset}
              strokeLinecap="round"
              stroke={theme.strokeColor}
              fill="transparent"
              className="transition-all duration-1000 ease-out"
            />
          </svg>
          {/* Inner Score Content */}
          <div className="absolute flex flex-col items-center justify-center text-center">
            <span className="text-4xl font-extrabold tracking-tight text-slate-900 dark:text-white font-mono">
              {score}
            </span>
            <span className="text-[11px] font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400 mt-0.5">
              Out of 100
            </span>
          </div>
        </div>

        {/* Quick Revenue & Growth trend chips */}
        <div className="mt-6 flex flex-wrap items-center justify-center gap-3 w-full">
          <div className="flex items-center gap-2 px-3 py-1.5 rounded-xl bg-slate-50 dark:bg-slate-800/80 border border-slate-200/60 dark:border-slate-700/60 text-xs font-semibold text-slate-700 dark:text-slate-300">
            <TrendingUp className="h-3.5 w-3.5 text-emerald-500" />
            <span>Revenue Trend:</span>
            <span className={revenueChangePercentage >= 0 ? "text-emerald-500 font-bold" : "text-red-500 font-bold"}>
              {revenueChangePercentage >= 0 ? "+" : ""}{revenueChangePercentage}%
            </span>
          </div>
          <div className="flex items-center gap-2 px-3 py-1.5 rounded-xl bg-slate-50 dark:bg-slate-800/80 border border-slate-200/60 dark:border-slate-700/60 text-xs font-semibold text-slate-700 dark:text-slate-300">
            <Users className="h-3.5 w-3.5 text-blue-500" />
            <span>Customer Growth:</span>
            <span className={customerGrowthPercentage >= 0 ? "text-blue-500 font-bold" : "text-red-500 font-bold"}>
              {customerGrowthPercentage >= 0 ? "+" : ""}{customerGrowthPercentage}%
            </span>
          </div>
        </div>
      </div>

      {/* AI Diagnostic Explanation Box */}
      <div className="p-4 rounded-2xl bg-indigo-500/5 dark:bg-indigo-950/20 border border-indigo-500/20 text-xs text-slate-700 dark:text-slate-300 space-y-2 leading-relaxed mb-6">
        <div className="flex items-center justify-between font-bold text-indigo-600 dark:text-indigo-400 uppercase tracking-wider text-[11px]">
          <span>AI Health Diagnostic</span>
          <span>Powered by BusinessOS AI</span>
        </div>
        <p className="line-clamp-3 hover:line-clamp-none transition-all cursor-default text-slate-600 dark:text-slate-300">
          {explanation}
        </p>
      </div>

      {/* 6-Pillar Dimension Scores Breakdown Grid */}
      <div className="space-y-3 pt-2 border-t border-slate-100 dark:border-slate-800/80">
        <h4 className="text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">
          6-Pillar Diagnostic Evaluation
        </h4>
        <div className="grid grid-cols-2 sm:grid-cols-3 gap-2.5">
          {Object.entries(dimensionScores || {}).map(([pillar, val]) => {
            const pillarText = dimensionExplanations?.[pillar] || "Score computed from real-time operational telemetry.";
            const icon = pillarIcons[pillar] || <HelpCircle className="h-4 w-4 text-slate-400" />;

            return (
              <div
                key={pillar}
                title={`${pillar}: ${pillarText}`}
                className="group relative p-2.5 rounded-xl bg-slate-50 dark:bg-slate-800/60 border border-slate-200/60 dark:border-slate-800 hover:border-indigo-500/40 transition-all cursor-help flex flex-col justify-between space-y-1.5"
              >
                <div className="flex items-center justify-between">
                  <span className="text-[11px] font-bold text-slate-800 dark:text-slate-200 flex items-center gap-1.5 line-clamp-1">
                    {icon} {pillar}
                  </span>
                </div>
                <div className="flex items-end justify-between">
                  <div className="w-full bg-slate-200 dark:bg-slate-700 h-1.5 rounded-full overflow-hidden mr-2">
                    <div
                      className={cn("h-full rounded-full transition-all duration-500", val >= 80 ? "bg-emerald-500" : val >= 65 ? "bg-blue-500" : val >= 50 ? "bg-amber-500" : "bg-red-500")}
                      style={{ width: `${val}%` }}
                    ></div>
                  </div>
                  <span className="text-xs font-extrabold font-mono text-slate-900 dark:text-white shrink-0">
                    {val}
                  </span>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};
