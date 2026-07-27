"use client";

import { useState } from "react";
import Link from "next/link";
import {
  Package,
  Plus,
  ShoppingBag,
  ArrowRightLeft,
  AlertTriangle,
  ChevronRight,
  RefreshCw,
  Layers,
  Building2,
  Users,
  BarChart3,
} from "lucide-react";
import {
  useDashboardSummary,
  useStockAlerts,
  useReorderSuggestions,
  useProductVelocityReport,
  useProducts,
  useSuppliers,
  useWarehouses,
  useCreatePurchaseOrder,
} from "@/hooks/use-inventory";
import { InventoryKpiCards } from "@/components/inventory/inventory-kpi-cards";
import { InventoryStockChart } from "@/components/inventory/inventory-stock-chart";
import { PurchaseOrderModal } from "@/components/inventory/purchase-order-modal";
import { StockOperationModal } from "@/components/inventory/stock-operation-modal";

export default function InventoryDashboardPage() {
  const { data: summary, isLoading: isSummaryLoading, refetch } = useDashboardSummary();
  const { data: alerts } = useStockAlerts();
  const { data: suggestions } = useReorderSuggestions();
  const { data: velocity } = useProductVelocityReport(30);

  const { data: productsData } = useProducts({ pageSize: 100 });
  const { data: suppliersData } = useSuppliers("", 1, 100);
  const { data: warehousesData } = useWarehouses();

  const createPOMutation = useCreatePurchaseOrder();

  const [isPoModalOpen, setIsPoModalOpen] = useState(false);
  const [isStockOpModalOpen, setIsStockOpModalOpen] = useState(false);

  const handleCreatePO = async (dto: any) => {
    await createPOMutation.mutateAsync(dto);
    refetch();
  };

  return (
    <div className="p-6 md:p-8 space-y-8 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border/50 pb-5">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold tracking-tight bg-gradient-to-r from-foreground to-foreground/70 bg-clip-text text-transparent">
            Inventory Command Center
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Real-time stock balance, automated alerts, reorder planning, and supplier metrics.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <button
            onClick={() => refetch()}
            className="p-2.5 rounded-xl border border-border/60 hover:bg-accent transition-colors text-muted-foreground hover:text-foreground"
            title="Refresh metrics"
          >
            <RefreshCw className="w-4 h-4" />
          </button>
          <button
            onClick={() => setIsStockOpModalOpen(true)}
            className="px-4 py-2.5 text-xs font-semibold rounded-xl border border-border/80 bg-muted/40 hover:bg-accent transition-all flex items-center gap-2"
          >
            <ArrowRightLeft className="w-4 h-4 text-indigo-500" /> Stock Operation
          </button>
          <button
            onClick={() => setIsPoModalOpen(true)}
            className="px-4 py-2.5 text-xs font-semibold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Issue Purchase Order
          </button>
        </div>
      </div>

      {/* KPI Cards */}
      <InventoryKpiCards summary={summary} isLoading={isSummaryLoading} />

      {/* Analytics Charts */}
      <InventoryStockChart velocityData={velocity} alertsData={alerts} />

      {/* Quick Navigation Cards */}
      <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
        <Link
          href="/dashboard/products"
          className="p-4 rounded-xl border border-border/60 bg-card/60 hover:bg-accent/50 transition-all flex items-center justify-between group"
        >
          <div className="flex items-center gap-3">
            <div className="p-2.5 rounded-lg bg-blue-500/10 text-blue-500">
              <Package className="w-5 h-5" />
            </div>
            <div>
              <div className="font-semibold text-sm">Products</div>
              <div className="text-xs text-muted-foreground">Catalog & SKUs</div>
            </div>
          </div>
          <ChevronRight className="w-4 h-4 text-muted-foreground group-hover:translate-x-0.5 transition-transform" />
        </Link>
        <Link
          href="/dashboard/categories"
          className="p-4 rounded-xl border border-border/60 bg-card/60 hover:bg-accent/50 transition-all flex items-center justify-between group"
        >
          <div className="flex items-center gap-3">
            <div className="p-2.5 rounded-lg bg-purple-500/10 text-purple-500">
              <Layers className="w-5 h-5" />
            </div>
            <div>
              <div className="font-semibold text-sm">Categories</div>
              <div className="text-xs text-muted-foreground">Hierarchy Tree</div>
            </div>
          </div>
          <ChevronRight className="w-4 h-4 text-muted-foreground group-hover:translate-x-0.5 transition-transform" />
        </Link>
        <Link
          href="/dashboard/warehouses"
          className="p-4 rounded-xl border border-border/60 bg-card/60 hover:bg-accent/50 transition-all flex items-center justify-between group"
        >
          <div className="flex items-center gap-3">
            <div className="p-2.5 rounded-lg bg-emerald-500/10 text-emerald-500">
              <Building2 className="w-5 h-5" />
            </div>
            <div>
              <div className="font-semibold text-sm">Warehouses</div>
              <div className="text-xs text-muted-foreground">Locations</div>
            </div>
          </div>
          <ChevronRight className="w-4 h-4 text-muted-foreground group-hover:translate-x-0.5 transition-transform" />
        </Link>
        <Link
          href="/dashboard/suppliers"
          className="p-4 rounded-xl border border-border/60 bg-card/60 hover:bg-accent/50 transition-all flex items-center justify-between group"
        >
          <div className="flex items-center gap-3">
            <div className="p-2.5 rounded-lg bg-amber-500/10 text-amber-500">
              <Users className="w-5 h-5" />
            </div>
            <div>
              <div className="font-semibold text-sm">Suppliers</div>
              <div className="text-xs text-muted-foreground">Vendor Ratings</div>
            </div>
          </div>
          <ChevronRight className="w-4 h-4 text-muted-foreground group-hover:translate-x-0.5 transition-transform" />
        </Link>
      </div>

      {/* Two Column Section: Reorder Suggestions & Low Stock Alerts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Reorder Suggestions */}
        <div className="p-5 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm space-y-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <ShoppingBag className="w-4 h-4 text-indigo-500" />
              <h3 className="font-semibold text-base">Reorder Planning Engine</h3>
            </div>
            <span className="text-xs text-muted-foreground">
              {suggestions?.length || 0} Suggestions
            </span>
          </div>

          <div className="space-y-2 max-h-80 overflow-y-auto pr-1">
            {suggestions && suggestions.length > 0 ? (
              suggestions.map((s, idx) => (
                <div
                  key={idx}
                  className="p-3.5 rounded-xl border border-border/50 bg-muted/20 flex items-center justify-between gap-3 text-xs"
                >
                  <div>
                    <div className="font-semibold text-foreground">{s.productName}</div>
                    <div className="text-muted-foreground text-[11px]">
                      SKU: {s.sku} | Avail Stock: <strong className="text-amber-500">{s.currentAvailableStock}</strong> (Reorder: {s.reorderPoint})
                    </div>
                  </div>
                  <div className="text-right flex flex-col items-end gap-1">
                    <span className="font-semibold text-indigo-500">
                      Reorder: +{s.suggestedReorderQuantity}
                    </span>
                    <button
                      onClick={() => setIsPoModalOpen(true)}
                      className="px-2.5 py-1 text-[10px] font-semibold text-white bg-indigo-600 hover:bg-indigo-700 rounded-md transition-colors"
                    >
                      Draft PO
                    </button>
                  </div>
                </div>
              ))
            ) : (
              <div className="p-8 text-center text-xs text-muted-foreground">
                All inventory levels are optimal. No reorder suggestions currently required.
              </div>
            )}
          </div>
        </div>

        {/* Low Stock Alerts */}
        <div className="p-5 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm space-y-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <AlertTriangle className="w-4 h-4 text-amber-500" />
              <h3 className="font-semibold text-base">Real-time Stock Risk Feed</h3>
            </div>
            <span className="text-xs text-muted-foreground">
              {alerts?.length || 0} Alerts
            </span>
          </div>

          <div className="space-y-2 max-h-80 overflow-y-auto pr-1">
            {alerts && alerts.length > 0 ? (
              alerts.map((a, idx) => (
                <div
                  key={idx}
                  className={`p-3.5 rounded-xl border flex items-start justify-between gap-3 text-xs ${
                    a.alertType === "OutOfStock"
                      ? "border-rose-500/30 bg-rose-500/5 text-rose-500"
                      : a.alertType === "Overstock"
                      ? "border-cyan-500/30 bg-cyan-500/5 text-cyan-500"
                      : "border-amber-500/30 bg-amber-500/5 text-amber-500"
                  }`}
                >
                  <div>
                    <div className="font-semibold">{a.productName} ({a.sku})</div>
                    <p className="text-[11px] text-muted-foreground mt-0.5">{a.recommendation}</p>
                  </div>
                  <span className="px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wider bg-background border border-current">
                    {a.alertType}
                  </span>
                </div>
              ))
            ) : (
              <div className="p-8 text-center text-xs text-muted-foreground">
                No active stock risk alerts.
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Modals */}
      <PurchaseOrderModal
        isOpen={isPoModalOpen}
        onClose={() => setIsPoModalOpen(false)}
        onSubmit={handleCreatePO}
        suppliers={suppliersData?.items || []}
        warehouses={warehousesData || []}
        products={productsData?.items || []}
        isLoading={createPOMutation.isPending}
      />

      <StockOperationModal
        isOpen={isStockOpModalOpen}
        onClose={() => setIsStockOpModalOpen(false)}
        onStockIn={async () => {}}
        onStockOut={async () => {}}
        onTransfer={async () => {}}
        products={productsData?.items || []}
        warehouses={warehousesData || []}
      />
    </div>
  );
}
