"use client";

import { Package, DollarSign, AlertTriangle, XCircle, ShoppingBag, Truck } from "lucide-react";
import { DashboardInventorySummaryDto } from "@/types/inventory";

interface InventoryKpiCardsProps {
  summary?: DashboardInventorySummaryDto;
  isLoading?: boolean;
}

export function InventoryKpiCards({ summary, isLoading }: InventoryKpiCardsProps) {
  const cards = [
    {
      title: "Total Products",
      value: summary?.totalProductsCount ?? 0,
      subtext: "Active catalog items",
      icon: Package,
      gradient: "from-blue-500/10 to-indigo-500/10 border-blue-500/20 text-blue-500",
    },
    {
      title: "Total Inventory Value",
      value: summary ? `$${summary.totalInventoryValue.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}` : "$0.00",
      subtext: "Combined asset valuation",
      icon: DollarSign,
      gradient: "from-emerald-500/10 to-teal-500/10 border-emerald-500/20 text-emerald-500",
    },
    {
      title: "Low Stock Items",
      value: summary?.lowStockCount ?? 0,
      subtext: "Below reorder point",
      icon: AlertTriangle,
      gradient: "from-amber-500/10 to-orange-500/10 border-amber-500/20 text-amber-500",
    },
    {
      title: "Out of Stock",
      value: summary?.outOfStockCount ?? 0,
      subtext: "Zero available quantity",
      icon: XCircle,
      gradient: "from-rose-500/10 to-red-500/10 border-rose-500/20 text-rose-500",
    },
    {
      title: "Pending POs",
      value: summary?.pendingPurchaseOrdersCount ?? 0,
      subtext: "Approved / Pending delivery",
      icon: ShoppingBag,
      gradient: "from-purple-500/10 to-violet-500/10 border-purple-500/20 text-purple-500",
    },
    {
      title: "Overstock Items",
      value: summary?.overstockCount ?? 0,
      subtext: "Above max threshold",
      icon: Truck,
      gradient: "from-cyan-500/10 to-sky-500/10 border-cyan-500/20 text-cyan-500",
    },
  ];

  if (isLoading) {
    return (
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-4">
        {Array.from({ length: 6 }).map((_, i) => (
          <div key={i} className="h-28 rounded-xl bg-muted/40 animate-pulse border border-border/50" />
        ))}
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-4">
      {cards.map((card, i) => {
        const Icon = card.icon;
        return (
          <div
            key={i}
            className={`p-4 rounded-xl border bg-gradient-to-br ${card.gradient} transition-all duration-200 hover:shadow-md backdrop-blur-sm`}
          >
            <div className="flex items-center justify-between">
              <span className="text-xs font-medium text-muted-foreground uppercase tracking-wider">
                {card.title}
              </span>
              <Icon className="w-5 h-5 opacity-80" />
            </div>
            <div className="mt-2 flex items-baseline justify-between">
              <span className="text-2xl font-bold tracking-tight">{card.value}</span>
            </div>
            <p className="mt-1 text-[11px] text-muted-foreground">{card.subtext}</p>
          </div>
        );
      })}
    </div>
  );
}
