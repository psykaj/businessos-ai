"use client";

import { AlertTriangle, CheckCircle2, X } from "lucide-react";

interface ActionConfirmationModalProps {
  isOpen: boolean;
  onClose: () => void;
  onConfirm: () => void;
  title: string;
  description: string;
  toolName?: string;
  isPending?: boolean;
}

export function ActionConfirmationModal({
  isOpen,
  onClose,
  onConfirm,
  title,
  description,
  toolName,
  isPending = false,
}: ActionConfirmationModalProps) {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in duration-200">
      <div className="w-full max-w-md rounded-2xl border border-border bg-card p-6 shadow-2xl transition-all">
        <div className="flex items-center justify-between border-b border-border/50 pb-4 mb-4">
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-amber-500/10 text-amber-500 border border-amber-500/20">
              <AlertTriangle className="h-5 w-5" />
            </div>
            <div>
              <h3 className="text-base font-bold text-foreground">Action Confirmation Required</h3>
              <p className="text-xs text-muted-foreground">Safety Layer Verification</p>
            </div>
          </div>
          <button
            onClick={onClose}
            className="rounded-lg p-1.5 text-muted-foreground hover:bg-muted hover:text-foreground"
          >
            <X className="h-4 w-4" />
          </button>
        </div>

        <div className="space-y-3 mb-6">
          <div className="rounded-xl border border-amber-500/20 bg-amber-500/5 p-3 text-xs text-amber-700 dark:text-amber-300">
            <span className="font-semibold block mb-0.5">Execution Details:</span>
            {title}
          </div>

          <p className="text-xs text-muted-foreground leading-relaxed">
            {description}
          </p>

          {toolName && (
            <div className="flex items-center justify-between text-xs text-muted-foreground pt-2 border-t border-border/40">
              <span>Target Service Tool:</span>
              <span className="font-mono text-indigo-500 font-semibold">{toolName}</span>
            </div>
          )}
        </div>

        <div className="flex items-center justify-end gap-3">
          <button
            type="button"
            onClick={onClose}
            disabled={isPending}
            className="rounded-xl border border-border bg-background px-4 py-2 text-xs font-semibold text-foreground hover:bg-muted transition-colors disabled:opacity-50"
          >
            Cancel
          </button>
          <button
            type="button"
            onClick={onConfirm}
            disabled={isPending}
            className="flex items-center gap-2 rounded-xl bg-amber-500 px-4 py-2 text-xs font-semibold text-white hover:bg-amber-600 shadow-md shadow-amber-500/20 transition-all disabled:opacity-50"
          >
            {isPending ? (
              <span>Processing Execution...</span>
            ) : (
              <>
                <CheckCircle2 className="h-4 w-4" />
                Confirm & Execute Action
              </>
            )}
          </button>
        </div>
      </div>
    </div>
  );
}
