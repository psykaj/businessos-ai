"use client";

import { useState } from "react";
import {
  ShoppingBag,
  Plus,
  Truck,
  CheckCircle2,
  Clock,
  AlertCircle,
  FileText,
  RefreshCw,
  Search,
} from "lucide-react";
import {
  usePurchaseOrders,
  useSuppliers,
  useWarehouses,
  useProducts,
  useCreatePurchaseOrder,
  useApprovePurchaseOrder,
  useReceiveGoods,
} from "@/hooks/use-inventory";
import { PurchaseOrderDto, PurchaseOrderStatus } from "@/types/inventory";
import { PurchaseOrderModal } from "@/components/inventory/purchase-order-modal";
import { ReceiveGoodsModal } from "@/components/inventory/receive-goods-modal";

export default function PurchaseOrdersPage() {
  const [selectedStatus, setSelectedStatus] = useState<PurchaseOrderStatus | undefined>(undefined);
  const [pageNumber, setPageNumber] = useState(1);

  const { data: pagedPOs, isLoading, refetch } = usePurchaseOrders({
    status: selectedStatus,
    pageNumber,
    pageSize: 15,
  });

  const { data: suppliersData } = useSuppliers("", 1, 100);
  const { data: warehousesData } = useWarehouses();
  const { data: productsData } = useProducts({ pageSize: 100 });

  const createPOMutation = useCreatePurchaseOrder();
  const approvePOMutation = useApprovePurchaseOrder();
  const receiveGoodsMutation = useReceiveGoods();

  const [isPoModalOpen, setIsPoModalOpen] = useState(false);
  const [receivingPO, setReceivingPO] = useState<PurchaseOrderDto | null>(null);

  const handleCreatePO = async (dto: any) => {
    await createPOMutation.mutateAsync(dto);
    refetch();
  };

  const handleApprovePO = async (id: string) => {
    await approvePOMutation.mutateAsync(id);
    refetch();
  };

  const handleReceiveGoods = async (dto: any) => {
    await receiveGoodsMutation.mutateAsync(dto);
    refetch();
  };

  const getStatusBadge = (status: PurchaseOrderStatus) => {
    switch (status) {
      case "Draft":
        return <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-muted text-muted-foreground border">Draft</span>;
      case "Submitted":
        return <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-blue-500/10 text-blue-500 border border-blue-500/20">Submitted</span>;
      case "Approved":
        return <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-purple-500/10 text-purple-500 border border-purple-500/20">Approved</span>;
      case "PartiallyReceived":
        return <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-amber-500/10 text-amber-500 border border-amber-500/20">Partially Received</span>;
      case "FullyReceived":
        return <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-emerald-500/10 text-emerald-500 border border-emerald-500/20">Fully Received</span>;
      case "Cancelled":
        return <span className="px-2.5 py-1 rounded-full text-[10px] font-semibold bg-rose-500/10 text-rose-500 border border-rose-500/20">Cancelled</span>;
    }
  };

  return (
    <div className="p-6 md:p-8 space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border/50 pb-5">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold tracking-tight">Purchasing Workspace</h1>
          <p className="text-sm text-muted-foreground mt-1">
            Issue purchase orders to suppliers, manage approval workflows, and receive shipments into stock.
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
            onClick={() => setIsPoModalOpen(true)}
            className="px-4 py-2.5 text-xs font-semibold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Issue Purchase Order
          </button>
        </div>
      </div>

      {/* Filter Tabs */}
      <div className="flex items-center gap-2 overflow-x-auto pb-1">
        {[
          { label: "All Orders", value: undefined },
          { label: "Draft", value: "Draft" },
          { label: "Approved", value: "Approved" },
          { label: "Partially Received", value: "PartiallyReceived" },
          { label: "Fully Received", value: "FullyReceived" },
        ].map((tab, i) => (
          <button
            key={i}
            onClick={() => setSelectedStatus(tab.value as PurchaseOrderStatus | undefined)}
            className={`px-3.5 py-2 text-xs font-medium rounded-xl whitespace-nowrap border transition-all ${
              selectedStatus === tab.value
                ? "bg-indigo-600 text-white border-indigo-600 font-semibold shadow-sm"
                : "border-border/60 bg-card/60 text-muted-foreground hover:bg-accent hover:text-foreground"
            }`}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {/* Purchase Orders Table */}
      <div className="rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left text-xs">
            <thead className="bg-muted/40 border-b border-border text-muted-foreground uppercase tracking-wider font-semibold text-[10px]">
              <tr>
                <th className="p-4">PO Number / Date</th>
                <th className="p-4">Supplier</th>
                <th className="p-4">Warehouse</th>
                <th className="p-4">Expected Delivery</th>
                <th className="p-4">Total Amount</th>
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
              ) : pagedPOs?.items && pagedPOs.items.length > 0 ? (
                pagedPOs.items.map((po) => (
                  <tr key={po.id} className="hover:bg-muted/20 transition-colors">
                    <td className="p-4">
                      <div className="font-bold text-foreground text-sm">{po.poNumber}</div>
                      <div className="text-[11px] text-muted-foreground">
                        {new Date(po.orderDate).toLocaleDateString()}
                      </div>
                    </td>
                    <td className="p-4 font-semibold text-foreground">{po.supplierName}</td>
                    <td className="p-4 text-muted-foreground">{po.warehouseName}</td>
                    <td className="p-4 font-mono text-muted-foreground">
                      {new Date(po.expectedDeliveryDate).toLocaleDateString()}
                    </td>
                    <td className="p-4 font-bold text-sm text-foreground">
                      ${po.totalAmount.toFixed(2)}
                    </td>
                    <td className="p-4">{getStatusBadge(po.status)}</td>
                    <td className="p-4 text-right">
                      <div className="flex items-center justify-end gap-2">
                        {po.status === "Draft" && (
                          <button
                            onClick={() => handleApprovePO(po.id)}
                            className="px-2.5 py-1.5 text-[11px] font-semibold text-white bg-purple-600 hover:bg-purple-700 rounded-lg shadow-sm"
                          >
                            Approve PO
                          </button>
                        )}
                        {(po.status === "Approved" || po.status === "PartiallyReceived") && (
                          <button
                            onClick={() => setReceivingPO(po)}
                            className="px-2.5 py-1.5 text-[11px] font-semibold text-white bg-emerald-600 hover:bg-emerald-700 rounded-lg shadow-sm flex items-center gap-1"
                          >
                            <Truck className="w-3.5 h-3.5" /> Receive Goods
                          </button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={7} className="p-12 text-center text-muted-foreground text-xs">
                    No purchase orders found matching the filter.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
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

      <ReceiveGoodsModal
        isOpen={!!receivingPO}
        onClose={() => setReceivingPO(null)}
        onSubmit={handleReceiveGoods}
        purchaseOrder={receivingPO}
        isLoading={receiveGoodsMutation.isPending}
      />
    </div>
  );
}
