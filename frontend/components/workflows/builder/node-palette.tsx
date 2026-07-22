"use client";

import React from "react";
import {
  Zap,
  FormInput,
  QrCode,
  CreditCard,
  UserCheck,
  Clock,
  Play,
  Mail,
  MessageSquare,
  CheckSquare,
  UserPlus,
  Bell,
  FileText,
  Bot,
  Globe,
  GitBranch,
  Filter,
  Plus
} from "lucide-react";
import { Button } from "@/components/ui/button";

export const triggerItems = [
  { type: "LeadCreated", label: "Lead Created", icon: Zap, group: "Triggers", desc: "When a new lead enters CRM" },
  { type: "FormSubmitted", label: "Form Submitted", icon: FormInput, group: "Triggers", desc: "When a public form is submitted" },
  { type: "QRCodeScanned", label: "QR Code Scanned", icon: QrCode, group: "Triggers", desc: "When dynamic QR is scanned" },
  { type: "PaymentReceived", label: "Payment Received", icon: CreditCard, group: "Triggers", desc: "When Stripe/Razorpay payment succeeds" },
  { type: "CustomerRegistered", label: "Customer Registered", icon: UserCheck, group: "Triggers", desc: "When new customer registers" },
  { type: "SubscriptionExpiring", label: "Subscription Expiring", icon: Clock, group: "Triggers", desc: "When subscription near expiry" },
  { type: "ManualTrigger", label: "Manual Trigger", icon: Play, group: "Triggers", desc: "Triggered manually on demand" },
  { type: "ScheduledTrigger", label: "Scheduled Trigger", icon: Clock, group: "Triggers", desc: "Cron or timed execution" },
];

export const actionItems = [
  { type: "SendEmail", label: "Send Email", icon: Mail, group: "Actions", desc: "Send transactional email" },
  { type: "SendWhatsApp", label: "Send WhatsApp", icon: MessageSquare, group: "Actions", desc: "Send automated WhatsApp message" },
  { type: "CreateCrmTask", label: "Create CRM Task", icon: CheckSquare, group: "Actions", desc: "Assign task to sales team" },
  { type: "UpdateCrmLead", label: "Update CRM Lead", icon: UserCheck, group: "Actions", desc: "Update lead status or pipeline stage" },
  { type: "AssignSalesperson", label: "Assign Salesperson", icon: UserPlus, group: "Actions", desc: "Round-robin sales assignment" },
  { type: "SendNotification", label: "Send Notification", icon: Bell, group: "Actions", desc: "In-app real-time notification" },
  { type: "GenerateInvoice", label: "Generate Invoice", icon: FileText, group: "Actions", desc: "Create draft or final invoice PDF" },
  { type: "GenerateQR", label: "Generate QR Code", icon: QrCode, group: "Actions", desc: "Create dynamic trackable QR" },
  { type: "CallAiAssistant", label: "Call AI Assistant", icon: Bot, group: "Actions", desc: "Run AI prompt on context data" },
  { type: "CallWebhook", label: "Call Webhook", icon: Globe, group: "Actions", desc: "HTTP POST payload to external API" },
];

export const logicItems = [
  { type: "IfElse", label: "IF / ELSE", icon: GitBranch, group: "Logic", desc: "Branch workflow based on rules" },
  { type: "Filter", label: "Filter Data", icon: Filter, group: "Logic", desc: "Filter context payload" },
  { type: "Delay", label: "Delay Execution", icon: Clock, group: "Logic", desc: "Pause execution for N seconds" },
];

interface NodePaletteProps {
  onAddNode: (kind: "trigger" | "action" | "logic", nodeType: string, label: string) => void;
}

