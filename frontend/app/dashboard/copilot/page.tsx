"use client";

import { CopilotNav } from "@/components/copilot/CopilotNav";
import { CopilotWorkspace } from "@/components/copilot/CopilotWorkspace";

export default function CopilotPage() {
  return (
    <div className="flex flex-col h-full space-y-4">
      <CopilotNav />
      <div className="px-6">
        <CopilotWorkspace />
      </div>
    </div>
  );
}
