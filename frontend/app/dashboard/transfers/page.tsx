"use client";

import React, { useState } from "react";
import { useTransfers, useBranchWarehouses } from "@/hooks/use-multi-branch";
import { WarehouseTransferDto, TransferStatus } from "@/lib/multi-branch-service";
import { CreateTransferModal, ApproveTransferModal, ReceiveTransferModal } from "@/components/branches/transfer-modals";
import { ArrowRightLeft, Plus, RefreshCw, Truck, ShieldCheck, CheckCircle2, XCircle, Clock, Search, Filter, Package, AlertTriangle } from "lucide-react";
import { cn } from "@/lib/utils";

export default function InventoryTransfersPage() {
  const { data: transfers = [], isLoading, refetch } = useTransfers();
  const { data: warehouses = [] } = useBranchWarehouses();

  const [statusFilter, setStatusFilter] = useState<string>("All");
  const [searchQuery, setSearchQuery] = useState("");

  // Modal control states
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [selectedForApproval, setSelectedForApproval] = useState<WarehouseTransferDto | null>(null);
  const [selectedForReceipt, setSelectedForReceipt] = useState<WarehouseTransferDto | null>(null);

  const pendingCount = transfers.filter(t => t.status === "Pending").length;
  const inTransitCount = transfers.filter(t => t.status === "InTransit" || t.status === "Approved").length;
  const receivedCount = transfers.filter(t => t.status === "Received").length;
  const totalValuationInTransit = transfers.filter(t => t.status === "InTransit" || t.status === "Approved" || t.status === "Pending").reduce((acc, t) => acc + t.totalTransferValue, 0);

  const filteredTransfers = transfers.filter(t => {
    const matchesStatus = statusFilter === "All" || t.status === statusFilter || (statusFilter === "InTransit" && t.status === "Approved");
    const matchesSearch = t.transferNumber.toLowerCase().includes(searchQuery.toLowerCase()) ||
      t.sourceWarehouseName.toLowerCase().includes(searchQuery.toLowerCase()) ||
      t.destinationWarehouseName.toLowerCase().includes(searchQuery.toLowerCase());
    return matchesStatus && matchesSearch;
  });

  const getStatusBadge = (status: TransferStatus) => {
    switch (status) {
      case "Pending":
        return <span className="px-2.5 py-1 rounded-full text-xs font-bold bg-amber-500/10 text-amber-600 border border-amber-500/20 flex items-center gap-1"><Clock className="w-3.5 h-3.5" /> Awaiting Approval</span>;
      case "Approved":
      case "InTransit":
        return <span className="px-2.5 py-1 rounded-full text-xs font-bold bg-blue-500/10 text-blue-600 border border-blue-500/20 flex items-center gap-1"><Truck className="w-3.5 h-3.5 animate-pulse" /> In Transit</span>;
      case "Received":
        return <span className="px-2.5 py-1 rounded-full text-xs font-bold bg-emerald-500/10 text-emerald-600 border border-emerald-500/20 flex items-center gap-1"><CheckCircle2 className="w-3.5 h-3.5" /> Completed & Received</span>;
      case "Rejected":
        return <span className="px-2.5 py-1 rounded-full text-xs font-bold bg-red-500/10 text-red-600 border border-red-500/20 flex items-center gap-1"><XCircle className="w-3.5 h-3.5" /> Rejected</span>;
      default:
        return <span className="px-2.5 py-1 rounded-full text-xs font-semibold bg-secondary text-muted-foreground">{status}</span>;
    }
  };

  return (
    <div className="p-6 md:p-8 space-y-8 max-w-7xl mx-auto animate-in fade-in-50 duration-500">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-border/60 pb-6">
        <div>
          <div className="flex items-center gap-2 text-indigo-600 dark:text-indigo-400 font-bold text-sm uppercase tracking-wider mb-1">
            <ArrowRightLeft className="w-4 h-4" /> Multi-Location Stock Redistribution
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight text-foreground">
            Inter-Warehouse Inventory Transfers
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Manage stock dispatch orders, enforce executive financial approval thresholds, and verify live shipment induction.
          </p>
        </div>

        <div className="flex items-center gap-3">
          <button
            onClick={() => refetch()}
            disabled={isLoading}
            className="p-2.5 rounded-xl border border-border/80 hover:bg-accent text-muted-foreground hover:text-foreground transition-all"
          >
            <RefreshCw className={cn("w-4 h-4", isLoading ? "animate-spin text-indigo-600" : "")} />
          </button>

          <button
            onClick={() => setIsCreateOpen(true)}
            className="px-5 py-2.5 text-xs font-bold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-lg shadow-indigo-600/25 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Initiate Transfer Order
          </button>
        </div>
      </div>

      {/* KPI Cards Row */}
      <div className="grid grid-cols-1 sm:grid-cols-4 gap-4">
        <div className="p-5 rounded-2xl bg-amber-500/5 border border-amber-500/20 shadow-sm flex items-center justify-between">
          <div>
            <span className="text-xs font-bold text-amber-700 dark:text-amber-400">Pending Executive Review</span>
            <p className="text-2xl font-black text-amber-600 font-mono mt-1">{pendingCount} <span className="text-xs font-normal text-muted-foreground">orders</span></p>
          </div>
          <div className="p-3 rounded-xl bg-amber-500/10 text-amber-600"><Clock className="w-6 h-6" /></div>
        </div>

        <div className="p-5 rounded-2xl bg-blue-500/5 border border-blue-500/20 shadow-sm flex items-center justify-between">
          <div>
            <span className="text-xs font-bold text-blue-700 dark:text-blue-400">Shipments In Transit</span>
            <p className="text-2xl font-black text-blue-600 font-mono mt-1">{inTransitCount} <span className="text-xs font-normal text-muted-foreground">orders</span></p>
          </div>
          <div className="p-3 rounded-xl bg-blue-500/10 text-blue-600"><Truck className="w-6 h-6 animate-bounce" /></div>
        </div>

        <div className="p-5 rounded-2xl bg-emerald-500/5 border border-emerald-500/20 shadow-sm flex items-center justify-between">
          <div>
            <span className="text-xs font-bold text-emerald-700 dark:text-emerald-400">Completed Receipts (Mtd)</span>
            <p className="text-2xl font-black text-emerald-600 font-mono mt-1">{receivedCount} <span className="text-xs font-normal text-muted-foreground">orders</span></p>
          </div>
          <div className="p-3 rounded-xl bg-emerald-500/10 text-emerald-600"><CheckCircle2 className="w-6 h-6" /></div>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm flex items-center justify-between">
          <div>
            <span className="text-xs font-semibold text-muted-foreground">Active Transit Valuation</span>
            <p className="text-2xl font-black text-indigo-600 font-mono mt-1">${totalValuationInTransit.toLocaleString()}</p>
          </div>
          <div className="p-3 rounded-xl bg-indigo-500/10 text-indigo-600"><ShieldCheck className="w-6 h-6" /></div>
        </div>
      </div>

      {/* Filter Tabs & Search Bar */}
      <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
        <div className="flex items-center gap-2 overflow-x-auto w-full sm:w-auto pb-2 sm:pb-0">
          {[
            { label: "All Transfer Activity", value: "All" },
            { label: "Pending Approval", value: "Pending" },
            { label: "In Transit & Shipped", value: "InTransit" },
            { label: "Received & Inducted", value: "Received" },
            { label: "Rejected Orders", value: "Rejected" },
          ].map((tab) => (
            <button
              key={tab.value}
              onClick={() => setStatusFilter(tab.value)}
              className={cn(
                "px-4 py-2 rounded-xl text-xs font-bold transition-all shrink-0",
                statusFilter === tab.value
                  ? "bg-indigo-600 text-white shadow-md shadow-indigo-600/20"
                  : "bg-accent/60 text-muted-foreground hover:text-foreground hover:bg-accent"
              )}
            >
              {tab.label}
            </button>
          ))}
        </div>

        <div className="relative w-full sm:w-72">
          <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
          <input
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            placeholder="Search TRF number or warehouse..."
            className="w-full pl-10 pr-4 py-2 rounded-xl border border-border/80 bg-card text-xs text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-indigo-500/50"
          />
        </div>
      </div>

      {/* Transfers Table / Card Feed */}
      {isLoading ? (
        <div className="py-24 text-center space-y-3">
          <RefreshCw className="w-8 h-8 animate-spin text-indigo-600 mx-auto" />
          <p className="text-sm text-muted-foreground font-medium">Synchronizing inter-warehouse manifest feeds...</p>
        </div>
      ) : filteredTransfers.length === 0 ? (
        <div className="py-16 text-center rounded-2xl bg-accent/20 border border-dashed border-border p-8">
          <ArrowRightLeft className="w-10 h-10 text-muted-foreground mx-auto mb-3 opacity-40" />
          <h3 className="text-base font-bold">No transfer orders found</h3>
          <p className="text-xs text-muted-foreground mt-1">No orders matching the selected filter criteria.</p>
        </div>
      ) : (
        <div className="space-y-4">
          {filteredTransfers.map((trf) => (
            <div key={trf.id} className="p-6 rounded-2xl bg-card border border-border/60 shadow-sm hover:border-indigo-500/30 transition-all space-y-4">
              <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3 pb-4 border-b border-border/50">
                <div className="space-y-1">
                  <div className="flex items-center gap-3">
                    <span className="text-sm font-black font-mono text-foreground">{trf.transferNumber}</span>
                    {getStatusBadge(trf.status)}
                    {trf.approvalStatus === "AutoApproved" && (
                      <span className="text-[11px] font-semibold text-indigo-600 bg-indigo-500/10 px-2.5 py-0.5 rounded-full border border-indigo-500/20">
                        ⚡ Auto-Approved by Manager Threshold
                      </span>
                    )}
                  </div>
                  <div className="flex items-center gap-2 text-sm font-bold text-foreground/90 pt-1">
                    <span className="text-indigo-600 dark:text-indigo-400">{trf.sourceWarehouseName}</span>
                    <ArrowRightLeft className="w-4 h-4 text-muted-foreground mx-1" />
                    <span className="text-emerald-600 dark:text-emerald-400">{trf.destinationWarehouseName}</span>
                  </div>
                </div>

                <div className="flex items-center gap-2 self-start sm:self-auto">
                  {trf.status === "Pending" && (
                    <button
                      onClick={() => setSelectedForApproval(trf)}
                      className="px-4 py-2 rounded-xl bg-indigo-600 hover:bg-indigo-700 text-white font-bold text-xs shadow-md shadow-indigo-600/20 transition-all flex items-center gap-1.5"
                    >
                      <ShieldCheck className="w-4 h-4" /> Review & Authorize
                    </button>
                  )}

                  {(trf.status === "InTransit" || trf.status === "Approved") && (
                    <button
                      onClick={() => setSelectedForReceipt(trf)}
                      className="px-4 py-2 rounded-xl bg-emerald-600 hover:bg-emerald-700 text-white font-bold text-xs shadow-md shadow-emerald-600/20 transition-all flex items-center gap-1.5"
                    >
                      <CheckCircle2 className="w-4 h-4" /> Receive & Induct Stock
                    </button>
                  )}
                </div>
              </div>

              {/* Transfer Details & Line Items Preview */}
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4 text-xs">
                <div className="p-3.5 rounded-xl bg-accent/30 border border-border/50 space-y-1">
                  <span className="font-bold text-muted-foreground uppercase text-[10px]">Financials & Units</span>
                  <p className="text-base font-black font-mono text-indigo-600">${trf.totalTransferValue.toLocaleString()} <span className="text-xs font-medium text-foreground/80">({trf.totalItemsCount} units)</span></p>
                  <p className="text-muted-foreground text-[11px]">Requested by: <strong>{trf.requestedByName}</strong></p>
                </div>

                <div className="p-3.5 rounded-xl bg-accent/30 border border-border/50 space-y-1">
                  <span className="font-bold text-muted-foreground uppercase text-[10px]">Carrier & Tracking Logs</span>
                  <p className="text-xs text-foreground font-medium leading-relaxed">{trf.trackingNotes || "Standard internal dispatch transfer."}</p>
                  {trf.shippedAt && <p className="text-[10px] font-mono text-muted-foreground">Shipped: {new Date(trf.shippedAt).toLocaleDateString()}</p>}
                </div>

                <div className="p-3.5 rounded-xl bg-accent/30 border border-border/50 space-y-1.5 overflow-hidden">
                  <span className="font-bold text-muted-foreground uppercase text-[10px] flex items-center gap-1">
                    <Package className="w-3.5 h-3.5 text-indigo-500" /> Manifest Line Items ({trf.items?.length || 0} SKUs)
                  </span>
                  <div className="space-y-1 max-h-20 overflow-y-auto pr-1 text-[11px]">
                    {trf.items?.map((it, idx) => (
                      <div key={idx} className="flex items-center justify-between font-mono bg-card px-2 py-1 rounded border border-border/40">
                        <span className="font-bold text-foreground truncate max-w-[140px]">{it.sku} ({it.productName})</span>
                        <span className="text-indigo-600 font-bold">{it.quantity}x</span>
                      </div>
                    ))}
                  </div>
                </div>
              </div>

              {trf.rejectionReason && (
                <div className="p-3 rounded-xl bg-red-500/10 border border-red-500/20 text-xs text-red-600 dark:text-red-400 flex items-center gap-2 font-medium">
                  <AlertTriangle className="w-4 h-4 shrink-0" />
                  <span><strong>Transfer Declined:</strong> {trf.rejectionReason}</span>
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {/* Modals */}
      <CreateTransferModal isOpen={isCreateOpen} onClose={() => setIsCreateOpen(false)} warehouses={warehouses} />
      <ApproveTransferModal isOpen={!!selectedForApproval} onClose={() => setSelectedForApproval(null)} transfer={selectedForApproval} />
      <ReceiveTransferModal isOpen={!!selectedForReceipt} onClose={() => setSelectedForReceipt(null)} transfer={selectedForReceipt} />
    </div>
  );
}
