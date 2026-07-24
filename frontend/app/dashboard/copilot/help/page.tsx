"use client";

import { CopilotNav } from "@/components/copilot/CopilotNav";
import {
  HelpCircle,
  Sparkles,
  Command,
  BookOpen,
  DollarSign,
  UserPlus,
  QrCode,
  FileText,
  Mail,
  Workflow,
  CheckCircle2,
} from "lucide-react";
import Link from "next/link";

export default function CopilotHelpPage() {
  return (
    <div className="flex flex-col space-y-6 pb-12">
      <CopilotNav />

      <div className="px-6 space-y-6 max-w-4xl">
        <div>
          <h2 className="text-xl font-bold text-foreground flex items-center gap-2">
            AI Business Copilot Guide & Knowledge Base
            <HelpCircle className="h-5 w-5 text-indigo-500" />
          </h2>
          <p className="text-xs text-muted-foreground">
            Learn how to manage your business operations using natural language commands.
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-3">
            <div className="flex items-center gap-2 text-indigo-500">
              <DollarSign className="h-5 w-5" />
              <h3 className="text-sm font-bold text-foreground">Revenue & Billing Commands</h3>
            </div>
            <ul className="text-xs text-muted-foreground space-y-2 list-disc pl-4">
              <li><span className="font-semibold text-foreground">"Show today's revenue"</span> — Get total paid revenue and outstanding balance.</li>
              <li><span className="font-semibold text-foreground">"Create invoice for $500"</span> — Issue a new invoice for a customer.</li>
              <li><span className="font-semibold text-foreground">"Show unpaid invoices"</span> — List pending customer balances.</li>
            </ul>
          </div>

          <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-3">
            <div className="flex items-center gap-2 text-indigo-500">
              <UserPlus className="h-5 w-5" />
              <h3 className="text-sm font-bold text-foreground">CRM & Sales Commands</h3>
            </div>
            <ul className="text-xs text-muted-foreground space-y-2 list-disc pl-4">
              <li><span className="font-semibold text-foreground">"Assign new leads to Rahul"</span> — Automatically assign leads.</li>
              <li><span className="font-semibold text-foreground">"Which customers haven't been contacted?"</span> — Discover inactive leads.</li>
              <li><span className="font-semibold text-foreground">"Show pending tasks"</span> — View upcoming CRM follow-ups.</li>
            </ul>
          </div>

          <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-3">
            <div className="flex items-center gap-2 text-indigo-500">
              <QrCode className="h-5 w-5" />
              <h3 className="text-sm font-bold text-foreground">Marketing & Outreach Commands</h3>
            </div>
            <ul className="text-xs text-muted-foreground space-y-2 list-disc pl-4">
              <li><span className="font-semibold text-foreground">"Create QR Code for my page"</span> — Generate dynamic QR code.</li>
              <li><span className="font-semibold text-foreground">"Send payment reminder email"</span> — Dispatch payment notifications.</li>
              <li><span className="font-semibold text-foreground">"Send WhatsApp message"</span> — Send instant client WhatsApp text.</li>
            </ul>
          </div>

          <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-3">
            <div className="flex items-center gap-2 text-indigo-500">
              <Workflow className="h-5 w-5" />
              <h3 className="text-sm font-bold text-foreground">Reports & Workflow Automation</h3>
            </div>
            <ul className="text-xs text-muted-foreground space-y-2 list-disc pl-4">
              <li><span className="font-semibold text-foreground">"Generate this week's report"</span> — Create BI executive summary.</li>
              <li><span className="font-semibold text-foreground">"Run lead nurture workflow"</span> — Trigger automated execution.</li>
              <li><span className="font-semibold text-foreground">"Show executive dashboard"</span> — High-level metric overview.</li>
            </ul>
          </div>
        </div>

        <div className="rounded-2xl border border-indigo-500/30 bg-indigo-500/5 p-6 space-y-3">
          <div className="flex items-center gap-2 text-indigo-600 dark:text-indigo-400">
            <Sparkles className="h-5 w-5" />
            <h3 className="text-sm font-bold">Try Copilot Workspace Now</h3>
          </div>
          <p className="text-xs text-muted-foreground">
            Ready to test out natural language commands? Head over to the primary AI Copilot workspace.
          </p>
          <Link
            href="/dashboard/copilot"
            className="inline-flex items-center gap-2 rounded-xl bg-indigo-600 px-4 py-2 text-xs font-semibold text-white shadow-md hover:bg-indigo-700 transition-colors"
          >
            Launch Copilot Workspace
          </Link>
        </div>
      </div>
    </div>
  );
}
