"use client";

import {
  TrendingUp,
  Users,
  DollarSign,
  QrCode,
  FileText,
  Mail,
  MessageSquare,
  Workflow,
  Sparkles,
  UserPlus,
  Clock,
  Send,
} from "lucide-react";

export interface CommandOption {
  label: string;
  command: string;
  category: "Sales" | "Customers" | "Finance" | "Marketing" | "Operations";
  icon: any;
  description: string;
}

const COMMAND_OPTIONS: CommandOption[] = [
  // Sales
  { label: "Show Today's Revenue", command: "Show today's revenue", category: "Sales", icon: DollarSign, description: "Instant revenue & sales aggregate" },
  { label: "Assign New Leads", command: "Assign new leads to Rahul", category: "Sales", icon: UserPlus, description: "Auto-assign incoming leads" },
  { label: "Executive Dashboard", command: "Show dashboard", category: "Sales", icon: TrendingUp, description: "High-level metrics summary" },
  
  // Customers
  { label: "Inactive Customers", command: "Which customers haven't been contacted?", category: "Customers", icon: Users, description: "Identify leads requiring follow-up" },
  { label: "Show Pending Tasks", command: "Show pending tasks", category: "Customers", icon: Clock, description: "List upcoming CRM tasks" },

  // Finance
  { label: "Generate Invoice", command: "Create invoice for $500", category: "Finance", icon: FileText, description: "Issue customer invoice" },
  { label: "Unpaid Invoices", command: "Show unpaid invoices", category: "Finance", icon: DollarSign, description: "Outstanding receivables summary" },

  // Marketing
  { label: "Generate QR Code", command: "Create QR Code for my landing page", category: "Marketing", icon: QrCode, description: "Create dynamic website QR" },
  { label: "Send WhatsApp Update", command: "Send WhatsApp message to client", category: "Marketing", icon: MessageSquare, description: "Dispatch instant WhatsApp text" },
  { label: "Send Payment Reminder", command: "Send payment reminder email", category: "Marketing", icon: Mail, description: "Email overdue payment notice" },

  // Operations
  { label: "Generate Weekly Report", command: "Generate this week's report", category: "Operations", icon: FileText, description: "Create BI performance report" },
  { label: "Run Automation Workflow", command: "Run lead nurture workflow", category: "Operations", icon: Workflow, description: "Trigger workflow automation" },
];

interface CommandCenterProps {
  onSelectCommand: (command: string) => void;
}

export function CommandCenter({ onSelectCommand }: CommandCenterProps) {
  return (
    <div className="rounded-2xl border border-border/60 bg-card p-5 shadow-sm">
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <Sparkles className="h-4 w-4 text-indigo-500" />
          <h2 className="text-sm font-semibold text-foreground">Business Command Center</h2>
        </div>
        <span className="text-xs text-muted-foreground">Click any prompt to execute</span>
      </div>

      <div className="grid grid-cols-1 gap-2.5 sm:grid-cols-2 lg:grid-cols-3">
        {COMMAND_OPTIONS.map((item, idx) => {
          const Icon = item.icon;
          return (
            <button
              key={idx}
              onClick={() => onSelectCommand(item.command)}
              className="group flex items-start gap-3 rounded-xl border border-border/50 bg-background/50 p-3 text-left transition-all duration-200 hover:border-indigo-500/50 hover:bg-indigo-500/5 hover:shadow-md hover:shadow-indigo-500/5"
            >
              <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-indigo-500/10 text-indigo-600 dark:text-indigo-400 group-hover:bg-indigo-600 group-hover:text-white transition-colors">
                <Icon className="h-4 w-4" />
              </div>
              <div className="min-w-0 flex-1">
                <div className="flex items-center justify-between">
                  <span className="text-xs font-semibold text-foreground group-hover:text-indigo-600 dark:group-hover:text-indigo-400 transition-colors">
                    {item.label}
                  </span>
                  <span className="rounded bg-muted px-1.5 py-0.5 text-[10px] font-medium text-muted-foreground">
                    {item.category}
                  </span>
                </div>
                <p className="text-[11px] text-muted-foreground truncate mt-0.5">
                  {item.description}
                </p>
              </div>
            </button>
          );
        })}
      </div>
    </div>
  );
}
