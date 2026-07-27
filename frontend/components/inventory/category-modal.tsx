"use client";

import { useState, useEffect } from "react";
import { X, Layers } from "lucide-react";
import { CategoryDto, CreateCategoryDto, UpdateCategoryDto } from "@/types/inventory";

interface CategoryModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: any) => Promise<void>;
  category?: CategoryDto | null;
  categories: CategoryDto[];
  isLoading?: boolean;
}

export function CategoryModal({
  isOpen,
  onClose,
  onSubmit,
  category,
  categories,
  isLoading,
}: CategoryModalProps) {
  const isEditing = !!category;

  const [formData, setFormData] = useState({
    name: "",
    code: "",
    description: "",
    parentCategoryId: "",
  });

  useEffect(() => {
    if (category) {
      setFormData({
        name: category.name,
        code: category.code,
        description: category.description || "",
        parentCategoryId: category.parentCategoryId || "",
      });
    } else {
      const genCode = `CAT-${Math.floor(100 + Math.random() * 900)}`;
      setFormData({
        name: "",
        code: genCode,
        description: "",
        parentCategoryId: "",
      });
    }
  }, [category, isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const payload = {
      name: formData.name,
      code: formData.code,
      description: formData.description,
      parentCategoryId: formData.parentCategoryId ? formData.parentCategoryId : undefined,
    };
    await onSubmit(payload);
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-md bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <Layers className="w-5 h-5 text-indigo-500" />
            <h2 className="text-lg font-semibold">{isEditing ? "Edit Category" : "Add Category"}</h2>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Category Code</label>
            <input
              type="text"
              required
              value={formData.code}
              onChange={(e) => setFormData({ ...formData, code: e.target.value })}
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
            />
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Category Name</label>
            <input
              type="text"
              required
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder="e.g. Electronics, Raw Materials..."
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
            />
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Parent Category (Optional)</label>
            <select
              value={formData.parentCategoryId}
              onChange={(e) => setFormData({ ...formData, parentCategoryId: e.target.value })}
              className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background"
            >
              <option value="">None (Top-Level Category)</option>
              {categories
                .filter((c) => !category || c.id !== category.id)
                .map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name} ({c.code})
                  </option>
                ))}
            </select>
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Description</label>
            <textarea
              rows={2}
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
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
              {isLoading ? "Saving..." : isEditing ? "Update Category" : "Add Category"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
