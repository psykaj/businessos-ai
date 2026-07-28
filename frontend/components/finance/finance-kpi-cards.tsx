"use client";

import { FinanceOverviewDto } from "@/types/finance";
import { 
  DollarSign, 
  TrendingUp, 
  TrendingDown, 
  Wallet, 
  ArrowUpRight, 
  ArrowDownLeft, 
  FileText, 
  PieChart 
} from "lucide-react";

interface FinanceKpiCardsProps {
  overview?: FinanceOverviewDto;
  isLoading?: boolean;
}

export function FinanceKpiCards({ overview, isLoading }: FinanceKpiCardsProps) {
  if (isLoading) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
        {[...Array(4)].map((_, i) => (
          <div key={i} className="h-32 bg-card/50 animate-pulse rounded-xl border border-border" />
        ))}
      </div>
    );
  }

  const formatCurrency = (val?: number) => {
    return new Intl.NumberFormat("en-US", { style: "currency", currency: "USD", maximumFractionDigits: 0 }).format(val || 0);
  };

  const cards = [
    {
      title: "Cash Available",
      value: formatCurrency(overview?.cashPosition),
      subtitle: "Current Bank Position",
      icon: Wallet,
      color: "text-emerald-500",
      bgColor: "bg-emerald-500/10",
      borderColor: "border-emerald-500/20",
    },
    {
      title: "Total Revenue",
      value: formatCurrency(overview?.totalRevenue),
      subtitle: `${overview?.pendingInvoicesCount || 0} Pending Invoices`,
      icon: TrendingUp,
      color: "text-blue-500",
      bgColor: "bg-blue-500/10",
      borderColor: "border-blue-500/20",
    },
    {
      title: "Total Expenses",
      value: formatCurrency(overview?.totalExpenses),
      subtitle: `${overview?.pendingBillsCount || 0} Pending Bills`,
      icon: TrendingDown,
      color: "text-rose-500",
      bgColor: "bg-rose-500/10",
      borderColor: "border-rose-500/20",
    },
    {
      title: "Net Profit Margin",
      value: `${overview?.profitMarginPercentage || 0}%`,
      subtitle: `Net Income: ${formatCurrency(overview?.netIncome)}`,
      icon: PieChart,
      color: (overview?.profitMarginPercentage || 0) >= 0 ? "text-emerald-400" : "text-rose-400",
      bgColor: (overview?.profitMarginPercentage || 0) >= 0 ? "bg-emerald-500/10" : "bg-rose-500/10",
      borderColor: "border-border",
    },
    {
      title: "Outstanding Receivables",
      value: formatCurrency(overview?.totalReceivables),
      subtitle: "Customer Invoices Due",
      icon: ArrowUpRight,
      color: "text-sky-400",
      bgColor: "bg-sky-500/10",
      borderColor: "border-sky-500/20",
    },
    {
      title: "Outstanding Payables",
      value: formatCurrency(overview?.totalPayables),
      subtitle: "Supplier Bills Due",
      icon: ArrowDownLeft,
      color: "text-amber-400",
      bgColor: "bg-amber-500/10",
      borderColor: "border-amber-500/20",
    },
  ];

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-4 mb-6">
      {cards.map((card, idx) => {
        const Icon = card.icon;
        return (
          <div
            key={idx}
            className={`p-4 rounded-xl bg-card border ${card.borderColor} shadow-sm hover:shadow-md transition-all duration-200`}
          >
            <div className="flex items-center justify-between mb-2">
              <span className="text-xs font-medium text-muted-foreground">{card.title}</span>
              <div className={`p-2 rounded-lg ${card.bgColor}`}>
                <Icon className={`w-4 h-4 ${card.color}`} />
              </div>
            </div>
            <div className="text-xl font-bold text-foreground mb-1">{card.value}</div>
            <div className="text-xs text-muted-foreground flex items-center gap-1">
              {card.subtitle}
            </div>
          </div>
        );
      })}
    </div>
  );
}
