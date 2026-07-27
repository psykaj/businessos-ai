"use client";

import { useState } from "react";
import { Building2, Plus, Edit, RefreshCw, MapPin, User, Phone, CheckCircle2 } from "lucide-react";
import { useWarehouses, useCreateWarehouse, useUpdateWarehouse } from "@/hooks/use-inventory";
import { WarehouseDto } from "@/types/inventory";
import { WarehouseModal } from "@/components/inventory/warehouse-modal";

export default function WarehousesPage() {
  const { data: warehouses = [], isLoading, refetch } = useWarehouses();
  const createWarehouseMutation = useCreateWarehouse();
  const updateWarehouseMutation = useUpdateWarehouse();

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingWarehouse, setEditingWarehouse] = useState<WarehouseDto | null>(null);

  const handleSubmit = async (dto: any) => {
    if (editingWarehouse) {
      await updateWarehouseMutation.mutateAsync({ id: editingWarehouse.id, dto });
    } else {
      await createWarehouseMutation.mutateAsync(dto);
    }
    refetch();
  };

  return (
    <div className="p-6 md:p-8 space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border/50 pb-5">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold tracking-tight">Multi-Warehouse Network</h1>
          <p className="text-sm text-muted-foreground mt-1">
            Manage distribution centers, primary stock default locations, and facility managers.
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
              setEditingWarehouse(null);
              setIsModalOpen(true);
            }}
            className="px-4 py-2.5 text-xs font-semibold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Add Warehouse
          </button>
        </div>
      </div>

      {/* Warehouse Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {isLoading ? (
          Array.from({ length: 3 }).map((_, i) => (
            <div key={i} className="h-48 rounded-2xl bg-muted/40 animate-pulse border border-border/50" />
          ))
        ) : warehouses.length > 0 ? (
          warehouses.map((w) => (
            <div
              key={w.id}
              className={`p-6 rounded-2xl border bg-card/60 backdrop-blur-md shadow-sm space-y-4 transition-all ${
                w.isPrimary ? "border-indigo-500/50 ring-1 ring-indigo-500/30" : "border-border/60"
              }`}
            >
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-3">
                  <div className="p-2.5 rounded-xl bg-indigo-500/10 text-indigo-500">
                    <Building2 className="w-5 h-5" />
                  </div>
                  <div>
                    <h3 className="font-semibold text-base flex items-center gap-2">
                      {w.name}
                      {w.isPrimary && (
                        <span className="px-2 py-0.5 rounded-full text-[10px] font-bold uppercase tracking-wider bg-indigo-500/10 text-indigo-500 border border-indigo-500/20">
                          Primary Default
                        </span>
                      )}
                    </h3>
                    <span className="font-mono text-xs text-muted-foreground">{w.code}</span>
                  </div>
                </div>
                <button
                  onClick={() => {
                    setEditingWarehouse(w);
                    setIsModalOpen(true);
                  }}
                  className="p-1.5 rounded-lg border border-border/50 hover:bg-accent text-muted-foreground hover:text-foreground"
                >
                  <Edit className="w-4 h-4" />
                </button>
              </div>

              <div className="space-y-2 text-xs text-muted-foreground">
                <div className="flex items-center gap-2">
                  <MapPin className="w-3.5 h-3.5 text-muted-foreground flex-shrink-0" />
                  <span className="truncate">
                    {[w.address, w.city, w.state, w.country].filter(Boolean).join(", ") || "No address assigned"}
                  </span>
                </div>
                {w.managerName && (
                  <div className="flex items-center gap-2">
                    <User className="w-3.5 h-3.5 text-muted-foreground flex-shrink-0" />
                    <span>Manager: {w.managerName}</span>
                  </div>
                )}
                {w.managerPhone && (
                  <div className="flex items-center gap-2">
                    <Phone className="w-3.5 h-3.5 text-muted-foreground flex-shrink-0" />
                    <span>{w.managerPhone}</span>
                  </div>
                )}
              </div>

              <div className="flex items-center justify-between pt-3 border-t border-border/50 text-xs">
                <span className="font-medium text-foreground">{w.totalItemsCount} Stock SKUs Logged</span>
                <span
                  className={`px-2 py-0.5 rounded text-[10px] font-medium ${
                    w.isActive ? "bg-emerald-500/10 text-emerald-500" : "bg-muted text-muted-foreground"
                  }`}
                >
                  {w.isActive ? "Active" : "Inactive"}
                </span>
              </div>
            </div>
          ))
        ) : (
          <div className="col-span-3 p-12 text-center text-xs text-muted-foreground rounded-2xl border border-border">
            No warehouses setup yet. Add your main facility.
          </div>
        )}
      </div>

      {/* Modal */}
      <WarehouseModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleSubmit}
        warehouse={editingWarehouse}
        isLoading={createWarehouseMutation.isPending || updateWarehouseMutation.isPending}
      />
    </div>
  );
}
