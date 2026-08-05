"use client";

import React, { useState } from "react";
import { useBranches, useLocations, useBranchPerformances, useUpdateBranchStatus } from "@/hooks/use-multi-branch";
import { BranchDto } from "@/lib/multi-branch-service";
import { BranchCard } from "@/components/branches/branch-card";
import { CreateBranchModal, AssignManagerModal, WorkingHoursModal } from "@/components/branches/branch-modals";
import { Building2, Plus, RefreshCw, DollarSign, TrendingUp, ShieldCheck, AlertTriangle, Award, Search, Filter, Layers, MapPin } from "lucide-react";
import { cn } from "@/lib/utils";

export default function BranchesHubPage() {
  const { data: branches = [], isLoading, refetch } = useBranches();
  const { data: locations = [] } = useLocations();
  const { data: performanceData } = useBranchPerformances(2026, 8);
  const statusMutation = useUpdateBranchStatus();

  const [searchQuery, setSearchQuery] = useState("");
  const [regionFilter, setRegionFilter] = useState("All");
  
  // Modals state
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [managerModalBranch, setManagerModalBranch] = useState<BranchDto | null>(null);
  const [hoursModalBranch, setHoursModalBranch] = useState<BranchDto | null>(null);

  // Executive KPI aggregations
  const totalBranches = branches.length;
  const activeBranches = branches.filter(b => b.status === "Active").length;
  const totalRevenue = branches.reduce((acc, b) => acc + b.monthlyRevenue, 0);
  const totalProfit = branches.reduce((acc, b) => acc + b.monthlyProfit, 0);
  const avgUtilization = branches.length > 0 ? Number((branches.reduce((acc, b) => acc + b.inventoryUtilization, 0) / branches.length).toFixed(1)) : 0;

  const filteredBranches = branches.filter(b => {
    const matchesSearch = b.name.toLowerCase().includes(searchQuery.toLowerCase()) || b.code.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesRegion = regionFilter === "All" || b.region === regionFilter;
    return matchesSearch && matchesRegion;
  });

  const handleToggleStatus = async (branch: BranchDto) => {
    const nextStatus = branch.status === "Active" ? "UnderMaintenance" : "Active";
    await statusMutation.mutateAsync({ id: branch.id, status: nextStatus });
  };

  return (
    <div className="p-6 md:p-8 space-y-8 max-w-7xl mx-auto animate-in fade-in-50 duration-500">
      {/* Header & Command Bar */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-border/60 pb-6">
        <div>
          <div className="flex items-center gap-2.5 text-indigo-600 dark:text-indigo-400 font-bold text-sm uppercase tracking-wider mb-1">
            <Building2 className="w-4 h-4" /> Multi-Branch Command Center
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight text-foreground">
            Enterprise Branches & Operations Hub
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Monitor distributed branch performance, manage executive accountability, and optimize multi-location inventory capacity.
          </p>
        </div>

        <div className="flex items-center gap-3">
          <button
            onClick={() => refetch()}
            disabled={isLoading}
            className="p-2.5 rounded-xl border border-border/80 hover:bg-accent text-muted-foreground hover:text-foreground transition-all flex items-center justify-center shadow-sm"
            title="Refresh Real-Time Telemetry"
          >
            <RefreshCw className={cn("w-4 h-4", isLoading ? "animate-spin text-indigo-600" : "")} />
          </button>

          <button
            onClick={() => setIsCreateOpen(true)}
            className="px-5 py-2.5 text-xs font-bold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-lg shadow-indigo-600/25 hover:shadow-indigo-600/40 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Deploy New Branch Hub
          </button>
        </div>
      </div>

      {/* 1. Executive KPI Cards Row */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-4">
        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm relative overflow-hidden group hover:border-indigo-500/30 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground text-xs font-semibold mb-2">
            <span>Total Network Hubs</span>
            <div className="p-2 rounded-xl bg-indigo-500/10 text-indigo-600"><Building2 className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">{totalBranches}</p>
          <span className="text-[11px] font-semibold text-emerald-600 mt-1 flex items-center gap-1">
            <ShieldCheck className="w-3.5 h-3.5" /> {activeBranches} Operational & Active
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm relative overflow-hidden group hover:border-emerald-500/30 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground text-xs font-semibold mb-2">
            <span>Monthly Branch Revenue</span>
            <div className="p-2 rounded-xl bg-emerald-500/10 text-emerald-600"><DollarSign className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">${totalRevenue.toLocaleString()}</p>
          <span className="text-[11px] font-semibold text-emerald-600 mt-1 block">
            +8.4% MoM Consolidated Growth
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm relative overflow-hidden group hover:border-indigo-500/30 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground text-xs font-semibold mb-2">
            <span>Net Branch Profitability</span>
            <div className="p-2 rounded-xl bg-indigo-500/10 text-indigo-600"><TrendingUp className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">${totalProfit.toLocaleString()}</p>
          <span className="text-[11px] font-semibold text-indigo-600 mt-1 block">
            {totalRevenue > 0 ? ((totalProfit / totalRevenue) * 100).toFixed(1) : 0}% Avg Operational Margin
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm relative overflow-hidden group hover:border-blue-500/30 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground text-xs font-semibold mb-2">
            <span>Avg Stock Utilization</span>
            <div className="p-2 rounded-xl bg-blue-500/10 text-blue-600"><Layers className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">{avgUtilization}%</p>
          <div className="w-full h-1.5 bg-secondary rounded-full mt-2 overflow-hidden">
            <div className="h-full bg-blue-600 rounded-full" style={{ width: `${avgUtilization}%` }} />
          </div>
        </div>

        <div className="p-5 rounded-2xl bg-gradient-to-br from-indigo-900 via-indigo-800 to-slate-900 text-white shadow-md relative overflow-hidden flex flex-col justify-between">
          <div className="flex items-center justify-between text-indigo-200 text-xs font-semibold">
            <span>Top Performance Hub</span>
            <Award className="w-4 h-4 text-amber-400 animate-bounce" />
          </div>
          <p className="text-sm font-bold text-white mt-1 truncate">
            {performanceData?.bestPerformingBranchName || "Manhattan Enterprise Hub"}
          </p>
          <p className="text-[10px] text-indigo-200 mt-1">
            Rank #1 by composite profit & growth score.
          </p>
        </div>
      </div>

      {/* 2. Branch Ranking & Leaderboard Banner */}
      {performanceData?.performances && (
        <div className="p-6 rounded-2xl bg-card border border-border/60 shadow-sm space-y-4">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
            <div>
              <h3 className="text-base font-bold text-foreground flex items-center gap-2">
                <Award className="w-5 h-5 text-indigo-600" /> Executive Branch Performance Ranking (August 2026)
              </h3>
              <p className="text-xs text-muted-foreground mt-0.5">
                Composite assessment combining Net Profit Margin (40%), Capacity Utilization (30%), and MoM Revenue Momentum (30%).
              </p>
            </div>
            <span className="text-xs font-mono px-3 py-1 bg-accent rounded-lg text-muted-foreground self-start sm:self-auto">
              {performanceData.performances.length} Hubs Scored
            </span>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-4 pt-2">
            {performanceData.performances.slice(0, 3).map((p) => (
              <div key={p.id} className="p-4 rounded-xl bg-accent/30 border border-border/50 flex items-center gap-4">
                <div className={cn(
                  "w-10 h-10 rounded-xl font-mono font-black text-sm flex items-center justify-center shrink-0 shadow-sm",
                  p.rank === 1 ? "bg-amber-500 text-white shadow-amber-500/20" :
                  p.rank === 2 ? "bg-slate-300 text-slate-900 dark:bg-slate-700 dark:text-white" :
                  "bg-amber-800/80 text-amber-100"
                )}>
                  #{p.rank}
                </div>
                <div className="overflow-hidden flex-1">
                  <p className="font-bold text-sm truncate text-foreground">{p.branchName}</p>
                  <p className="text-xs text-muted-foreground font-mono">${p.totalRevenue.toLocaleString()} Rev | {p.profitMarginPercentage}% Margin</p>
                </div>
                <div className="text-right shrink-0 font-mono">
                  <p className="text-sm font-black text-indigo-600 dark:text-indigo-400">{p.performanceScore}</p>
                  <span className="text-[10px] text-muted-foreground uppercase">Score</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* 3. Filter & Search Bar */}
      <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-2">
        <div className="flex items-center gap-2 overflow-x-auto w-full sm:w-auto pb-2 sm:pb-0">
          {["All", "North America", "EMEA", "APAC"].map((reg) => (
            <button
              key={reg}
              onClick={() => setRegionFilter(reg)}
              className={cn(
                "px-4 py-2 rounded-xl text-xs font-semibold transition-all shrink-0",
                regionFilter === reg
                  ? "bg-indigo-600 text-white shadow-md shadow-indigo-600/20"
                  : "bg-accent/60 text-muted-foreground hover:text-foreground hover:bg-accent"
              )}
            >
              {reg === "All" ? "All Global Territories" : reg}
            </button>
          ))}
        </div>

        <div className="relative w-full sm:w-72">
          <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
          <input
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            placeholder="Search branch hubs or codes..."
            className="w-full pl-10 pr-4 py-2 rounded-xl border border-border/80 bg-card text-xs text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-indigo-500/50"
          />
        </div>
      </div>

      {/* 4. Branch Cards Grid */}
      {isLoading ? (
        <div className="py-24 text-center space-y-3">
          <RefreshCw className="w-8 h-8 animate-spin text-indigo-600 mx-auto" />
          <p className="text-sm text-muted-foreground font-medium">Synchronizing distributed branch telemetry...</p>
        </div>
      ) : filteredBranches.length === 0 ? (
        <div className="py-16 text-center rounded-2xl bg-accent/20 border border-dashed border-border/80 p-8">
          <Building2 className="w-10 h-10 text-muted-foreground mx-auto mb-3 opacity-40" />
          <h3 className="text-base font-bold">No matching branch hubs found</h3>
          <p className="text-xs text-muted-foreground mt-1 max-w-sm mx-auto">
            We couldn&apos;t locate any branch locations matching &quot;{searchQuery}&quot; in the selected regional territory.
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredBranches.map((branch) => (
            <BranchCard
              key={branch.id}
              branch={branch}
              onAssignManager={(b) => setManagerModalBranch(b)}
              onConfigHours={(b) => setHoursModalBranch(b)}
              onToggleStatus={handleToggleStatus}
            />
          ))}
        </div>
      )}

      {/* Modals */}
      <CreateBranchModal isOpen={isCreateOpen} onClose={() => setIsCreateOpen(false)} locations={locations} />
      <AssignManagerModal isOpen={!!managerModalBranch} onClose={() => setManagerModalBranch(null)} branch={managerModalBranch} />
      <WorkingHoursModal isOpen={!!hoursModalBranch} onClose={() => setHoursModalBranch(null)} branch={hoursModalBranch} />
    </div>
  );
}