export function NodePalette({ onAddNode }: NodePaletteProps) {
  return (
    <aside className="w-80 shrink-0 border-r border-border bg-background/95 p-4 backdrop-blur-xl flex flex-col gap-6 overflow-y-auto">
      <div>
        <h3 className="text-sm font-bold text-foreground tracking-tight">Workflow Nodes</h3>
        <p className="text-xs text-muted-foreground mt-0.5">Click or drag nodes into the editor canvas</p>
      </div>

      {/* Triggers Group */}
      <div className="flex flex-col gap-2">
        <div className="flex items-center gap-1.5 text-xs font-bold uppercase tracking-wider text-amber-500">
          <Zap className="h-3.5 w-3.5" />
          <span>Triggers</span>
        </div>
        <div className="flex flex-col gap-1.5">
          {triggerItems.map((item) => {
            const Icon = item.icon;
            return (
              <div
                key={item.type}
                onClick={() => onAddNode("trigger", item.type, item.label)}
                className="group flex items-center justify-between rounded-xl border border-border/50 bg-card/60 p-2.5 text-left transition-all duration-200 hover:border-amber-500/50 hover:bg-amber-500/5 cursor-pointer shadow-sm"
              >
                <div className="flex items-center gap-2.5 min-w-0">
                  <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-amber-500/10 text-amber-500">
                    <Icon className="h-4 w-4" />
                  </div>
                  <div className="flex flex-col min-w-0">
                    <span className="text-xs font-semibold text-foreground truncate">{item.label}</span>
                    <span className="text-[10px] text-muted-foreground truncate">{item.desc}</span>
                  </div>
                </div>
                <Plus className="h-4 w-4 text-muted-foreground opacity-0 group-hover:opacity-100 transition-opacity" />
              </div>
            );
          })}
        </div>
      </div>

      {/* Actions Group */}
      <div className="flex flex-col gap-2">
        <div className="flex items-center gap-1.5 text-xs font-bold uppercase tracking-wider text-primary">
          <CheckSquare className="h-3.5 w-3.5" />
          <span>Actions</span>
        </div>
        <div className="flex flex-col gap-1.5">
          {actionItems.map((item) => {
            const Icon = item.icon;
            return (
              <div
                key={item.type}
                onClick={() => onAddNode("action", item.type, item.label)}
                className="group flex items-center justify-between rounded-xl border border-border/50 bg-card/60 p-2.5 text-left transition-all duration-200 hover:border-primary/50 hover:bg-primary/5 cursor-pointer shadow-sm"
              >
                <div className="flex items-center gap-2.5 min-w-0">
                  <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-primary/10 text-primary">
                    <Icon className="h-4 w-4" />
                  </div>
                  <div className="flex flex-col min-w-0">
                    <span className="text-xs font-semibold text-foreground truncate">{item.label}</span>
                    <span className="text-[10px] text-muted-foreground truncate">{item.desc}</span>
                  </div>
                </div>
                <Plus className="h-4 w-4 text-muted-foreground opacity-0 group-hover:opacity-100 transition-opacity" />
              </div>
            );
          })}
        </div>
      </div>

      {/* Logic Group */}
      <div className="flex flex-col gap-2">
        <div className="flex items-center gap-1.5 text-xs font-bold uppercase tracking-wider text-purple-500">
          <GitBranch className="h-3.5 w-3.5" />
          <span>Condition Logic</span>
        </div>
        <div className="flex flex-col gap-1.5">
          {logicItems.map((item) => {
            const Icon = item.icon;
            return (
              <div
                key={item.type}
                onClick={() => onAddNode("logic", item.type, item.label)}
                className="group flex items-center justify-between rounded-xl border border-border/50 bg-card/60 p-2.5 text-left transition-all duration-200 hover:border-purple-500/50 hover:bg-purple-500/5 cursor-pointer shadow-sm"
              >
                <div className="flex items-center gap-2.5 min-w-0">
                  <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-purple-500/10 text-purple-500">
                    <Icon className="h-4 w-4" />
                  </div>
                  <div className="flex flex-col min-w-0">
                    <span className="text-xs font-semibold text-foreground truncate">{item.label}</span>
                    <span className="text-[10px] text-muted-foreground truncate">{item.desc}</span>
                  </div>
                </div>
                <Plus className="h-4 w-4 text-muted-foreground opacity-0 group-hover:opacity-100 transition-opacity" />
              </div>
            );
          })}
        </div>
      </div>
    </aside>
  );
}
