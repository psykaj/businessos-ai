"use client";

import { useState } from "react";
import { ExpenseCategoryDto, CreateExpenseDto } from "@/types/finance";
import { X, Upload, Check } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

interface CreateExpenseModalProps {
  isOpen: boolean;
  onClose: () => void;
  categories: ExpenseCategoryDto[];
  onSubmit: (dto: CreateExpenseDto) => Promise<void>;
  onUploadReceipt?: (file: File) => Promise<string>;
}

export function CreateExpenseModal({ isOpen, onClose, categories, onSubmit, onUploadReceipt }: CreateExpenseModalProps) {
  const [title, setTitle] = useState("");
  const [amount, setAmount] = useState("");
  const [taxRate, setTaxRate] = useState("0");
  const [expenseCategoryId, setExpenseCategoryId] = useState("");
  const [vendorName, setVendorName] = useState("");
  const [paymentMethod, setPaymentMethod] = useState("BankTransfer");
  const [expenseDate, setExpenseDate] = useState(new Date().toISOString().split("T")[0]);
  const [isRecurring, setIsRecurring] = useState(false);
  const [recurringInterval, setRecurringInterval] = useState("Monthly");
  const [receiptUrl, setReceiptUrl] = useState("");
  const [isUploading, setIsUploading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  if (!isOpen) return null;

  const handleFileUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file || !onUploadReceipt) return;
    try {
      setIsUploading(true);
      const url = await onUploadReceipt(file);
      setReceiptUrl(url);
    } catch (err) {
      console.error("Receipt upload failed:", err);
    } finally {
      setIsUploading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!title || !amount || !expenseCategoryId) return;

    try {
      setIsSubmitting(true);
      const numericAmount = parseFloat(amount);
      const numericTaxRate = parseFloat(taxRate) || 0;
      const taxAmount = (numericAmount * numericTaxRate) / 100;

      await onSubmit({
        title,
        amount: numericAmount,
        taxAmount,
        taxRate: numericTaxRate,
        expenseCategoryId,
        expenseDate,
        vendorName,
        paymentMethod,
        receiptUrl,
        isRecurring,
        recurringInterval: isRecurring ? recurringInterval : undefined,
      });
      onClose();
    } catch (err) {
      console.error("Failed to submit expense:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/60 backdrop-blur-sm flex items-center justify-center p-4">
      <div className="bg-card border border-border rounded-xl w-full max-w-lg shadow-2xl p-6 relative">
        <button onClick={onClose} className="absolute right-4 top-4 text-muted-foreground hover:text-foreground">
          <X className="w-5 h-5" />
        </button>

        <h2 className="text-lg font-bold text-foreground mb-4">Add New Expense</h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="text-xs font-semibold text-muted-foreground block mb-1">Expense Title *</label>
            <Input
              placeholder="e.g. AWS Cloud Hosting Services"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
            />
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Amount ($) *</label>
              <Input
                type="number"
                step="0.01"
                placeholder="0.00"
                value={amount}
                onChange={(e) => setAmount(e.target.value)}
                required
              />
            </div>
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Tax Rate (%)</label>
              <Input
                type="number"
                step="0.1"
                placeholder="0"
                value={taxRate}
                onChange={(e) => setTaxRate(e.target.value)}
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Category *</label>
              <select
                value={expenseCategoryId}
                onChange={(e) => setExpenseCategoryId(e.target.value)}
                className="w-full bg-background border border-border rounded-lg px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
                required
              >
                <option value="">Select Category</option>
                {categories.map((c) => (
                  <option key={c.id} value={c.id}>{c.name}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Payment Method</label>
              <select
                value={paymentMethod}
                onChange={(e) => setPaymentMethod(e.target.value)}
                className="w-full bg-background border border-border rounded-lg px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
              >
                <option value="BankTransfer">Bank Transfer</option>
                <option value="CreditCard">Credit Card</option>
                <option value="Cash">Cash</option>
                <option value="UPI">UPI</option>
                <option value="Check">Check</option>
              </select>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Vendor Name</label>
              <Input
                placeholder="e.g. Amazon Web Services"
                value={vendorName}
                onChange={(e) => setVendorName(e.target.value)}
              />
            </div>
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Date</label>
              <Input
                type="date"
                value={expenseDate}
                onChange={(e) => setExpenseDate(e.target.value)}
              />
            </div>
          </div>

          <div className="flex items-center justify-between p-3 border border-border rounded-lg bg-background/50">
            <div className="flex items-center gap-2">
              <input
                type="checkbox"
                id="recurring"
                checked={isRecurring}
                onChange={(e) => setIsRecurring(e.target.checked)}
                className="rounded border-border"
              />
              <label htmlFor="recurring" className="text-xs font-medium text-foreground cursor-pointer">
                Recurring Expense
              </label>
            </div>

            {isRecurring && (
              <select
                value={recurringInterval}
                onChange={(e) => setRecurringInterval(e.target.value)}
                className="bg-card border border-border rounded px-2 py-1 text-xs text-foreground"
              >
                <option value="Monthly">Monthly</option>
                <option value="Quarterly">Quarterly</option>
                <option value="Yearly">Yearly</option>
              </select>
            )}
          </div>

          <div>
            <label className="text-xs font-semibold text-muted-foreground block mb-1">Receipt Attachment</label>
            <div className="flex items-center gap-3">
              <label className="cursor-pointer inline-flex items-center gap-2 px-3 py-2 border border-border rounded-lg text-xs font-medium bg-muted/40 hover:bg-muted text-foreground transition-colors">
                <Upload className="w-4 h-4 text-primary" /> {isUploading ? "Uploading..." : "Upload Receipt File"}
                <input type="file" onChange={handleFileUpload} className="hidden" accept="image/*,application/pdf" />
              </label>
              {receiptUrl && (
                <span className="text-xs text-emerald-500 flex items-center gap-1">
                  <Check className="w-4 h-4" /> Receipt Uploaded
                </span>
              )}
            </div>
          </div>

          <div className="flex items-center justify-end gap-3 pt-4 border-t border-border">
            <Button type="button" variant="outline" onClick={onClose} className="text-xs">
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting} className="text-xs font-semibold">
              {isSubmitting ? "Saving..." : "Save Expense"}
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
