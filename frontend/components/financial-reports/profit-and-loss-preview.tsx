"use client";

import { ProfitAndLossReportDto } from "@/types/finance";
import { Printer, Download } from "lucide-react";
import { Button } from "@/components/ui/button";

interface ProfitAndLossPreviewProps {
  report?: ProfitAndLossReportDto;
  isLoading?: boolean;
}

export function ProfitAndLossPreview({ report, isLoading }: ProfitAndLossPreviewProps) {
  if (isLoading) {
    return <div className="h-96 bg-card animate-pulse rounded-xl border border-border" />;
  }

  const fmt = (v?: number) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(v || 0);

  const handlePrint = () => {
    window.print();
  };

  return (
    <div className="bg-card border border-border rounded-xl p-6 shadow-sm">
      <div className="flex items-center justify-between border-b border-border pb-4 mb-6">
        <div>
          <h2 className="text-xl font-bold text-foreground">Profit & Loss Statement (P&L)</h2>
          <p className="text-xs text-muted-foreground">
            Period: {new Date(report?.periodStart || Date.now()).toLocaleDateString()} — {new Date(report?.periodEnd || Date.now()).toLocaleDateString()}
          </p>
        </div>
        <div className="flex items-center gap-2">
          <Button variant="outline" size="sm" onClick={handlePrint} className="text-xs gap-1">
            <Printer className="w-3.5 h-3.5" /> Print / Export PDF
          </Button>
        </div>
      </div>

      <div className="space-y-6 text-sm">
        {/* Operating Revenue */}
        <div>
          <div className="flex justify-between font-bold text-foreground py-2 border-b border-border bg-muted/30 px-3 rounded">
            <span>Operating Revenue</span>
            <span>{fmt(report?.operatingRevenue)}</span>
          </div>
          {report?.revenueBreakdown.map((r, i) => (
            <div key={i} className="flex justify-between py-1.5 px-3 text-muted-foreground border-b border-border/40">
              <span className="pl-4">{r.categoryName}</span>
              <span>{fmt(r.amount)}</span>
            </div>
          ))}
        </div>

        {/* Cost of Goods Sold */}
        <div>
          <div className="flex justify-between font-semibold text-foreground py-2 border-b border-border px-3">
            <span>Cost of Goods Sold (COGS)</span>
            <span className="text-rose-500">({fmt(report?.costOfGoodsSold)})</span>
          </div>
        </div>

        {/* Gross Profit */}
        <div className="flex justify-between font-extrabold text-foreground py-3 border-t-2 border-b-2 border-border bg-primary/10 px-3 rounded-lg text-base">
          <span>Gross Profit</span>
          <span className="text-emerald-500">{fmt(report?.grossProfit)}</span>
        </div>

        {/* Operating Expenses */}
        <div>
          <div className="flex justify-between font-bold text-foreground py-2 border-b border-border bg-muted/30 px-3 rounded">
            <span>Operating Expenses</span>
            <span className="text-rose-500">({fmt(report?.operatingExpenses)})</span>
          </div>
          {report?.expenseBreakdown.map((e, i) => (
            <div key={i} className="flex justify-between py-1.5 px-3 text-muted-foreground border-b border-border/40">
              <span className="pl-4">{e.categoryName}</span>
              <span>{fmt(e.amount)}</span>
            </div>
          ))}
        </div>

        {/* Net Income */}
        <div className="flex justify-between font-black text-foreground py-4 border-t-2 border-b-2 border-primary bg-card px-4 rounded-xl text-lg shadow-inner">
          <span>Net Operating Income</span>
          <span className={(report?.netIncome || 0) >= 0 ? "text-emerald-500" : "text-rose-500"}>
            {fmt(report?.netIncome)}
          </span>
        </div>
      </div>
    </div>
  );
}
