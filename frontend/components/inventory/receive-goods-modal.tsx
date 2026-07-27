"use client";

import { useState } from "react";
import { X, Truck, CheckCircle2 } from "lucide-react";
import { PurchaseOrderDto, CreateGoodsReceiptDto } from "@/types/inventory";

interface ReceiveGoodsModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: CreateGoodsReceiptDto) => Promise<void>;
  purchaseOrder: PurchaseOrderDto | null;
  isLoading?: boolean;
}

export function ReceiveGoodsModal({
  isOpen,
  onClose,
  onSubmit,
  purchaseOrder,
  isLoading,
}: ReceiveGoodsModalProps) {
  if (!isOpen || !purchaseOrder) return null;

  const [notes, setNotes] = useState("");
  const [items, setItems] = useState(
    purchaseOrder.items.map((item) => {
      const remaining = Math.max(0, item.quantityOrdered - item.quantityReceived);
      return {
        purchaseOrderItemId: item.id,
        productId: item.productId,
        productName: item.productName,
        productSKU: item.productSKU,
        quantityOrdered: item.quantityOrdered,
        quantityReceivedSoFar: item.quantityReceived,
        quantityReceived: remaining,
        quantityRejected: 0,
        rejectionReason: "",
        unitCost: item.unitPrice,
      };
    })
  );

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload: CreateGoodsReceiptDto = {
      purchaseOrderId: purchaseOrder.id,
      notes,
      items: items.map((i) => ({
        purchaseOrderItemId: i.purchaseOrderItemId,
        productId: i.productId,
        quantityReceived: Number(i.quantityReceived),
        quantityRejected: Number(i.quantityRejected),
        rejectionReason: i.rejectionReason,
        unitCost: Number(i.unitCost),
      })),
    };

    await onSubmit(payload);
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-3xl bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <Truck className="w-5 h-5 text-emerald-500" />
            <div>
              <h2 className="text-lg font-semibold">Receive Goods (GRN)</h2>
              <p className="text-xs text-muted-foreground">PO Number: {purchaseOrder.poNumber}</p>
            </div>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4 max-h-[80vh] overflow-y-auto">
          <div className="p-3 rounded-xl bg-emerald-500/10 border border-emerald-500/20 text-emerald-500 text-xs flex items-center gap-2">
            <CheckCircle2 className="w-4 h-4 flex-shrink-0" />
            <span>
              Receiving goods will automatically increment inventory stock in warehouse <strong>{purchaseOrder.warehouseName}</strong> and log an audit trail.
            </span>
          </div>

          <div className="space-y-3">
            <label className="text-xs font-semibold uppercase tracking-wider text-muted-foreground">
              Received Items Checklist
            </label>
            {items.map((item, idx) => (
              <div key={idx} className="p-4 rounded-xl border border-border bg-muted/20 space-y-3">
                <div className="flex items-center justify-between">
                  <div>
                    <span className="font-semibold text-sm">{item.productName}</span>
                    <span className="ml-2 text-xs text-muted-foreground">({item.productSKU})</span>
                  </div>
                  <div className="text-xs text-muted-foreground">
                    Ordered: <strong>{item.quantityOrdered}</strong> | Prev Received: <strong>{item.quantityReceivedSoFar}</strong>
                  </div>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                  <div>
                    <label className="block text-[11px] font-medium text-muted-foreground mb-1">
                      Qty Received Now
                    </label>
                    <input
                      type="number"
                      min="0"
                      required
                      value={item.quantityReceived}
                      onChange={(e) => {
                        const updated = [...items];
                        updated[idx].quantityReceived = Number(e.target.value);
                        setItems(updated);
                      }}
                      className="w-full px-2.5 py-1.5 text-xs rounded-md border border-input bg-background"
                    />
                  </div>
                  <div>
                    <label className="block text-[11px] font-medium text-muted-foreground mb-1">
                      Qty Rejected (Damaged)
                    </label>
                    <input
                      type="number"
                      min="0"
                      value={item.quantityRejected}
                      onChange={(e) => {
                        const updated = [...items];
                        updated[idx].quantityRejected = Number(e.target.value);
                        setItems(updated);
                      }}
                      className="w-full px-2.5 py-1.5 text-xs rounded-md border border-input bg-background"
                    />
                  </div>
                  <div>
                    <label className="block text-[11px] font-medium text-muted-foreground mb-1">
                      Unit Cost ($)
                    </label>
                    <input
                      type="number"
                      step="0.01"
                      min="0"
                      value={item.unitCost}
                      onChange={(e) => {
                        const updated = [...items];
                        updated[idx].unitCost = Number(e.target.value);
                        setItems(updated);
                      }}
                      className="w-full px-2.5 py-1.5 text-xs rounded-md border border-input bg-background"
                    />
                  </div>
                </div>

                {item.quantityRejected > 0 && (
                  <div>
                    <input
                      type="text"
                      placeholder="Reason for rejection (e.g. Broken packaging / Damaged in transit)"
                      value={item.rejectionReason}
                      onChange={(e) => {
                        const updated = [...items];
                        updated[idx].rejectionReason = e.target.value;
                        setItems(updated);
                      }}
                      className="w-full px-2.5 py-1.5 text-xs rounded-md border border-rose-500/30 bg-rose-500/5 text-foreground placeholder:text-muted-foreground"
                    />
                  </div>
                )}
              </div>
            ))}
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Delivery Notes / GRN Memo</label>
            <textarea
              rows={2}
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
              placeholder="Driver name, waybill reference, or inspection notes..."
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
            />
          </div>

          <div className="flex items-center justify-end gap-3 pt-4 border-t border-border">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 text-sm font-medium rounded-xl border border-input hover:bg-accent"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="px-5 py-2 text-sm font-medium text-white rounded-xl bg-emerald-600 hover:bg-emerald-700 shadow-md shadow-emerald-600/20 disabled:opacity-50"
            >
              {isLoading ? "Posting GRN..." : "Confirm Goods Receipt"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
