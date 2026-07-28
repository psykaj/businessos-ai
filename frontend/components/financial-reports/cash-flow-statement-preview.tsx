"use client";

import { CashFlowStatementReportDto } from "@/types/finance";
import { Printer } from "lucide-react";
import { Button } from "@/components/ui/button";

interface CashFlowStatementPreviewProps {
  statement?: CashFlowStatementReportDto;
  isLoading?: boolean;
}

export function CashFlowStatementPreview({ statement, isLoading }: CashFlowStatementPreviewProps) {
  if (isLoading) {
    return <div className="h-96 bg-card animate-pulse rounded-xl border border-border" />;
  }

  const fmt = (v?: number) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(v || 0);

  return (
    <div className="bg-card border border-border rounded-xl p-6 shadow-sm">
      <div className="flex items-center justify-between border-b border-border pb-4 mb-6">
        <div>
          <h2 className="text-xl font-bold text-foreground">Statement of Cash Flows</h2>
          <p className="text-xs text-muted-foreground">
            Period: {new Date(statement?.periodStart || Date.now()).toLocaleDateString()} — {new Date(statement?.periodEnd || Date.now()).toLocaleDateString()}
          </p>
        </div>
        <Button variant="outline" size="sm" onClick={() => window.print()} className="text-xs gap-1">
          <Printer className="w-3.5 h-3.5" /> Print / Export PDF
        </Button>
      </div>

      <div className="space-y-4 text-sm">
        <div className="flex justify-between py-2 border-b border-border font-medium">
          <span>Cash flows from Operating Activities</span>
          <span className={(statement?.cashFromOperatingActivities || 0) >= 0 ? "text-emerald-500 font-semibold" : "text-rose-500 font-semibold"}>
            {fmt(statement?.cashFromOperatingActivities)}
          </span>
        </div>

        <div className="flex justify-between py-2 border-b border-border font-medium">
          <span>Cash flows from Investing Activities</span>
          <span className="font-semibold text-foreground">{fmt(statement?.cashFromInvestingActivities)}</span>
        </div>

        <div className="flex justify-between py-2 border-b border-border font-medium">
          <span>Cash flows from Financing Activities</span>
          <span className="font-semibold text-foreground">{fmt(statement?.cashFromFinancingActivities)}</span>
        </div>

        <div className="flex justify-between py-3 border-t-2 border-b-2 border-border bg-muted/30 px-3 font-bold text-foreground">
          <span>Net Increase / Decrease in Cash</span>
          <span className={(statement?.netIncreaseInCash || 0) >= 0 ? "text-emerald-500" : "text-rose-500"}>
            {fmt(statement?.netIncreaseInCash)}
          </span>
        </div>

        <div className="flex justify-between py-2 text-muted-foreground px-3">
          <span>Cash at beginning of period</span>
          <span>{fmt(statement?.beginningCashBalance)}</span>
        </div>

        <div className="flex justify-between py-3 bg-primary/10 px-4 rounded-xl font-black text-foreground text-base">
          <span>Cash & Cash Equivalents at End of Period</span>
          <span className="text-primary">{fmt(statement?.endingCashBalance)}</span>
        </div>
      </div>
    </div>
  );
}
