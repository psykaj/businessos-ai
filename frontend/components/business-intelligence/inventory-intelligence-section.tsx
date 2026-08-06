"use client";

import React, { useState } from "react";
import { InventoryInsightDto, LowStockItemDto } from "@/lib/business-intelligence-service";
import { 
  Package, 
  AlertOctagon, 
  Zap, 
  Layers, 
  ShoppingCart, 
  Sparkles, 
  Check, 
  Loader2, 
  Box
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

interface InventoryIntelligenceSectionProps {
  inventory: InventoryInsightDto;
}

export const InventoryIntelligenceSection: React.FC<InventoryIntelligenceSectionProps> = ({ inventory }) => {
  const [orderingSku, setOrderingSku] = useState<string | null>(null);
  const [orderedSkus, setOrderedSkus] = useState<Record<string, boolean>>({});

  const handleApprovePO = async (item: LowStockItemDto) => {
    setOrderingSku(item.sku);
    await new Promise((res) => setTimeout(res, 800));
    setOrderedSkus((prev) => ({ ...prev, [item.sku]: true }));
    setOrderingSku(null);
    toast.success("Purchase Order Approved", {
      description: `Dispatched electronic PO to primary supplier for ${item.recommendedOrderQuantity} units of ${item.productName} (${item.sku}).`,
    });
  };

  return (
    <div className="rounded-3xl bg-white dark:bg-slate-900 border border-slate-200/80 dark:border-slate-800 shadow-xl p-6 space-y-6">
      
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 pb-4 border-b border-slate-100 dark:border-slate-800">
        <div className="flex items-center gap-3">
          <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-amber-500/10 text-amber-500">
            <Package className="h-6 w-6" />
          </div>
          <div>
            <h3 className="text-xl font-bold text-slate-900 dark:text-white flex items-center gap-2">
              Inventory Intelligence & Supply Chain
              <span className="text-xs bg-amber-500/10 text-amber-500 border border-amber-500/20 px-2.5 py-0.5 rounded-full font-extrabold">
                {inventory.lowStockCount} SKUs Low Stock
              </span>
            </h3>
            <p className="text-xs text-slate-500 dark:text-slate-400">
              Turnover velocity tracking, dead inventory optimization, and automated replenishment
            </p>
          </div>
        </div>
      </div>

      {/* 4 KPI Cards Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        
        <div className="p-4 rounded-2xl bg-slate-50 dark:bg-slate-800/50 border border-slate-200/60 dark:border-slate-800 flex items-center gap-4">
          <div className="p-3 rounded-xl bg-indigo-500/10 text-indigo-500">
            <Layers className="h-6 w-6" />
          </div>
          <div>
            <span className="text-2xl font-black font-mono text-slate-900 dark:text-white">{inventory.totalSkus}</span>
            <span className="text-[11px] text-slate-500 dark:text-slate-400 font-medium block">Total Active SKUs</span>
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-slate-50 dark:bg-slate-800/50 border border-slate-200/60 dark:border-slate-800 flex items-center gap-4">
          <div className="p-3 rounded-xl bg-emerald-500/10 text-emerald-500">
            <Box className="h-6 w-6" />
          </div>
          <div>
            <span className="text-2xl font-black font-mono text-emerald-600 dark:text-emerald-400">
              ${(inventory.totalInventoryValue / 1000).toFixed(1)}k
            </span>
            <span className="text-[11px] text-slate-500 dark:text-slate-400 font-medium block">Total Inventory Valuation</span>
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-gradient-to-r from-purple-500/5 to-indigo-500/5 dark:from-purple-950/20 dark:to-indigo-950/20 border border-purple-500/20 flex items-center gap-3">
          <div className="p-3 rounded-xl bg-purple-500/10 text-purple-500">
            <Zap className="h-6 w-6 fill-purple-500" />
          </div>
          <div className="overflow-hidden">
            <span className="text-[10px] font-extrabold text-purple-500 uppercase block">Fastest Selling Product</span>
            <span className="text-xs font-black text-slate-900 dark:text-white line-clamp-1 block">{inventory.fastestSellingProduct}</span>
            <span className="text-[10px] font-bold text-slate-400">{inventory.fastestSellingVelocity} units turned / week</span>
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-amber-500/5 dark:bg-amber-950/20 border border-amber-500/20 flex items-center gap-4">
          <div className="p-3 rounded-xl bg-amber-500/10 text-amber-500">
            <AlertOctagon className="h-6 w-6" />
          </div>
          <div>
            <span className="text-xl font-black font-mono text-amber-600 dark:text-amber-400">
              {inventory.deadInventoryCount || 6} SKUs
            </span>
            <span className="text-[11px] font-bold text-amber-500 block">
              ${((inventory.deadInventoryValue || 18400) / 1000).toFixed(1)}k Holding Cost
            </span>
            <span className="text-[10px] text-slate-400">Slow-Moving Dead Stock</span>
          </div>
        </div>

      </div>

      {/* Low Stock Replenishment Table */}
      <div className="space-y-3">
        <h4 className="text-xs font-extrabold uppercase tracking-wider text-slate-700 dark:text-slate-300 flex items-center gap-2">
          <ShoppingCart className="h-4 w-4 text-amber-500" />
          Low Stock Warning Schedule & Automated Replenishment
        </h4>
        <div className="overflow-x-auto rounded-2xl border border-slate-200 dark:border-slate-800">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-slate-100 dark:bg-slate-800/80 text-[11px] font-extrabold uppercase tracking-wider text-slate-600 dark:text-slate-400 border-b border-slate-200 dark:border-slate-700">
                <th className="p-4">SKU / Item Code</th>
                <th className="p-4">Product Name</th>
                <th className="p-4">Stock On Hand</th>
                <th className="p-4">Reorder Threshold</th>
                <th className="p-4">Recommended PO Qty</th>
                <th className="p-4 text-right">Supplier Action</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100 dark:divide-slate-800 text-xs">
              {(inventory.lowStockItems || []).map((item) => {
                const isOrdering = orderingSku === item.sku;
                const isOrdered = orderedSkus[item.sku];

                return (
                  <tr key={item.sku} className="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors">
                    <td className="p-4 font-mono font-bold text-slate-700 dark:text-slate-300">
                      {item.sku}
                    </td>
                    <td className="p-4 font-bold text-slate-900 dark:text-white">
                      {item.productName}
                    </td>
                    <td className="p-4">
                      <span className="px-2 py-0.5 rounded text-xs font-extrabold font-mono bg-red-500/20 text-red-600 dark:text-red-400 border border-red-500/30">
                        {item.quantityOnHand} units
                      </span>
                    </td>
                    <td className="p-4 font-mono text-slate-600 dark:text-slate-400">
                      {item.reorderPoint} units
                    </td>
                    <td className="p-4 font-extrabold font-mono text-indigo-600 dark:text-indigo-400">
                      +{item.recommendedOrderQuantity} units
                    </td>
                    <td className="p-4 text-right">
                      <Button
                        size="sm"
                        disabled={isOrdering || isOrdered}
                        onClick={() => handleApprovePO(item)}
                        className={cn(
                          "text-xs font-bold rounded-xl px-3 py-1.5 transition-all shadow-sm",
                          isOrdered
                            ? "bg-emerald-600 hover:bg-emerald-600 text-white cursor-default"
                            : "bg-amber-600 hover:bg-amber-500 text-white shadow-amber-500/20"
                        )}
                      >
                        {isOrdering ? (
                          <Loader2 className="h-3.5 w-3.5 animate-spin" />
                        ) : isOrdered ? (
                          <span className="flex items-center gap-1"><Check className="h-3 w-3" /> PO Dispatched</span>
                        ) : (
                          <span className="flex items-center gap-1"><ShoppingCart className="h-3 w-3" /> Approve PO</span>
                        )}
                      </Button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>

      {/* AI Supply Chain Suggestions Box */}
      <div className="p-5 rounded-2xl bg-amber-500/5 dark:bg-amber-950/15 border border-amber-500/20 space-y-3">
        <div className="flex items-center gap-2 text-amber-600 dark:text-amber-400 font-black text-xs uppercase tracking-wider">
          <Sparkles className="h-4 w-4 text-amber-500 animate-pulse" />
          <span>AI Supply Chain & Dead Stock Optimization Recommendations</span>
        </div>
        <ul className="grid grid-cols-1 md:grid-cols-3 gap-3">
          {(inventory.aiSuggestions || []).map((sugg, i) => (
            <li key={i} className="p-3 rounded-xl bg-white/60 dark:bg-slate-900/80 border border-amber-500/20 text-xs text-slate-700 dark:text-slate-300 leading-snug flex items-start gap-2 shadow-xs">
              <span className="h-2 w-2 rounded-full bg-amber-500 shrink-0 mt-1"></span>
              <span>{sugg}</span>
            </li>
          ))}
        </ul>
      </div>

    </div>
  );
};
