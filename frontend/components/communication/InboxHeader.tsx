"use client";

import React from "react";
import {
  Search,
  Filter,
  SlidersHorizontal,
  CheckSquare,
  AlertTriangle,
  Inbox as InboxIcon,
  Sparkles,
  ArrowUpDown
} from "lucide-react";
import { CommunicationChannelType, ConversationPriority } from "@/lib/communication-service";
import { Button } from "@/components/ui/button";

interface InboxHeaderProps {
  selectedChannel: CommunicationChannelType | "All";
  onSelectChannel: (channel: CommunicationChannelType | "All") => void;
  selectedStatus: string;
  onSelectStatus: (status: string) => void;
  selectedPriority: ConversationPriority | "All";
  onSelectPriority: (priority: ConversationPriority | "All") => void;
  searchKeyword: string;
  onSearchChange: (keyword: string) => void;
  totalActiveCount: number;
  totalUnreadCount: number;
  totalSlaBreached: number;
  onBulkAssign?: () => void;
  selectedCount?: number;
}

const CHANNELS: (CommunicationChannelType | "All")[] = [
  "All",
  "WhatsApp",
  "Email",
  "LiveChat",
  "SMS",
  "FacebookMessenger",
  "InstagramDm"
];

const STATUSES = ["All", "Open", "New", "Snoozed", "Resolved", "Closed"];
const PRIORITIES: (ConversationPriority | "All")[] = ["All", "Urgent", "High", "Medium", "Low"];

export function InboxHeader({
  selectedChannel,
  onSelectChannel,
  selectedStatus,
  onSelectStatus,
  selectedPriority,
  onSelectPriority,
  searchKeyword,
  onSearchChange,
  totalActiveCount,
  totalUnreadCount,
  totalSlaBreached,
  onBulkAssign,
  selectedCount = 0
}: InboxHeaderProps) {
  return (
    <div className="w-full flex flex-col gap-4 p-5 bg-card/60 dark:bg-card/40 border border-border/60 rounded-2xl shadow-sm backdrop-blur-md transition-all">
      {/* Top Banner & KPI indicators */}
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
        <div className="flex items-center gap-3">
          <div className="p-2.5 bg-primary/10 text-primary rounded-xl flex items-center justify-center shadow-xs">
            <InboxIcon className="w-6 h-6" />
          </div>
          <div>
            <h1 className="text-xl font-bold text-foreground flex items-center gap-2">
              Omnichannel Unified Inbox
              <span className="text-xs font-semibold px-2 py-0.5 rounded-full bg-emerald-500/10 text-emerald-500 border border-emerald-500/20">
                SignalR Real-Time
              </span>
            </h1>
            <p className="text-xs text-muted-foreground">
              Centralizing customer messaging across 6 connected providers with SLA triage & AI assistance.
            </p>
          </div>
        </div>

        <div className="flex items-center gap-3 w-full sm:w-auto overflow-x-auto pb-1 sm:pb-0">
          <div className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-background border border-border text-xs font-medium whitespace-nowrap shadow-2xs">
            <span className="w-2 h-2 rounded-full bg-primary animate-pulse" />
            <span className="text-muted-foreground">Active:</span>
            <span className="font-bold text-foreground">{totalActiveCount}</span>
          </div>
          <div className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-amber-500/10 border border-amber-500/20 text-xs font-medium whitespace-nowrap text-amber-600 dark:text-amber-400 shadow-2xs">
            <span>Unread:</span>
            <span className="font-bold">{totalUnreadCount}</span>
          </div>
          {totalSlaBreached > 0 && (
            <div className="flex items-center gap-1.5 px-3 py-1.5 rounded-lg bg-rose-500/15 border border-rose-500/30 text-xs font-semibold whitespace-nowrap text-rose-600 dark:text-rose-400 animate-pulse shadow-2xs">
              <AlertTriangle className="w-3.5 h-3.5" />
              <span>SLA Breaches: {totalSlaBreached}</span>
            </div>
          )}
        </div>
      </div>

      {/* Channel switcher tabs */}
      <div className="flex items-center gap-1.5 overflow-x-auto pb-1 border-b border-border/40 text-xs font-medium">
        {CHANNELS.map(ch => {
          const isSelected = selectedChannel === ch;
          return (
            <button
              key={ch}
              onClick={() => onSelectChannel(ch)}
              className={`px-3.5 py-2 rounded-t-xl transition-all whitespace-nowrap border-b-2 font-semibold flex items-center gap-1.5 ${
                isSelected
                  ? "border-primary text-primary bg-primary/5 shadow-2xs"
                  : "border-transparent text-muted-foreground hover:text-foreground hover:bg-muted/50"
              }`}
            >
              <span>{ch === "All" ? "🌐 All Channels" : ch}</span>
            </button>
          );
        })}
      </div>

      {/* Search and dropdown filters */}
      <div className="flex flex-col md:flex-row items-center justify-between gap-3 pt-1">
        <div className="relative w-full md:w-80">
          <Search className="w-4 h-4 text-muted-foreground absolute left-3 top-1/2 -translate-y-1/2 pointer-events-none" />
          <input
            type="text"
            value={searchKeyword}
            onChange={e => onSearchChange(e.target.value)}
            placeholder="Search customer, subject or snippet..."
            className="w-full pl-9 pr-4 py-1.5 text-xs bg-background border border-border rounded-lg text-foreground focus:outline-hidden focus:ring-2 focus:ring-primary/40 focus:border-primary transition-all placeholder:text-muted-foreground"
          />
        </div>

        <div className="flex items-center gap-2.5 w-full md:w-auto overflow-x-auto justify-end">
          {selectedCount > 0 && (
            <Button
              variant="outline"
              size="sm"
              onClick={onBulkAssign}
              className="text-xs bg-primary/10 text-primary border-primary/30 h-8 gap-1.5 animate-in fade-in"
            >
              <CheckSquare className="w-3.5 h-3.5" />
              <span>Bulk Assign ({selectedCount})</span>
            </Button>
          )}

          <div className="flex items-center gap-1 text-xs text-muted-foreground shrink-0">
            <Filter className="w-3.5 h-3.5" />
            <span>Status:</span>
          </div>
          <select
            value={selectedStatus}
            onChange={e => onSelectStatus(e.target.value)}
            className="text-xs font-medium bg-background border border-border rounded-lg px-2.5 py-1.5 text-foreground focus:outline-hidden focus:ring-1 focus:ring-primary cursor-pointer"
          >
            {STATUSES.map(st => (
              <option key={st} value={st}>
                {st}
              </option>
            ))}
          </select>

          <div className="flex items-center gap-1 text-xs text-muted-foreground shrink-0 ml-2">
            <SlidersHorizontal className="w-3.5 h-3.5" />
            <span>Priority:</span>
          </div>
          <select
            value={selectedPriority}
            onChange={e => onSelectPriority(e.target.value as any)}
            className="text-xs font-medium bg-background border border-border rounded-lg px-2.5 py-1.5 text-foreground focus:outline-hidden focus:ring-1 focus:ring-primary cursor-pointer"
          >
            {PRIORITIES.map(p => (
              <option key={p} value={p}>
                {p}
              </option>
            ))}
          </select>

          <Button variant="ghost" size="icon" className="h-8 w-8 text-muted-foreground hover:text-foreground" title="Sort Order">
            <ArrowUpDown className="w-3.5 h-3.5" />
          </Button>
        </div>
      </div>
    </div>
  );
}
