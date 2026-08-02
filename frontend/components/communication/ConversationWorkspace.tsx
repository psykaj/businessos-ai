"use client";

import React, { useState } from "react";
import { format } from "date-fns";
import {
  MessageSquare,
  Lock,
  Loader2,
  AlertCircle,
  Paperclip,
  CheckCheck,
  Check,
  Clock,
  ArrowLeft,
  Search,
  Sparkles,
  RefreshCw,
  MoreVertical
} from "lucide-react";
import { useConversations, useConversationMessages, useConversationById } from "@/hooks/use-communication";
import { ConversationDto, MessageDto } from "@/lib/communication-service";
import { ChannelBadge } from "./ChannelBadge";
import { CustomerProfileSidebar } from "./CustomerProfileSidebar";
import { MessageComposer } from "./MessageComposer";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";

interface ConversationWorkspaceProps {
  initialConversationId?: string;
  onBackToInbox?: () => void;
}

export function ConversationWorkspace({ initialConversationId = "conv-101", onBackToInbox }: ConversationWorkspaceProps) {
  const { data: conversationsData, isLoading: isListLoading } = useConversations();
  const [activeId, setActiveId] = useState<string>(initialConversationId);
  const [searchQuery, setSearchQuery] = useState("");

  const activeConversation = conversationsData?.items.find(c => c.id === activeId) || conversationsData?.items[0];
  
  const { data: messages, isLoading: isMessagesLoading, refetch } = useConversationMessages(activeConversation?.id || "");

  const filteredConversations = conversationsData?.items.filter(c =>
    c.customerName.toLowerCase().includes(searchQuery.toLowerCase()) ||
    c.subject.toLowerCase().includes(searchQuery.toLowerCase())
  ) || [];

  return (
    <div className="flex-1 flex flex-col lg:flex-row border border-border/60 rounded-2xl bg-background shadow-md overflow-hidden min-h-[700px] h-[calc(100vh-140px)]">
      {/* COLUMN 1: Thread List Sidebar (Desktop) */}
      <div className="hidden xl:flex xl:flex-col w-80 border-r border-border bg-card/30 shrink-0 overflow-hidden">
        <div className="p-3.5 border-b border-border bg-card/60 font-semibold flex items-center justify-between text-xs">
          <span>Active Tickets ({filteredConversations.length})</span>
          <Button variant="ghost" size="icon" className="w-7 h-7 text-muted-foreground hover:text-foreground" onClick={() => refetch()}>
            <RefreshCw className="w-3.5 h-3.5" />
          </Button>
        </div>
        <div className="p-2 border-b border-border/40">
          <div className="relative">
            <Search className="w-3.5 h-3.5 text-muted-foreground absolute left-2.5 top-1/2 -translate-y-1/2 pointer-events-none" />
            <input
              type="text"
              value={searchQuery}
              onChange={e => setSearchQuery(e.target.value)}
              placeholder="Search active queues..."
              className="w-full pl-8 pr-3 py-1.5 text-xs bg-background border border-border rounded-lg focus:outline-hidden focus:ring-1 focus:ring-primary text-foreground placeholder:text-muted-foreground"
            />
          </div>
        </div>
        <div className="flex-1 overflow-y-auto divide-y divide-border/40">
          {isListLoading ? (
            <div className="p-8 text-center text-muted-foreground"><Loader2 className="w-6 h-6 animate-spin mx-auto mb-2" /></div>
          ) : (
            filteredConversations.map(conv => {
              const isSelected = conv.id === (activeConversation?.id || "");
              return (
                <div
                  key={conv.id}
                  onClick={() => setActiveId(conv.id)}
                  className={cn(
                    "p-3.5 transition-colors cursor-pointer text-xs space-y-1 select-none",
                    isSelected ? "bg-primary/10 font-medium border-l-4 border-primary pl-2.5" : "hover:bg-muted/60 opacity-90"
                  )}
                >
                  <div className="flex items-center justify-between">
                    <span className="font-bold text-foreground truncate max-w-[140px]">{conv.customerName}</span>
                    <ChannelBadge type={conv.channelType} size="sm" showLabel={false} />
                  </div>
                  <h5 className="font-semibold text-foreground/80 truncate">{conv.subject}</h5>
                  <p className="text-2xs text-muted-foreground line-clamp-1">{conv.lastMessagePreview}</p>
                </div>
              );
            })
          )}
        </div>
      </div>

      {/* COLUMN 2: Center Live Message Stream */}
      <div className="flex-1 flex flex-col min-w-0 bg-background/80 relative">
        {activeConversation ? (
          <>
            {/* Header toolbar */}
            <div className="flex items-center justify-between p-4 border-b border-border bg-card/60 backdrop-blur-md shrink-0">
              <div className="flex items-center gap-3 min-w-0">
                {onBackToInbox && (
                  <Button variant="ghost" size="icon" onClick={onBackToInbox} className="w-8 h-8 xl:hidden mr-1">
                    <ArrowLeft className="w-4 h-4" />
                  </Button>
                )}
                <div className="w-9 h-9 rounded-xl bg-linear-to-br from-indigo-500 to-purple-600 text-white font-bold text-sm flex items-center justify-center shadow-2xs shrink-0">
                  {activeConversation.customerName.charAt(0)}
                </div>
                <div className="min-w-0">
                  <h2 className="text-sm font-bold text-foreground truncate flex items-center gap-2">
                    <span>{activeConversation.subject}</span>
                    <ChannelBadge type={activeConversation.channelType} size="sm" showStatusPulse />
                  </h2>
                  <p className="text-2xs text-muted-foreground truncate">
                    Ticket ID: <span className="font-mono">{activeConversation.id}</span> • Assigned to: <span className="font-semibold text-primary">{activeConversation.assignedToUserName}</span>
                  </p>
                </div>
              </div>

              <div className="flex items-center gap-2 shrink-0">
                <Button variant="outline" size="sm" onClick={() => refetch()} className="text-xs h-8 gap-1 border-border">
                  <RefreshCw className="w-3 h-3" />
                  <span className="hidden md:inline-block">Sync</span>
                </Button>
                <Button variant="ghost" size="icon" className="w-8 h-8 text-muted-foreground">
                  <MoreVertical className="w-4 h-4" />
                </Button>
              </div>
            </div>

            {/* Message Stream Scroll Area */}
            <div className="flex-1 overflow-y-auto p-5 space-y-4 bg-muted/20">
              {isMessagesLoading ? (
                <div className="flex flex-col items-center justify-center h-full text-muted-foreground">
                  <Loader2 className="w-8 h-8 animate-spin text-primary mb-2" />
                  <span className="text-xs font-semibold">Decrypting secure chat transcript...</span>
                </div>
              ) : messages && messages.length > 0 ? (
                messages.map((msg: MessageDto) => {
                  const isNote = msg.direction === "InternalNote";
                  const isOutbound = msg.direction === "Outbound" || msg.direction === "System";
                  const isInbound = msg.direction === "Inbound";

                  let timestampFormatted = "Today";
                  try {
                    timestampFormatted = format(new Date(msg.createdAt), "hh:mm a");
                  } catch {
                    // ignore
                  }

                  return (
                    <div
                      key={msg.id}
                      className={cn(
                        "flex flex-col max-w-[85%] text-xs space-y-1 animate-in fade-in slide-in-from-bottom-2 duration-200",
                        isNote
                          ? "w-full max-w-full my-3"
                          : isOutbound
                          ? "ml-auto items-end"
                          : "mr-auto items-start"
                      )}
                    >
                      {/* Sender identification */}
                      <div className="flex items-center gap-1.5 text-2xs font-bold text-muted-foreground px-1">
                        <span>{msg.senderName}</span>
                        <span>•</span>
                        <span>{timestampFormatted}</span>
                      </div>

                      {/* Bubble Body */}
                      <div
                        className={cn(
                          "p-3.5 rounded-2xl shadow-xs text-xs font-normal leading-relaxed border",
                          isNote
                            ? "w-full bg-amber-500/10 border-amber-500/30 text-foreground dark:text-amber-100 rounded-lg shadow-sm"
                            : isOutbound
                            ? "bg-primary text-primary-foreground border-primary/20 rounded-tr-none"
                            : "bg-card text-foreground border-border/80 rounded-tl-none"
                        )}
                      >
                        {isNote && (
                          <div className="flex items-center gap-1 font-bold text-amber-600 dark:text-amber-400 mb-1.5 pb-1 border-b border-amber-500/20 text-2xs">
                            <Lock className="w-3 h-3" />
                            <span>CONFIDENTIAL INTERNAL TEAM NOTE (INVISIBLE TO CUSTOMER)</span>
                          </div>
                        )}
                        <p className="whitespace-pre-wrap font-sans">{msg.content}</p>

                        {/* Attachments UI */}
                        {msg.attachmentsJson && msg.attachmentsJson !== "[]" && (
                          <div className="mt-2 pt-2 border-t border-current/20 flex flex-wrap gap-2">
                            <div className="flex items-center gap-1.5 p-2 rounded-lg bg-background/60 dark:bg-black/20 border border-current/20 text-2xs font-mono">
                              <Paperclip className="w-3.5 h-3.5" />
                              <span>APAC-Seat-Schedule.pdf</span>
                              <span className="opacity-70">(1.4 MB)</span>
                            </div>
                          </div>
                        )}

                        {/* Status Checkmark */}
                        {isOutbound && (
                          <div className="flex items-center justify-end gap-1 mt-1 text-2xs opacity-80 font-mono" title="Delivered & Read">
                            <CheckCheck className="w-3.5 h-3.5 text-emerald-300" />
                            <span>{msg.status}</span>
                          </div>
                        )}
                      </div>
                    </div>
                  );
                })
              ) : (
                <div className="text-center py-12 text-muted-foreground text-xs">No previous communication transcript in this channel yet.</div>
              )}
            </div>

            {/* Bottom Message Composer Deck */}
            <MessageComposer conversation={activeConversation} onSendSuccess={() => refetch()} />
          </>
        ) : (
          <div className="flex flex-col items-center justify-center h-full p-12 text-center text-muted-foreground">
            <MessageSquare className="w-12 h-12 stroke-1 mb-3 text-primary/50" />
            <h3 className="text-base font-bold text-foreground">No Conversation Selected</h3>
            <p className="text-xs text-muted-foreground max-w-sm mt-1">
              Choose an inquiry from the inbox queue on the left to begin real-time support, AI drafting, or ticket assignment.
            </p>
          </div>
        )}
      </div>

      {/* COLUMN 3: Customer 360 & SLA Profile Sidebar */}
      {activeConversation && (
        <CustomerProfileSidebar conversation={activeConversation} />
      )}
    </div>
  );
}
