"use client";

import React, { useState } from "react";
import { useRegionalSummaries, useBranchPerformances, useBranches } from "@/hooks/use-multi-branch";
import { BarChart, Bar, LineChart, Line, PieChart, Pie, Cell, XAxis, YAxis, Tooltip, ResponsiveContainer, Legend, AreaChart, Area } from "recharts";
import { BarChart3, TrendingUp, DollarSign, Users, Layers, Award, ShieldCheck, RefreshCw, Globe, ArrowUpRight, Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

const COLORS = ["#6366F1", "#10B981", "#F59E0B", "#3B82F6", "#EC4899", "#8B5CF6"];

export default function RegionalAnalyticsDashboardPage() {
  const { data: regionalData = [], isLoading: regLoading, refetch: refetchReg } = useRegionalSummaries();
  const { data: performanceData, isLoading: perfLoading, refetch: refetchPerf } = useBranchPerformances(2026, 8);
  const { data: branches = [] } = useBranches();

  const [selectedPeriod, setSelectedPeriod] = useState("Q3-2026");
  const isLoading = regLoading || perfLoading;

  const totalRevenue = regionalData.reduce((acc, r) => acc + r.totalRevenue, 0);
  const totalProfit = regionalData.reduce((acc, r) => acc + r.totalProfit, 0);
  const totalCustomers = regionalData.reduce((acc, r) => acc + r.totalCustomers, 0);
  const totalEmployees = regionalData.reduce((acc, r) => acc + r.totalEmployees, 0);
  const overallMargin = totalRevenue > 0 ? Number(((totalProfit / totalRevenue) * 100).toFixed(1)) : 0;

  const handleRefresh = () => {
    refetchReg();
    refetchPerf();
  };

  // Prepare chart datasets
  const revenueVsProfitChartData = regionalData.map(r => ({
    region: r.region,
    Revenue: r.totalRevenue,
    Profit: r.totalProfit,
    Margin: r.profitMargin,
  }));

  const branchPerformanceChartData = performanceData?.performances?.map(p => ({
    name: p.branchName.split(" ")[0], // short name
    fullName: p.branchName,
    Revenue: p.totalRevenue,
    Profit: p.totalProfit,
    Growth: p.moMRevenueGrowthPercentage,
    Score: p.performanceScore,
  })) || [];

  const customerDistributionData = regionalData.map(r => ({
    name: r.region,
    value: r.totalCustomers,
    revenuePerEmployee: r.revenuePerEmployee
  }));

  return (
    <div className="p-6 md:p-8 space-y-8 max-w-7xl mx-auto animate-in fade-in-50 duration-500">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-border/60 pb-6">
        <div>
          <div className="flex items-center gap-2 text-indigo-600 dark:text-indigo-400 font-bold text-sm uppercase tracking-wider mb-1">
            <BarChart3 className="w-4 h-4" /> Consolidated Executive Intelligence
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight text-foreground">
            Regional Analytics & Branch Growth
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Analyze consolidated revenue trends, compare regional profitability margins, and optimize customer distribution yield.
          </p>
        </div>

        <div className="flex items-center gap-3">
          <select
            value={selectedPeriod}
            onChange={(e) => setSelectedPeriod(e.target.value)}
            className="px-4 py-2.5 rounded-xl border border-border bg-card text-xs font-bold text-foreground focus:outline-none focus:ring-2 focus:ring-indigo-500 shadow-sm"
          >
            <option value="Q3-2026">Current Quarter (Q3 2026)</option>
            <option value="Q2-2026">Previous Quarter (Q2 2026)</option>
            <option value="YTD-2026">Year to Date (YTD 2026)</option>
          </select>

          <button
            onClick={handleRefresh}
            disabled={isLoading}
            className="p-2.5 rounded-xl border border-border/80 hover:bg-accent text-muted-foreground hover:text-foreground transition-all flex items-center justify-center shadow-sm"
            title="Refresh AI Analytical Aggregator"
          >
            <RefreshCw className={cn("w-4 h-4", isLoading ? "animate-spin text-indigo-600" : "")} />
          </button>
        </div>
      </div>

      {/* 1. Executive KPI Strip */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm relative overflow-hidden group hover:border-indigo-500/40 transition-all">
          <div className="flex items-center justify-between text-xs font-semibold text-muted-foreground mb-2">
            <span>Global Consolidated Revenue</span>
            <div className="p-2 rounded-xl bg-indigo-500/10 text-indigo-600"><DollarSign className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">${totalRevenue.toLocaleString()}</p>
          <span className="text-[11px] font-semibold text-emerald-600 mt-1 flex items-center gap-1">
            <ArrowUpRight className="w-3.5 h-3.5" /> +9.2% vs Previous Quarter
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm relative overflow-hidden group hover:border-emerald-500/40 transition-all">
          <div className="flex items-center justify-between text-xs font-semibold text-muted-foreground mb-2">
            <span>Net Regional Profitability</span>
            <div className="p-2 rounded-xl bg-emerald-500/10 text-emerald-600"><TrendingUp className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">${totalProfit.toLocaleString()}</p>
          <span className="text-[11px] font-bold text-emerald-600 mt-1 block">
            {overallMargin}% Net Profit Margin
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm relative overflow-hidden">
          <div className="flex items-center justify-between text-xs font-semibold text-muted-foreground mb-2">
            <span>Active Enterprise Customers</span>
            <div className="p-2 rounded-xl bg-blue-500/10 text-blue-600"><Users className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">{totalCustomers.toLocaleString()}</p>
          <span className="text-[11px] text-muted-foreground font-medium mt-1 block">
            Distributed across {regionalData.length} global territories
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-gradient-to-br from-slate-900 via-indigo-950 to-indigo-900 text-white shadow-md relative overflow-hidden flex flex-col justify-between">
          <div className="flex items-center justify-between text-indigo-200 text-xs font-semibold">
            <span>Workforce Productivity</span>
            <Sparkles className="w-4 h-4 text-amber-400" />
          </div>
          <p className="text-2xl font-black text-white font-mono mt-1">
            ${totalEmployees > 0 ? Math.floor(totalRevenue / totalEmployees).toLocaleString() : "24,500"} <span className="text-xs font-normal text-indigo-200">/ employee</span>
          </p>
          <p className="text-[11px] text-indigo-200 mt-1">
            {totalEmployees} corporate employees network-wide
          </p>
        </div>
      </div>

      {/* 2. Charts Row: Revenue & Profit Comparison vs Customer Distribution */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Bar Chart: Regional Revenue & Profit */}
        <div className="lg:col-span-2 p-6 rounded-2xl bg-card border border-border/60 shadow-sm space-y-4 flex flex-col justify-between">
          <div className="flex items-center justify-between">
            <div>
              <h3 className="text-base font-bold text-foreground flex items-center gap-2">
                <BarChart3 className="w-4 h-4 text-indigo-600" /> Regional Revenue vs. Profit Comparison
              </h3>
              <p className="text-xs text-muted-foreground">Gross fiscal performance grouped by continental operations.</p>
            </div>
            <span className="text-[11px] font-mono font-semibold px-2.5 py-1 bg-indigo-500/10 text-indigo-600 rounded-lg border border-indigo-500/20">
              USD ($)
            </span>
          </div>

          <div className="h-[300px] w-full pt-4">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={revenueVsProfitChartData} margin={{ top: 10, right: 10, left: 15, bottom: 0 }}>
                <XAxis dataKey="region" stroke="#888888" fontSize={12} tickLine={false} axisLine={false} />
                <YAxis stroke="#888888" fontSize={12} tickLine={false} axisLine={false} tickFormatter={(val) => `$${val / 1000}k`} />
                <Tooltip
                  contentStyle={{ backgroundColor: "rgba(15, 23, 42, 0.9)", borderRadius: "12px", border: "1px solid rgba(255,255,255,0.1)", color: "#fff", fontSize: "12px", padding: "10px" }}
                  formatter={(val: any) => [`$${Number(val || 0).toLocaleString()}`, "Amount"]}
                />
                <Legend iconType="circle" wrapperStyle={{ fontSize: "12px", paddingTop: "10px" }} />
                <Bar dataKey="Revenue" fill="#6366F1" radius={[6, 6, 0, 0]} barSize={36} />
                <Bar dataKey="Profit" fill="#10B981" radius={[6, 6, 0, 0]} barSize={36} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Pie Chart: Customer Distribution */}
        <div className="p-6 rounded-2xl bg-card border border-border/60 shadow-sm space-y-4 flex flex-col justify-between">
          <div>
            <h3 className="text-base font-bold text-foreground flex items-center gap-2">
              <Globe className="w-4 h-4 text-blue-500" /> Customer Distribution
            </h3>
            <p className="text-xs text-muted-foreground">Active client proportion per region.</p>
          </div>

          <div className="h-[240px] w-full flex items-center justify-center">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={customerDistributionData}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={90}
                  paddingAngle={5}
                  dataKey="value"
                >
                  {customerDistributionData.map((entry, idx) => (
                    <Cell key={`cell-${idx}`} fill={COLORS[idx % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip
                  contentStyle={{ backgroundColor: "rgba(15, 23, 42, 0.9)", borderRadius: "12px", border: "none", color: "#fff", fontSize: "12px" }}
                  formatter={(val: any) => [Number(val || 0).toLocaleString(), "Active Customers"]}
                />
              </PieChart>
            </ResponsiveContainer>
          </div>

          <div className="space-y-2 pt-2 border-t border-border/50 text-xs">
            {customerDistributionData.map((reg, idx) => (
              <div key={reg.name} className="flex items-center justify-between">
                <span className="flex items-center gap-2 font-semibold text-foreground">
                  <span className="w-2.5 h-2.5 rounded-full" style={{ backgroundColor: COLORS[idx % COLORS.length] }} />
                  {reg.name}
                </span>
                <span className="font-mono text-muted-foreground font-bold">{reg.value} clients ({totalCustomers > 0 ? ((reg.value / totalCustomers) * 100).toFixed(0) : 0}%)</span>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* 3. Branch Growth & Performance Trajectory */}
      <div className="p-6 rounded-2xl bg-card border border-border/60 shadow-sm space-y-4">
        <div className="flex items-center justify-between">
          <div>
            <h3 className="text-base font-bold text-foreground flex items-center gap-2">
              <TrendingUp className="w-5 h-5 text-indigo-600" /> Branch Revenue Growth Momentum & Performance Score
            </h3>
            <p className="text-xs text-muted-foreground">Month-over-Month revenue growth (%) vs. automated composite performance score (0-100).</p>
          </div>
        </div>

        <div className="h-[280px] w-full pt-2">
          <ResponsiveContainer width="100%" height="100%">
            <LineChart data={branchPerformanceChartData} margin={{ top: 10, right: 20, left: 10, bottom: 0 }}>
              <XAxis dataKey="name" stroke="#888888" fontSize={12} tickLine={false} axisLine={false} />
              <YAxis yAxisId="left" stroke="#888888" fontSize={12} tickLine={false} axisLine={false} unit="%" />
              <YAxis yAxisId="right" orientation="right" stroke="#888888" fontSize={12} tickLine={false} axisLine={false} domain={[50, 100]} />
              <Tooltip
                contentStyle={{ backgroundColor: "rgba(15, 23, 42, 0.9)", borderRadius: "12px", border: "1px solid rgba(255,255,255,0.1)", color: "#fff", fontSize: "12px" }}
              />
              <Legend iconType="circle" wrapperStyle={{ fontSize: "12px", paddingTop: "10px" }} />
              <Line yAxisId="left" name="MoM Revenue Growth (%)" type="monotone" dataKey="Growth" stroke="#10B981" strokeWidth={3} dot={{ r: 6 }} activeDot={{ r: 8 }} />
              <Line yAxisId="right" name="AI Performance Score (0-100)" type="monotone" dataKey="Score" stroke="#6366F1" strokeWidth={3} dot={{ r: 6 }} activeDot={{ r: 8 }} />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* 4. Regional Territory Deep-Dive Table */}
      <div className="p-6 rounded-2xl bg-card border border-border/60 shadow-sm space-y-4">
        <h3 className="text-base font-bold text-foreground">Consolidated Territory Audit Table</h3>
        <div className="overflow-x-auto">
          <table className="w-full text-left text-xs text-muted-foreground">
            <thead className="bg-accent/50 text-foreground text-xs uppercase font-extrabold border-b border-border">
              <tr>
                <th className="py-3 px-4 rounded-tl-xl">Regional Territory</th>
                <th className="py-3 px-4">Branches</th>
                <th className="py-3 px-4">Total Revenue</th>
                <th className="py-3 px-4">Net Profit Margin</th>
                <th className="py-3 px-4">Stock Valuation</th>
                <th className="py-3 px-4">Workforce Yield</th>
                <th className="py-3 px-4 rounded-tr-xl">Top Champion Branch</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border/50 text-sm font-medium text-foreground">
              {regionalData.map((reg) => (
                <tr key={reg.id} className="hover:bg-accent/40 transition-colors">
                  <td className="py-3.5 px-4 font-bold flex items-center gap-2 text-indigo-600 dark:text-indigo-400">
                    <Globe className="w-4 h-4" /> {reg.region}
                  </td>
                  <td className="py-3.5 px-4 font-mono">{reg.totalBranches} Hubs</td>
                  <td className="py-3.5 px-4 font-mono font-extrabold">${reg.totalRevenue.toLocaleString()}</td>
                  <td className="py-3.5 px-4 font-mono text-emerald-600 font-bold">${reg.totalProfit.toLocaleString()} ({reg.profitMargin}%)</td>
                  <td className="py-3.5 px-4 font-mono">${reg.totalInventoryValue.toLocaleString()}</td>
                  <td className="py-3.5 px-4 font-mono">${reg.revenuePerEmployee.toLocaleString()} / emp</td>
                  <td className="py-3.5 px-4 font-bold text-amber-600 flex items-center gap-1">
                    <Award className="w-4 h-4" /> {reg.topPerformingBranchName}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
