"use client";

import React, { useState } from "react";
import { X, Key, ShieldCheck, Lock, Globe } from "lucide-react";
import { Button } from "@/components/ui/button";
import { IntegrationProviderInfo } from "./integration-card";

interface ConnectIntegrationModalProps {
  providerInfo: IntegrationProviderInfo | null;
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (data: { displayName: string; apiKey?: string; accessToken?: string; metadataJson?: string }) => void;
  isLoading?: boolean;
}

export function ConnectIntegrationModal({
  providerInfo,
  isOpen,
  onClose,
  onSubmit,
  isLoading = false,
}: ConnectIntegrationModalProps) {
  const [displayName, setDisplayName] = useState("");
  const [apiKey, setApiKey] = useState("");
  const [accessToken, setAccessToken] = useState("");

  if (!isOpen || !providerInfo) return null;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      displayName: displayName || `${providerInfo.name} Connection`,
      apiKey: apiKey || undefined,
      accessToken: accessToken || undefined,
    });
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4">
      <div className="w-full max-w-md rounded-3xl border border-border bg-background p-6 shadow-2xl animate-in fade-in zoom-in-95 duration-200">
        <div className="flex items-center justify-between border-b border-border/50 pb-4">
          <div className="flex items-center gap-3">
            <div className={`flex h-10 w-10 items-center justify-center rounded-xl ${providerInfo.color} font-bold text-white shadow-md`}>
              {providerInfo.name.slice(0, 2).toUpperCase()}
            </div>
            <div>
              <h3 className="text-base font-bold text-foreground">Connect {providerInfo.name}</h3>
              <p className="text-xs text-muted-foreground">Encrypted SaaS Provider Integration</p>
            </div>
          </div>
          <button
            onClick={onClose}
            className="flex h-8 w-8 items-center justify-center rounded-lg text-muted-foreground hover:bg-muted hover:text-foreground"
          >
            <X className="h-4 w-4" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="mt-6 flex flex-col gap-4">
          <div>
            <label className="text-xs font-semibold text-foreground">Connection Display Name</label>
            <input
              type="text"
              value={displayName}
              onChange={(e) => setDisplayName(e.target.value)}
              placeholder={`e.g. My ${providerInfo.name} Production`}
              className="mt-1 w-full rounded-xl border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
            />
          </div>

          <div>
            <label className="text-xs font-semibold text-foreground flex items-center gap-1">
              <Key className="h-3.5 w-3.5 text-primary" />
              API Key / Access Token
            </label>
            <input
              type="password"
              value={apiKey}
              onChange={(e) => setApiKey(e.target.value)}
              placeholder="Paste secret API key or OAuth token"
              required
              className="mt-1 w-full font-mono text-xs rounded-xl border border-border bg-card px-3 py-2 text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
            />
            <p className="mt-1.5 text-[11px] text-muted-foreground flex items-center gap-1">
              <Lock className="h-3 w-3 text-emerald-500" />
              Credentials are encrypted at rest with AES-256
            </p>
          </div>

          <div className="mt-4 flex items-center justify-end gap-3 pt-4 border-t border-border/40">
            <Button type="button" variant="outline" size="sm" onClick={onClose}>
              Cancel
            </Button>
            <Button type="submit" size="sm" disabled={isLoading} className="gap-2 shadow-md">
              <ShieldCheck className="h-4 w-4" />
              <span>{isLoading ? "Encrypting & Connecting..." : "Save Connection"}</span>
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
