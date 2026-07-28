"use client";

import { usePayments } from "@/hooks/use-finance";
import { ArrowDownLeft, ArrowUpRight, Receipt, CheckCircle2 } from "lucide-react";

export default function PaymentsPage() {
  const { data: payments = [], isLoading } = usePayments();

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div className="border-b border-border pb-5">
        <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
          <Receipt className="w-7 h-7 text-primary" /> Payment Transactions
        </h1>
        <p className="text-sm text-muted-foreground mt-1">
          Complete audit history of incoming collections and outgoing disbursements
        </p>
      </div>

      <div className="bg-card border border-border rounded-xl shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="bg-muted/50 text-xs font-semibold uppercase text-muted-foreground border-b border-border">
              <tr>
                <th className="p-4">Payment #</th>
                <th className="p-4">Type</th>
                <th className="p-4">Method</th>
                <th className="p-4">Date</th>
                <th className="p-4">Reference</th>
                <th className="p-4 text-right">Amount</th>
                <th className="p-4 text-center">Status</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border">
              {isLoading ? (
                [...Array(5)].map((_, i) => (
                  <tr key={i} className="animate-pulse">
                    <td colSpan={7} className="p-4"><div className="h-4 bg-muted rounded w-full" /></td>
                  </tr>
                ))
              ) : payments.length === 0 ? (
                <tr>
                  <td colSpan={7} className="p-8 text-center text-muted-foreground">
                    No payment transaction records found.
                  </td>
                </tr>
              ) : (
                payments.map((p) => (
                  <tr key={p.id} className="hover:bg-muted/30 transition-colors">
                    <td className="p-4 font-mono font-medium text-foreground">{p.paymentNumber}</td>
                    <td className="p-4">
                      <span className={`inline-flex items-center gap-1 text-xs font-semibold px-2.5 py-0.5 rounded-full ${p.paymentType === "Incoming" ? "bg-emerald-500/10 text-emerald-500" : "bg-rose-500/10 text-rose-500"}`}>
                        {p.paymentType === "Incoming" ? <ArrowDownLeft className="w-3 h-3" /> : <ArrowUpRight className="w-3 h-3" />}
                        {p.paymentType}
                      </span>
                    </td>
                    <td className="p-4 text-muted-foreground">{p.paymentMethod}</td>
                    <td className="p-4 text-muted-foreground">{new Date(p.paymentDate).toLocaleDateString()}</td>
                    <td className="p-4 text-muted-foreground">{p.referenceNumber || "—"}</td>
                    <td className={`p-4 text-right font-bold ${p.paymentType === "Incoming" ? "text-emerald-500" : "text-foreground"}`}>
                      {p.paymentType === "Incoming" ? `+${new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(p.amount)}` : new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(p.amount)}
                    </td>
                    <td className="p-4 text-center">
                      <span className="inline-flex items-center gap-1 text-xs text-emerald-400 font-medium">
                        <CheckCircle2 className="w-3 h-3" /> {p.status}
                      </span>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
