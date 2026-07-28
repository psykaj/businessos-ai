"use client";

import { ExpenseDto, FinancePaymentDto } from "@/types/finance";
import { ArrowDownLeft, ArrowUpRight, CheckCircle2, Clock } from "lucide-react";
import Link from "next/link";

interface RecentTransactionsProps {
  expenses?: ExpenseDto[];
  payments?: FinancePaymentDto[];
  isLoading?: boolean;
}

export function RecentTransactions({ expenses, payments, isLoading }: RecentTransactionsProps) {
  if (isLoading) {
    return <div className="h-72 bg-card/50 animate-pulse rounded-xl border border-border" />;
  }

  const combined = [
    ...(expenses || []).map((e) => ({
      id: e.id,
      title: e.title,
      type: "Expense",
      amount: -e.amount,
      date: new Date(e.expenseDate).toLocaleDateString(),
      category: e.categoryName,
      status: e.status,
    })),
    ...(payments || []).map((p) => ({
      id: p.id,
      title: p.paymentNumber,
      type: p.paymentType,
      amount: p.paymentType === "Incoming" ? p.amount : -p.amount,
      date: new Date(p.paymentDate).toLocaleDateString(),
      category: p.paymentMethod,
      status: p.status,
    })),
  ].sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()).slice(0, 6);

  return (
    <div className="bg-card border border-border rounded-xl p-5 shadow-sm">
      <div className="flex items-center justify-between mb-4">
        <div>
          <h3 className="text-base font-semibold text-foreground">Recent Transactions</h3>
          <p className="text-xs text-muted-foreground">Latest income collections and operational expenses</p>
        </div>
        <Link href="/dashboard/payments" className="text-xs font-medium text-primary hover:underline">
          View All
        </Link>
      </div>

      {combined.length === 0 ? (
        <div className="text-center py-10 text-muted-foreground text-sm">
          No recent financial transactions found.
        </div>
      ) : (
        <div className="space-y-3">
          {combined.map((tx) => {
            const isPositive = tx.amount > 0;
            return (
              <div key={tx.id} className="flex items-center justify-between p-3 rounded-lg bg-background/50 border border-border/50 hover:border-border transition-colors">
                <div className="flex items-center gap-3">
                  <div className={`p-2 rounded-lg ${isPositive ? "bg-emerald-500/10 text-emerald-500" : "bg-rose-500/10 text-rose-500"}`}>
                    {isPositive ? <ArrowDownLeft className="w-4 h-4" /> : <ArrowUpRight className="w-4 h-4" />}
                  </div>
                  <div>
                    <div className="text-sm font-medium text-foreground">{tx.title}</div>
                    <div className="text-xs text-muted-foreground flex items-center gap-2">
                      <span>{tx.category}</span>
                      <span>•</span>
                      <span>{tx.date}</span>
                    </div>
                  </div>
                </div>

                <div className="text-right">
                  <div className={`text-sm font-semibold ${isPositive ? "text-emerald-500" : "text-foreground"}`}>
                    {isPositive ? `+${new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(tx.amount)}` : new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(tx.amount)}
                  </div>
                  <span className="inline-flex items-center gap-1 text-[10px] text-muted-foreground">
                    <CheckCircle2 className="w-3 h-3 text-emerald-400" /> {tx.status}
                  </span>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}
