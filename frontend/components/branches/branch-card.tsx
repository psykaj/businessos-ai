"use client";

import React from "react";
import { BranchDto } from "@/lib/multi-branch-service";
import { MapPin, User, Clock, Settings, TrendingUp, DollarSign, ShieldCheck, ShieldAlert, ChevronRight, Sparkles, AlertCircle } from "lucide-react";
import { cn } from "@/lib/utils";

interface BranchCardProps {
  branch: BranchDto;
  onAssignManager: (branch: BranchDto) => void;
  onConfigHours: (branch: BranchDto) => void;
  onToggleStatus: (branch: BranchDto) => void;
}

export function BranchCard({ branch, onAssignManager, onConfigHours, onToggleStatus }: BranchCardProps) {
  const isMaintenance = branch.status === "UnderMaintenance";
  const isClosed = branch.status === "Inactive" || branch.status === "PermanentlyClosed";

  return (
    <div className={cn(
      "group relative flex flex-col justify-between p-6 rounded-2xl bg-card border border-border/60 shadow-sm hover:shadow-lg hover:border-indigo-500/40 transition-all duration-300 overflow-hidden",
      isMaintenance ? "border-amber-500/40 bg-amber-500/5" : "",
      isClosed ? "opacity-75 border-red-500/30 bg-red-500/5" : ""
    )}>
      {/* Primary Badge Ribbon */}
      {branch.isPrimary && (
        <div className="absolute top-0 right-0 px-3 py-1 bg-indigo-600 text-white font-semibold text-[10px] uppercase tracking-wider rounded-bl-xl shadow-sm flex items-center gap-1">
          <Sparkles className="w-3 h-3 animate-pulse" /> Primary HQ
        </div>
      )}

      {/* Top Section */}
      <div>
        <div className="flex items-center gap-2 mb-2">
          <span className={cn(
            "px-2.5 py-0.5 rounded-full text-xs font-medium border flex items-center gap-1.5",
            branch.status === "Active" ? "bg-emerald-500/10 text-emerald-600 border-emerald-500/20" :
            branch.status === "UnderMaintenance" ? "bg-amber-500/10 text-amber-600 border-amber-500/20" :
            "bg-red-500/10 text-red-600 border-red-500/20"
          )}>
            <span className={cn(
              "w-1.5 h-1.5 rounded-full",
              branch.status === "Active" ? "bg-emerald-500" : branch.status === "UnderMaintenance" ? "bg-amber-500" : "bg-red-500"
            )} />
            {branch.status}
          </span>
          <span className="text-xs text-muted-foreground font-mono font-medium border border-border px-2 py-0.5 rounded-md">
            {branch.code}
          </span>
        </div>

        <h3 className="text-lg font-bold text-foreground tracking-tight group-hover:text-indigo-600 transition-colors">
          {branch.name}
        </h3>

        <div className="flex items-center gap-1.5 text-sm text-muted-foreground mt-1 mb-4">
          <MapPin className="w-4 h-4 text-indigo-500 shrink-0" />
          <span className="truncate font-medium">{branch.locationName || "Global Territory"}</span>
          <span className="text-xs px-2 py-0.5 bg-accent/60 rounded-md ml-auto text-foreground/80">{branch.region || "Region"}</span>
        </div>

        {/* Manager Info */}
        <div className="p-3 rounded-xl bg-accent/40 border border-border/40 flex items-center justify-between gap-3 mb-5">
          <div className="flex items-center gap-2.5 overflow-hidden">
            <div className="w-8 h-8 rounded-full bg-indigo-500/10 border border-indigo-500/20 flex items-center justify-center text-indigo-600 font-bold text-xs shrink-0">
              {branch.managerName ? branch.managerName.charAt(0) : <User className="w-4 h-4" />}
            </div>
            <div className="truncate">
              <p className="text-xs font-semibold text-foreground truncate">
                {branch.managerName || "Unassigned Manager"}
              </p>
              <p className="text-[11px] text-muted-foreground truncate">
                {branch.managerEmail || "No email provided"}
              </p>
            </div>
          </div>
          <button
            onClick={() => onAssignManager(branch)}
            className="text-[11px] font-semibold text-indigo-600 hover:text-indigo-700 hover:underline shrink-0"
          >
            {branch.managerName ? "Reassign" : "Assign +"}
          </button>
        </div>

        {/* Operational KPI Grid */}
        <div className="grid grid-cols-2 gap-3 mb-5">
          <div className="p-3 rounded-xl bg-card border border-border/50">
            <span className="text-[11px] font-medium text-muted-foreground flex items-center gap-1">
              <DollarSign className="w-3.5 h-3.5 text-emerald-500" /> Monthly Revenue
            </span>
            <p className="text-lg font-bold text-foreground mt-0.5 font-mono">
              ${branch.monthlyRevenue.toLocaleString()}
            </p>
            <span className="text-[11px] font-semibold text-emerald-600">
              +{branch.profitMargin}% Margin
            </span>
          </div>
          <div className="p-3 rounded-xl bg-card border border-border/50">
            <span className="text-[11px] font-medium text-muted-foreground flex items-center gap-1">
              <TrendingUp className="w-3.5 h-3.5 text-blue-500" /> Stock Utilization
            </span>
            <div className="flex items-end justify-between mt-0.5">
              <span className="text-lg font-bold text-foreground font-mono">
                {branch.inventoryUtilization}%
              </span>
              <span className="text-[10px] text-muted-foreground font-medium mb-0.5">Capacity</span>
            </div>
            {/* Progress Bar */}
            <div className="w-full h-1.5 bg-secondary rounded-full mt-1.5 overflow-hidden">
              <div
                className={cn(
                  "h-full rounded-full transition-all duration-500",
                  branch.inventoryUtilization > 85 ? "bg-amber-500" : "bg-indigo-600"
                )}
                style={{ width: `${Math.min(100, branch.inventoryUtilization)}%` }}
              />
            </div>
          </div>
        </div>
      </div>

      {/* Footer Action Bar */}
      <div className="pt-3 border-t border-border/50 flex items-center justify-between text-xs font-semibold">
        <button
          onClick={() => onConfigHours(branch)}
          className="flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent transition-colors"
        >
          <Clock className="w-3.5 h-3.5" /> Working Hours
        </button>

        <button
          onClick={() => onToggleStatus(branch)}
          className={cn(
            "px-3 py-1.5 rounded-lg transition-colors flex items-center gap-1",
            branch.status === "Active" ? "text-amber-600 hover:bg-amber-500/10" : "text-emerald-600 hover:bg-emerald-500/10"
          )}
        >
          {branch.status === "Active" ? "Set Maintenance" : "Activate Branch"}
          <ChevronRight className="w-3 h-3" />
        </button>
      </div>
    </div>
  );
}
