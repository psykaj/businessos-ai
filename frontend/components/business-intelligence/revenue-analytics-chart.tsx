"use client";

import React, { useState } from "react";
import { RevenueInsightDto } from "@/lib/business-intelligence-service";
import { 
  ResponsiveContainer, 
  AreaChart, 
  Area, 
  BarChart, 
  Bar, 
  XAxis, 
  YAxis, 
  Tooltip, 
  Legend,
  CartesianGrid 
} from "recharts";
import { 
  TrendingUp, 
  TrendingDown, 
  BarChart3, 
  Sparkles, 
  Repeat, 
  Layers
} from "lucide-react";
import { cn } from "@/lib/utils";

interface RevenueAnalyticsChartProps {
  revenue: RevenueInsightDto;
}

export const RevenueAnalyticsChart: React.FC<RevenueAnalyticsChartProps> = ({ revenue }) => {
  const [timeHorizon, setTimeHorizon] = useState<"Daily" | "Weekly" | "Monthly">("Weekly");
  const [chartType, setChartType] = useState<"Area" | "Bar">("Area");

  // Choose appropriate data points based on horizon toggle
  const getData = () => {
    switch (timeHorizon) {
      case "Daily":
        return revenue.dailyTrends || revenue.revenueTrends;
      case "Monthly":
        return revenue.monthlyTrends || revenue.revenueTrends;
      default:
        return revenue.revenueTrends;
    }
  };

  const chartData = getData();
  const isPositive = revenue.percentageChange >= 0;

  return (
    <div className="h-full rounded-3xl bg-white dark:bg-slate-900 border border-slate-200/80 dark:border-slate-800 shadow-xl p-6 flex flex-col justify-between transition-all duration-300">
      
      {/* Top Header: Title, Metric KPIs & Horizon Switcher */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 pb-6 border-b border-slate-100 dark:border-slate-800/80">
        <div>
          <div className="flex items-center gap-2">
            <div className="p-2 rounded-xl bg-emerald-500/10 text-emerald-600 dark:text-emerald-400">
              <BarChart3 className="h-5 w-5" />
            </div>
            <div>
              <h3 className="text-lg font-bold text-slate-900 dark:text-white">
                Revenue Intelligence & Trends
              </h3>
              <p className="text-xs text-slate-500 dark:text-slate-400">
                Multi-period velocity vs comparative historical baselines
              </p>
            </div>
          </div>

          {/* Key KPI inline snapshot */}
          <div className="mt-4 flex flex-wrap items-baseline gap-4">
            <div>
              <span className="text-2xl font-black font-mono text-slate-900 dark:text-white">
                ${revenue.currentRevenue.toLocaleString()}
              </span>
              <span className="text-xs text-slate-400 ml-1.5 uppercase font-bold">Current Period</span>
            </div>
            <div className={`inline-flex items-center gap-1 text-xs font-extrabold px-2 py-1 rounded-full ${
              isPositive ? "bg-emerald-500/10 text-emerald-500 border border-emerald-500/20" : "bg-red-500/10 text-red-500 border border-red-500/20"
            }`}>
              {isPositive ? <TrendingUp className="h-3.5 w-3.5" /> : <TrendingDown className="h-3.5 w-3.5" />}
              <span>{isPositive ? "+" : ""}{revenue.percentageChange}% vs Prior Period</span>
            </div>
            <div className="inline-flex items-center gap-1 text-xs font-bold px-2 py-1 rounded-full bg-purple-500/10 text-purple-600 dark:text-purple-400 border border-purple-500/20">
              <Repeat className="h-3.5 w-3.5" />
              <span>{revenue.repeatCustomerRevenuePercentage}% Repeat Customer Rate</span>
            </div>
          </div>
        </div>

        {/* Control Toggles: Horizon & Chart Type */}
        <div className="flex flex-col sm:flex-row items-end sm:items-center gap-2">
          {/* Daily / Weekly / Monthly Switch */}
          <div className="inline-flex items-center p-1 rounded-2xl bg-slate-100 dark:bg-slate-800/60 border border-slate-200 dark:border-slate-700">
            {(["Daily", "Weekly", "Monthly"] as const).map((horizon) => (
              <button
                key={horizon}
                onClick={() => setTimeHorizon(horizon)}
                className={cn(
                  "px-3 py-1 rounded-xl text-xs font-extrabold transition-all duration-200",
                  timeHorizon === horizon 
                    ? "bg-white dark:bg-indigo-600 text-slate-900 dark:text-white shadow-md shadow-indigo-500/10" 
                    : "text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-white"
                )}
              >
                {horizon}
              </button>
            ))}
          </div>

          {/* Chart Type Toggle */}
          <button
            onClick={() => setChartType(chartType === "Area" ? "Bar" : "Area")}
            title={`Switch to ${chartType === "Area" ? "Bar" : "Area"} Chart`}
            className="p-2 rounded-xl bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 dark:hover:bg-slate-700 text-slate-700 dark:text-slate-300 text-xs font-bold transition-all border border-slate-200 dark:border-slate-700 flex items-center gap-1"
          >
            <Layers className="h-4 w-4" />
            <span>{chartType}</span>
          </button>
        </div>
      </div>

      {/* Recharts Main Chart Container */}
      <div className="my-6 h-72 w-full">
        <ResponsiveContainer width="100%" height="100%">
          {chartType === "Area" ? (
            <AreaChart data={chartData} margin={{ top: 10, right: 10, left: 10, bottom: 0 }}>
              <defs>
                <linearGradient id="colorCurrent" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#10B981" stopOpacity={0.6} />
                  <stop offset="95%" stopColor="#10B981" stopOpacity={0.0} />
                </linearGradient>
                <linearGradient id="colorPrevious" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#6366F1" stopOpacity={0.3} />
                  <stop offset="95%" stopColor="#6366F1" stopOpacity={0.0} />
                </linearGradient>
              </defs>
              <CartesianGrid strokeDasharray="3 3" stroke="#33415520" vertical={false} />
              <XAxis 
                dataKey="period" 
                stroke="#64748B" 
                fontSize={12} 
                tickLine={false} 
                axisLine={false} 
              />
              <YAxis 
                stroke="#64748B" 
                fontSize={12} 
                tickLine={false} 
                axisLine={false} 
                tickFormatter={(value) => `$${value >= 1000 ? `${Math.round(value / 1000)}k` : value}`}
              />
              <Tooltip
                contentStyle={{ 
                  backgroundColor: "rgba(15, 23, 42, 0.95)", 
                  borderColor: "rgba(99, 102, 241, 0.3)",
                  borderRadius: "12px",
                  color: "#fff",
                  fontSize: "12px",
                  boxShadow: "0 20px 25px -5px rgba(0, 0, 0, 0.3)" 
                }}
                formatter={(value: unknown, name: unknown) => [
                  `$${Number(Array.isArray(value) ? value[0] : (value || 0)).toLocaleString()}`, 
                  String(name) === "amount" ? "Current Revenue" : "Comparison Baseline"
                ]}
              />
              <Legend verticalAlign="top" height={36} formatter={(val) => val === "amount" ? "Current Revenue ($)" : "Comparison Baseline ($)"} />
              <Area 
                type="monotone" 
                dataKey="comparisonAmount" 
                stroke="#6366F1" 
                strokeWidth={2}
                strokeDasharray="4 4"
                fillOpacity={1} 
                fill="url(#colorPrevious)" 
                name="comparisonAmount"
              />
              <Area 
                type="monotone" 
                dataKey="amount" 
                stroke="#10B981" 
                strokeWidth={3} 
                fillOpacity={1} 
                fill="url(#colorCurrent)" 
                name="amount"
              />
            </AreaChart>
          ) : (
            <BarChart data={chartData} margin={{ top: 10, right: 10, left: 10, bottom: 0 }}>
              <CartesianGrid strokeDasharray="3 3" stroke="#33415520" vertical={false} />
              <XAxis dataKey="period" stroke="#64748B" fontSize={12} tickLine={false} axisLine={false} />
              <YAxis 
                stroke="#64748B" 
                fontSize={12} 
                tickLine={false} 
                axisLine={false} 
                tickFormatter={(value) => `$${value >= 1000 ? `${Math.round(value / 1000)}k` : value}`}
              />
              <Tooltip
                contentStyle={{ 
                  backgroundColor: "rgba(15, 23, 42, 0.95)", 
                  borderColor: "rgba(99, 102, 241, 0.3)",
                  borderRadius: "12px",
                  color: "#fff",
                  fontSize: "12px",
                  boxShadow: "0 20px 25px -5px rgba(0, 0, 0, 0.3)" 
                }}
                formatter={(value: unknown, name: unknown) => [
                  `$${Number(Array.isArray(value) ? value[0] : (value || 0)).toLocaleString()}`, 
                  String(name) === "amount" ? "Current Revenue" : "Comparison Baseline"
                ]}
              />
              <Legend verticalAlign="top" height={36} formatter={(val) => val === "amount" ? "Current Revenue ($)" : "Comparison Baseline ($)"} />
              <Bar dataKey="comparisonAmount" fill="#6366F1" opacity={0.5} radius={[6, 6, 0, 0]} name="comparisonAmount" />
              <Bar dataKey="amount" fill="#10B981" radius={[6, 6, 0, 0]} name="amount" />
            </BarChart>
          )}
        </ResponsiveContainer>
      </div>

      {/* AI Primary Driver Explanation Footer */}
      <div className="p-4 rounded-2xl bg-gradient-to-r from-emerald-500/10 via-slate-900/40 to-indigo-500/10 border border-emerald-500/20 text-xs text-slate-700 dark:text-slate-300 flex items-start gap-3">
        <Sparkles className="h-5 w-5 text-emerald-500 shrink-0 mt-0.5 animate-pulse" />
        <div className="space-y-1">
          <span className="text-[11px] font-extrabold text-emerald-600 dark:text-emerald-400 uppercase tracking-wider block">
            AI Revenue Driver Analysis
          </span>
          <p className="leading-relaxed text-slate-600 dark:text-slate-300 font-medium">
            {revenue.primaryDriverExplanation}
          </p>
        </div>
      </div>

    </div>
  );
};
