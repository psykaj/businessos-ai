"use client";

import React, { useState } from "react";
import { useRouter } from "next/navigation";
import { useConversations, useInboxSummary, useAssignConversation } from "@/hooks/use-communication";
import { CommunicationChannelType, ConversationPriority } from "@/lib/communication-service";
import { InboxHeader } from "@/components/communication/InboxHeader";
import { ConversationList } from "@/components/communication/ConversationList";
import { Button } from "@/components/ui/button";
import { CheckSquare, User, X } from "lucide-react";
import { toast } from "sonner";

const TEAM_AGENTS = [
  { id: "usr-201", name: "Sarah Jenkins (Senior AE)" },
  { id: "usr-202", name: "Marcus Vance (Support Eng)" },
  { id: "usr-203", name: "Chloe Bennett (CSM)" }
];

export default function UnifiedInboxPage() {
  const router = useRouter();
  
  // Filter States
  const [selectedChannel, setSelectedChannel] = useState<CommunicationChannelType | "All">("All");
  const [selectedStatus, setSelectedStatus] = useState("Open");
  const [selectedPriority, setSelectedPriority] = useState<ConversationPriority | "All">("All");
  const [searchKeyword, setSearchKeyword] = useState("");
  const [selectedCheckIds, setSelectedCheckIds] = useState<string[]>([]);
  const [showBulkAssignModal, setShowBulkAssignModal] = useState(false);

  const { data: summary } = useInboxSummary();
  const { data: conversationsData, isLoading } = useConversations({
    channelType: selectedChannel === "All" ? undefined : selectedChannel,
    status: selectedStatus === "All" ? undefined : selectedStatus,
    priority: selectedPriority === "All" ? undefined : selectedPriority,
    searchKeyword: searchKeyword || undefined
  });

  const assignMutation = useAssignConversation();

  const handleSelectConversation = (id: string) => {
    router.push(`/dashboard/conversations?id=${id}`);
  };

  const handleToggleCheck = (id: string) => {
    setSelectedCheckIds(prev =>
      prev.includes(id) ? prev.filter(item => item !== id) : [...prev, id]
    );
  };

  const handleBulkAssignConfirm = (agentId: string, agentName: string) => {
    selectedCheckIds.forEach(id => {
      assignMutation.mutate({ id, assignedToUserId: agentId, assignedToUserName: agentName });
    });
    toast.success(`Successfully assigned ${selectedCheckIds.length} tickets to ${agentName}!`);
    setSelectedCheckIds([]);
    setShowBulkAssignModal(false);
  };

  return (
    <div className="w-full flex flex-col gap-4 min-h-[750px] animate-in fade-in duration-200 pb-10">
      <InboxHeader
        selectedChannel={selectedChannel}
        onSelectChannel={setSelectedChannel}
        selectedStatus={selectedStatus}
        onSelectStatus={setSelectedStatus}
        selectedPriority={selectedPriority}
        onSelectPriority={setSelectedPriority}
        searchKeyword={searchKeyword}
        onSearchChange={setSearchKeyword}
        totalActiveCount={summary?.totalActiveConversations ?? 24}
        totalUnreadCount={summary?.totalUnreadConversations ?? 4}
        totalSlaBreached={summary?.totalSlaBreached ?? 1}
        onBulkAssign={() => setShowBulkAssignModal(true)}
        selectedCount={selectedCheckIds.length}
      />

      <ConversationList
        conversations={conversationsData?.items || []}
        isLoading={isLoading}
        onSelect={handleSelectConversation}
        selectedCheckIds={selectedCheckIds}
        onToggleCheck={handleToggleCheck}
      />

      {/* Bulk Assign Modal */}
      {showBulkAssignModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-xs p-4">
          <div className="w-full max-w-sm bg-card border border-border rounded-2xl p-6 shadow-xl space-y-4 text-xs animate-in zoom-in-95 duration-150">
            <div className="flex items-center justify-between border-b border-border/60 pb-3">
              <h3 className="text-base font-bold text-foreground flex items-center gap-2">
                <CheckSquare className="w-4 h-4 text-primary" />
                <span>Bulk Assign ({selectedCheckIds.length} Tickets)</span>
              </h3>
              <Button variant="ghost" size="icon" onClick={() => setShowBulkAssignModal(false)} className="w-7 h-7">
                <X className="w-4 h-4" />
              </Button>
            </div>
            <p className="text-muted-foreground leading-relaxed">
              Choose a target support agent or account executive to receive immediate responsibility for these selected customer inquiries.
            </p>

            <div className="space-y-2 pt-2">
              {TEAM_AGENTS.map(agent => (
                <button
                  key={agent.id}
                  onClick={() => handleBulkAssignConfirm(agent.id, agent.name)}
                  className="w-full flex items-center justify-between p-3 rounded-xl border border-border/70 hover:border-primary bg-background/50 hover:bg-primary/5 font-semibold text-foreground transition-all text-xs"
                >
                  <div className="flex items-center gap-2">
                    <User className="w-3.5 h-3.5 text-primary" />
                    <span>{agent.name}</span>
                  </div>
                  <span className="text-2xs text-primary font-bold">Select</span>
                </button>
              ))}
            </div>

            <div className="flex justify-end pt-2">
              <Button variant="ghost" size="sm" onClick={() => setShowBulkAssignModal(false)}>
                Cancel
              </Button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
