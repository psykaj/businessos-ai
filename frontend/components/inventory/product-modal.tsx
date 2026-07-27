"use client";

import { useState, useEffect } from "react";
import { X, Sparkles } from "lucide-react";
import { ProductDto, CreateProductDto, UpdateProductDto, CategoryDto } from "@/types/inventory";

interface ProductModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: any) => Promise<void>;
  product?: ProductDto | null;
  categories: CategoryDto[];
  isLoading?: boolean;
}

export function ProductModal({
  isOpen,
  onClose,
  onSubmit,
  product,
  categories,
  isLoading,
}: ProductModalProps) {
  const isEditing = !!product;

  const [formData, setFormData] = useState({
    sku: "",
    name: "",
    description: "",
    barcode: "",
    qrCode: "",
    categoryId: "",
    unitOfMeasure: "PCS",
    costPrice: 0,
    sellingPrice: 0,
    reorderPoint: 10,
    safetyStock: 5,
    reorderQuantity: 50,
    minimumStockLevel: 5,
    maxStockLevel: 500,
    isActive: true,
  });

  useEffect(() => {
    if (product) {
      setFormData({
        sku: product.sku,
        name: product.name,
        description: product.description || "",
        barcode: product.barcode || "",
        qrCode: product.qrCode || "",
        categoryId: product.categoryId,
        unitOfMeasure: product.unitOfMeasure,
        costPrice: product.costPrice,
        sellingPrice: product.sellingPrice,
        reorderPoint: product.reorderPoint,
        safetyStock: product.safetyStock,
        reorderQuantity: product.reorderQuantity,
        minimumStockLevel: product.minimumStockLevel,
        maxStockLevel: product.maxStockLevel,
        isActive: product.isActive,
      });
    } else {
      const generatedSKU = `SKU-${Math.floor(100000 + Math.random() * 900000)}`;
      setFormData({
        sku: generatedSKU,
        name: "",
        description: "",
        barcode: `890${Math.floor(1000000009 + Math.random() * 9000000000)}`,
        qrCode: "",
        categoryId: categories[0]?.id || "",
        unitOfMeasure: "PCS",
        costPrice: 0,
        sellingPrice: 0,
        reorderPoint: 10,
        safetyStock: 5,
        reorderQuantity: 50,
        minimumStockLevel: 5,
        maxStockLevel: 500,
        isActive: true,
      });
    }
  }, [product, categories, isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (isEditing) {
      const updatePayload: UpdateProductDto = {
        name: formData.name,
        description: formData.description,
        barcode: formData.barcode,
        qrCode: formData.qrCode,
        categoryId: formData.categoryId,
        unitOfMeasure: formData.unitOfMeasure,
        costPrice: Number(formData.costPrice),
        sellingPrice: Number(formData.sellingPrice),
        reorderPoint: Number(formData.reorderPoint),
        safetyStock: Number(formData.safetyStock),
        reorderQuantity: Number(formData.reorderQuantity),
        minimumStockLevel: Number(formData.minimumStockLevel),
        maxStockLevel: Number(formData.maxStockLevel),
        isActive: formData.isActive,
      };
      await onSubmit(updatePayload);
    } else {
      const createPayload: CreateProductDto = {
        sku: formData.sku,
        name: formData.name,
        description: formData.description,
        barcode: formData.barcode,
        qrCode: formData.qrCode,
        categoryId: formData.categoryId,
        unitOfMeasure: formData.unitOfMeasure,
        costPrice: Number(formData.costPrice),
        sellingPrice: Number(formData.sellingPrice),
        reorderPoint: Number(formData.reorderPoint),
        safetyStock: Number(formData.safetyStock),
        reorderQuantity: Number(formData.reorderQuantity),
        minimumStockLevel: Number(formData.minimumStockLevel),
        maxStockLevel: Number(formData.maxStockLevel),
      };
      await onSubmit(createPayload);
    }
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-2xl bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <Sparkles className="w-5 h-5 text-indigo-500" />
            <h2 className="text-lg font-semibold">{isEditing ? "Edit Product" : "Add New Product"}</h2>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4 max-h-[80vh] overflow-y-auto">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">SKU Code</label>
              <input
                type="text"
                required
                disabled={isEditing}
                value={formData.sku}
                onChange={(e) => setFormData({ ...formData, sku: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50 disabled:opacity-50"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Product Name</label>
              <input
                type="text"
                required
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                placeholder="e.g. Ergonomic Office Chair"
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              />
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Category</label>
              <select
                required
                value={formData.categoryId}
                onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              >
                <option value="">Select Category</option>
                {categories.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Unit of Measure</label>
              <select
                value={formData.unitOfMeasure}
                onChange={(e) => setFormData({ ...formData, unitOfMeasure: e.target.value })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              >
                <option value="PCS">Pieces (PCS)</option>
                <option value="BOX">Box (BOX)</option>
                <option value="KG">Kilogram (KG)</option>
                <option value="L">Litre (L)</option>
                <option value="SET">Set (SET)</option>
              </select>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Cost Price ($)</label>
              <input
                type="number"
                step="0.01"
                min="0"
                required
                value={formData.costPrice}
                onChange={(e) => setFormData({ ...formData, costPrice: Number(e.target.value) })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              />
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Selling Price ($)</label>
              <input
                type="number"
                step="0.01"
                min="0"
                required
                value={formData.sellingPrice}
                onChange={(e) => setFormData({ ...formData, sellingPrice: Number(e.target.value) })}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 md:grid-cols-4 gap-3 p-4 rounded-xl border border-border/50 bg-muted/20">
            <div>
              <label className="block text-[11px] font-medium text-muted-foreground mb-1">Reorder Point</label>
              <input
                type="number"
                min="0"
                value={formData.reorderPoint}
                onChange={(e) => setFormData({ ...formData, reorderPoint: Number(e.target.value) })}
                className="w-full px-2 py-1.5 text-xs rounded-md border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-[11px] font-medium text-muted-foreground mb-1">Safety Stock</label>
              <input
                type="number"
                min="0"
                value={formData.safetyStock}
                onChange={(e) => setFormData({ ...formData, safetyStock: Number(e.target.value) })}
                className="w-full px-2 py-1.5 text-xs rounded-md border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-[11px] font-medium text-muted-foreground mb-1">Reorder Qty</label>
              <input
                type="number"
                min="1"
                value={formData.reorderQuantity}
                onChange={(e) => setFormData({ ...formData, reorderQuantity: Number(e.target.value) })}
                className="w-full px-2 py-1.5 text-xs rounded-md border border-input bg-background"
              />
            </div>
            <div>
              <label className="block text-[11px] font-medium text-muted-foreground mb-1">Max Stock</label>
              <input
                type="number"
                min="1"
                value={formData.maxStockLevel}
                onChange={(e) => setFormData({ ...formData, maxStockLevel: Number(e.target.value) })}
                className="w-full px-2 py-1.5 text-xs rounded-md border border-input bg-background"
              />
            </div>
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Barcode / EAN</label>
            <input
              type="text"
              value={formData.barcode}
              onChange={(e) => setFormData({ ...formData, barcode: e.target.value })}
              placeholder="e.g. 890123456789"
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
            />
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Description</label>
            <textarea
              rows={2}
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Detailed product specifications..."
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
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
              {isLoading ? "Saving..." : isEditing ? "Update Product" : "Create Product"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
