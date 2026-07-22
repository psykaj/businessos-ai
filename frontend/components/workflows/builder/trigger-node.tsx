"use client";

import React, { memo } from "react";
import { Handle, Position } from "@xyflow/react";
import { Zap, Play, FormInput, QrCode, CreditCard, UserCheck, Clock, Sparkles } from "lucide-react";

const triggerIcons: Record<string, any> = {
  LeadCreated: Zap,
  FormSubmitted: FormInput,
  QRCodeScanned: QrCode,
  PaymentReceived: CreditCard,
  CustomerRegistered: UserCheck,
  SubscriptionExpiring: Clock,
  ManualTrigger: Play,
  ScheduledTrigger: Clock,
};

export const TriggerNode = memo(({ data, selected }: { data: any; selected?: boolean }) => {
  const Icon = triggerIcons[data.triggerType] || Zap;
  const label = data.label || data.triggerType || "Trigger";

  return (
    <div
      className={`relative min-w-[220px] rounded-2xl border bg-background/95 p-4 backdrop-blur-md transition-all duration-200 shadow-lg ${
        selected ? "border-amber-500 ring-2 ring-amber-500/30" : "border-amber-500/30 hover:border-amber-500/60"
      }`}
    >
      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-amber-500/10 text-amber-500">
          <Icon className="h-5 w-5" />
        </div>
        <div className="flex flex-col min-w-0">
          <span className="text-[10px] font-bold uppercase tracking-wider text-amber-500">Trigger</span>
          <span className="truncate text-sm font-semibold text-foreground">{label}</span>
        </div>
      </div>
      {data.description && (
        <p className="mt-2 text-xs text-muted-foreground line-clamp-2">{data.description}</p>
      )}

      {/* Output Handle */}
      <Handle
        type="source"
        position={Position.Bottom}
        className="!h-3.5 !w-3.5 !border-2 !border-background !bg-amber-500 shadow-md"
      />
    </div>
  );
});

TriggerNode.displayName = "TriggerNode";
