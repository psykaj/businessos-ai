"use client";

import { useAccountsReceivable, useARAging, useSendAROverdueReminder } from "@/hooks/use-finance";
import { ARAgingCards } from "@/components/accounts-receivable/ar-aging-cards";
import { ARListTable } from "@/components/accounts-receivable/ar-list-table";
import { ArrowUpRight } from "lucide-react";

export default function AccountsReceivablePage() {
  const { data: receivables = [], isLoading: isReceivablesLoading } = useAccountsReceivable();
  const { data: aging, isLoading: isAgingLoading } = useARAging();
  const sendReminderMutation = useSendAROverdueReminder();

  const handleSendReminder = async (id: string) => {
    const customNote = prompt("Optional custom note to attach with overdue reminder email:") || undefined;
    await sendReminderMutation.mutateAsync({ id, customNote });
    alert("Overdue payment reminder notification sent to customer.");
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div className="border-b border-border pb-5">
        <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
          <ArrowUpRight className="w-7 h-7 text-sky-400" /> Accounts Receivable Workspace
        </h1>
        <p className="text-sm text-muted-foreground mt-1">
          Monitor outstanding customer receivables, aging breakdown, and overdue collection workflows
        </p>
      </div>

      <ARAgingCards aging={aging} isLoading={isAgingLoading} />

      <ARListTable
        receivables={receivables}
        isLoading={isReceivablesLoading}
        onSendReminder={handleSendReminder}
      />
    </div>
  );
}
