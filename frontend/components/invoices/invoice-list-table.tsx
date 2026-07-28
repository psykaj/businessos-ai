"use client";

import { useState } from "react";
import { FinanceInvoiceDto } from "@/types/finance";
import { Search, Plus, CheckCircle2, Clock, AlertTriangle, FileText, DollarSign } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

interface InvoiceListTableProps {
  invoices: FinanceInvoiceDto[];
  isLoading: boolean;
  onOpenCreateModal: () => void;
  onRecordPayment: (id: string, balanceDue: number) => void;
}

export function InvoiceListTable({ invoices, isLoading, onOpenCreateModal, onRecordPayment }: InvoiceListTableProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState<string>("ALL");

  const filteredInvoices = invoices.filter((i) => {
    const matchesSearch = i.invoiceNumber.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          i.customerName.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = statusFilter === "ALL" || i.status === statusFilter;
    return matchesSearch && matchesStatus;
  });

  const getStatusBadge = (status: string, dueDate: string) => {
    const isOverdue = new Date(dueDate) < new Date() && status !== "Paid";
    if (isOverdue || status === "Overdue") {
      return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-rose-500/10 text-rose-500 font-medium border border-rose-500/20"><AlertTriangle className="w-3 h-3" /> Overdue</span>;
    }
    switch (status) {
      case "Paid":
        return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-emerald-500/10 text-emerald-500 font-medium border border-emerald-500/20"><CheckCircle2 className="w-3 h-3" /> Paid</span>;
      case "Partial":
        return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-amber-500/10 text-amber-500 font-medium border border-amber-500/20"><Clock className="w-3 h-3" /> Partial</span>;
      default:
        return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-blue-500/10 text-blue-500 font-medium border border-blue-500/20">{status}</span>;
    }
  };

  return (
    <div className="bg-card border border-border rounded-xl shadow-sm overflow-hidden">
      <div className="p-4 border-b border-border flex flex-col md:flex-row items-center justify-between gap-4">
        <div className="relative w-full md:w-72">
          <Search className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search invoice # or customer..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="pl-9 bg-background"
          />
        </div>

        <div className="flex items-center gap-3 w-full md:w-auto justify-end">
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="bg-background border border-border rounded-lg px-3 py-2 text-xs font-medium text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
          >
            <option value="ALL">All Statuses</option>
            <option value="Sent">Sent</option>
            <option value="Partial">Partial</option>
            <option value="Paid">Paid</option>
            <option value="Overdue">Overdue</option>
          </select>

          <Button onClick={onOpenCreateModal} className="gap-2 text-xs font-semibold">
            <Plus className="w-4 h-4" /> Create Invoice
          </Button>
        </div>
      </div>

      <div className="overflow-x-auto">
        <table className="w-full text-left text-sm">
          <thead className="bg-muted/50 text-xs font-semibold uppercase text-muted-foreground border-b border-border">
            <tr>
              <th className="p-4">Invoice #</th>
              <th className="p-4">Customer</th>
              <th className="p-4">Issue Date</th>
              <th className="p-4">Due Date</th>
              <th className="p-4 text-right">Total Amount</th>
              <th className="p-4 text-right">Balance Due</th>
              <th className="p-4 text-center">Status</th>
              <th className="p-4 text-right">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-border">
            {isLoading ? (
              [...Array(5)].map((_, i) => (
                <tr key={i} className="animate-pulse">
                  <td colSpan={8} className="p-4"><div className="h-4 bg-muted rounded w-full" /></td>
                </tr>
              ))
            ) : filteredInvoices.length === 0 ? (
              <tr>
                <td colSpan={8} className="p-8 text-center text-muted-foreground">
                  No invoices found.
                </td>
              </tr>
            ) : (
              filteredInvoices.map((inv) => (
                <tr key={inv.id} className="hover:bg-muted/30 transition-colors">
                  <td className="p-4 font-mono font-medium text-foreground">{inv.invoiceNumber}</td>
                  <td className="p-4">
                    <div className="font-medium text-foreground">{inv.customerName}</div>
                    <div className="text-xs text-muted-foreground">{inv.customerEmail || "No email"}</div>
                  </td>
                  <td className="p-4 text-muted-foreground">{new Date(inv.issueDate).toLocaleDateString()}</td>
                  <td className="p-4 text-muted-foreground">{new Date(inv.dueDate).toLocaleDateString()}</td>
                  <td className="p-4 text-right font-medium text-foreground">
                    {new Intl.NumberFormat("en-US", { style: "currency", currency: inv.currency || "USD" }).format(inv.totalAmount)}
                  </td>
                  <td className="p-4 text-right font-bold text-foreground">
                    {new Intl.NumberFormat("en-US", { style: "currency", currency: inv.currency || "USD" }).format(inv.balanceDue)}
                  </td>
                  <td className="p-4 text-center">{getStatusBadge(inv.status, inv.dueDate)}</td>
                  <td className="p-4 text-right space-x-2">
                    {inv.balanceDue > 0 && (
                      <Button
                        size="sm"
                        variant="outline"
                        className="text-xs h-7 px-2 border-emerald-500/30 text-emerald-500 hover:bg-emerald-500/10"
                        onClick={() => onRecordPayment(inv.id, inv.balanceDue)}
                      >
                        <DollarSign className="w-3.5 h-3.5 mr-1" /> Record Payment
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
