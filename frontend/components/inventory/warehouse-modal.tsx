"use client";

import { useState, useEffect } from "react";
import { X, Building2 } from "lucide-react";
import { WarehouseDto, CreateWarehouseDto, UpdateWarehouseDto } from "@/types/inventory";

interface WarehouseModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: any) => Promise<void>;
  warehouse?: WarehouseDto | null;
  isLoading?: boolean;
}

export function WarehouseModal({
  isOpen,
  onClose,
  onSubmit,
  warehouse,
  isLoading,
}: WarehouseModalProps) {
  const isEditing = !!warehouse;

  const [formData, setFormData] = useState({
    name: "",
    code: "",
    address: "",
    city: "",
    state: "",
    country: "",
    postalCode: "",
    managerName: "",
    managerPhone: "",
    isPrimary: false,
    isActive: true,
  });

  useEffect(() => {
    if (warehouse) {
      setFormData({
        name: warehouse.name,
        code: warehouse.code,
        address: warehouse.address || "",
        city: warehouse.city || "",
        state: warehouse.state || "",
        country: warehouse.country || "",
        postalCode: warehouse.postalCode || "",
        managerName: warehouse.managerName || "",
        managerPhone: warehouse.managerPhone || "",
        isPrimary: warehouse.isPrimary,
        isActive: warehouse.isActive,
      });
    } else {
      const genCode = `WH-${Math.floor(10 + Math.random() * 90)}`;
      setFormData({
        name: "",
        code: genCode,
        address: "",
        city: "",
        state: "",
        country: "",
        postalCode: "",
        managerName: "",
        managerPhone: "",
        isPrimary: false,
        isActive: true,
      });
    }
  }, [warehouse, isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (isEditing) {
      const updatePayload: UpdateWarehouseDto = { ...formData };
      await onSubmit(updatePayload);
    } else {
      const createPayload: CreateWarehouseDto = { ...formData };
      await onSubmit(createPayload);
    }
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-lg bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <Building2 className="w-5 h-5 text-indigo-500" />
            <h2 className="text-lg font-semibold">{isEditing ? "Edit Warehouse" : "Add Warehouse"}</h2>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Warehouse Code</label>
              <input
                type="text"
                required
                value={formData.code}
                onChange={(e) => setFormData({ ...formData, code: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Warehouse Name</label>
              <input
                type="text"
                required
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                placeholder="e.g. North Distribution Hub"
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Manager Name</label>
              <input
                type="text"
                value={formData.managerName}
                onChange={(e) => setFormData({ ...formData, managerName: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Manager Phone</label>
              <input
                type="tel"
                value={formData.managerPhone}
                onChange={(e) => setFormData({ ...formData, managerPhone: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Street Address</label>
            <input
              type="text"
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
            />
          </div>

          <div className="grid grid-cols-3 gap-3">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">City</label>
              <input
                type="text"
                value={formData.city}
                onChange={(e) => setFormData({ ...formData, city: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">State</label>
              <input
                type="text"
                value={formData.state}
                onChange={(e) => setFormData({ ...formData, state: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Country</label>
              <input
                type="text"
                value={formData.country}
                onChange={(e) => setFormData({ ...formData, country: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
              />
            </div>
          </div>

          <div className="flex items-center gap-2 pt-2">
            <input
              type="checkbox"
              id="isPrimary"
              checked={formData.isPrimary}
              onChange={(e) => setFormData({ ...formData, isPrimary: e.target.checked })}
              className="rounded border-input text-indigo-600 focus:ring-indigo-500"
            />
            <label htmlFor="isPrimary" className="text-xs font-medium text-foreground cursor-pointer">
              Set as Primary Default Warehouse
            </label>
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
              {isLoading ? "Saving..." : isEditing ? "Update Warehouse" : "Add Warehouse"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
