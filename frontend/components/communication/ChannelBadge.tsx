"use client";

import React from "react";
import {
  Mail,
  MessageCircle,
  Smartphone,
  MessageSquare,
  Share2,
  Camera,
  Activity
} from "lucide-react";
import { cn } from "@/lib/utils";
import { CommunicationChannelType } from "@/lib/communication-service";

interface ChannelBadgeProps {
  type: CommunicationChannelType;
  showLabel?: boolean;
  size?: "sm" | "md" | "lg";
  className?: string;
  showStatusPulse?: boolean;
}

const CHANNEL_CONFIG: Record<
  CommunicationChannelType,
  { label: string; icon: React.ElementType; badgeClass: string; iconColor: string; bgStyle: string }
> = {
  WhatsApp: {
    label: "WhatsApp VIP",
    icon: MessageCircle,
    badgeClass: "bg-emerald-500/15 text-emerald-600 dark:text-emerald-400 border-emerald-500/30",
    iconColor: "text-emerald-500",
    bgStyle: "bg-emerald-50 dark:bg-emerald-950/40"
  },
  Email: {
    label: "Email Support",
    icon: Mail,
    badgeClass: "bg-blue-500/15 text-blue-600 dark:text-blue-400 border-blue-500/30",
    iconColor: "text-blue-500",
    bgStyle: "bg-blue-50 dark:bg-blue-950/40"
  },
  SMS: {
    label: "SMS Text",
    icon: Smartphone,
    badgeClass: "bg-amber-500/15 text-amber-600 dark:text-amber-400 border-amber-500/30",
    iconColor: "text-amber-500",
    bgStyle: "bg-amber-50 dark:bg-amber-950/40"
  },
  LiveChat: {
    label: "Live Chat Widget",
    icon: MessageSquare,
    badgeClass: "bg-indigo-500/15 text-indigo-600 dark:text-indigo-400 border-indigo-500/30",
    iconColor: "text-indigo-500",
    bgStyle: "bg-indigo-50 dark:bg-indigo-950/40"
  },
  FacebookMessenger: {
    label: "FB Messenger",
    icon: Share2,
    badgeClass: "bg-sky-500/15 text-sky-600 dark:text-sky-400 border-sky-500/30",
    iconColor: "text-sky-500",
    bgStyle: "bg-sky-50 dark:bg-sky-950/40"
  },
  InstagramDm: {
    label: "Instagram DM",
    icon: Camera,
    badgeClass: "bg-pink-500/15 text-pink-600 dark:text-pink-400 border-pink-500/30",
    iconColor: "text-pink-500",
    bgStyle: "bg-pink-50 dark:bg-pink-950/40"
  }
};

export function ChannelBadge({
  type,
  showLabel = true,
  size = "md",
  className,
  showStatusPulse = false
}: ChannelBadgeProps) {
  const config = CHANNEL_CONFIG[type] || {
    label: type,
    icon: Activity,
    badgeClass: "bg-slate-500/15 text-slate-600 dark:text-slate-400 border-slate-500/30",
    iconColor: "text-slate-500",
    bgStyle: "bg-slate-50 dark:bg-slate-950/40"
  };

  const Icon = config.icon;

  const sizeStyles = {
    sm: "text-xs px-2 py-0.5 gap-1",
    md: "text-xs font-semibold px-2.5 py-1 gap-1.5",
    lg: "text-sm font-semibold px-3 py-1.5 gap-2"
  }[size];

  const iconSizes = {
    sm: "w-3 h-3",
    md: "w-3.5 h-3.5",
    lg: "w-4 h-4"
  }[size];

  return (
    <div
      className={cn(
        "inline-flex items-center rounded-full border border-current/10 shadow-xs backdrop-blur-xs select-none transition-colors",
        config.badgeClass,
        sizeStyles,
        className
      )}
    >
      <Icon className={cn(iconSizes, config.iconColor, "shrink-0")} />
      {showLabel && <span className="truncate max-w-[120px]">{config.label}</span>}
      {showStatusPulse && (
        <span className="relative flex h-2 w-2 ml-1">
          <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75" />
          <span className="relative inline-flex rounded-full h-2 w-2 bg-emerald-500" />
        </span>
      )}
    </div>
  );
}
