"use client";

import { useState } from "react";
import { X, ArrowRightLeft, ArrowDownRight, ArrowUpRight } from "lucide-react";
import { ProductDto, WarehouseDto } from "@/types/inventory";

interface StockOperationModalProps {
  isOpen: boolean;
  onClose: () => void;
  onStockIn: (dto: any) => Promise<void>;
  onStockOut: (dto: any) => Promise<void>;
  onTransfer: (dto: any) => Promise<void>;
  products: ProductDto[];
  warehouses: WarehouseDto[];
  isLoading?: boolean;
}

export function StockOperationModal({
  isOpen,
  onClose,
  onStockIn,
  onStockOut,
  onTransfer,
  products,
  warehouses,
  isLoading,
}: StockOperationModalProps) {
  if (!isOpen) return null;

  const [operationType, setOperationType] = useState<"StockIn" | "StockOut" | "Transfer">("StockIn");
  const [productId, setProductId] = useState(products[0]?.id || "");
  const [sourceWarehouseId, setSourceWarehouseId] = useState(warehouses[0]?.id || "");
  const [destinationWarehouseId, setDestinationWarehouseId] = useState(warehouses[1]?.id || warehouses[0]?.id || "");
  const [quantity, setQuantity] = useState(10);
  const [reason, setReason] = useState("");
  const [unitCost, setUnitCost] = useState(products[0]?.costPrice || 0);

  const handleProductChange = (pId: string) => {
    setProductId(pId);
    const selectedProd = products.find((p) => p.id === pId);
    if (selectedProd) setUnitCost(selectedProd.costPrice);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (quantity <= 0) return;

    if (operationType === "StockIn") {
      await onStockIn({
        productId,
        warehouseId: destinationWarehouseId || warehouses[0]?.id,
        quantity: Number(quantity),
        reason: reason || "Manual Stock In",
        unitCost: Number(unitCost),
      });
    } else if (operationType === "StockOut") {
      await onStockOut({
        productId,
        warehouseId: sourceWarehouseId || warehouses[0]?.id,
        quantity: Number(quantity),
        reason: reason || "Manual Stock Out",
        unitCost: Number(unitCost),
      });
    } else {
      await onTransfer({
        productId,
        sourceWarehouseId,
        destinationWarehouseId,
        quantity: Number(quantity),
        reason: reason || "Inter-warehouse stock transfer",
      });
    }

    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-lg bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <ArrowRightLeft className="w-5 h-5 text-indigo-500" />
            <h2 className="text-lg font-semibold">Stock Operation & Transfer</h2>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          {/* Tabs */}
          <div className="grid grid-cols-3 gap-1 p-1 bg-muted rounded-xl text-xs font-medium text-center">
            <button
              type="button"
              onClick={() => setOperationType("StockIn")}
              className={`py-2 rounded-lg flex items-center justify-center gap-1 transition-all ${
                operationType === "StockIn" ? "bg-background shadow text-emerald-500 font-semibold" : "text-muted-foreground"
              }`}
            >
              <ArrowDownRight className="w-3.5 h-3.5" /> Stock In
            </button>
            <button
              type="button"
              onClick={() => setOperationType("StockOut")}
              className={`py-2 rounded-lg flex items-center justify-center gap-1 transition-all ${
                operationType === "StockOut" ? "bg-background shadow text-rose-500 font-semibold" : "text-muted-foreground"
              }`}
            >
              <ArrowUpRight className="w-3.5 h-3.5" /> Stock Out
            </button>
            <button
              type="button"
              onClick={() => setOperationType("Transfer")}
              className={`py-2 rounded-lg flex items-center justify-center gap-1 transition-all ${
                operationType === "Transfer" ? "bg-background shadow text-indigo-500 font-semibold" : "text-muted-foreground"
              }`}
            >
              <ArrowRightLeft className="w-3.5 h-3.5" /> Transfer
            </button>
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Select Product</label>
            <select
              required
              value={productId}
              onChange={(e) => handleProductChange(e.target.value)}
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
            >
              {products.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.name} ({p.sku}) - Total Stock: {p.totalQuantityOnHand}
                </option>
              ))}
            </select>
          </div>

          {operationType === "Transfer" ? (
            <div className="grid grid-cols-2 gap-3">
              <div>
                <label className="block text-xs font-medium text-muted-foreground mb-1">Source Warehouse</label>
                <select
                  required
                  value={sourceWarehouseId}
                  onChange={(e) => setSourceWarehouseId(e.target.value)}
                  className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
                >
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      {w.name}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-xs font-medium text-muted-foreground mb-1">Destination Warehouse</label>
                <select
                  required
                  value={destinationWarehouseId}
                  onChange={(e) => setDestinationWarehouseId(e.target.value)}
                  className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
                >
                  {warehouses.map((w) => (
                    <option key={w.id} value={w.id}>
                      {w.name}
                    </option>
                  ))}
                </select>
              </div>
            </div>
          ) : (
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Target Warehouse</label>
              <select
                required
                value={operationType === "StockIn" ? destinationWarehouseId : sourceWarehouseId}
                onChange={(e) =>
                  operationType === "StockIn"
                    ? setDestinationWarehouseId(e.target.value)
                    : setSourceWarehouseId(e.target.value)
                }
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              >
                {warehouses.map((w) => (
                  <option key={w.id} value={w.id}>
                    {w.name} ({w.code})
                  </option>
                ))}
              </select>
            </div>
          )}

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Quantity</label>
              <input
                type="number"
                min="1"
                required
                value={quantity}
                onChange={(e) => setQuantity(Number(e.target.value))}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Unit Valuation ($)</label>
              <input
                type="number"
                step="0.01"
                min="0"
                value={unitCost}
                onChange={(e) => setUnitCost(Number(e.target.value))}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Reason / Notes</label>
            <input
              type="text"
              required
              value={reason}
              onChange={(e) => setReason(e.target.value)}
              placeholder="e.g. Replenishment, Customer return, Stock shift..."
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
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
              className="px-5 py-2 text-sm font-medium text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 disabled:opacity-50"
            >
              {isLoading ? "Executing..." : `Execute ${operationType}`}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
