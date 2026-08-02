"use client";

import React from "react";
import { Inbox, Sparkles, Loader2, ArrowRight } from "lucide-react";
import { ConversationDto } from "@/lib/communication-service";
import { ConversationItem } from "./ConversationItem";
import { Button } from "@/components/ui/button";

interface ConversationListProps {
  conversations: ConversationDto[];
  isLoading?: boolean;
  selectedId?: string;
  onSelect: (id: string) => void;
  selectedCheckIds: string[];
  onToggleCheck: (id: string) => void;
}

export function ConversationList({
  conversations,
  isLoading = false,
  selectedId,
  onSelect,
  selectedCheckIds,
  onToggleCheck
}: ConversationListProps) {
  if (isLoading) {
    return (
      <div className="flex-1 flex flex-col items-center justify-center p-12 text-muted-foreground min-h-[400px] border border-border/60 rounded-2xl bg-card/30 backdrop-blur-xs">
        <Loader2 className="w-8 h-8 animate-spin text-primary mb-3" />
        <p className="text-sm font-medium">Synchronizing live multichannel conversations...</p>
        <p className="text-xs text-muted-foreground mt-1">Checking SignalR real-time websocket queues...</p>
      </div>
    );
  }

  if (conversations.length === 0) {
    return (
      <div className="flex-1 flex flex-col items-center justify-center p-12 text-center min-h-[400px] border border-dashed border-border rounded-2xl bg-card/20 backdrop-blur-xs">
        <div className="p-4 bg-emerald-500/10 text-emerald-500 rounded-full mb-3 shadow-inner">
          <Inbox className="w-8 h-8" />
        </div>
        <h3 className="text-base font-semibold text-foreground">Inbox is Completely Zero-Clear! 🎉</h3>
        <p className="text-xs text-muted-foreground max-w-sm mt-1 mb-4">
          No conversations match your selected filters or channel triage rules. You are up to date across all connected providers!
        </p>
        <Button variant="outline" size="sm" onClick={() => window.location.reload()} className="text-xs">
          Refresh Live Queue
        </Button>
      </div>
    );
  }

  return (
    <div className="flex-1 flex flex-col gap-2.5 overflow-y-auto pr-1">
      {conversations.map(conv => (
        <ConversationItem
          key={conv.id}
          conversation={conv}
          isSelected={selectedId === conv.id}
          onSelect={() => onSelect(conv.id)}
          isChecked={selectedCheckIds.includes(conv.id)}
          onToggleCheck={() => onToggleCheck(conv.id)}
        />
      ))}

      <div className="py-3 text-center text-2xs font-medium text-muted-foreground flex items-center justify-center gap-1.5 border-t border-border/30 mt-2">
        <Sparkles className="w-3.5 h-3.5 text-amber-500" />
        <span>End of triage queue • Displaying {conversations.length} active threads</span>
      </div>
    </div>
  );
}
