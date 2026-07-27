"use client";

import { useState, useEffect } from "react";
import { X, Building } from "lucide-react";
import { SupplierDto, CreateSupplierDto, UpdateSupplierDto } from "@/types/inventory";

interface SupplierModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: any) => Promise<void>;
  supplier?: SupplierDto | null;
  isLoading?: boolean;
}

export function SupplierModal({
  isOpen,
  onClose,
  onSubmit,
  supplier,
  isLoading,
}: SupplierModalProps) {
  const isEditing = !!supplier;

  const [formData, setFormData] = useState({
    name: "",
    code: "",
    contactPerson: "",
    email: "",
    phone: "",
    address: "",
    city: "",
    country: "",
    taxId: "",
    paymentTerms: "Net 30",
    isActive: true,
  });

  useEffect(() => {
    if (supplier) {
      setFormData({
        name: supplier.name,
        code: supplier.code,
        contactPerson: supplier.contactPerson,
        email: supplier.email,
        phone: supplier.phone,
        address: supplier.address || "",
        city: supplier.city || "",
        country: supplier.country || "",
        taxId: supplier.taxId || "",
        paymentTerms: supplier.paymentTerms,
        isActive: supplier.isActive,
      });
    } else {
      const generatedCode = `SUP-${Math.floor(100 + Math.random() * 900)}`;
      setFormData({
        name: "",
        code: generatedCode,
        contactPerson: "",
        email: "",
        phone: "",
        address: "",
        city: "",
        country: "",
        taxId: "",
        paymentTerms: "Net 30",
        isActive: true,
      });
    }
  }, [supplier, isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (isEditing) {
      const updatePayload: UpdateSupplierDto = { ...formData };
      await onSubmit(updatePayload);
    } else {
      const createPayload: CreateSupplierDto = { ...formData };
      await onSubmit(createPayload);
    }
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-xl bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <Building className="w-5 h-5 text-indigo-500" />
            <h2 className="text-lg font-semibold">{isEditing ? "Edit Supplier" : "Register Supplier"}</h2>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Supplier Code</label>
              <input
                type="text"
                required
                value={formData.code}
                onChange={(e) => setFormData({ ...formData, code: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Company Name</label>
              <input
                type="text"
                required
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                placeholder="e.g. Apex Logistics Ltd"
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Contact Person</label>
              <input
                type="text"
                required
                value={formData.contactPerson}
                onChange={(e) => setFormData({ ...formData, contactPerson: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Email Address</label>
              <input
                type="email"
                required
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Phone Number</label>
              <input
                type="tel"
                required
                value={formData.phone}
                onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Payment Terms</label>
              <select
                value={formData.paymentTerms}
                onChange={(e) => setFormData({ ...formData, paymentTerms: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              >
                <option value="Immediate">Immediate / Advance</option>
                <option value="Net 15">Net 15 Days</option>
                <option value="Net 30">Net 30 Days</option>
                <option value="Net 60">Net 60 Days</option>
              </select>
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Tax ID / VAT</label>
              <input
                type="text"
                value={formData.taxId}
                onChange={(e) => setFormData({ ...formData, taxId: e.target.value })}
                placeholder="Tax identification number"
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Address & Location</label>
            <input
              type="text"
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              placeholder="Street Address, City, Country"
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
            />
          </div>

          <div className="flex items-center justify-end gap-3 pt-4 border-t border-border">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 text-sm font-medium rounded-xl border border-input hover:bg-accent"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="px-5 py-2 text-sm font-medium text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 disabled:opacity-50"
            >
              {isLoading ? "Saving..." : isEditing ? "Update Supplier" : "Register Supplier"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
