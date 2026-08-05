"use client";

import React, { useState } from "react";
import { useForm, useFieldArray } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { BranchWarehouseDto, WarehouseTransferDto, TransferStatus } from "@/lib/multi-branch-service";
import { useRequestTransfer, useApproveTransfer, useUpdateTransferTracking, useReceiveTransfer } from "@/hooks/use-multi-branch";
import { X, ArrowRightLeft, ShieldCheck, CheckCircle2, Truck, Package, Plus, Trash2, AlertCircle } from "lucide-react";
import { cn } from "@/lib/utils";

// ─── Zod Schemas ─────────────────────────────────────────────────────────────

const createTransferSchema = z.object({
  sourceWarehouseId: z.string().min(1, "Source warehouse required"),
  destinationWarehouseId: z.string().min(1, "Destination warehouse required"),
  requestedByName: z.string().min(2, "Requester name required"),
  notes: z.string().optional(),
  items: z.array(z.object({
    sku: z.string().min(2, "SKU required"),
    productName: z.string().min(2, "Product description required"),
    quantity: z.number().min(1, "Quantity must be at least 1"),
    unitPrice: z.number().min(0, "Price cannot be negative"),
  })).min(1, "At least one item must be included in transfer order"),
}).refine(data => data.sourceWarehouseId !== data.destinationWarehouseId, {
  message: "Destination warehouse must be different from source warehouse.",
  path: ["destinationWarehouseId"],
});

// ─── 1. Create Transfer Request Modal ────────────────────────────────────────

