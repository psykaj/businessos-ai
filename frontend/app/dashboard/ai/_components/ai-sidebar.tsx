"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { useParams, useRouter } from "next/navigation";
import { formatDistanceToNow } from "date-fns";
import { MessageSquare, Plus, MoreVertical, Trash2, Edit2 } from "lucide-react";
import { getConversations, AIConversation } from "@/lib/api/ai";
import { Button } from "@/components/ui/button";
import { ScrollArea } from "@/components/ui/scroll-area";
import { cn } from "@/lib/utils";

export function AiSidebar() {
  const router = useRouter();
  const params = useParams();
  const currentId = params.conversationId as string;
  const [conversations, setConversations] = useState<AIConversation[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchConversations = async () => {
      try {
        const data = await getConversations();
        setConversations(data);
      } catch (error) {
        console.error("Failed to load conversations", error);
      } finally {
        setLoading(false);
      }
    };
    fetchConversations();
  }, [currentId]); // Re-fetch on navigation for now

  return (
    <div className="w-64 border-r border-border bg-muted/20 hidden md:flex flex-col">
      <div className="p-4 border-b">
        <Button 
          className="w-full justify-start gap-2" 
          onClick={() => router.push('/dashboard/ai')}
        >
          <Plus className="h-4 w-4" />
          New Chat
        </Button>
      </div>
      <ScrollArea className="flex-1">
        <div className="p-2 space-y-1">
          {loading ? (
            <div className="p-4 text-center text-xs text-muted-foreground animate-pulse">Loading...</div>
          ) : conversations.length === 0 ? (
            <div className="p-4 text-center text-xs text-muted-foreground">No conversations yet</div>
          ) : (
            conversations.map((conv) => (
              <Link
                key={conv.id}
                href={`/dashboard/ai/${conv.id}`}
                className={cn(
                  "flex items-center gap-2 px-3 py-2 rounded-md text-sm transition-colors hover:bg-muted group",
                  currentId === conv.id ? "bg-muted font-medium text-foreground" : "text-muted-foreground"
                )}
              >
                <MessageSquare className="h-4 w-4 shrink-0" />
                <span className="truncate flex-1">{conv.title || "New Chat"}</span>
              </Link>
            ))
          )}
        </div>
      </ScrollArea>
    </div>
  );
}
