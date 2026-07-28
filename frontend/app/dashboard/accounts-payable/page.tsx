"use client";

import { useState } from "react";
import { useAccountsPayable, useAPAging, useCreateBill, useRecordBillPayment } from "@/hooks/use-finance";
import { APAgingCards } from "@/components/accounts-payable/ap-aging-cards";
import { CreateBillModal } from "@/components/accounts-payable/create-bill-modal";
import { CreateAccountsPayableDto } from "@/types/finance";
import { ArrowDownLeft, Plus, DollarSign, CheckCircle2, AlertTriangle } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function AccountsPayablePage() {
  const { data: payables = [], isLoading: isPayablesLoading } = useAccountsPayable();
  const { data: aging, isLoading: isAgingLoading } = useAPAging();
  const createBillMutation = useCreateBill();
  const recordBillPaymentMutation = useRecordBillPayment();

  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleCreateBill = async (dto: CreateAccountsPayableDto) => {
    await createBillMutation.mutateAsync(dto);
  };

  const handleRecordPayment = async (id: string, balanceDue: number) => {
    const amountStr = prompt(`Enter payment amount to disburse for bill (Balance Due: $${balanceDue}):`, balanceDue.toString());
    if (!amountStr) return;
    const amount = parseFloat(amountStr);
    if (isNaN(amount) || amount <= 0) return;

    await recordBillPaymentMutation.mutateAsync({ id, amount });
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border pb-5">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
            <ArrowDownLeft className="w-7 h-7 text-amber-400" /> Accounts Payable Workspace
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Manage vendor bills, supplier balances, payment due dates, and aging analysis
          </p>
        </div>

        <Button onClick={() => setIsModalOpen(true)} className="gap-2 text-xs font-semibold">
          <Plus className="w-4 h-4" /> Log Supplier Bill
        </Button>
      </div>

      <APAgingCards aging={aging} isLoading={isAgingLoading} />

      <div className="bg-card border border-border rounded-xl shadow-sm overflow-hidden">
        <div className="p-4 border-b border-border">
          <h3 className="text-base font-semibold text-foreground">Supplier Bills & Payable Ledger</h3>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="bg-muted/50 text-xs font-semibold uppercase text-muted-foreground border-b border-border">
              <tr>
                <th className="p-4">Bill #</th>
                <th className="p-4">Supplier Name</th>
                <th className="p-4">Total Amount</th>
                <th className="p-4">Amount Paid</th>
                <th className="p-4">Balance Due</th>
                <th className="p-4">Due Date</th>
                <th className="p-4 text-center">Status</th>
                <th className="p-4 text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border">
              {isPayablesLoading ? (
                [...Array(5)].map((_, i) => (
                  <tr key={i} className="animate-pulse">
                    <td colSpan={8} className="p-4"><div className="h-4 bg-muted rounded w-full" /></td>
                  </tr>
                ))
              ) : payables.length === 0 ? (
                <tr>
                  <td colSpan={8} className="p-8 text-center text-muted-foreground">
                    No accounts payable bills recorded.
                  </td>
                </tr>
              ) : (
                payables.map((ap) => (
                  <tr key={ap.id} className="hover:bg-muted/30 transition-colors">
                    <td className="p-4 font-mono font-medium text-foreground">{ap.billNumber}</td>
                    <td className="p-4 font-medium text-foreground">{ap.supplierName}</td>
                    <td className="p-4 text-muted-foreground">${ap.totalAmount.toLocaleString()}</td>
                    <td className="p-4 text-emerald-500 font-medium">${ap.amountPaid.toLocaleString()}</td>
                    <td className="p-4 font-bold text-foreground">${ap.balanceDue.toLocaleString()}</td>
                    <td className="p-4 text-muted-foreground">{new Date(ap.dueDate).toLocaleDateString()}</td>
                    <td className="p-4 text-center">
                      {ap.daysOverdue > 0 ? (
                        <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-rose-500/10 text-rose-500 font-medium border border-rose-500/20">
                          <AlertTriangle className="w-3 h-3" /> {ap.daysOverdue} Days Overdue
                        </span>
                      ) : ap.balanceDue <= 0 ? (
                        <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-emerald-500/10 text-emerald-500 font-medium border border-emerald-500/20">
                          <CheckCircle2 className="w-3 h-3" /> Paid
                        </span>
                      ) : (
                        <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-amber-500/10 text-amber-500 font-medium border border-amber-500/20">
                          Current
                        </span>
                      )}
                    </td>
                    <td className="p-4 text-right">
                      {ap.balanceDue > 0 && (
                        <Button
                          size="sm"
                          variant="outline"
                          className="text-xs h-7 px-2 border-emerald-500/30 text-emerald-500 hover:bg-emerald-500/10"
                          onClick={() => handleRecordPayment(ap.id, ap.balanceDue)}
                        >
                          <DollarSign className="w-3.5 h-3.5 mr-1" /> Pay Bill
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

      <CreateBillModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleCreateBill}
      />
    </div>
  );
}
