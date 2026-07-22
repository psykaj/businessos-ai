"use client";

import React, { use } from "react";
import Link from "next/link";
import { ArrowLeft, CheckCircle2, RefreshCw, Key, ShieldCheck } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useIntegrations } from "@/hooks/use-integrations";

export default function IntegrationProviderDetailPage({ params }: { params: Promise<{ provider: string }> }) {
  const { provider } = use(params);
  const { data: integrations } = useIntegrations();

  const integration = (integrations || []).find((i) => i.provider.toLowerCase() === provider.toLowerCase());

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-4xl mx-auto">
      <div className="flex items-center gap-4">
        <Link
          href="/dashboard/integrations"
          className="flex h-9 w-9 items-center justify-center rounded-xl border border-border/50 text-muted-foreground transition-colors hover:bg-muted hover:text-foreground"
        >
          <ArrowLeft className="h-4 w-4" />
        </Link>
        <div>
          <h1 className="text-2xl font-bold text-foreground capitalize">{provider} Integration</h1>
          <p className="text-xs text-muted-foreground">Manage API credentials & integration health</p>
        </div>
      </div>

      <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm flex flex-col gap-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-primary/10 text-primary font-bold text-lg uppercase">
              {provider.slice(0, 2)}
            </div>
            <div>
              <h3 className="text-base font-bold text-foreground capitalize">{provider}</h3>
              <p className="text-xs text-muted-foreground">
                Status: {integration ? integration.status : "Not Connected"}
              </p>
            </div>
          </div>

          {integration && (
            <span className="inline-flex items-center gap-1.5 rounded-full bg-emerald-500/10 px-3 py-1 text-xs font-semibold text-emerald-500">
              <CheckCircle2 className="h-4 w-4" />
              Active Connection
            </span>
          )}
        </div>

        {integration ? (
          <div className="flex flex-col gap-4 pt-4 border-t border-border/40">
            <div>
              <label className="text-xs font-semibold text-muted-foreground">Display Name</label>
              <p className="text-sm font-bold text-foreground">{integration.displayName}</p>
            </div>

            <div>
              <label className="text-xs font-semibold text-muted-foreground">Masked API Key</label>
              <p className="text-sm font-mono text-foreground">{integration.maskedApiKey || "****"}</p>
            </div>

            <div>
              <label className="text-xs font-semibold text-muted-foreground">Connected On</label>
              <p className="text-xs text-muted-foreground">{new Date(integration.createdAt).toLocaleString()}</p>
            </div>
          </div>
        ) : (
          <div className="p-6 text-center text-sm text-muted-foreground">
            No active connection configured. Return to Integration Center to connect.
          </div>
        )}
      </div>
    </div>
  );
}
