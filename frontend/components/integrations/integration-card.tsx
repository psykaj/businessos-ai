"use client";

import React from "react";
import { CheckCircle2, AlertCircle, RefreshCw, Unlink, Globe, Sparkles } from "lucide-react";
import { Button } from "@/components/ui/button";

export interface IntegrationProviderInfo {
  key: string;
  name: string;
  category: string;
  description: string;
  iconUrl?: string;
  color: string;
}

interface IntegrationCardProps {
  providerInfo: IntegrationProviderInfo;
  activeIntegration?: any;
  onConnect: () => void;
  onDisconnect: () => void;
  onTest: () => void;
  isTesting?: boolean;
}

export function IntegrationCard({
  providerInfo,
  activeIntegration,
  onConnect,
  onDisconnect,
  onTest,
  isTesting = false,
}: IntegrationCardProps) {
  const isConnected = !!activeIntegration && activeIntegration.status === "Active";

  return (
    <div className="group relative flex flex-col justify-between rounded-2xl border border-border/60 bg-card p-6 shadow-sm transition-all duration-200 hover:border-primary/40 hover:shadow-md">
      <div className="flex flex-col gap-4">
        {/* Header with status badge */}
        <div className="flex items-start justify-between">
          <div className={`flex h-12 w-12 items-center justify-center rounded-2xl ${providerInfo.color} font-bold text-white shadow-md`}>
            {providerInfo.name.slice(0, 2).toUpperCase()}
          </div>
          {isConnected ? (
            <span className="inline-flex items-center gap-1.5 rounded-full bg-emerald-500/10 px-2.5 py-1 text-xs font-semibold text-emerald-500">
              <CheckCircle2 className="h-3.5 w-3.5" />
              Connected
            </span>
          ) : (
            <span className="inline-flex items-center gap-1.5 rounded-full bg-muted px-2.5 py-1 text-xs font-medium text-muted-foreground">
              Not Connected
            </span>
          )}
        </div>

        <div>
          <h3 className="text-base font-bold text-foreground group-hover:text-primary transition-colors">
            {providerInfo.name}
          </h3>
          <p className="mt-1 text-xs text-muted-foreground leading-relaxed line-clamp-2">
            {providerInfo.description}
          </p>
        </div>

        {isConnected && activeIntegration?.maskedApiKey && (
          <div className="rounded-xl bg-muted/40 p-2.5 text-[11px] font-mono text-muted-foreground flex items-center justify-between">
            <span>API Key:</span>
            <span className="font-semibold text-foreground">{activeIntegration.maskedApiKey}</span>
          </div>
        )}
      </div>

      {/* Action Buttons */}
      <div className="mt-6 flex items-center gap-2 pt-4 border-t border-border/40">
        {isConnected ? (
          <>
            <Button
              variant="outline"
              size="sm"
              onClick={onTest}
              disabled={isTesting}
              className="flex-1 gap-1.5 text-xs"
            >
              <RefreshCw className={`h-3.5 w-3.5 ${isTesting ? "animate-spin" : ""}`} />
              <span>{isTesting ? "Testing..." : "Test"}</span>
            </Button>
            <Button
              variant="ghost"
              size="sm"
              onClick={onDisconnect}
              className="text-xs text-destructive hover:bg-destructive/10 hover:text-destructive"
            >
              <Unlink className="h-3.5 w-3.5" />
            </Button>
          </>
        ) : (
          <Button size="sm" onClick={onConnect} className="w-full text-xs font-semibold shadow-sm">
            <span>Connect {providerInfo.name}</span>
          </Button>
        )}
      </div>
    </div>
  );
}
