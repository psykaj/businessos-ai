"use client";

import React, { memo } from "react";
import { Handle, Position } from "@xyflow/react";
import {
  Mail,
  MessageSquare,
  CheckSquare,
  UserCheck,
  UserPlus,
  Bell,
  FileText,
  QrCode,
  Bot,
  Globe,
  Clock,
  Activity,
} from "lucide-react";

const actionIcons: Record<string, any> = {
  SendEmail: Mail,
  SendWhatsApp: MessageSquare,
  CreateCrmTask: CheckSquare,
  UpdateCrmLead: UserCheck,
  AssignSalesperson: UserPlus,
  SendNotification: Bell,
  GenerateInvoice: FileText,
  GenerateQR: QrCode,
  CallAiAssistant: Bot,
  CallWebhook: Globe,
  DelayExecution: Clock,
  LogActivity: Activity,
};

export const ActionNode = memo(({ data, selected }: { data: any; selected?: boolean }) => {
  const Icon = actionIcons[data.actionType] || Activity;
  const label = data.label || data.actionType || "Action";

  return (
    <div
      className={`relative min-w-[220px] rounded-2xl border bg-background/95 p-4 backdrop-blur-md transition-all duration-200 shadow-lg ${
        selected ? "border-primary ring-2 ring-primary/30" : "border-primary/30 hover:border-primary/60"
      }`}
    >
      {/* Input Handle */}
      <Handle
        type="target"
        position={Position.Top}
        className="!h-3.5 !w-3.5 !border-2 !border-background !bg-primary shadow-md"
      />

      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-primary/10 text-primary">
          <Icon className="h-5 w-5" />
        </div>
        <div className="flex flex-col min-w-0">
          <span className="text-[10px] font-bold uppercase tracking-wider text-primary">Action</span>
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
        className="!h-3.5 !w-3.5 !border-2 !border-background !bg-primary shadow-md"
      />
    </div>
  );
});

ActionNode.displayName = "ActionNode";
