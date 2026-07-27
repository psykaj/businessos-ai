"use client";

import { useState } from "react";
import {
  History,
  ArrowRightLeft,
  ArrowDownRight,
  ArrowUpRight,
  RefreshCw,
  Search,
  Filter,
} from "lucide-react";
import {
  useStockMovements,
  useProducts,
  useWarehouses,
  useRecordStockIn,
  useRecordStockOut,
  useTransferStock,
} from "@/hooks/use-inventory";
import { MovementType } from "@/types/inventory";
import { StockOperationModal } from "@/components/inventory/stock-operation-modal";

export default function StockMovementsPage() {
  const [selectedType, setSelectedType] = useState<MovementType | undefined>(undefined);
  const [pageNumber, setPageNumber] = useState(1);

  const { data: pagedMovements, isLoading, refetch } = useStockMovements({
    movementType: selectedType,
    pageNumber,
    pageSize: 20,
  });

  const { data: productsData } = useProducts({ pageSize: 100 });
  const { data: warehousesData } = useWarehouses();

  const stockInMutation = useRecordStockIn();
  const stockOutMutation = useRecordStockOut();
  const transferMutation = useTransferStock();

  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleStockIn = async (dto: any) => {
    await stockInMutation.mutateAsync(dto);
    refetch();
  };

  const handleStockOut = async (dto: any) => {
    await stockOutMutation.mutateAsync(dto);
    refetch();
  };

  const handleTransfer = async (dto: any) => {
    await transferMutation.mutateAsync(dto);
    refetch();
  };

  return (
    <div className="p-6 md:p-8 space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border/50 pb-5">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold tracking-tight">Stock Movement Audit Trail</h1>
          <p className="text-sm text-muted-foreground mt-1">
            Complete immutable ledger of stock additions, issues, transfers, and inventory reconciliations.
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
            onClick={() => setIsModalOpen(true)}
            className="px-4 py-2.5 text-xs font-semibold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 transition-all flex items-center gap-2"
          >
            <ArrowRightLeft className="w-4 h-4" /> Record Operation / Transfer
          </button>
        </div>
      </div>

      {/* Movement Filter Tabs */}
      <div className="flex items-center gap-2 overflow-x-auto pb-1">
        {[
          { label: "All Movements", value: undefined },
          { label: "Stock In", value: "StockIn" },
          { label: "Stock Out", value: "StockOut" },
          { label: "Transfers", value: "Transfer" },
          { label: "Purchase Receipts", value: "PurchaseReceipt" },
          { label: "Adjustments", value: "Adjustment" },
        ].map((tab, i) => (
          <button
            key={i}
            onClick={() => setSelectedType(tab.value as MovementType | undefined)}
            className={`px-3.5 py-2 text-xs font-medium rounded-xl whitespace-nowrap border transition-all ${
              selectedType === tab.value
                ? "bg-indigo-600 text-white border-indigo-600 font-semibold shadow-sm"
                : "border-border/60 bg-card/60 text-muted-foreground hover:bg-accent hover:text-foreground"
            }`}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {/* Table */}
      <div className="rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left text-xs">
            <thead className="bg-muted/40 border-b border-border text-muted-foreground uppercase tracking-wider font-semibold text-[10px]">
              <tr>
                <th className="p-4">Timestamp</th>
                <th className="p-4">Product / SKU</th>
                <th className="p-4">Movement Type</th>
                <th className="p-4">Quantity</th>
                <th className="p-4">Source $\rightarrow$ Destination</th>
                <th className="p-4">Reference / Reason</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border/40">
              {isLoading ? (
                Array.from({ length: 5 }).map((_, i) => (
                  <tr key={i} className="animate-pulse">
                    <td colSpan={6} className="p-4">
                      <div className="h-6 bg-muted/30 rounded-lg" />
                    </td>
                  </tr>
                ))
              ) : pagedMovements?.items && pagedMovements.items.length > 0 ? (
                pagedMovements.items.map((m) => (
                  <tr key={m.id} className="hover:bg-muted/20 transition-colors">
                    <td className="p-4 font-mono text-muted-foreground">
                      {new Date(m.movementDate).toLocaleString()}
                    </td>
                    <td className="p-4">
                      <div className="font-semibold text-foreground text-sm">{m.productName}</div>
                      <div className="text-[10px] text-muted-foreground font-mono">{m.productSKU}</div>
                    </td>
                    <td className="p-4">
                      <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-indigo-500/10 text-indigo-500 border border-indigo-500/20">
                        {m.movementType}
                      </span>
                    </td>
                    <td className="p-4 font-bold text-sm text-foreground">{m.quantity}</td>
                    <td className="p-4 text-muted-foreground">
                      {m.sourceWarehouseName || "External"} $\rightarrow$ {m.destinationWarehouseName || "External"}
                    </td>
                    <td className="p-4">
                      <div className="font-medium text-foreground">{m.referenceType || "Manual Operation"}</div>
                      <div className="text-[10px] text-muted-foreground">{m.reason || "No notes"}</div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={6} className="p-12 text-center text-muted-foreground text-xs">
                    No stock movements recorded yet.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* Modal */}
      <StockOperationModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onStockIn={handleStockIn}
        onStockOut={handleStockOut}
        onTransfer={handleTransfer}
        products={productsData?.items || []}
        warehouses={warehousesData || []}
        isLoading={stockInMutation.isPending || stockOutMutation.isPending || transferMutation.isPending}
      />
    </div>
  );
}
