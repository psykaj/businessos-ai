"use client";

import { useState } from "react";
import { useExpenses, useExpenseCategories, useCreateExpense, useApproveExpense } from "@/hooks/use-finance";
import { financeService } from "@/lib/finance-service";
import { ExpenseListTable } from "@/components/expenses/expense-list-table";
import { CreateExpenseModal } from "@/components/expenses/create-expense-modal";
import { CreateExpenseDto } from "@/types/finance";
import { CreditCard, Plus } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function ExpensesPage() {
  const { data: expenses = [], isLoading: isExpensesLoading } = useExpenses();
  const { data: categories = [] } = useExpenseCategories();
  const createExpenseMutation = useCreateExpense();
  const approveExpenseMutation = useApproveExpense();

  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleCreateExpense = async (dto: CreateExpenseDto) => {
    await createExpenseMutation.mutateAsync(dto);
  };

  const handleApproveExpense = async (id: string) => {
    await approveExpenseMutation.mutateAsync(id);
  };

  const handleUploadReceipt = async (file: File) => {
    const res = await financeService.uploadReceipt(file);
    return res.url;
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border pb-5">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
            <CreditCard className="w-7 h-7 text-primary" /> Expense Management
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Record, categorize, approve, and attach receipt documentation for SME expenses
          </p>
        </div>

        <Button onClick={() => setIsModalOpen(true)} className="gap-2 text-xs font-semibold">
          <Plus className="w-4 h-4" /> Add Expense
        </Button>
      </div>

      <ExpenseListTable
        expenses={expenses}
        categories={categories}
        isLoading={isExpensesLoading}
        onApprove={handleApproveExpense}
        onOpenCreateModal={() => setIsModalOpen(true)}
      />

      <CreateExpenseModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        categories={categories}
        onSubmit={handleCreateExpense}
        onUploadReceipt={handleUploadReceipt}
      />
    </div>
  );
}