export function CreateTransferModal({ isOpen, onClose, warehouses = [] }: { isOpen: boolean; onClose: () => void; warehouses: BranchWarehouseDto[] }) {
  const requestMutation = useRequestTransfer();
  const { register, control, handleSubmit, reset, watch, formState: { errors, isSubmitting } } = useForm<z.infer<typeof createTransferSchema>>({
    resolver: zodResolver(createTransferSchema),
    defaultValues: {
      requestedByName: "Current Managing VP",
      notes: "Urgent inventory stock balancing dispatch.",
      items: [{ sku: "SERVER-PRO-X1", productName: "Rackmount Enterprise Server X1", quantity: 10, unitPrice: 3200 }],
    }
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "items",
  });

  const itemsWatch = watch("items") || [];
  const totalVal = itemsWatch.reduce((acc, idx) => acc + ((Number(idx.quantity) || 0) * (Number(idx.unitPrice) || 0)), 0);
  const totalCount = itemsWatch.reduce((acc, idx) => acc + (Number(idx.quantity) || 0), 0);

  if (!isOpen) return null;

  const onSubmit = async (data: z.infer<typeof createTransferSchema>) => {
    await requestMutation.mutateAsync({
      sourceWarehouseId: data.sourceWarehouseId,
      destinationWarehouseId: data.destinationWarehouseId,
      requestedByName: data.requestedByName,
      items: data.items,
      notes: data.notes,
    });
    reset();
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
      <div className="relative w-full max-w-2xl rounded-2xl bg-card border border-border shadow-2xl overflow-hidden p-6 space-y-4 max-h-[90vh] overflow-y-auto">
        <div className="flex items-center justify-between border-b border-border/50 pb-3">
          <div className="flex items-center gap-2 text-indigo-600 font-bold text-lg">
            <ArrowRightLeft className="w-5 h-5" /> Initiate Inter-Warehouse Transfer Order
          </div>
          <button onClick={onClose} className="p-1 text-muted-foreground hover:text-foreground"><X className="w-5 h-5" /></button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 text-sm">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold block mb-1 text-xs">Source Origin Warehouse *</label>
              <select {...register("sourceWarehouseId")} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-medium">
                <option value="">-- Select Source Depot --</option>
                {warehouses.map(w => (
                  <option key={w.id} value={w.id}>{w.name} ({w.branchName}) - {w.totalStockItemsCount} items</option>
                ))}
              </select>
              {errors.sourceWarehouseId && <p className="text-red-500 text-[10px] mt-1">{errors.sourceWarehouseId.message}</p>}
            </div>

            <div>
              <label className="font-semibold block mb-1 text-xs">Destination Receiving Warehouse *</label>
              <select {...register("destinationWarehouseId")} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-medium">
                <option value="">-- Select Destination Depot --</option>
                {warehouses.map(w => (
                  <option key={w.id} value={w.id}>{w.name} ({w.branchName})</option>
                ))}
              </select>
              {errors.destinationWarehouseId && <p className="text-red-500 text-[10px] mt-1">{errors.destinationWarehouseId.message}</p>}
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold block mb-1 text-xs">Requested By (Executive / Manager) *</label>
              <input {...register("requestedByName")} placeholder="e.g. Sarah Jenkins" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
              {errors.requestedByName && <p className="text-red-500 text-[10px] mt-1">{errors.requestedByName.message}</p>}
            </div>
            <div>
              <label className="font-semibold block mb-1 text-xs">Transfer Order Notes / Carrier SLA</label>
              <input {...register("notes")} placeholder="e.g. FedEx Freight Specialized #908" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
            </div>
          </div>

          {/* Line Items Table */}
          <div className="space-y-2 pt-2 border-t border-border/40">
            <div className="flex items-center justify-between">
              <span className="font-bold text-xs flex items-center gap-1.5 text-foreground">
                <Package className="w-4 h-4 text-indigo-500" /> Transfer Line Items ({totalCount} Units | Total Value: <strong className="text-indigo-600 font-mono">${totalVal.toLocaleString()}</strong>)
              </span>
              <button
                type="button"
                onClick={() => append({ sku: `SKU-${Math.floor(1000 + Math.random() * 9000)}`, productName: "Standard Stock Component", quantity: 20, unitPrice: 150 })}
                className="px-2.5 py-1 text-[11px] font-semibold text-indigo-600 bg-indigo-500/10 hover:bg-indigo-500/20 rounded-lg flex items-center gap-1"
              >
                <Plus className="w-3.5 h-3.5" /> Add SKU Item
              </button>
            </div>

            <div className="space-y-2 max-h-48 overflow-y-auto pr-1">
              {fields.map((field, idx) => (
                <div key={field.id} className="grid grid-cols-12 gap-2 p-2.5 rounded-xl bg-accent/40 border border-border/50 items-center">
                  <div className="col-span-3">
                    <input {...register(`items.${idx}.sku`)} placeholder="SKU" className="w-full px-2 py-1 rounded-lg border border-border bg-background font-mono text-xs font-semibold" />
                  </div>
                  <div className="col-span-4">
                    <input {...register(`items.${idx}.productName`)} placeholder="Product Name" className="w-full px-2 py-1 rounded-lg border border-border bg-background text-xs" />
                  </div>
                  <div className="col-span-2">
                    <input type="number" {...register(`items.${idx}.quantity`, { valueAsNumber: true })} placeholder="Qty" className="w-full px-2 py-1 rounded-lg border border-border bg-background text-xs font-mono font-bold text-center" />
                  </div>
                  <div className="col-span-2">
                    <input type="number" {...register(`items.${idx}.unitPrice`, { valueAsNumber: true })} placeholder="Unit $" className="w-full px-2 py-1 rounded-lg border border-border bg-background text-xs font-mono" />
                  </div>
                  <div className="col-span-1 flex justify-center">
                    {fields.length > 1 && (
                      <button type="button" onClick={() => remove(idx)} className="text-red-500 hover:text-red-700 p-1">
                        <Trash2 className="w-4 h-4" />
                      </button>
                    )}
                  </div>
                </div>
              ))}
            </div>
            {errors.items && <p className="text-red-500 text-xs">{errors.items.message}</p>}
          </div>

          <div className="p-3 rounded-xl bg-indigo-500/5 border border-indigo-500/20 text-[11px] text-muted-foreground flex items-center gap-2">
            <ShieldCheck className="w-5 h-5 text-indigo-500 shrink-0" />
            <span>
              <strong>Automated Threshold Engine:</strong> If the requesting executive holds active Manager approval authority with a threshold exceeding <strong>${totalVal.toLocaleString()}</strong>, this transfer will auto-approve and move directly to shipping.
            </span>
          </div>

          <div className="flex justify-end gap-3 pt-3 border-t border-border/50">
            <button type="button" onClick={onClose} className="px-4 py-2 rounded-xl border border-border text-xs font-semibold">Cancel</button>
            <button type="submit" disabled={isSubmitting} className="px-6 py-2 rounded-xl bg-indigo-600 text-white font-semibold text-xs hover:bg-indigo-700 shadow-md">
              Submit Transfer Order
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

// ─── 2. Approve Transfer Modal ───────────────────────────────────────────────

export function ApproveTransferModal({ isOpen, onClose, transfer }: { isOpen: boolean; onClose: () => void; transfer: WarehouseTransferDto | null }) {
  const approveMutation = useApproveTransfer();
  const [approverName, setApproverName] = useState("Marcus Vance, Director East");
  const [rejectionReason, setRejectionReason] = useState("Stock reserves allocated for critical corporate SLA commitments.");

  if (!isOpen || !transfer) return null;

  const handleAction = async (isApproved: boolean) => {
    await approveMutation.mutateAsync({
      id: transfer.id,
      isApproved,
      approvedByName: approverName,
      rejectionReason: !isApproved ? rejectionReason : undefined,
    });
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
      <div className="relative w-full max-w-md rounded-2xl bg-card border border-border shadow-2xl p-6 space-y-4">
        <div className="flex items-center justify-between border-b border-border/50 pb-3">
          <div className="flex items-center gap-2 text-indigo-600 font-bold text-base">
            <ShieldCheck className="w-5 h-5" /> Review & Authorize Transfer
          </div>
          <button onClick={onClose} className="p-1 text-muted-foreground hover:text-foreground"><X className="w-5 h-5" /></button>
        </div>

        <div className="p-3 rounded-xl bg-accent/40 border border-border/50 text-xs space-y-1">
          <p><strong>Transfer Order:</strong> {transfer.transferNumber}</p>
          <p><strong>Route:</strong> {transfer.sourceWarehouseName} ➔ {transfer.destinationWarehouseName}</p>
          <p><strong>Valuation:</strong> <span className="text-indigo-600 font-mono font-bold">${transfer.totalTransferValue.toLocaleString()}</span> ({transfer.totalItemsCount} units)</p>
          <p><strong>Requested By:</strong> {transfer.requestedByName}</p>
        </div>

        <div className="space-y-3 text-sm">
          <div>
            <label className="text-xs font-semibold block mb-1">Authorizing Executive Name *</label>
            <input value={approverName} onChange={(e) => setApproverName(e.target.value)} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-medium" />
          </div>

          <div>
            <label className="text-xs font-semibold block mb-1 text-red-500">Rejection Reason (if declining)</label>
            <textarea value={rejectionReason} onChange={(e) => setRejectionReason(e.target.value)} rows={2} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
          </div>
        </div>

        <div className="flex justify-end gap-2 pt-3 border-t border-border/50">
          <button onClick={() => handleAction(false)} className="px-4 py-2 rounded-xl border border-red-500/30 text-red-600 hover:bg-red-500/10 font-semibold text-xs">
            Reject Transfer
          </button>
          <button onClick={() => handleAction(true)} className="px-5 py-2 rounded-xl bg-emerald-600 text-white hover:bg-emerald-700 shadow-md font-semibold text-xs flex items-center gap-1">
            <CheckCircle2 className="w-4 h-4" /> Authorize & Release Shipment
          </button>
        </div>
      </div>
    </div>
  );
}

// ─── 3. Receive Inventory Modal ──────────────────────────────────────────────

export function ReceiveTransferModal({ isOpen, onClose, transfer }: { isOpen: boolean; onClose: () => void; transfer: WarehouseTransferDto | null }) {
  const receiveMutation = useReceiveTransfer();
  const [notes, setNotes] = useState("All stock items verified intact against manifest. Inducted into warehouse inventory.");

  if (!isOpen || !transfer) return null;

  const handleReceive = async () => {
    await receiveMutation.mutateAsync({
      id: transfer.id,
      receiptNotes: notes
    });
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
      <div className="relative w-full max-w-md rounded-2xl bg-card border border-border shadow-2xl p-6 space-y-4">
        <div className="flex items-center justify-between border-b border-border/50 pb-3">
          <div className="flex items-center gap-2 text-emerald-600 font-bold text-base">
            <CheckCircle2 className="w-5 h-5" /> Receive & Induct Shipment
          </div>
          <button onClick={onClose} className="p-1 text-muted-foreground hover:text-foreground"><X className="w-5 h-5" /></button>
        </div>

        <p className="text-xs text-muted-foreground">
          Confirm arrival and physical induction of stock shipment for **{transfer.destinationWarehouseName}**.
        </p>

        <div className="p-3 rounded-xl bg-emerald-500/10 border border-emerald-500/20 text-xs space-y-1.5 text-emerald-700 dark:text-emerald-400">
          <p className="font-bold flex items-center gap-1.5">
            <Package className="w-4 h-4" /> Automated Stock Reconciliation:
          </p>
          <ul className="list-disc list-inside text-[11px] space-y-0.5 ml-1">
            <li><strong>+{transfer.totalItemsCount} units</strong> added to {transfer.destinationWarehouseName}</li>
            <li><strong>+${transfer.totalTransferValue.toLocaleString()}</strong> added to receiving facility valuation</li>
            <li>Source warehouse stock counts decremented and locked</li>
          </ul>
        </div>

        <div>
          <label className="text-xs font-semibold block mb-1">Receiving Verification Logs & Carrier Sign-off</label>
          <textarea value={notes} onChange={(e) => setNotes(e.target.value)} rows={3} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
        </div>

        <div className="flex justify-end gap-3 pt-3 border-t border-border/50">
          <button onClick={onClose} className="px-4 py-2 rounded-xl border border-border text-xs font-semibold">Cancel</button>
          <button onClick={handleReceive} className="px-6 py-2 rounded-xl bg-emerald-600 text-white font-semibold text-xs hover:bg-emerald-700 shadow-md">
            Confirm Receipt & Update Valuation
          </button>
        </div>
      </div>
    </div>
  );
}
