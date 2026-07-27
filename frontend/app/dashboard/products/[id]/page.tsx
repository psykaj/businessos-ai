"use client";

import { use } from "react";
import Link from "next/link";
import {
  ArrowLeft,
  Package,
  QrCode,
  Building2,
  History,
  DollarSign,
  AlertTriangle,
  CheckCircle2,
  Layers,
} from "lucide-react";
import { useProduct, useProductStock, useStockMovements } from "@/hooks/use-inventory";

export default function ProductDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const { id } = use(params);

  const { data: product, isLoading: isProductLoading } = useProduct(id);
  const { data: stockItems, isLoading: isStockLoading } = useProductStock(id);
  const { data: movementsResult } = useStockMovements({ productId: id, pageSize: 20 });

  if (isProductLoading) {
    return (
      <div className="p-8 max-w-7xl mx-auto space-y-6 animate-pulse">
        <div className="h-8 w-48 bg-muted/40 rounded-lg" />
        <div className="h-40 bg-muted/40 rounded-2xl" />
        <div className="h-64 bg-muted/40 rounded-2xl" />
      </div>
    );
  }

  if (!product) {
    return (
      <div className="p-12 text-center text-muted-foreground space-y-4">
        <p className="text-lg">Product not found.</p>
        <Link href="/dashboard/products" className="text-indigo-500 hover:underline text-sm font-medium">
          Back to Product Catalog
        </Link>
      </div>
    );
  }

  const isLow = product.totalQuantityAvailable <= product.reorderPoint;
  const isOut = product.totalQuantityAvailable === 0;

  return (
    <div className="p-6 md:p-8 space-y-8 max-w-7xl mx-auto">
      {/* Back Button */}
      <Link
        href="/dashboard/products"
        className="inline-flex items-center gap-2 text-xs font-medium text-muted-foreground hover:text-foreground transition-colors"
      >
        <ArrowLeft className="w-4 h-4" /> Back to Products
      </Link>

      {/* Product Hero Header */}
      <div className="p-6 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm space-y-4">
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
          <div className="space-y-1">
            <div className="flex items-center gap-3">
              <h1 className="text-2xl md:text-3xl font-bold tracking-tight">{product.name}</h1>
              <span className="font-mono text-xs px-2.5 py-1 rounded bg-muted border border-border font-semibold">
                {product.sku}
              </span>
            </div>
            <p className="text-xs text-muted-foreground">{product.description || "No description specified."}</p>
          </div>

          <div className="flex items-center gap-3">
            {isOut ? (
              <span className="px-3 py-1.5 rounded-full text-xs font-semibold bg-rose-500/10 text-rose-500 border border-rose-500/20 flex items-center gap-1.5">
                <AlertTriangle className="w-4 h-4" /> Out of Stock
              </span>
            ) : isLow ? (
              <span className="px-3 py-1.5 rounded-full text-xs font-semibold bg-amber-500/10 text-amber-500 border border-amber-500/20 flex items-center gap-1.5">
                <AlertTriangle className="w-4 h-4" /> Low Stock Warning
              </span>
            ) : (
              <span className="px-3 py-1.5 rounded-full text-xs font-semibold bg-emerald-500/10 text-emerald-500 border border-emerald-500/20 flex items-center gap-1.5">
                <CheckCircle2 className="w-4 h-4" /> Optimal Stock
              </span>
            )}
          </div>
        </div>

        {/* Stats Grid */}
        <div className="grid grid-cols-2 sm:grid-cols-4 gap-4 pt-4 border-t border-border/50 text-xs">
          <div>
            <span className="text-muted-foreground block text-[11px]">Selling Price</span>
            <span className="font-bold text-base text-foreground">${product.sellingPrice.toFixed(2)}</span>
          </div>
          <div>
            <span className="text-muted-foreground block text-[11px]">Cost Valuation</span>
            <span className="font-bold text-base text-foreground">${product.costPrice.toFixed(2)}</span>
          </div>
          <div>
            <span className="text-muted-foreground block text-[11px]">Total Available Stock</span>
            <span className="font-bold text-base text-indigo-500">
              {product.totalQuantityAvailable} {product.unitOfMeasure}
            </span>
          </div>
          <div>
            <span className="text-muted-foreground block text-[11px]">Reorder Threshold</span>
            <span className="font-bold text-base text-foreground">
              {product.reorderPoint} {product.unitOfMeasure}
            </span>
          </div>
        </div>
      </div>

      {/* Warehouse Multi-Stock Grid */}
      <div className="space-y-4">
        <div className="flex items-center gap-2">
          <Building2 className="w-5 h-5 text-indigo-500" />
          <h2 className="text-lg font-semibold">Warehouse Stock Breakdown</h2>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {isStockLoading ? (
            Array.from({ length: 3 }).map((_, i) => (
              <div key={i} className="h-32 rounded-xl bg-muted/40 animate-pulse border" />
            ))
          ) : stockItems && stockItems.length > 0 ? (
            stockItems.map((s) => (
              <div
                key={s.id}
                className="p-5 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm space-y-3"
              >
                <div className="flex items-center justify-between">
                  <span className="font-semibold text-sm">{s.warehouseName}</span>
                  <span className="text-[10px] font-mono px-2 py-0.5 rounded bg-muted border border-border">
                    {s.locationBin || "General Bin"}
                  </span>
                </div>
                <div className="grid grid-cols-3 gap-2 text-center text-xs pt-2 border-t border-border/50">
                  <div>
                    <span className="text-muted-foreground block text-[10px]">On Hand</span>
                    <span className="font-bold text-sm">{s.quantityOnHand}</span>
                  </div>
                  <div>
                    <span className="text-muted-foreground block text-[10px]">Reserved</span>
                    <span className="font-bold text-sm text-amber-500">{s.quantityReserved}</span>
                  </div>
                  <div>
                    <span className="text-muted-foreground block text-[10px]">Available</span>
                    <span className="font-bold text-sm text-emerald-500">{s.quantityAvailable}</span>
                  </div>
                </div>
              </div>
            ))
          ) : (
            <div className="col-span-3 p-8 text-center text-xs text-muted-foreground rounded-2xl border border-border">
              No stock entries assigned for this product.
            </div>
          )}
        </div>
      </div>

      {/* Historical Stock Movements */}
      <div className="space-y-4">
        <div className="flex items-center gap-2">
          <History className="w-5 h-5 text-indigo-500" />
          <h2 className="text-lg font-semibold">Stock Movement Audit History</h2>
        </div>

        <div className="rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left text-xs">
              <thead className="bg-muted/40 border-b border-border text-muted-foreground uppercase tracking-wider font-semibold text-[10px]">
                <tr>
                  <th className="p-4">Date</th>
                  <th className="p-4">Movement Type</th>
                  <th className="p-4">Quantity</th>
                  <th className="p-4">Source / Destination</th>
                  <th className="p-4">Reference / Reason</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-border/40">
                {movementsResult?.items && movementsResult.items.length > 0 ? (
                  movementsResult.items.map((m) => (
                    <tr key={m.id} className="hover:bg-muted/20 transition-colors">
                      <td className="p-4 font-mono text-muted-foreground">
                        {new Date(m.movementDate).toLocaleString()}
                      </td>
                      <td className="p-4">
                        <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-indigo-500/10 text-indigo-500 border border-indigo-500/20">
                          {m.movementType}
                        </span>
                      </td>
                      <td className="p-4 font-bold text-sm">{m.quantity}</td>
                      <td className="p-4 text-muted-foreground">
                        {m.sourceWarehouseName || "—"} $\rightarrow$ {m.destinationWarehouseName || "—"}
                      </td>
                      <td className="p-4">
                        <div className="font-medium text-foreground">{m.referenceType || "Manual"}</div>
                        <div className="text-[10px] text-muted-foreground">{m.reason || "No notes"}</div>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan={5} className="p-8 text-center text-xs text-muted-foreground">
                      No stock movement audit records found.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}
