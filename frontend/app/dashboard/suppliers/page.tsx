"use client";

import { useState } from "react";
import {
  Users,
  Plus,
  Search,
  Edit,
  Building,
  Mail,
  Phone,
  Award,
  RefreshCw,
  Clock,
  CheckCircle2,
} from "lucide-react";
import { useSuppliers, useCreateSupplier, useUpdateSupplier } from "@/hooks/use-inventory";
import { SupplierDto } from "@/types/inventory";
import { SupplierModal } from "@/components/inventory/supplier-modal";

export default function SuppliersPage() {
  const [searchQuery, setSearchQuery] = useState("");
  const [pageNumber, setPageNumber] = useState(1);

  const { data: pagedSuppliers, isLoading, refetch } = useSuppliers(searchQuery, pageNumber, 15);
  const createSupplierMutation = useCreateSupplier();
  const updateSupplierMutation = useUpdateSupplier();

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingSupplier, setEditingSupplier] = useState<SupplierDto | null>(null);

  const handleSubmit = async (dto: any) => {
    if (editingSupplier) {
      await updateSupplierMutation.mutateAsync({ id: editingSupplier.id, dto });
    } else {
      await createSupplierMutation.mutateAsync(dto);
    }
    refetch();
  };

  return (
    <div className="p-6 md:p-8 space-y-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-border/50 pb-5">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold tracking-tight">Supplier Directory & Ratings</h1>
          <p className="text-sm text-muted-foreground mt-1">
            Vendor contact management, payment terms, and automated on-time delivery (OTD) performance scores.
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
              setEditingSupplier(null);
              setIsModalOpen(true);
            }}
            className="px-4 py-2.5 text-xs font-semibold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Register Supplier
          </button>
        </div>
      </div>

      {/* Search Bar */}
      <div className="relative w-full max-w-md">
        <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-muted-foreground" />
        <input
          type="text"
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          placeholder="Search suppliers by name, code, contact, or email..."
          className="w-full pl-9 pr-4 py-2 text-xs rounded-xl border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
        />
      </div>

      {/* Supplier Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {isLoading ? (
          Array.from({ length: 6 }).map((_, i) => (
            <div key={i} className="h-52 rounded-2xl bg-muted/40 animate-pulse border border-border/50" />
          ))
        ) : pagedSuppliers?.items && pagedSuppliers.items.length > 0 ? (
          pagedSuppliers.items.map((s) => {
            const scoreColor =
              s.performanceScore >= 90
                ? "text-emerald-500 bg-emerald-500/10 border-emerald-500/20"
                : s.performanceScore >= 70
                ? "text-amber-500 bg-amber-500/10 border-amber-500/20"
                : "text-rose-500 bg-rose-500/10 border-rose-500/20";

            return (
              <div
                key={s.id}
                className="p-6 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm space-y-4 transition-all hover:border-indigo-500/30"
              >
                <div className="flex items-start justify-between gap-2">
                  <div className="flex items-center gap-3">
                    <div className="p-2.5 rounded-xl bg-indigo-500/10 text-indigo-500">
                      <Building className="w-5 h-5" />
                    </div>
                    <div>
                      <h3 className="font-semibold text-base">{s.name}</h3>
                      <span className="font-mono text-xs text-muted-foreground">{s.code}</span>
                    </div>
                  </div>
                  <button
                    onClick={() => {
                      setEditingSupplier(s);
                      setIsModalOpen(true);
                    }}
                    className="p-1.5 rounded-lg border border-border/50 hover:bg-accent text-muted-foreground hover:text-foreground"
                  >
                    <Edit className="w-4 h-4" />
                  </button>
                </div>

                <div className="space-y-2 text-xs text-muted-foreground">
                  <div className="flex items-center gap-2">
                    <Users className="w-3.5 h-3.5 text-muted-foreground flex-shrink-0" />
                    <span>Contact: {s.contactPerson}</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <Mail className="w-3.5 h-3.5 text-muted-foreground flex-shrink-0" />
                    <span className="truncate">{s.email}</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <Phone className="w-3.5 h-3.5 text-muted-foreground flex-shrink-0" />
                    <span>{s.phone}</span>
                  </div>
                </div>

                {/* Scorecard */}
                <div className="p-3 rounded-xl border border-border/50 bg-muted/20 flex items-center justify-between">
                  <div className="flex items-center gap-2">
                    <Award className="w-4 h-4 text-amber-500" />
                    <span className="text-xs font-medium">Performance Score</span>
                  </div>
                  <span className={`px-2.5 py-1 rounded-full text-xs font-bold border ${scoreColor}`}>
                    {s.performanceScore.toFixed(0)}%
                  </span>
                </div>

                <div className="flex items-center justify-between pt-2 border-t border-border/40 text-xs text-muted-foreground">
                  <span>Terms: <strong>{s.paymentTerms}</strong></span>
                  <span>Total Orders: <strong>{s.totalOrdersCount}</strong></span>
                </div>
              </div>
            );
          })
        ) : (
          <div className="col-span-3 p-12 text-center text-xs text-muted-foreground rounded-2xl border border-border">
            No suppliers registered yet.
          </div>
        )}
      </div>

      {/* Modal */}
      <SupplierModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleSubmit}
        supplier={editingSupplier}
        isLoading={createSupplierMutation.isPending || updateSupplierMutation.isPending}
      />
    </div>
  );
}
