"use client";

import { useState } from "react";
import { CreateAccountsPayableDto } from "@/types/finance";
import { X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

interface CreateBillModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: CreateAccountsPayableDto) => Promise<void>;
}

export function CreateBillModal({ isOpen, onClose, onSubmit }: CreateBillModalProps) {
  const [billNumber, setBillNumber] = useState(`BILL-${new Date().getFullYear()}-${Math.floor(1000 + Math.random() * 9000)}`);
  const [supplierName, setSupplierName] = useState("");
  const [totalAmount, setTotalAmount] = useState("");
  const [dueDate, setDueDate] = useState(new Date(Date.now() + 30 * 86400000).toISOString().split("T")[0]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!billNumber || !supplierName || !totalAmount) return;

    try {
      setIsSubmitting(true);
      await onSubmit({
        billNumber,
        supplierName,
        totalAmount: parseFloat(totalAmount),
        dueDate,
      });
      onClose();
    } catch (err) {
      console.error("Failed to log supplier bill:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/60 backdrop-blur-sm flex items-center justify-center p-4">
      <div className="bg-card border border-border rounded-xl w-full max-w-md shadow-2xl p-6 relative">
        <button onClick={onClose} className="absolute right-4 top-4 text-muted-foreground hover:text-foreground">
          <X className="w-5 h-5" />
        </button>

        <h2 className="text-lg font-bold text-foreground mb-4">Log Supplier Bill</h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="text-xs font-semibold text-muted-foreground block mb-1">Bill Number *</label>
            <Input value={billNumber} onChange={(e) => setBillNumber(e.target.value)} required />
          </div>

          <div>
            <label className="text-xs font-semibold text-muted-foreground block mb-1">Supplier Name *</label>
            <Input placeholder="e.g. Dell Logistics" value={supplierName} onChange={(e) => setSupplierName(e.target.value)} required />
          </div>

          <div>
            <label className="text-xs font-semibold text-muted-foreground block mb-1">Total Bill Amount ($) *</label>
            <Input type="number" step="0.01" placeholder="0.00" value={totalAmount} onChange={(e) => setTotalAmount(e.target.value)} required />
          </div>

          <div>
            <label className="text-xs font-semibold text-muted-foreground block mb-1">Payment Due Date</label>
            <Input type="date" value={dueDate} onChange={(e) => setDueDate(e.target.value)} required />
          </div>

          <div className="flex items-center justify-end gap-3 pt-4 border-t border-border">
            <Button type="button" variant="outline" onClick={onClose} className="text-xs">
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting} className="text-xs font-semibold">
              {isSubmitting ? "Logging..." : "Log Bill"}
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
