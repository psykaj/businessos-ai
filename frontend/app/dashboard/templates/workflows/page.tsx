"use client";

import React from "react";
import { useRouter } from "next/navigation";
import { Sparkles, ArrowLeft, Zap, Search } from "lucide-react";
import { TemplateCard, WorkflowTemplateInfo } from "@/components/workflows/templates/template-card";
import Link from "next/link";
import { useCreateWorkflow } from "@/hooks/use-workflows";

const templateList: WorkflowTemplateInfo[] = [
  {
    id: "tpl-1",
    name: "New Lead Follow-up & WhatsApp",
    category: "CRM & Sales",
    description: "Assign new CRM lead, send personalized welcome email and automated WhatsApp greeting.",
    triggerName: "Lead Created",
    actionsCount: 3,
    estimatedHoursSaved: 25,
    popular: true,
  },
  {
    id: "tpl-2",
    name: "Payment Confirmation & PDF Invoice",
    category: "Billing & Finance",
    description: "Generate invoice PDF on successful payment and email receipt to customer.",
    triggerName: "Payment Received",
    actionsCount: 2,
    estimatedHoursSaved: 18,
    popular: true,
  },
  {
    id: "tpl-3",
    name: "Subscription Renewal Reminder",
    category: "Subscriptions",
    description: "Notify customer 7 days before subscription expires and trigger renewal discount offer.",
    triggerName: "Subscription Expiring",
    actionsCount: 2,
    estimatedHoursSaved: 12,
  },
  {
    id: "tpl-4",
    name: "Welcome Customer Onboarding",
    category: "Customer Success",
    description: "Onboard new registered customer, assign onboarding agent, and schedule follow-up task.",
    triggerName: "Customer Registered",
    actionsCount: 4,
    estimatedHoursSaved: 30,
    popular: true,
  },
  {
    id: "tpl-5",
    name: "High-Value Deal Slack Alert",
    category: "Team Notifications",
    description: "Send instant Slack channel alert when a deal value exceeds $10,000.",
    triggerName: "Lead Updated",
    actionsCount: 2,
    estimatedHoursSaved: 15,
  },
  {
    id: "tpl-6",
    name: "Dynamic QR Intent Scoring",
    category: "AI & Analytics",
    description: "Run AI assistant analysis when dynamic QR code is scanned and update lead score.",
    triggerName: "QR Code Scanned",
    actionsCount: 3,
    estimatedHoursSaved: 20,
  },
];

export default function WorkflowTemplatesPage() {
  const router = useRouter();
  const createWorkflow = useCreateWorkflow();

  const handleInstallTemplate = (tpl: WorkflowTemplateInfo) => {
    createWorkflow.mutate(
      {
        name: tpl.name,
        description: tpl.description,
        trigger: {
          triggerType: tpl.triggerName.replace(/\s+/g, ""),
          triggerConfiguration: "{}",
          enabled: true,
        },
        actions: [
          {
            actionType: "SendWhatsApp",
            configuration: JSON.stringify({ Message: `Automated ${tpl.name}` }),
            executionOrder: 1,
          },
        ],
      },
      {
        onSuccess: (data) => {
          router.push(`/dashboard/workflows/${data.id}`);
        },
      }
    );
  };

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Link
            href="/dashboard/workflows"
            className="flex h-9 w-9 items-center justify-center rounded-xl border border-border/50 text-muted-foreground transition-colors hover:bg-muted hover:text-foreground"
          >
            <ArrowLeft className="h-4 w-4" />
          </Link>
          <div>
            <div className="flex items-center gap-2">
              <h1 className="text-2xl font-bold tracking-tight text-foreground lg:text-3xl">Workflow Template Library</h1>
              <span className="inline-flex items-center gap-1 rounded-full bg-amber-500/10 px-2.5 py-0.5 text-xs font-bold text-amber-500">
                <Sparkles className="h-3.5 w-3.5" />
                No-Code Ready
              </span>
            </div>
            <p className="mt-1 text-sm text-muted-foreground">
              Select a pre-configured automation template tailored for SME business workflows.
            </p>
          </div>
        </div>
      </div>

      <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        {templateList.map((tpl) => (
          <TemplateCard key={tpl.id} template={tpl} onInstall={handleInstallTemplate} />
        ))}
      </div>
    </div>
  );
}
