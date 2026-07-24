"use client";

import { useState } from "react";
import { CopilotNav } from "@/components/copilot/CopilotNav";
import { useCopilot } from "@/hooks/use-copilot";
import {
  MessageSquare,
  Search,
  Archive,
  Trash2,
  Calendar,
  ExternalLink,
  Clock,
  Filter,
} from "lucide-react";
import Link from "next/link";

export default function CopilotHistoryPage() {
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState<string | undefined>(undefined);

  const { useConversations, archiveConversation, deleteConversation } = useCopilot();
  const { data: conversationsData, isLoading } = useConversations({
    search: search || undefined,
    status: statusFilter,
  });

  const conversations = conversationsData?.items || [];

  return (
    <div className="flex flex-col space-y-6 pb-12">
      <CopilotNav />

      <div className="px-6 space-y-6">
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h2 className="text-xl font-bold text-foreground">Conversation History</h2>
            <p className="text-xs text-muted-foreground">
              Search and manage your AI Business Assistant chat sessions and threads.
            </p>
          </div>

          <div className="flex items-center gap-3">
            <div className="relative flex-1 sm:w-64">
              <Search className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
              <input
                type="text"
                placeholder="Search conversation title..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="w-full rounded-xl border border-border/60 bg-card pl-9 pr-4 py-2 text-xs text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-indigo-500/20"
              />
            </div>

            <select
              value={statusFilter || ""}
              onChange={(e) => setStatusFilter(e.target.value || undefined)}
              className="rounded-xl border border-border/60 bg-card px-3 py-2 text-xs text-foreground focus:outline-none"
            >
              <option value="">All Statuses</option>
              <option value="Active">Active</option>
              <option value="Archived">Archived</option>
            </select>
          </div>
        </div>

        {isLoading ? (
          <div className="rounded-2xl border border-border/60 bg-card p-12 text-center text-xs text-muted-foreground animate-pulse">
            Loading conversation history...
          </div>
        ) : conversations.length === 0 ? (
          <div className="rounded-2xl border border-border/60 bg-card p-12 text-center space-y-3">
            <MessageSquare className="h-10 w-10 text-muted-foreground mx-auto" />
            <h3 className="text-sm font-semibold text-foreground">No conversations found</h3>
            <p className="text-xs text-muted-foreground">
              Start a new chat from the Copilot Workspace.
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {conversations.map((conv: any) => (
              <div
                key={conv.id}
                className="group flex flex-col justify-between rounded-2xl border border-border/60 bg-card p-5 shadow-sm transition-all hover:border-indigo-500/50 hover:shadow-md"
              >
                <div className="space-y-3">
                  <div className="flex items-start justify-between gap-2">
                    <span className="flex items-center gap-2 text-sm font-bold text-foreground group-hover:text-indigo-600 dark:group-hover:text-indigo-400 transition-colors">
                      <MessageSquare className="h-4 w-4 text-indigo-500 shrink-0" />
                      <span className="truncate">{conv.title}</span>
                    </span>
                    <span
                      className={`rounded-full px-2 py-0.5 text-[10px] font-semibold border ${
                        conv.status === "Active"
                          ? "bg-emerald-500/10 text-emerald-500 border-emerald-500/20"
                          : "bg-muted text-muted-foreground border-border"
                      }`}
                    >
                      {conv.status}
                    </span>
                  </div>

                  <div className="flex items-center gap-4 text-xs text-muted-foreground">
                    <span className="flex items-center gap-1">
                      <Clock className="h-3.5 w-3.5" />
                      {new Date(conv.updatedAt).toLocaleDateString()}
                    </span>
                    <span>{conv.messageCount || 0} messages</span>
                  </div>
                </div>

                <div className="flex items-center justify-between pt-4 mt-4 border-t border-border/50">
                  <Link
                    href={`/dashboard/copilot?thread=${conv.id}`}
                    className="flex items-center gap-1.5 text-xs font-semibold text-indigo-600 dark:text-indigo-400 hover:underline"
                  >
                    Open Thread
                    <ExternalLink className="h-3.5 w-3.5" />
                  </Link>

                  <div className="flex items-center gap-2">
                    {conv.status === "Active" && (
                      <button
                        onClick={() => archiveConversation(conv.id)}
                        className="rounded-lg p-1.5 text-muted-foreground hover:bg-muted hover:text-foreground"
                        title="Archive"
                      >
                        <Archive className="h-3.5 w-3.5" />
                      </button>
                    )}
                    <button
                      onClick={() => deleteConversation(conv.id)}
                      className="rounded-lg p-1.5 text-muted-foreground hover:bg-red-500/10 hover:text-red-500"
                      title="Delete"
                    >
                      <Trash2 className="h-3.5 w-3.5" />
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
