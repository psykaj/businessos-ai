"use client";

import { useState } from "react";
import { ExpenseDto, ExpenseCategoryDto } from "@/types/finance";
import { Search, Filter, Plus, FileText, CheckCircle2, Clock, XCircle, ArrowUpDown, Tag } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

interface ExpenseListTableProps {
  expenses: ExpenseDto[];
  categories: ExpenseCategoryDto[];
  isLoading: boolean;
  onApprove?: (id: string) => void;
  onOpenCreateModal: () => void;
}

export function ExpenseListTable({ expenses, categories, isLoading, onApprove, onOpenCreateModal }: ExpenseListTableProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<string>("ALL");
  const [selectedStatus, setSelectedStatus] = useState<string>("ALL");

  const filteredExpenses = expenses.filter((e) => {
    const matchesSearch = e.title.toLowerCase().includes(searchTerm.toLowerCase()) || 
                          (e.vendorName && e.vendorName.toLowerCase().includes(searchTerm.toLowerCase()));
    const matchesCategory = selectedCategory === "ALL" || e.expenseCategoryId === selectedCategory;
    const matchesStatus = selectedStatus === "ALL" || e.status === selectedStatus;
    return matchesSearch && matchesCategory && matchesStatus;
  });

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Approved":
      case "Paid":
        return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-emerald-500/10 text-emerald-500 font-medium border border-emerald-500/20"><CheckCircle2 className="w-3 h-3" /> {status}</span>;
      case "PendingApproval":
        return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-amber-500/10 text-amber-500 font-medium border border-amber-500/20"><Clock className="w-3 h-3" /> Pending</span>;
      case "Rejected":
        return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-rose-500/10 text-rose-500 font-medium border border-rose-500/20"><XCircle className="w-3 h-3" /> Rejected</span>;
      default:
        return <span className="inline-flex items-center gap-1 text-xs px-2.5 py-0.5 rounded-full bg-gray-500/10 text-gray-400 font-medium border border-gray-500/20">{status}</span>;
    }
  };

  return (
    <div className="bg-card border border-border rounded-xl shadow-sm overflow-hidden">
      {/* Header controls */}
      <div className="p-4 border-b border-border flex flex-col md:flex-row items-center justify-between gap-4">
        <div className="relative w-full md:w-72">
          <Search className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search expenses or vendors..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="pl-9 bg-background"
          />
        </div>

        <div className="flex flex-wrap items-center gap-3 w-full md:w-auto justify-end">
          <select
            value={selectedCategory}
            onChange={(e) => setSelectedCategory(e.target.value)}
            className="bg-background border border-border rounded-lg px-3 py-2 text-xs font-medium text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
          >
            <option value="ALL">All Categories</option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>{c.name}</option>
            ))}
          </select>

          <select
            value={selectedStatus}
            onChange={(e) => setSelectedStatus(e.target.value)}
            className="bg-background border border-border rounded-lg px-3 py-2 text-xs font-medium text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
          >
            <option value="ALL">All Statuses</option>
            <option value="Paid">Paid</option>
            <option value="Approved">Approved</option>
            <option value="PendingApproval">Pending Approval</option>
            <option value="Rejected">Rejected</option>
          </select>

          <Button onClick={onOpenCreateModal} className="gap-2 text-xs font-semibold">
            <Plus className="w-4 h-4" /> Add Expense
          </Button>
        </div>
      </div>

      {/* Table */}
      <div className="overflow-x-auto">
        <table className="w-full text-left text-sm">
          <thead className="bg-muted/50 text-xs font-semibold uppercase text-muted-foreground border-b border-border">
            <tr>
              <th className="p-4">Expense Title</th>
              <th className="p-4">Category</th>
              <th className="p-4">Vendor</th>
              <th className="p-4">Date</th>
              <th className="p-4">Payment Method</th>
              <th className="p-4 text-right">Amount</th>
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
            ) : filteredExpenses.length === 0 ? (
              <tr>
                <td colSpan={8} className="p-8 text-center text-muted-foreground">
                  No expense records match the specified filters.
                </td>
              </tr>
            ) : (
              filteredExpenses.map((exp) => (
                <tr key={exp.id} className="hover:bg-muted/30 transition-colors">
                  <td className="p-4">
                    <div className="font-medium text-foreground">{exp.title}</div>
                    {exp.isRecurring && (
                      <span className="text-[10px] text-primary font-semibold flex items-center gap-1 mt-0.5">
                        <Tag className="w-3 h-3" /> Recurring ({exp.recurringInterval})
                      </span>
                    )}
                  </td>
                  <td className="p-4">
                    <span className="inline-flex items-center px-2.5 py-1 rounded-md text-xs font-medium bg-primary/10 text-primary">
                      {exp.categoryName}
                    </span>
                  </td>
                  <td className="p-4 text-muted-foreground">{exp.vendorName || "—"}</td>
                  <td className="p-4 text-muted-foreground">{new Date(exp.expenseDate).toLocaleDateString()}</td>
                  <td className="p-4 text-muted-foreground">{exp.paymentMethod}</td>
                  <td className="p-4 text-right font-semibold text-foreground">
                    {new Intl.NumberFormat("en-US", { style: "currency", currency: exp.currency || "USD" }).format(exp.amount)}
                  </td>
                  <td className="p-4 text-center">{getStatusBadge(exp.status)}</td>
                  <td className="p-4 text-right space-x-2">
                    {exp.receiptUrl && (
                      <a href={exp.receiptUrl} target="_blank" rel="noopener noreferrer" className="inline-flex items-center gap-1 text-xs text-primary hover:underline">
                        <FileText className="w-3.5 h-3.5" /> Receipt
                      </a>
                    )}
                    {exp.status === "PendingApproval" && onApprove && (
                      <Button size="sm" variant="outline" className="text-xs h-7 px-2" onClick={() => onApprove(exp.id)}>
                        Approve
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
