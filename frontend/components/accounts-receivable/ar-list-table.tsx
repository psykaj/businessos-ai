"use client";

import { AccountsReceivableDto } from "@/types/finance";
import { Send, CheckCircle2, Clock, AlertTriangle } from "lucide-react";
import { Button } from "@/components/ui/button";

interface ARListTableProps {
  receivables: AccountsReceivableDto[];
  isLoading: boolean;
  onSendReminder: (id: string) => void;
}

export function ARListTable({ receivables, isLoading, onSendReminder }: ARListTableProps) {
  const getStatusBadge = (status: string, daysOverdue: number) => {
    if (daysOverdue > 0 || status === "Overdue") {
      return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-rose-500/10 text-rose-500 font-medium border border-rose-500/20"><AlertTriangle className="w-3 h-3" /> {daysOverdue} Days Overdue</span>;
    }
    return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-emerald-500/10 text-emerald-500 font-medium border border-emerald-500/20"><CheckCircle2 className="w-3 h-3" /> Current</span>;
  };

  return (
    <div className="bg-card border border-border rounded-xl shadow-sm overflow-hidden">
      <div className="p-4 border-b border-border">
        <h3 className="text-base font-semibold text-foreground">Customer Accounts Receivable Ledger</h3>
        <p className="text-xs text-muted-foreground">Track outstanding receivables, aging, and send payment reminders</p>
      </div>

      <div className="overflow-x-auto">
        <table className="w-full text-left text-sm">
          <thead className="bg-muted/50 text-xs font-semibold uppercase text-muted-foreground border-b border-border">
            <tr>
              <th className="p-4">Customer Name</th>
              <th className="p-4">Total Amount</th>
              <th className="p-4">Amount Paid</th>
              <th className="p-4">Balance Due</th>
              <th className="p-4">Due Date</th>
              <th className="p-4 text-center">Status</th>
              <th className="p-4 text-right">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-border">
            {isLoading ? (
              [...Array(5)].map((_, i) => (
                <tr key={i} className="animate-pulse">
                  <td colSpan={7} className="p-4"><div className="h-4 bg-muted rounded w-full" /></td>
                </tr>
              ))
            ) : receivables.length === 0 ? (
              <tr>
                <td colSpan={7} className="p-8 text-center text-muted-foreground">
                  No accounts receivable records.
                </td>
              </tr>
            ) : (
              receivables.map((ar) => (
                <tr key={ar.id} className="hover:bg-muted/30 transition-colors">
                  <td className="p-4 font-medium text-foreground">{ar.customerName}</td>
                  <td className="p-4 text-muted-foreground">${ar.totalAmount.toLocaleString()}</td>
                  <td className="p-4 text-emerald-500 font-medium">${ar.amountPaid.toLocaleString()}</td>
                  <td className="p-4 font-bold text-foreground">${ar.balanceDue.toLocaleString()}</td>
                  <td className="p-4 text-muted-foreground">{new Date(ar.dueDate).toLocaleDateString()}</td>
                  <td className="p-4 text-center">{getStatusBadge(ar.status, ar.daysOverdue)}</td>
                  <td className="p-4 text-right">
                    {ar.balanceDue > 0 && (
                      <Button
                        size="sm"
                        variant="outline"
                        className="text-xs h-7 gap-1"
                        onClick={() => onSendReminder(ar.id)}
                      >
                        <Send className="w-3 h-3 text-primary" /> Send Reminder
                      </Button>
                    )}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
