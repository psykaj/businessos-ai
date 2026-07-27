"use client";

import { X, QrCode as QrIcon, Printer, Copy, Check } from "lucide-react";
import { useState } from "react";
import { ProductDto } from "@/types/inventory";

interface ProductBarcodeModalProps {
  isOpen: boolean;
  onClose: () => void;
  product: ProductDto | null;
}

export function ProductBarcodeModal({ isOpen, onClose, product }: ProductBarcodeModalProps) {
  const [copied, setCopied] = useState(false);

  if (!isOpen || !product) return null;

  const qrPayload = product.qrCode || `BIZOS:PROD:${btoa(JSON.stringify({ sku: product.sku, id: product.id }))}`;

  const copyQr = () => {
    navigator.clipboard.writeText(qrPayload);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="relative w-full max-w-md bg-card border border-border rounded-2xl shadow-2xl overflow-hidden my-8 text-center">
        <div className="flex items-center justify-between p-5 border-b border-border bg-muted/30">
          <div className="flex items-center gap-2">
            <QrIcon className="w-5 h-5 text-indigo-500" />
            <h2 className="text-lg font-semibold text-left">Barcode & QR Scanner Label</h2>
          </div>
          <button
            onClick={onClose}
            className="p-1.5 rounded-lg text-muted-foreground hover:text-foreground hover:bg-accent"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <div className="p-6 space-y-6">
          <div className="p-6 rounded-2xl bg-white text-slate-900 border border-slate-200 inline-block shadow-inner">
            <div className="font-bold text-base tracking-wide">{product.name}</div>
            <div className="text-xs text-slate-500 mb-4">SKU: {product.sku}</div>

            {/* Simulated Visual QR/Barcode Container */}
            <div className="w-40 h-40 mx-auto bg-slate-100 p-3 rounded-xl border border-slate-300 flex flex-col items-center justify-center relative">
              <QrIcon className="w-28 h-28 text-slate-900" />
              <span className="text-[10px] font-mono mt-1 text-slate-600 tracking-widest">{product.barcode || product.sku}</span>
            </div>

            <div className="mt-3 text-[11px] font-mono text-slate-500">
              Price: ${product.sellingPrice.toFixed(2)} | UOM: {product.unitOfMeasure}
            </div>
          </div>

          <div className="p-3 rounded-xl border border-border bg-muted/20 text-left text-xs font-mono break-all text-muted-foreground">
            <div className="font-semibold text-foreground font-sans mb-1 text-[11px] flex items-center justify-between">
              <span>QR Payload String</span>
              <button onClick={copyQr} className="text-indigo-500 hover:text-indigo-600 flex items-center gap-1">
                {copied ? <Check className="w-3.5 h-3.5 text-emerald-500" /> : <Copy className="w-3.5 h-3.5" />}
                {copied ? "Copied" : "Copy"}
              </button>
            </div>
            {qrPayload}
          </div>

          <div className="flex items-center justify-center gap-3">
            <button
              onClick={() => window.print()}
              className="px-5 py-2.5 text-sm font-medium text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-md shadow-indigo-600/20 flex items-center gap-2"
            >
              <Printer className="w-4 h-4" /> Print Warehouse Label
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
