"use client";

import { useState } from "react";
import Link from "next/link";
import {
  Package,
  Plus,
  Search,
  Filter,
  QrCode,
  Archive,
  Edit,
  Eye,
  CheckCircle2,
  AlertTriangle,
  XCircle,
  RefreshCw,
} from "lucide-react";
import {
  useProducts,
  useCategories,
  useCreateProduct,
  useUpdateProduct,
  useArchiveProduct,
} from "@/hooks/use-inventory";
import { ProductDto } from "@/types/inventory";
import { ProductModal } from "@/components/inventory/product-modal";
import { ProductBarcodeModal } from "@/components/inventory/product-barcode-modal";

export default function ProductsPage() {
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<string | undefined>(undefined);
  const [lowStockOnly, setLowStockOnly] = useState(false);
  const [pageNumber, setPageNumber] = useState(1);

  const { data: categories = [] } = useCategories();
  const { data: pagedProducts, isLoading, refetch } = useProducts({
    query: searchQuery,
    categoryId: selectedCategory,
    lowStockOnly,
    pageNumber,
    pageSize: 15,
  });

  const createProductMutation = useCreateProduct();
  const updateProductMutation = useUpdateProduct();
  const archiveProductMutation = useArchiveProduct();

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<ProductDto | null>(null);
  const [barcodeProduct, setBarcodeProduct] = useState<ProductDto | null>(null);

  const handleCreateOrUpdate = async (dto: any) => {
    if (editingProduct) {
      await updateProductMutation.mutateAsync({ id: editingProduct.id, dto });
    } else {
      await createProductMutation.mutateAsync(dto);
    }
    refetch();
  };

  const handleArchive = async (id: string) => {
    if (confirm("Are you sure you want to archive this product catalog entry?")) {
      await archiveProductMutation.mutateAsync(id);
      refetch();
    }
  };

  return (
    <div className="p-6 md:p-8 space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border/50 pb-5">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold tracking-tight">Product Catalog & Stock</h1>
          <p className="text-sm text-muted-foreground mt-1">
            Manage product catalog, SKUs, barcode/QR payloads, and inventory parameters.
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
              setEditingProduct(null);
              setIsModalOpen(true);
            }}
            className="px-4 py-2.5 text-xs font-semibold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Add Product
          </button>
        </div>
      </div>

      {/* Search & Filter Bar */}
      <div className="flex flex-col md:flex-row items-center justify-between gap-4 p-4 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md">
        <div className="relative w-full md:w-96">
          <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-muted-foreground" />
          <input
            type="text"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            placeholder="Search by Product Name, SKU, or Barcode..."
            className="w-full pl-9 pr-4 py-2 text-xs rounded-xl border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
          />
        </div>

        <div className="flex flex-wrap items-center gap-3 w-full md:w-auto">
          <select
            value={selectedCategory || ""}
            onChange={(e) => setSelectedCategory(e.target.value || undefined)}
            className="px-3 py-2 text-xs rounded-xl border border-input bg-background"
          >
            <option value="">All Categories</option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>

          <button
            onClick={() => setLowStockOnly(!lowStockOnly)}
            className={`px-3 py-2 text-xs font-medium rounded-xl border transition-colors flex items-center gap-1.5 ${
              lowStockOnly
                ? "bg-amber-500/10 border-amber-500/30 text-amber-500 font-semibold"
                : "border-input hover:bg-accent text-muted-foreground"
            }`}
          >
            <Filter className="w-3.5 h-3.5" /> Low Stock Only
          </button>
        </div>
      </div>

      {/* Product Table */}
      <div className="rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left text-xs">
            <thead className="bg-muted/40 border-b border-border text-muted-foreground uppercase tracking-wider font-semibold text-[10px]">
              <tr>
                <th className="p-4">Product / SKU</th>
                <th className="p-4">Category</th>
                <th className="p-4">Pricing</th>
                <th className="p-4">Available Stock</th>
                <th className="p-4">Reorder Level</th>
                <th className="p-4">Status</th>
                <th className="p-4 text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border/40">
              {isLoading ? (
                Array.from({ length: 5 }).map((_, i) => (
                  <tr key={i} className="animate-pulse">
                    <td colSpan={7} className="p-4">
                      <div className="h-6 bg-muted/30 rounded-lg" />
                    </td>
                  </tr>
                ))
              ) : pagedProducts?.items && pagedProducts.items.length > 0 ? (
                pagedProducts.items.map((product) => {
                  const isLow = product.totalQuantityAvailable <= product.reorderPoint;
                  const isOut = product.totalQuantityAvailable === 0;

                  return (
                    <tr key={product.id} className="hover:bg-muted/20 transition-colors">
                      <td className="p-4">
                        <div className="font-semibold text-foreground text-sm">{product.name}</div>
                        <div className="text-[11px] text-muted-foreground flex items-center gap-2 mt-0.5">
                          <span className="font-mono bg-muted/40 px-1.5 py-0.5 rounded border border-border/40">
                            {product.sku}
                          </span>
                          {product.unitOfMeasure}
                        </div>
                      </td>
                      <td className="p-4">
                        <span className="px-2.5 py-1 rounded-full text-[11px] bg-indigo-500/10 text-indigo-500 border border-indigo-500/20 font-medium">
                          {product.categoryName || "Uncategorized"}
                        </span>
                      </td>
                      <td className="p-4">
                        <div className="font-semibold text-foreground">${product.sellingPrice.toFixed(2)}</div>
                        <div className="text-[10px] text-muted-foreground">Cost: ${product.costPrice.toFixed(2)}</div>
                      </td>
                      <td className="p-4">
                        <div className="flex items-center gap-1.5">
                          <span
                            className={`font-bold text-sm ${
                              isOut ? "text-rose-500" : isLow ? "text-amber-500" : "text-emerald-500"
                            }`}
                          >
                            {product.totalQuantityAvailable}
                          </span>
                          <span className="text-muted-foreground text-[10px]">
                            (OnHand: {product.totalQuantityOnHand})
                          </span>
                        </div>
                      </td>
                      <td className="p-4 font-mono text-muted-foreground">
                        {product.reorderPoint} / {product.reorderQuantity}
                      </td>
                      <td className="p-4">
                        {isOut ? (
                          <span className="inline-flex items-center gap-1 px-2.5 py-1 rounded-full text-[10px] font-semibold bg-rose-500/10 text-rose-500 border border-rose-500/20">
                            <XCircle className="w-3 h-3" /> Out of Stock
                          </span>
                        ) : isLow ? (
                          <span className="inline-flex items-center gap-1 px-2.5 py-1 rounded-full text-[10px] font-semibold bg-amber-500/10 text-amber-500 border border-amber-500/20">
                            <AlertTriangle className="w-3 h-3" /> Low Stock
                          </span>
                        ) : (
                          <span className="inline-flex items-center gap-1 px-2.5 py-1 rounded-full text-[10px] font-semibold bg-emerald-500/10 text-emerald-500 border border-emerald-500/20">
                            <CheckCircle2 className="w-3 h-3" /> In Stock
                          </span>
                        )}
                      </td>
                      <td className="p-4 text-right">
                        <div className="flex items-center justify-end gap-1.5">
                          <Link
                            href={`/dashboard/products/${product.id}`}
                            className="p-1.5 rounded-lg border border-border/50 hover:bg-accent text-muted-foreground hover:text-foreground"
                            title="View Stock Details"
                          >
                            <Eye className="w-4 h-4" />
                          </Link>
                          <button
                            onClick={() => setBarcodeProduct(product)}
                            className="p-1.5 rounded-lg border border-border/50 hover:bg-accent text-indigo-500"
                            title="Barcode & QR Code"
                          >
                            <QrCode className="w-4 h-4" />
                          </button>
                          <button
                            onClick={() => {
                              setEditingProduct(product);
                              setIsModalOpen(true);
                            }}
                            className="p-1.5 rounded-lg border border-border/50 hover:bg-accent text-muted-foreground hover:text-foreground"
                            title="Edit Product"
                          >
                            <Edit className="w-4 h-4" />
                          </button>
                          <button
                            onClick={() => handleArchive(product.id)}
                            className="p-1.5 rounded-lg border border-border/50 hover:bg-rose-500/10 text-rose-500"
                            title="Archive Product"
                          >
                            <Archive className="w-4 h-4" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  );
                })
              ) : (
                <tr>
                  <td colSpan={7} className="p-12 text-center text-muted-foreground text-xs">
                    No products found matching the criteria.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {/* Pagination Footer */}
        {pagedProducts && pagedProducts.totalPages > 1 && (
          <div className="p-4 border-t border-border flex items-center justify-between text-xs text-muted-foreground">
            <span>
              Showing Page {pagedProducts.pageNumber} of {pagedProducts.totalPages} ({pagedProducts.totalCount} total)
            </span>
            <div className="flex items-center gap-2">
              <button
                disabled={pageNumber <= 1}
                onClick={() => setPageNumber((p) => Math.max(1, p - 1))}
                className="px-3 py-1.5 rounded-lg border border-input disabled:opacity-40"
              >
                Previous
              </button>
              <button
                disabled={pageNumber >= pagedProducts.totalPages}
                onClick={() => setPageNumber((p) => p + 1)}
                className="px-3 py-1.5 rounded-lg border border-input disabled:opacity-40"
              >
                Next
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Modals */}
      <ProductModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleCreateOrUpdate}
        product={editingProduct}
        categories={categories}
        isLoading={createProductMutation.isPending || updateProductMutation.isPending}
      />

      <ProductBarcodeModal
        isOpen={!!barcodeProduct}
        onClose={() => setBarcodeProduct(null)}
        product={barcodeProduct}
      />
    </div>
  );
}
