"use client";

import React from "react";
import { formatDistanceToNow } from "date-fns";
import {
  Clock,
  User,
  AlertCircle,
  CheckCircle2,
  Pin,
  Sparkles,
  ChevronRight,
  ShieldAlert
} from "lucide-react";
import { ConversationDto } from "@/lib/communication-service";
import { ChannelBadge } from "./ChannelBadge";
import { cn } from "@/lib/utils";

interface ConversationItemProps {
  conversation: ConversationDto;
  isSelected?: boolean;
  onSelect?: () => void;
  onToggleCheck?: () => void;
  isChecked?: boolean;
}

export function ConversationItem({
  conversation,
  isSelected = false,
  onSelect,
  onToggleCheck,
  isChecked = false
}: ConversationItemProps) {
  const isUnread = conversation.unreadMessagesCount > 0;
  const isBreached = conversation.isSlaBreached;

  const priorityBadge = {
    Urgent: "bg-rose-500/15 text-rose-600 dark:text-rose-400 border-rose-500/30",
    High: "bg-orange-500/15 text-orange-600 dark:text-orange-400 border-orange-500/30",
    Medium: "bg-blue-500/15 text-blue-600 dark:text-blue-400 border-blue-500/30",
    Low: "bg-slate-500/15 text-slate-600 dark:text-slate-400 border-slate-500/30"
  }[conversation.priority] || "bg-slate-500/10 text-slate-500";

  let timeAgo = "Just now";
  try {
    timeAgo = formatDistanceToNow(new Date(conversation.lastMessageAt), { addSuffix: true });
  } catch {
    // ignore invalid dates
  }

  return (
    <div
      onClick={onSelect}
      className={cn(
        "group relative flex items-start gap-3.5 p-4 rounded-xl border transition-all duration-200 cursor-pointer select-none",
        isSelected
          ? "bg-primary/5 dark:bg-primary/10 border-primary/40 shadow-sm ring-1 ring-primary/20"
          : isUnread
          ? "bg-card font-medium border-border/80 shadow-xs hover:border-primary/30"
          : "bg-card/50 hover:bg-card border-border/40 opacity-90 hover:opacity-100 hover:shadow-xs"
      )}
    >
      {/* Left selection checkbox or unread dot */}
      <div className="flex flex-col items-center justify-between h-full pt-1 gap-2" onClick={e => e.stopPropagation()}>
        <input
          type="checkbox"
          checked={isChecked}
          onChange={onToggleCheck}
          className="rounded-sm border-border text-primary focus:ring-primary/40 w-3.5 h-3.5 cursor-pointer opacity-40 group-hover:opacity-100 transition-opacity checked:opacity-100"
        />
        {isUnread && (
          <span className="w-2 h-2 rounded-full bg-primary ring-2 ring-primary/20 animate-pulse" title="Unread Message" />
        )}
      </div>

      {/* Main Conversation Details */}
      <div className="flex-1 min-w-0 space-y-1.5">
        {/* Row 1: Customer info and time elapsed */}
        <div className="flex items-center justify-between gap-2">
          <div className="flex items-center gap-2 truncate">
            <span className={cn("text-sm font-semibold truncate", isUnread ? "text-foreground font-bold" : "text-foreground/90")}>
              {conversation.customerName}
            </span>
            {conversation.customerCompany && (
              <span className="text-xs text-muted-foreground truncate hidden sm:inline-block">
                • {conversation.customerCompany}
              </span>
            )}
          </div>
          <span className="text-2xs font-medium text-muted-foreground whitespace-nowrap shrink-0">
            {timeAgo}
          </span>
        </div>

        {/* Row 2: Subject & Channel Badge */}
        <div className="flex items-center justify-between gap-2">
          <h4 className={cn("text-xs font-semibold truncate max-w-[70%]", isUnread ? "text-primary" : "text-foreground")}>
            {conversation.subject}
          </h4>
          <div className="flex items-center gap-1.5 shrink-0">
            <span className={cn("text-2xs font-semibold px-2 py-0.5 rounded-md border", priorityBadge)}>
              {conversation.priority}
            </span>
            <ChannelBadge type={conversation.channelType} size="sm" showLabel={false} />
          </div>
        </div>

        {/* Row 3: Message preview */}
        <p className="text-xs text-muted-foreground line-clamp-2 leading-relaxed font-normal">
          {conversation.lastMessagePreview || "No preview available."}
        </p>

        {/* Row 4: Triage meta tags (Assigned user & SLA status) */}
        <div className="flex flex-wrap items-center justify-between gap-2 pt-1.5 border-t border-border/30 text-2xs font-medium">
          <div className="flex items-center gap-1 text-muted-foreground">
            <User className="w-3 h-3 text-primary/70" />
            <span className="truncate max-w-[150px]">{conversation.assignedToUserName || "Unassigned"}</span>
          </div>

          <div className="flex items-center gap-2">
            {isBreached ? (
              <span className="inline-flex items-center gap-1 font-semibold text-rose-500 bg-rose-500/10 px-2 py-0.5 rounded border border-rose-500/20">
                <ShieldAlert className="w-3 h-3" />
                <span>SLA Breached</span>
              </span>
            ) : conversation.status === "Resolved" || conversation.status === "Closed" ? (
              <span className="inline-flex items-center gap-1 text-emerald-500">
                <CheckCircle2 className="w-3 h-3" />
                <span>{conversation.status}</span>
              </span>
            ) : (
              <span className="inline-flex items-center gap-1 text-muted-foreground" title="SLA Target Active">
                <Clock className="w-3 h-3 text-amber-500/80" />
                <span>On Target</span>
              </span>
            )}

            <ChevronRight className="w-3.5 h-3.5 text-muted-foreground/40 group-hover:text-primary transition-colors" />
          </div>
        </div>
      </div>
    </div>
  );
}
