"use client";

import { TaxSummaryDto } from "@/types/finance";
import { ShieldCheck, Receipt } from "lucide-react";

interface TaxSummaryPreviewProps {
  taxSummary?: TaxSummaryDto;
  isLoading?: boolean;
}

export function TaxSummaryPreview({ taxSummary, isLoading }: TaxSummaryPreviewProps) {
  if (isLoading) {
    return <div className="h-64 bg-card animate-pulse rounded-xl border border-border" />;
  }

  const fmt = (v?: number) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(v || 0);

  return (
    <div className="bg-card border border-border rounded-xl p-6 shadow-sm">
      <div className="flex items-center gap-2 border-b border-border pb-4 mb-4">
        <Receipt className="w-5 h-5 text-primary" />
        <h2 className="text-xl font-bold text-foreground">Tax Liability & Summary</h2>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
        <div className="p-4 rounded-lg bg-emerald-500/10 border border-emerald-500/20">
          <div className="text-xs text-emerald-400 font-medium mb-1">Total Output Tax Collected (Sales)</div>
          <div className="text-xl font-bold text-emerald-400">{fmt(taxSummary?.totalTaxCollected)}</div>
        </div>

        <div className="p-4 rounded-lg bg-blue-500/10 border border-blue-500/20">
          <div className="text-xs text-blue-400 font-medium mb-1">Total Input Tax Paid (Purchases)</div>
          <div className="text-xl font-bold text-blue-400">{fmt(taxSummary?.totalTaxPaid)}</div>
        </div>

        <div className="p-4 rounded-lg bg-amber-500/10 border border-amber-500/20">
          <div className="text-xs text-amber-400 font-medium mb-1">Net Tax Payable / Liability</div>
          <div className="text-xl font-bold text-amber-400">{fmt(taxSummary?.netTaxLiability)}</div>
        </div>
      </div>

      <h4 className="text-xs font-semibold text-muted-foreground uppercase mb-2">Tax Code Breakdown</h4>
      <div className="border border-border rounded-lg overflow-hidden">
        <table className="w-full text-left text-xs">
          <thead className="bg-muted/40 font-semibold text-muted-foreground border-b border-border">
            <tr>
              <th className="p-3">Tax Code</th>
              <th className="p-3">Tax Name</th>
              <th className="p-3 text-center">Rate</th>
              <th className="p-3 text-right">Tax Collected</th>
              <th className="p-3 text-right">Tax Paid</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-border">
            {taxSummary?.taxBreakdowns.map((tb, idx) => (
              <tr key={idx} className="hover:bg-muted/20">
                <td className="p-3 font-mono font-medium">{tb.taxCode}</td>
                <td className="p-3 font-medium text-foreground">{tb.taxName}</td>
                <td className="p-3 text-center">{tb.rate}%</td>
                <td className="p-3 text-right text-emerald-400">{fmt(tb.taxCollected)}</td>
                <td className="p-3 text-right text-blue-400">{fmt(tb.taxPaid)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
