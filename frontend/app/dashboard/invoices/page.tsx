"use client";

import { useState } from "react";
import { useInvoices, useCreateInvoice, useRecordInvoicePayment } from "@/hooks/use-finance";
import { InvoiceListTable } from "@/components/invoices/invoice-list-table";
import { CreateInvoiceModal } from "@/components/invoices/create-invoice-modal";
import { CreateFinanceInvoiceDto } from "@/types/finance";
import { FileText, Plus } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function InvoicesPage() {
  const { data: invoices = [], isLoading: isInvoicesLoading } = useInvoices();
  const createInvoiceMutation = useCreateInvoice();
  const recordPaymentMutation = useRecordInvoicePayment();

  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleCreateInvoice = async (dto: CreateFinanceInvoiceDto) => {
    await createInvoiceMutation.mutateAsync(dto);
  };

  const handleRecordPayment = async (id: string, balanceDue: number) => {
    const amountStr = prompt(`Enter payment amount received for invoice (Balance Due: $${balanceDue}):`, balanceDue.toString());
    if (!amountStr) return;
    const amount = parseFloat(amountStr);
    if (isNaN(amount) || amount <= 0) return;

    await recordPaymentMutation.mutateAsync({ id, amount });
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border pb-5">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
            <FileText className="w-7 h-7 text-primary" /> Customer Invoices
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Create professional customer invoices, track due dates, and record payments
          </p>
        </div>

        <Button onClick={() => setIsModalOpen(true)} className="gap-2 text-xs font-semibold">
          <Plus className="w-4 h-4" /> Create Invoice
        </Button>
      </div>

      <InvoiceListTable
        invoices={invoices}
        isLoading={isInvoicesLoading}
        onOpenCreateModal={() => setIsModalOpen(true)}
        onRecordPayment={handleRecordPayment}
      />

      <CreateInvoiceModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleCreateInvoice}
      />
    </div>
  );
}
