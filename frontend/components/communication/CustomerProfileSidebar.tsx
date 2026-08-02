"use client";

import React, { useState } from "react";
import {
  User,
  Building2,
  Mail,
  Phone,
  DollarSign,
  Tag,
  Clock,
  CheckCircle,
  AlertTriangle,
  Award,
  Sparkles,
  UserCheck,
  Shield,
  History,
  TrendingUp,
  Star
} from "lucide-react";
import { ConversationDto, CommunicationChannelType } from "@/lib/communication-service";
import { useAssignConversation, useUpdateConversationStatus } from "@/hooks/use-communication";
import { Button } from "@/components/ui/button";

interface CustomerProfileSidebarProps {
  conversation: ConversationDto;
  onCloseMobile?: () => void;
}

const TEAM_MEMBERS = [
  { id: "usr-201", name: "Sarah Jenkins (Senior AE)" },
  { id: "usr-202", name: "Marcus Vance (Support Eng)" },
  { id: "usr-203", name: "Chloe Bennett (CSM)" },
  { id: "usr-204", name: "Alex Rivera (VP Sales)" },
  { id: "usr-000", name: "Unassigned" }
];

export function CustomerProfileSidebar({ conversation, onCloseMobile }: CustomerProfileSidebarProps) {
  const assignMutation = useAssignConversation();
  const statusMutation = useUpdateConversationStatus();

  const [selectedAgent, setSelectedAgent] = useState(conversation.assignedToUserId || "usr-000");

  const handleAssignChange = (newUserId: string) => {
    setSelectedAgent(newUserId);
    const found = TEAM_MEMBERS.find(m => m.id === newUserId);
    assignMutation.mutate({
      id: conversation.id,
      assignedToUserId: newUserId,
      assignedToUserName: found?.name || "Unassigned",
      assignmentReason: "Agent manual routing"
    });
  };

  const handleStatusToggle = (newStatus: string) => {
    statusMutation.mutate({ id: conversation.id, status: newStatus });
  };

  const ltvFormatted = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    maximumFractionDigits: 0
  }).format(conversation.customerLifetimeValue || 12500);

  const tagsList = conversation.tags
    ? conversation.tags.split(",").map(t => t.trim()).filter(Boolean)
    : ["Enterprise", "Inbound", "VIP"];

  return (
    <div className="w-full lg:w-80 border-t lg:border-t-0 lg:border-l border-border bg-card/40 flex flex-col h-full overflow-y-auto p-4 gap-5 text-xs font-normal">
      {/* Customer Avatar & Primary Details */}
      <div className="flex items-center gap-3 pb-4 border-b border-border/60">
        <div className="w-12 h-12 rounded-2xl bg-linear-to-tr from-primary to-indigo-500 flex items-center justify-center text-primary-foreground font-bold text-lg shadow-md shrink-0">
          {conversation.customerName.charAt(0).toUpperCase()}
        </div>
        <div className="min-w-0 flex-1">
          <h3 className="text-sm font-bold text-foreground truncate">{conversation.customerName}</h3>
          {conversation.customerCompany && (
            <p className="text-xs text-primary font-semibold truncate flex items-center gap-1 mt-0.5">
              <Building2 className="w-3.5 h-3.5 shrink-0" />
              <span>{conversation.customerCompany}</span>
            </p>
          )}
          <span className="inline-block mt-1 px-2 py-0.5 rounded-full text-2xs font-semibold bg-emerald-500/10 text-emerald-500 border border-emerald-500/20">
            Active Verified Client
          </span>
        </div>
      </div>

      {/* Business Value Pillar KPI Box */}
      <div className="p-3.5 rounded-xl bg-linear-to-r from-primary/10 via-indigo-500/10 to-transparent border border-primary/20 space-y-2 shadow-xs">
        <div className="flex items-center justify-between">
          <span className="text-2xs font-bold text-muted-foreground uppercase tracking-wider flex items-center gap-1">
            <TrendingUp className="w-3.5 h-3.5 text-emerald-500" />
            Lifetime Revenue Value
          </span>
          <Award className="w-4 h-4 text-amber-500 animate-bounce" />
        </div>
        <div className="text-xl font-black text-foreground">{ltvFormatted}</div>
        <p className="text-2xs text-muted-foreground leading-relaxed">
          High LTV accounts have SLA response targets prioritized automatically by BusinessOS AI.
        </p>
      </div>

      {/* Assignment & Routing Controls */}
      <div className="space-y-3 pb-4 border-b border-border/60">
        <h4 className="text-xs font-bold text-foreground uppercase tracking-wider flex items-center gap-1.5 text-muted-foreground">
          <UserCheck className="w-3.5 h-3.5 text-primary" />
          <span>Ticket Assignment</span>
        </h4>
        <div className="space-y-1">
          <label className="text-2xs font-medium text-muted-foreground">Assigned Team Agent</label>
          <select
            value={selectedAgent}
            onChange={e => handleAssignChange(e.target.value)}
            disabled={assignMutation.isPending}
            className="w-full text-xs font-medium bg-background border border-border rounded-lg p-2 text-foreground focus:ring-2 focus:ring-primary/40 focus:outline-hidden cursor-pointer"
          >
            {TEAM_MEMBERS.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </div>
      </div>

      {/* SLA Target & Status */}
      <div className="space-y-3 pb-4 border-b border-border/60">
        <h4 className="text-xs font-bold uppercase tracking-wider flex items-center gap-1.5 text-muted-foreground">
          <Clock className="w-3.5 h-3.5 text-amber-500" />
          <span>SLA Target & Status</span>
        </h4>
        <div className="flex flex-col gap-2">
          <div className="flex items-center justify-between text-2xs p-2.5 rounded-lg bg-background border border-border">
            <span className="text-muted-foreground">Current State:</span>
            <span className="font-bold text-foreground capitalize px-2 py-0.5 rounded bg-muted/70">
              {conversation.status}
            </span>
          </div>

          <div className="grid grid-cols-2 gap-2 pt-1">
            {conversation.status !== "Resolved" && (
              <Button
                variant="outline"
                size="sm"
                onClick={() => handleStatusToggle("Resolved")}
                disabled={statusMutation.isPending}
                className="text-xs font-semibold border-emerald-500/30 text-emerald-600 dark:text-emerald-400 bg-emerald-500/10 hover:bg-emerald-500/20 h-8"
              >
                <CheckCircle className="w-3.5 h-3.5 mr-1" />
                Resolve
              </Button>
            )}
            <Button
              variant="outline"
              size="sm"
              onClick={() => handleStatusToggle(conversation.status === "Snoozed" ? "Open" : "Snoozed")}
              disabled={statusMutation.isPending}
              className="text-xs font-medium h-8"
            >
              {conversation.status === "Snoozed" ? "Un-snooze" : "Snooze 24h"}
            </Button>
          </div>
        </div>
      </div>

      {/* Contact & Verification info */}
      <div className="space-y-2.5 pb-4 border-b border-border/60">
        <h4 className="text-xs font-bold uppercase tracking-wider flex items-center gap-1.5 text-muted-foreground">
          <Shield className="w-3.5 h-3.5 text-indigo-500" />
          <span>Contact Coordinates</span>
        </h4>
        {conversation.customerEmail && (
          <div className="flex items-center gap-2 text-2xs p-2 rounded-lg bg-background border border-border/60 truncate">
            <Mail className="w-3.5 h-3.5 text-muted-foreground shrink-0" />
            <span className="truncate select-all text-foreground font-mono">{conversation.customerEmail}</span>
          </div>
        )}
        {conversation.customerPhone && (
          <div className="flex items-center gap-2 text-2xs p-2 rounded-lg bg-background border border-border/60 truncate">
            <Phone className="w-3.5 h-3.5 text-muted-foreground shrink-0" />
            <span className="truncate select-all text-foreground font-mono">{conversation.customerPhone}</span>
          </div>
        )}
      </div>

      {/* Tags List */}
      <div className="space-y-2 pb-4 border-b border-border/60">
        <h4 className="text-xs font-bold uppercase tracking-wider flex items-center gap-1.5 text-muted-foreground">
          <Tag className="w-3.5 h-3.5 text-sky-500" />
          <span>Customer Tags</span>
        </h4>
        <div className="flex flex-wrap gap-1.5">
          {tagsList.map((tag, i) => (
            <span
              key={i}
              className="px-2.5 py-1 rounded-md text-2xs font-semibold bg-muted text-foreground border border-border shadow-2xs"
            >
              #{tag}
            </span>
          ))}
          <button className="px-2 py-1 rounded-md text-2xs font-medium text-primary hover:bg-primary/10 border border-dashed border-primary/40 transition-colors">
            + Add Tag
          </button>
        </div>
      </div>

      {/* CSAT Rating history if present */}
      {conversation.csatRating && (
        <div className="p-3 rounded-xl bg-amber-500/10 border border-amber-500/20 space-y-1">
          <div className="flex items-center justify-between text-amber-600 dark:text-amber-400 font-bold text-xs">
            <span className="flex items-center gap-1">
              <Star className="w-3.5 h-3.5 fill-current" />
              <span>CSAT Rating: {conversation.csatRating}/5</span>
            </span>
            <span className="text-2xs font-medium">Verified</span>
          </div>
          <p className="text-2xs text-muted-foreground italic">&ldquo;{conversation.csatFeedback}&rdquo;</p>
        </div>
      )}

      {/* AI Suggested Response Prompt Box */}
      <div className="mt-auto p-3 rounded-xl bg-primary/5 border border-primary/20 space-y-2">
        <div className="flex items-center gap-1.5 text-primary font-bold text-xs">
          <Sparkles className="w-4 h-4 text-amber-500 animate-spin" style={{ animationDuration: "6s" }} />
          <span>BusinessOS AI Copilot Ready</span>
        </div>
        <p className="text-2xs text-muted-foreground">
          AI analyzes recent messages and document templates to draft instantaneous context-aware customer answers.
        </p>
      </div>
    </div>
  );
}
