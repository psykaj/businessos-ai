"use client";

import { AccountsReceivableAgingDto } from "@/types/finance";
import { AlertCircle, CheckCircle2, Clock } from "lucide-react";

interface ARAgingCardsProps {
  aging?: AccountsReceivableAgingDto;
  isLoading?: boolean;
}

export function ARAgingCards({ aging, isLoading }: ARAgingCardsProps) {
  if (isLoading) {
    return <div className="grid grid-cols-2 md:grid-cols-5 gap-3 mb-6 animate-pulse"><div className="h-24 bg-card rounded-xl" /></div>;
  }

  const fmt = (v?: number) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD", maximumFractionDigits: 0 }).format(v || 0);

  const buckets = [
    { title: "Current (Not Due)", value: fmt(aging?.current), color: "border-emerald-500/30 text-emerald-400" },
    { title: "1 - 30 Days Overdue", value: fmt(aging?.days1To30), color: "border-amber-500/30 text-amber-400" },
    { title: "31 - 60 Days Overdue", value: fmt(aging?.days31To60), color: "border-orange-500/30 text-orange-400" },
    { title: "61 - 90 Days Overdue", value: fmt(aging?.days61To90), color: "border-rose-500/30 text-rose-400" },
    { title: "90+ Days Overdue", value: fmt(aging?.days90Plus), color: "border-red-600/40 text-red-500" },
  ];

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-3 mb-6">
      {buckets.map((b, idx) => (
        <div key={idx} className={`p-3.5 rounded-xl bg-card border ${b.color} shadow-sm`}>
          <div className="text-xs text-muted-foreground mb-1 font-medium">{b.title}</div>
          <div className="text-lg font-bold">{b.value}</div>
        </div>
      ))}
    </div>
  );
}
