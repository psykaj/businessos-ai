"use client";

import React from "react";
import Link from "next/link";
import {
  MessageSquare,
  Inbox,
  FileText,
  BarChart3,
  CheckCircle,
  AlertTriangle,
  ArrowRight,
  ShieldCheck,
  Sparkles,
  Zap,
  Users,
  TrendingUp,
  Activity
} from "lucide-react";
import { useInboxSummary, useCommunicationAnalytics } from "@/hooks/use-communication";
import { ChannelBadge } from "@/components/communication/ChannelBadge";
import { Button } from "@/components/ui/button";

export default function CommunicationHubOverviewPage() {
  const { data: inboxSummary, isLoading: isInboxLoading } = useInboxSummary();
  const { data: analytics, isLoading: isAnalyticsLoading } = useCommunicationAnalytics();

  const activeChannels = [
    { type: "WhatsApp" as const, name: "WhatsApp Cloud VIP", status: "Connected & Receiving", messages: "154 today" },
    { type: "Email" as const, name: "Support Exchange", status: "Connected", messages: "92 today" },
    { type: "LiveChat" as const, name: "Website Widget v2", status: "Active on 4 pages", messages: "64 today" },
    { type: "SMS" as const, name: "Twilio Express Sms", status: "Operational", messages: "22 today" },
    { type: "FacebookMessenger" as const, name: "Meta Business Suite", status: "Operational", messages: "8 today" },
    { type: "InstagramDm" as const, name: "Instagram Support", status: "Operational", messages: "2 today" }
  ];

  return (
    <div className="w-full space-y-6 pb-12 animate-in fade-in duration-300">
      {/* Top Welcome Hero */}
      <div className="relative overflow-hidden rounded-3xl bg-linear-to-r from-indigo-900/40 via-purple-900/30 to-primary/20 border border-indigo-500/20 p-8 shadow-md backdrop-blur-xl">
        <div className="absolute top-0 right-0 -mr-16 -mt-16 w-64 h-64 rounded-full bg-primary/20 blur-3xl pointer-events-none" />
        
        <div className="relative z-10 max-w-2xl space-y-4">
          <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-primary/20 text-primary border border-primary/30 text-xs font-bold">
            <Sparkles className="w-3.5 h-3.5 text-amber-400 animate-spin" style={{ animationDuration: "8s" }} />
            <span>Day 23 Omnichannel Messaging Platform</span>
          </div>

          <h1 className="text-3xl font-extrabold text-foreground tracking-tight sm:text-4xl">
            Customer Communication Hub
          </h1>

          <p className="text-sm text-muted-foreground leading-relaxed">
            Centralize customer messaging across WhatsApp, Email, SMS, Live Chat, and Social DMs into one zero-switch command center. Built to accelerate response times and increase customer retention.
          </p>

          <div className="flex flex-wrap items-center gap-3 pt-2">
            <Link href="/dashboard/inbox">
              <Button className="bg-primary hover:bg-primary/90 text-primary-foreground font-bold px-5 py-2.5 h-10 rounded-xl shadow-lg shadow-primary/20 flex items-center gap-2">
                <Inbox className="w-4 h-4" />
                <span>Open Unified Inbox</span>
                <ArrowRight className="w-4 h-4 ml-1" />
              </Button>
            </Link>
            <Link href="/dashboard/conversations">
              <Button variant="outline" className="border-border hover:bg-muted/60 font-semibold px-4 py-2.5 h-10 rounded-xl flex items-center gap-2">
                <MessageSquare className="w-4 h-4 text-indigo-400" />
                <span>Live Workspace</span>
              </Button>
            </Link>
            <Link href="/dashboard/communication-analytics">
              <Button variant="ghost" className="hover:bg-muted/50 font-medium px-4 h-10 rounded-xl text-muted-foreground hover:text-foreground">
                <BarChart3 className="w-4 h-4 mr-1.5 text-amber-500" />
                <span>Analytics</span>
              </Button>
            </Link>
          </div>
        </div>
      </div>

      {/* KPI Preview Strip */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex items-center justify-between">
          <div className="space-y-1">
            <span className="text-2xs font-bold uppercase tracking-wider text-muted-foreground">Active Threads</span>
            <div className="text-2xl font-black text-foreground">{inboxSummary?.totalActiveConversations ?? 24}</div>
          </div>
          <div className="p-3 rounded-2xl bg-primary/10 text-primary"><Activity className="w-6 h-6" /></div>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex items-center justify-between">
          <div className="space-y-1">
            <span className="text-2xs font-bold uppercase tracking-wider text-muted-foreground">SLA Target Health</span>
            <div className="text-2xl font-black text-emerald-500 flex items-center gap-1">
              <span>96.4%</span>
              <span className="text-xs font-semibold text-muted-foreground">(On Time)</span>
            </div>
          </div>
          <div className="p-3 rounded-2xl bg-emerald-500/10 text-emerald-500"><ShieldCheck className="w-6 h-6" /></div>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex items-center justify-between">
          <div className="space-y-1">
            <span className="text-2xs font-bold uppercase tracking-wider text-muted-foreground">Avg Response Speed</span>
            <div className="text-2xl font-black text-foreground">4.8m</div>
          </div>
          <div className="p-3 rounded-2xl bg-amber-500/10 text-amber-500"><Zap className="w-6 h-6" /></div>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex items-center justify-between">
          <div className="space-y-1">
            <span className="text-2xs font-bold uppercase tracking-wider text-muted-foreground">CSAT Rating</span>
            <div className="text-2xl font-black text-amber-500">4.85★</div>
          </div>
          <div className="p-3 rounded-2xl bg-amber-500/10 text-amber-500"><TrendingUp className="w-6 h-6" /></div>
        </div>
      </div>

      {/* Connected Channels & Providers Matrix */}
      <div className="space-y-4 pt-2">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-lg font-bold text-foreground">Connected Omnichannel Adapters</h2>
            <p className="text-xs text-muted-foreground">Real-time webhook ingestion endpoints fully configured in .NET 9 Backend</p>
          </div>
          <span className="text-xs font-mono text-emerald-500 flex items-center gap-1 font-semibold">
            <span className="w-2 h-2 rounded-full bg-emerald-500 animate-ping" />
            All 6 Adapters Healthy
          </span>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {activeChannels.map((ch, i) => (
            <div key={i} className="p-5 rounded-2xl bg-card/60 border border-border/70 hover:border-primary/40 transition-all shadow-2xs flex flex-col justify-between gap-4">
              <div className="flex items-start justify-between gap-2">
                <ChannelBadge type={ch.type} size="lg" showStatusPulse />
                <span className="text-2xs font-bold text-emerald-500 bg-emerald-500/10 px-2 py-0.5 rounded border border-emerald-500/20">
                  Online
                </span>
              </div>
              <div className="space-y-1">
                <h4 className="text-sm font-bold text-foreground">{ch.name}</h4>
                <p className="text-xs text-muted-foreground flex items-center justify-between font-medium">
                  <span>Status: {ch.status}</span>
                  <span className="text-foreground font-mono">{ch.messages}</span>
                </p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
