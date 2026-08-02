"use client";

import React, { Suspense } from "react";
import { useSearchParams, useRouter } from "next/navigation";
import { ConversationWorkspace } from "@/components/communication/ConversationWorkspace";
import { Loader2 } from "lucide-react";

function ConversationWorkspaceContent() {
  const searchParams = useSearchParams();
  const router = useRouter();
  const initialId = searchParams.get("id") || "conv-101";

  return (
    <ConversationWorkspace
      initialConversationId={initialId}
      onBackToInbox={() => router.push("/dashboard/inbox")}
    />
  );
}

export default function ConversationsWorkspacePage() {
  return (
    <div className="w-full flex flex-col min-h-[750px] animate-in fade-in duration-200 pb-8">
      <Suspense
        fallback={
          <div className="w-full h-[600px] flex items-center justify-center text-muted-foreground">
            <Loader2 className="w-8 h-8 animate-spin text-primary mr-2" />
            <span className="font-semibold text-sm">Initializing 3-Column Conversation Workspace...</span>
          </div>
        }
      >
        <ConversationWorkspaceContent />
      </Suspense>
    </div>
  );
}
