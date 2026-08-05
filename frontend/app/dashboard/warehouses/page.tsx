"use client";

import React, { useState } from "react";
import { useBranchWarehouses, useBranches, useCreateBranchWarehouse } from "@/hooks/use-multi-branch";
import { BranchWarehouseDto } from "@/lib/multi-branch-service";
import { Building, Plus, RefreshCw, Layers, DollarSign, MapPin, User, Phone, Sparkles, AlertTriangle, CheckCircle2, ArrowRightLeft } from "lucide-react";
import { cn } from "@/lib/utils";
import Link from "next/link";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

const warehouseSchema = z.object({
  name: z.string().min(3, "Warehouse facility name required"),
  code: z.string().min(2, "Code required (e.g. WH-USA-A)"),
  branchId: z.string().min(1, "Assigned Branch required"),
  storageCapacitySqFt: z.number().min(500, "Minimum 500 SqFt required"),
  contactPerson: z.string().min(2, "Facility Manager required"),
  contactPhone: z.string().optional(),
  isPrimary: z.boolean(),
});

export default function MultiBranchWarehousesPage() {
  const { data: warehouses = [], isLoading, refetch } = useBranchWarehouses();
  const { data: branches = [] } = useBranches();
  const createMutation = useCreateBranchWarehouse();
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedBranchFilter, setSelectedBranchFilter] = useState("All");

  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<z.infer<typeof warehouseSchema>>({
    resolver: zodResolver(warehouseSchema),
    defaultValues: { storageCapacitySqFt: 75000, isPrimary: false }
  });

  const totalCapacitySqFt = warehouses.reduce((acc, w) => acc + w.storageCapacitySqFt, 0);
  const totalStockCount = warehouses.reduce((acc, w) => acc + w.totalStockItemsCount, 0);
  const totalStockValue = warehouses.reduce((acc, w) => acc + w.estimatedStockValue, 0);
  const avgUtilization = warehouses.length > 0 ? Number((warehouses.reduce((acc, w) => acc + w.currentUtilizationPercentage, 0) / warehouses.length).toFixed(1)) : 0;

  const filteredWarehouses = selectedBranchFilter === "All"
    ? warehouses
    : warehouses.filter(w => w.branchId === selectedBranchFilter);

  const onSubmit = async (data: z.infer<typeof warehouseSchema>) => {
    await createMutation.mutateAsync({
      name: data.name,
      code: data.code,
      branchId: data.branchId,
      storageCapacitySqFt: Number(data.storageCapacitySqFt),
      contactPerson: data.contactPerson,
      contactPhone: data.contactPhone,
      isPrimary: data.isPrimary,
      status: "Operational",
    });
    reset();
    setIsModalOpen(false);
  };

  return (
    <div className="p-6 md:p-8 space-y-8 max-w-7xl mx-auto animate-in fade-in-50 duration-500">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-border/60 pb-6">
        <div>
          <div className="flex items-center gap-2 text-indigo-600 dark:text-indigo-400 font-bold text-sm uppercase tracking-wider mb-1">
            <Building className="w-4 h-4" /> Multi-Branch Storage & Logistics Network
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight text-foreground">
            Multi-Branch Warehouses & Facilities
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Manage physical storage capacity (SqFt), track real-time inventory valuations, and coordinate regional fulfillment centers.
          </p>
        </div>

        <div className="flex items-center gap-3">
          <Link
            href="/dashboard/transfers"
            className="px-4 py-2.5 rounded-xl border border-border/80 hover:bg-accent text-xs font-bold transition-all flex items-center gap-2 shadow-sm text-foreground"
          >
            <ArrowRightLeft className="w-4 h-4 text-indigo-500" /> Inter-Warehouse Transfers
          </Link>
          <button
            onClick={() => refetch()}
            disabled={isLoading}
            className="p-2.5 rounded-xl border border-border/80 hover:bg-accent text-muted-foreground hover:text-foreground transition-all"
          >
            <RefreshCw className={cn("w-4 h-4", isLoading ? "animate-spin text-indigo-600" : "")} />
          </button>
          <button
            onClick={() => setIsModalOpen(true)}
            className="px-5 py-2.5 text-xs font-bold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-lg shadow-indigo-600/25 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Register Facility
          </button>
        </div>
      </div>

      {/* KPI Row */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm">
          <div className="flex items-center justify-between text-xs font-semibold text-muted-foreground mb-2">
            <span>Total Network Storage</span>
            <div className="p-2 rounded-xl bg-indigo-500/10 text-indigo-600"><Building className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">{totalCapacitySqFt.toLocaleString()} <span className="text-sm font-normal text-muted-foreground">SqFt</span></p>
          <span className="text-[11px] text-emerald-600 font-semibold mt-1 flex items-center gap-1">
            <CheckCircle2 className="w-3.5 h-3.5" /> {warehouses.length} Active Fulfillment Centers
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm">
          <div className="flex items-center justify-between text-xs font-semibold text-muted-foreground mb-2">
            <span>Total Stock Units Held</span>
            <div className="p-2 rounded-xl bg-blue-500/10 text-blue-600"><Layers className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">{totalStockCount.toLocaleString()} <span className="text-sm font-normal text-muted-foreground">Items</span></p>
          <p className="text-[11px] text-muted-foreground mt-1">Real-time SKU tally across branches</p>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm">
          <div className="flex items-center justify-between text-xs font-semibold text-muted-foreground mb-2">
            <span>Consolidated Stock Valuation</span>
            <div className="p-2 rounded-xl bg-emerald-500/10 text-emerald-600"><DollarSign className="w-4 h-4" /></div>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">${totalStockValue.toLocaleString()}</p>
          <span className="text-[11px] text-emerald-600 font-semibold mt-1 block">
            Automated insurance & balance sheet sync
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm">
          <div className="flex items-center justify-between text-xs font-semibold text-muted-foreground mb-2">
            <span>Avg Facility Utilization</span>
            <span className="font-mono text-xs font-bold text-indigo-600">{avgUtilization}%</span>
          </div>
          <p className="text-2xl font-black text-foreground font-mono">{avgUtilization}% <span className="text-sm font-normal text-muted-foreground">Full</span></p>
          <div className="w-full h-1.5 bg-secondary rounded-full mt-2 overflow-hidden">
            <div className={cn("h-full rounded-full", avgUtilization > 85 ? "bg-amber-500" : "bg-indigo-600")} style={{ width: `${avgUtilization}%` }} />
          </div>
        </div>
      </div>

      {/* Branch Filter Tabs */}
      <div className="flex items-center gap-2 overflow-x-auto border-b border-border/40 pb-4">
        <button
          onClick={() => setSelectedBranchFilter("All")}
          className={cn(
            "px-4 py-2 rounded-xl text-xs font-bold transition-all shrink-0",
            selectedBranchFilter === "All" ? "bg-indigo-600 text-white shadow-md shadow-indigo-600/20" : "bg-accent/50 text-muted-foreground hover:text-foreground"
          )}
        >
          All Corporate Facilities
        </button>
        {branches.map((b) => (
          <button
            key={b.id}
            onClick={() => setSelectedBranchFilter(b.id)}
            className={cn(
              "px-4 py-2 rounded-xl text-xs font-bold transition-all shrink-0 flex items-center gap-1.5",
              selectedBranchFilter === b.id ? "bg-indigo-600 text-white shadow-md shadow-indigo-600/20" : "bg-accent/50 text-muted-foreground hover:text-foreground"
            )}
          >
            {b.name}
          </button>
        ))}
      </div>

      {/* Warehouse Grid */}
      {isLoading ? (
        <div className="py-24 text-center space-y-3">
          <RefreshCw className="w-8 h-8 animate-spin text-indigo-600 mx-auto" />
          <p className="text-sm text-muted-foreground font-medium">Synchronizing warehouse storage metrics...</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredWarehouses.map((wh) => {
            const isNearFull = wh.currentUtilizationPercentage >= 85;
            return (
              <div key={wh.id} className="group relative flex flex-col justify-between p-6 rounded-2xl bg-card border border-border/60 shadow-sm hover:shadow-lg hover:border-indigo-500/40 transition-all duration-300 overflow-hidden">
                {wh.isPrimary && (
                  <div className="absolute top-0 right-0 px-3 py-1 bg-indigo-600 text-white font-semibold text-[10px] uppercase tracking-wider rounded-bl-xl shadow-sm flex items-center gap-1">
                    <Sparkles className="w-3 h-3 animate-pulse" /> Primary Hub Depot
                  </div>
                )}

                <div>
                  <div className="flex items-center gap-2 mb-2">
                    <span className={cn(
                      "px-2.5 py-0.5 rounded-full text-[11px] font-bold border flex items-center gap-1.5",
                      wh.status === "Operational" ? "bg-emerald-500/10 text-emerald-600 border-emerald-500/20" :
                      wh.status === "Maintenance" ? "bg-amber-500/10 text-amber-600 border-amber-500/20" :
                      "bg-red-500/10 text-red-600 border-red-500/20"
                    )}>
                      <span className="w-1.5 h-1.5 rounded-full bg-emerald-500" />
                      {wh.status}
                    </span>
                    <span className="text-[11px] font-mono font-bold px-2 py-0.5 border border-border rounded text-muted-foreground">
                      {wh.code}
                    </span>
                  </div>

                  <h3 className="text-lg font-bold text-foreground group-hover:text-indigo-600 transition-colors">
                    {wh.name}
                  </h3>

                  <p className="text-xs font-semibold text-muted-foreground mt-1 mb-4 flex items-center gap-1">
                    <MapPin className="w-3.5 h-3.5 text-indigo-500" /> {wh.branchName || "Assigned Branch"}
                  </p>

                  <div className="p-3.5 rounded-xl bg-accent/30 border border-border/50 grid grid-cols-2 gap-3 mb-4">
                    <div>
                      <span className="text-[11px] text-muted-foreground font-medium block">Total Stock Valuation</span>
                      <span className="text-base font-black text-foreground font-mono">${wh.estimatedStockValue.toLocaleString()}</span>
                    </div>
                    <div>
                      <span className="text-[11px] text-muted-foreground font-medium block">Stock Items Count</span>
                      <span className="text-base font-black text-foreground font-mono">{wh.totalStockItemsCount.toLocaleString()} <span className="text-xs font-normal text-muted-foreground">units</span></span>
                    </div>
                  </div>

                  {/* Utilization Bar */}
                  <div className="space-y-1.5 mb-5">
                    <div className="flex items-center justify-between text-xs">
                      <span className="font-semibold text-muted-foreground flex items-center gap-1">
                        Storage Capacity ({wh.storageCapacitySqFt.toLocaleString()} SqFt)
                      </span>
                      <span className={cn("font-mono font-black text-xs", isNearFull ? "text-amber-500" : "text-foreground")}>
                        {wh.currentUtilizationPercentage}% Used
                      </span>
                    </div>
                    <div className="w-full h-2 bg-secondary rounded-full overflow-hidden">
                      <div className={cn("h-full rounded-full transition-all duration-500", isNearFull ? "bg-amber-500" : "bg-indigo-600")} style={{ width: `${Math.min(100, wh.currentUtilizationPercentage)}%` }} />
                    </div>
                    {isNearFull && (
                      <p className="text-[11px] text-amber-600 font-medium flex items-center gap-1 pt-0.5">
                        <AlertTriangle className="w-3 h-3" /> Nearing facility capacity limits. Initiate inter-warehouse stock redistribution.
                      </p>
                    )}
                  </div>
                </div>

                <div className="pt-3 border-t border-border/50 flex items-center justify-between text-xs text-muted-foreground">
                  <span className="flex items-center gap-1 font-medium text-foreground">
                    <User className="w-3.5 h-3.5 text-indigo-500" /> {wh.contactPerson || "Facility Ops Lead"}
                  </span>
                  <span className="font-mono text-[11px]">{wh.contactPhone}</span>
                </div>
              </div>
            );
          })}
        </div>
      )}

      {/* Modal */}
      {isModalOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
          <div className="relative w-full max-w-lg rounded-2xl bg-card border border-border shadow-2xl p-6 space-y-4">
            <div className="flex items-center justify-between border-b border-border/50 pb-3">
              <div className="flex items-center gap-2 text-indigo-600 font-bold text-lg">
                <Building className="w-5 h-5" /> Register Branch Warehouse Depot
              </div>
              <button onClick={() => setIsModalOpen(false)} className="p-1 text-muted-foreground hover:text-foreground">✕</button>
            </div>

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 text-sm">
              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="font-semibold block mb-1 text-xs">Facility Name *</label>
                  <input {...register("name")} placeholder="e.g. Austin Regional Fulfillment Center" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
                  {errors.name && <p className="text-red-500 text-[10px] mt-1">{errors.name.message}</p>}
                </div>
                <div>
                  <label className="font-semibold block mb-1 text-xs">Facility Code *</label>
                  <input {...register("code")} placeholder="e.g. WH-USA-AUS-A" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-mono" />
                  {errors.code && <p className="text-red-500 text-[10px] mt-1">{errors.code.message}</p>}
                </div>
              </div>

              <div>
                <label className="font-semibold block mb-1 text-xs">Assign to Corporate Branch Hub *</label>
                <select {...register("branchId")} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-medium">
                  <option value="">-- Select Parent Branch --</option>
                  {branches.map(b => (
                    <option key={b.id} value={b.id}>{b.name} ({b.code})</option>
                  ))}
                </select>
                {errors.branchId && <p className="text-red-500 text-[10px] mt-1">{errors.branchId.message}</p>}
              </div>

              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="font-semibold block mb-1 text-xs">Storage Capacity (SqFt) *</label>
                  <input type="number" {...register("storageCapacitySqFt", { valueAsNumber: true })} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-mono font-bold" />
                </div>
                <div>
                  <label className="font-semibold block mb-1 text-xs">Facility Ops Lead Name *</label>
                  <input {...register("contactPerson")} placeholder="e.g. Rachel Vance" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
                </div>
              </div>

              <div className="flex items-center gap-2 p-3 rounded-xl bg-accent/40 border border-border/40 text-xs">
                <input type="checkbox" id="isPri" {...register("isPrimary")} className="w-4 h-4 rounded text-indigo-600" />
                <label htmlFor="isPri" className="font-medium cursor-pointer">Flag as Primary Branch Logistics Hub</label>
              </div>

              <div className="flex justify-end gap-3 pt-3 border-t border-border/50">
                <button type="button" onClick={() => setIsModalOpen(false)} className="px-4 py-2 rounded-xl border border-border text-xs font-semibold">Cancel</button>
                <button type="submit" disabled={isSubmitting} className="px-5 py-2 rounded-xl bg-indigo-600 text-white font-semibold text-xs hover:bg-indigo-700 shadow-md">
                  Register Facility
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
