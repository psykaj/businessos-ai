"use client";

import React, { useState } from "react";
import { Globe, RefreshCw, Sparkles, Search, CheckCircle2 } from "lucide-react";
import { IntegrationCard, IntegrationProviderInfo } from "@/components/integrations/integration-card";
import { ConnectIntegrationModal } from "@/components/integrations/connect-integration-modal";
import { useIntegrations, useCreateIntegration, useDeleteIntegration, useTestIntegration } from "@/hooks/use-integrations";

const providers: IntegrationProviderInfo[] = [
  { key: "GoogleSheets", name: "Google Sheets", category: "Productivity", description: "Sync CRM leads and sales data directly into Google Spreadsheets.", color: "bg-emerald-600" },
  { key: "Slack", name: "Slack", category: "Communication", description: "Send real-time deal alerts and lead notifications to Slack channels.", color: "bg-purple-600" },
  { key: "MicrosoftTeams", name: "Microsoft Teams", category: "Communication", description: "Post automated workflow activity to Microsoft Teams channels.", color: "bg-blue-600" },
  { key: "Discord", name: "Discord", category: "Communication", description: "Stream activity notifications to Discord webhooks.", color: "bg-indigo-600" },
  { key: "Stripe", name: "Stripe", category: "Payments", description: "Trigger workflow automations on Stripe charges and invoice payments.", color: "bg-violet-600" },
  { key: "Razorpay", name: "Razorpay", category: "Payments", description: "Receive instant webhook payment confirmations and trigger invoices.", color: "bg-sky-600" },
  { key: "Twilio", name: "Twilio", category: "SMS & WhatsApp", description: "Send automated SMS and WhatsApp transactional messages.", color: "bg-rose-600" },
  { key: "Resend", name: "Resend", category: "Email", description: "Send high-deliverability transactional emails with Resend API.", color: "bg-neutral-800" },
  { key: "GoogleCalendar", name: "Google Calendar", category: "Calendar", description: "Auto-create calendar meetings when deals reach qualified stage.", color: "bg-amber-600" },
  { key: "OutlookCalendar", name: "Outlook Calendar", category: "Calendar", description: "Sync customer appointments with Microsoft Outlook Calendar.", color: "bg-blue-700" },
  { key: "Webhook", name: "Generic Webhook", category: "Developer Tools", description: "Send or receive HTTP webhooks to any custom backend server.", color: "bg-slate-700" },
  { key: "RestApi", name: "REST API", category: "Developer Tools", description: "Connect custom REST endpoints with bearer token auth.", color: "bg-zinc-700" },
];

export default function IntegrationsPage() {
  const [search, setSearch] = useState("");
  const [selectedProvider, setSelectedProvider] = useState<IntegrationProviderInfo | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const { data: activeIntegrations, isLoading } = useIntegrations();
  const createIntegration = useCreateIntegration();
  const deleteIntegration = useDeleteIntegration();
  const testIntegration = useTestIntegration();

  const handleOpenConnect = (provider: IntegrationProviderInfo) => {
    setSelectedProvider(provider);
    setIsModalOpen(true);
  };

  const handleConnectSubmit = (data: { displayName: string; apiKey?: string; accessToken?: string }) => {
    if (!selectedProvider) return;
    createIntegration.mutate(
      {
        provider: selectedProvider.key,
        displayName: data.displayName,
        apiKey: data.apiKey,
        accessToken: data.accessToken,
      },
      {
        onSuccess: () => {
          setIsModalOpen(false);
        },
      }
    );
  };

  const filteredProviders = providers.filter(
    (p) => p.name.toLowerCase().includes(search.toLowerCase()) || p.category.toLowerCase().includes(search.toLowerCase())
  );

  const activeMap = new Map((activeIntegrations || []).map((i) => [i.provider, i]));

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8">
      {/* Header */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <div className="flex items-center gap-2">
            <h1 className="text-2xl font-bold tracking-tight text-foreground lg:text-3xl">Integration Center</h1>
            <span className="inline-flex items-center gap-1 rounded-full bg-emerald-500/10 px-2.5 py-0.5 text-xs font-semibold text-emerald-500">
              <CheckCircle2 className="h-3.5 w-3.5" />
              Encrypted AES-256
            </span>
          </div>
          <p className="mt-1 text-sm text-muted-foreground">
            Connect third-party SaaS applications, payment gateways, messaging tools, and webhooks.
          </p>
        </div>

        <div className="relative w-full max-w-xs">
          <Search className="absolute left-3.5 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <input
            type="text"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Search providers..."
            className="w-full rounded-xl border border-border bg-card pl-10 pr-4 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 shadow-sm"
          />
        </div>
      </div>

      {/* Provider Grid */}
      {isLoading ? (
        <div className="p-12 text-center text-sm text-muted-foreground">Loading integrations...</div>
      ) : (
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {filteredProviders.map((provider) => {
            const active = activeMap.get(provider.key);
            return (
              <IntegrationCard
                key={provider.key}
                providerInfo={provider}
                activeIntegration={active}
                onConnect={() => handleOpenConnect(provider)}
                onDisconnect={() => active && deleteIntegration.mutate(active.id)}
                onTest={() => active && testIntegration.mutate(active.id)}
                isTesting={testIntegration.isPending}
              />
            );
          })}
        </div>
      )}

      {/* Modal */}
      <ConnectIntegrationModal
        providerInfo={selectedProvider}
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleConnectSubmit}
        isLoading={createIntegration.isPending}
      />
    </div>
  );
}
