"use client";

import { useState } from "react";
import { CreateFinanceInvoiceDto } from "@/types/finance";
import { X, Plus, Trash2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

interface CreateInvoiceModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: CreateFinanceInvoiceDto) => Promise<void>;
}

interface LineItem {
  description: string;
  quantity: number;
  unitPrice: number;
  taxRate: number;
  discountAmount: number;
}

export function CreateInvoiceModal({ isOpen, onClose, onSubmit }: CreateInvoiceModalProps) {
  const [customerName, setCustomerName] = useState("");
  const [customerEmail, setCustomerEmail] = useState("");
  const [invoiceNumber, setInvoiceNumber] = useState(`INV-${new Date().getFullYear()}-${Math.floor(1000 + Math.random() * 9000)}`);
  const [issueDate, setIssueDate] = useState(new Date().toISOString().split("T")[0]);
  const [dueDate, setDueDate] = useState(new Date(Date.now() + 30 * 86400000).toISOString().split("T")[0]);
  const [notes, setNotes] = useState("Thank you for your business!");
  const [items, setItems] = useState<LineItem[]>([
    { description: "Professional Services / Consulting", quantity: 1, unitPrice: 1500, taxRate: 0, discountAmount: 0 },
  ]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  if (!isOpen) return null;

  const addItem = () => {
    setItems([...items, { description: "", quantity: 1, unitPrice: 0, taxRate: 0, discountAmount: 0 }]);
  };

  const removeItem = (index: number) => {
    if (items.length === 1) return;
    setItems(items.filter((_, i) => i !== index));
  };

  const updateItem = (index: number, field: keyof LineItem, value: any) => {
    const updated = [...items];
    updated[index] = { ...updated[index], [field]: value };
    setItems(updated);
  };

  const calculateSubtotal = () => items.reduce((sum, item) => sum + item.quantity * item.unitPrice, 0);
  const calculateTaxTotal = () => items.reduce((sum, item) => sum + (item.quantity * item.unitPrice - item.discountAmount) * (item.taxRate / 100), 0);
  const calculateTotal = () => calculateSubtotal() + calculateTaxTotal();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!customerName || items.length === 0) return;

    try {
      setIsSubmitting(true);
      await onSubmit({
        invoiceNumber,
        customerName,
        customerEmail,
        issueDate,
        dueDate,
        notes,
        items,
      });
      onClose();
    } catch (err) {
      console.error("Failed to create invoice:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/60 backdrop-blur-sm flex items-center justify-center p-4">
      <div className="bg-card border border-border rounded-xl w-full max-w-2xl shadow-2xl p-6 relative max-h-[90vh] overflow-y-auto">
        <button onClick={onClose} className="absolute right-4 top-4 text-muted-foreground hover:text-foreground">
          <X className="w-5 h-5" />
        </button>

        <h2 className="text-lg font-bold text-foreground mb-4">Create Customer Invoice</h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Invoice Number *</label>
              <Input value={invoiceNumber} onChange={(e) => setInvoiceNumber(e.target.value)} required />
            </div>
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Customer Name *</label>
              <Input placeholder="Acme Corp" value={customerName} onChange={(e) => setCustomerName(e.target.value)} required />
            </div>
          </div>

          <div className="grid grid-cols-3 gap-3">
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Customer Email</label>
              <Input type="email" placeholder="billing@acme.com" value={customerEmail} onChange={(e) => setCustomerEmail(e.target.value)} />
            </div>
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Issue Date</label>
              <Input type="date" value={issueDate} onChange={(e) => setIssueDate(e.target.value)} required />
            </div>
            <div>
              <label className="text-xs font-semibold text-muted-foreground block mb-1">Due Date</label>
              <Input type="date" value={dueDate} onChange={(e) => setDueDate(e.target.value)} required />
            </div>
          </div>

          {/* Line Items */}
          <div className="space-y-2 pt-2">
            <div className="flex items-center justify-between">
              <label className="text-xs font-semibold text-muted-foreground">Line Items</label>
              <Button type="button" variant="outline" size="sm" onClick={addItem} className="text-xs h-7 gap-1">
                <Plus className="w-3.5 h-3.5" /> Add Line Item
              </Button>
            </div>

            {items.map((item, idx) => (
              <div key={idx} className="flex items-center gap-2 bg-background/50 p-2.5 rounded-lg border border-border">
                <Input
                  placeholder="Item Description"
                  value={item.description}
                  onChange={(e) => updateItem(idx, "description", e.target.value)}
                  className="flex-1 text-xs"
                  required
                />
                <Input
                  type="number"
                  placeholder="Qty"
                  value={item.quantity}
                  onChange={(e) => updateItem(idx, "quantity", parseFloat(e.target.value) || 0)}
                  className="w-16 text-xs text-center"
                  required
                />
                <Input
                  type="number"
                  placeholder="Price"
                  value={item.unitPrice}
                  onChange={(e) => updateItem(idx, "unitPrice", parseFloat(e.target.value) || 0)}
                  className="w-24 text-xs"
                  required
                />
                <Input
                  type="number"
                  placeholder="Tax %"
                  value={item.taxRate}
                  onChange={(e) => updateItem(idx, "taxRate", parseFloat(e.target.value) || 0)}
                  className="w-16 text-xs text-center"
                />
                <button type="button" onClick={() => removeItem(idx)} className="text-rose-400 hover:text-rose-300 p-1">
                  <Trash2 className="w-4 h-4" />
                </button>
              </div>
            ))}
          </div>

          {/* Totals Summary */}
          <div className="bg-muted/30 rounded-lg p-3 space-y-1 text-xs text-right font-medium">
            <div className="text-muted-foreground">Subtotal: ${calculateSubtotal().toLocaleString()}</div>
            <div className="text-muted-foreground">Tax: ${calculateTaxTotal().toLocaleString()}</div>
            <div className="text-sm font-bold text-foreground">Total: ${calculateTotal().toLocaleString()}</div>
          </div>

          <div>
            <label className="text-xs font-semibold text-muted-foreground block mb-1">Notes / Terms</label>
            <Input value={notes} onChange={(e) => setNotes(e.target.value)} />
          </div>

          <div className="flex items-center justify-end gap-3 pt-4 border-t border-border">
            <Button type="button" variant="outline" onClick={onClose} className="text-xs">
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting} className="text-xs font-semibold">
              {isSubmitting ? "Creating..." : "Create & Send Invoice"}
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
