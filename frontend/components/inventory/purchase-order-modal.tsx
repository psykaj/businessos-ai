"use client";

import { useState } from "react";
import { X, Plus, Trash2, ShoppingBag } from "lucide-react";
import { SupplierDto, WarehouseDto, ProductDto, CreatePurchaseOrderDto } from "@/types/inventory";

interface PurchaseOrderModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (dto: CreatePurchaseOrderDto) => Promise<void>;
  suppliers: SupplierDto[];
  warehouses: WarehouseDto[];
  products: ProductDto[];
  isLoading?: boolean;
}

export function PurchaseOrderModal({
  isOpen,
  onClose,
  onSubmit,
  suppliers,
  warehouses,
  products,
  isLoading,
}: PurchaseOrderModalProps) {
  const [supplierId, setSupplierId] = useState(suppliers[0]?.id || "");
  const [warehouseId, setWarehouseId] = useState(warehouses[0]?.id || "");
  const [expectedDeliveryDate, setExpectedDeliveryDate] = useState(
    new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString().split("T")[0]
  );
  const [shippingCost, setShippingCost] = useState(0);
  const [notes, setNotes] = useState("");
  const [termsAndConditions, setTermsAndConditions] = useState("Payment term: Net 30 days.");

  const [items, setItems] = useState<
    { productId: string; quantityOrdered: number; unitPrice: number; notes: string }[]
  >([
    {
      productId: products[0]?.id || "",
      quantityOrdered: 50,
      unitPrice: products[0]?.costPrice || 10,
      notes: "",
    },
  ]);

  if (!isOpen) return null;

  const addItemRow = () => {
    const defaultProd = products[0];
    setItems([
      ...items,
      {
        productId: defaultProd?.id || "",
        quantityOrdered: 10,
        unitPrice: defaultProd?.costPrice || 0,
        notes: "",
      },
    ]);
  };

  const removeItemRow = (index: number) => {
    if (items.length <= 1) return;
    setItems(items.filter((_, i) => i !== index));
  };

  const handleProductChange = (index: number, pId: string) => {
    const selectedProd = products.find((p) => p.id === pId);
    const updated = [...items];
    updated[index].productId = pId;
    if (selectedProd) {
      updated[index].unitPrice = selectedProd.costPrice;
    }
    setItems(updated);
  };

  const subTotal = items.reduce((acc, item) => acc + item.quantityOrdered * item.unitPrice, 0);
  const estimatedTax = subTotal * 0.18;
  const grandTotal = subTotal + estimatedTax + Number(shippingCost);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (items.length === 0) return;

    const payload: CreatePurchaseOrderDto = {
      supplierId: supplierId || suppliers[0]?.id,
      warehouseId: warehouseId || warehouses[0]?.id,
      expectedDeliveryDate: new Date(expectedDeliveryDate).toISOString(),
      shippingCost: Number(shippingCost),
      notes,
      termsAndConditions,
      items: items.map((i) => ({
        productId: i.productId,
        quantityOrdered: Number(i.quantityOrdered),
        unitPrice: Number(i.unitPrice),
        notes: i.notes,
      })),
    };

    await onSubmit(payload);
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-3xl bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <ShoppingBag className="w-5 h-5 text-indigo-500" />
            <h2 className="text-lg font-semibold">Create Purchase Order</h2>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-4 max-h-[80vh] overflow-y-auto">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Supplier</label>
              <select
                required
                value={supplierId}
                onChange={(e) => setSupplierId(e.target.value)}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              >
                {suppliers.map((s) => (
                  <option key={s.id} value={s.id}>
                    {s.name} ({s.code})
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Destination Warehouse</label>
              <select
                required
                value={warehouseId}
                onChange={(e) => setWarehouseId(e.target.value)}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              >
                {warehouses.map((w) => (
                  <option key={w.id} value={w.id}>
                    {w.name} ({w.code})
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-xs font-medium text-muted-foreground mb-1">Expected Delivery</label>
              <input
                type="date"
                required
                value={expectedDeliveryDate}
                onChange={(e) => setExpectedDeliveryDate(e.target.value)}
                className="w-full px-3 py-2 text-sm rounded-lg border border-input bg-background focus:outline-none focus:ring-2 focus:ring-primary/50"
              />
            </div>
          </div>

          {/* Line Items Table */}
          <div className="space-y-2">
            <div className="flex items-center justify-between">
              <label className="text-xs font-semibold uppercase tracking-wider text-muted-foreground">
                Order Line Items
              </label>
              <button
                type="button"
                onClick={addItemRow}
                className="flex items-center gap-1 text-xs font-medium text-indigo-500 hover:text-indigo-600"
              >
                <Plus className="w-3.5 h-3.5" /> Add Product
              </button>
            </div>

            <div className="space-y-2 max-h-60 overflow-y-auto pr-1">
              {items.map((item, idx) => (
                <div
                  key={idx}
                  className="flex items-center gap-2 p-3 rounded-xl border border-border/60 bg-muted/20"
                >
                  <div className="flex-1">
                    <select
                      value={item.productId}
                      onChange={(e) => handleProductChange(idx, e.target.value)}
                      className="w-full px-2.5 py-1.5 text-xs rounded-md border border-input bg-background"
                    >
                      {products.map((p) => (
                        <option key={p.id} value={p.id}>
                          {p.name} ({p.sku}) - Stock: {p.totalQuantityOnHand}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="w-24">
                    <input
                      type="number"
                      min="1"
                      placeholder="Qty"
                      value={item.quantityOrdered}
                      onChange={(e) => {
                        const updated = [...items];
                        updated[idx].quantityOrdered = Number(e.target.value);
                        setItems(updated);
                      }}
                      className="w-full px-2.5 py-1.5 text-xs rounded-md border border-input bg-background text-right"
                    />
                  </div>
                  <div className="w-28">
                    <input
                      type="number"
                      step="0.01"
                      min="0"
                      placeholder="Cost ($)"
                      value={item.unitPrice}
                      onChange={(e) => {
                        const updated = [...items];
                        updated[idx].unitPrice = Number(e.target.value);
                        setItems(updated);
                      }}
                      className="w-full px-2.5 py-1.5 text-xs rounded-md border border-input bg-background text-right"
                    />
                  </div>
                  <div className="w-28 text-right font-medium text-xs">
                    ${(item.quantityOrdered * item.unitPrice).toFixed(2)}
                  </div>
                  {items.length > 1 && (
                    <button
                      type="button"
                      onClick={() => removeItemRow(idx)}
                      className="p-1 text-rose-500 hover:text-rose-600 rounded-md"
                    >
                      <Trash2 className="w-4 h-4" />
                    </button>
                  )}
                </div>
              ))}
            </div>
          </div>

          {/* Pricing Summary */}
          <div className="p-4 rounded-xl border border-border bg-muted/40 space-y-2 text-sm">
            <div className="flex justify-between text-muted-foreground text-xs">
              <span>Subtotal:</span>
              <span>${subTotal.toFixed(2)}</span>
            </div>
            <div className="flex justify-between items-center text-xs">
              <span className="text-muted-foreground">Shipping Cost:</span>
              <input
                type="number"
                step="0.01"
                min="0"
                value={shippingCost}
                onChange={(e) => setShippingCost(Number(e.target.value))}
                className="w-28 px-2 py-1 text-xs text-right rounded border border-input bg-background"
              />
            </div>
            <div className="flex justify-between text-muted-foreground text-xs">
              <span>Est. Tax (18%):</span>
              <span>${estimatedTax.toFixed(2)}</span>
            </div>
            <div className="flex justify-between font-bold text-base border-t border-border pt-2">
              <span>Total PO Amount:</span>
              <span className="text-indigo-500">${grandTotal.toFixed(2)}</span>
            </div>
          </div>

          <div>
            <label className="block text-xs font-medium text-muted-foreground mb-1">Order Notes</label>
            <textarea
              rows={2}
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
              placeholder="Internal order references or instructions..."
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
              disabled={isLoading || items.length === 0}
              className="px-5 py-2 text-sm font-medium text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 disabled:opacity-50"
            >
              {isLoading ? "Creating PO..." : "Issue Purchase Order"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
