"use client";

import { useState } from "react";
import { Layers, Plus, Edit, RefreshCw, FolderTree, Package } from "lucide-react";
import { useCategories, useCreateCategory, useUpdateCategory } from "@/hooks/use-inventory";
import { CategoryDto } from "@/types/inventory";
import { CategoryModal } from "@/components/inventory/category-modal";

export default function CategoriesPage() {
  const { data: categories = [], isLoading, refetch } = useCategories();
  const createCategoryMutation = useCreateCategory();
  const updateCategoryMutation = useUpdateCategory();

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<CategoryDto | null>(null);

  const handleSubmit = async (dto: any) => {
    if (editingCategory) {
      await updateCategoryMutation.mutateAsync({ id: editingCategory.id, dto });
    } else {
      await createCategoryMutation.mutateAsync(dto);
    }
    refetch();
  };

  return (
    <div className="p-6 md:p-8 space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border/50 pb-5">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold tracking-tight">Product Categories</h1>
          <p className="text-sm text-muted-foreground mt-1">
            Organize catalog items into hierarchical product trees.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <button
            onClick={() => refetch()}
            className="p-2.5 rounded-xl border border-border/60 hover:bg-accent transition-colors text-muted-foreground hover:text-foreground"
          >
            <RefreshCw className="w-4 h-4" />
          </button>
          <button
            onClick={() => {
              setEditingCategory(null);
              setIsModalOpen(true);
            }}
            className="px-4 py-2.5 text-xs font-semibold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Add Category
          </button>
        </div>
      </div>

      {/* Category Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {isLoading ? (
          Array.from({ length: 6 }).map((_, i) => (
            <div key={i} className="h-36 rounded-2xl bg-muted/40 animate-pulse border border-border/50" />
          ))
        ) : categories.length > 0 ? (
          categories.map((c) => (
            <div
              key={c.id}
              className="p-5 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm space-y-3 transition-all hover:border-indigo-500/30"
            >
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2.5">
                  <div className="p-2 rounded-xl bg-indigo-500/10 text-indigo-500">
                    <Layers className="w-4 h-4" />
                  </div>
                  <div>
                    <h3 className="font-semibold text-sm">{c.name}</h3>
                    <span className="font-mono text-[10px] text-muted-foreground">{c.code}</span>
                  </div>
                </div>
                <button
                  onClick={() => {
                    setEditingCategory(c);
                    setIsModalOpen(true);
                  }}
                  className="p-1.5 rounded-lg border border-border/50 hover:bg-accent text-muted-foreground hover:text-foreground"
                >
                  <Edit className="w-3.5 h-3.5" />
                </button>
              </div>

              <p className="text-xs text-muted-foreground line-clamp-2">
                {c.description || "No description provided."}
              </p>

              <div className="flex items-center justify-between pt-2 border-t border-border/40 text-xs">
                <div className="flex items-center gap-1.5 text-muted-foreground">
                  <Package className="w-3.5 h-3.5" />
                  <span>{c.productsCount} Products</span>
                </div>
                {c.parentCategoryName && (
                  <span className="text-[10px] font-medium px-2 py-0.5 rounded bg-muted border border-border text-muted-foreground">
                    Parent: {c.parentCategoryName}
                  </span>
                )}
              </div>
            </div>
          ))
        ) : (
          <div className="col-span-3 p-12 text-center text-xs text-muted-foreground rounded-2xl border border-border">
            No product categories created yet.
          </div>
        )}
      </div>

      {/* Modal */}
      <CategoryModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleSubmit}
        category={editingCategory}
        categories={categories}
        isLoading={createCategoryMutation.isPending || updateCategoryMutation.isPending}
      />
    </div>
  );
}
