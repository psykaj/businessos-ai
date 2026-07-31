"use client";

import { use } from "react";
import { WorkflowBuilder } from "@/components/automation-studio/WorkflowBuilder";
import { useAutomationWorkflow } from "@/hooks/useAutomationStudio";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import Link from "next/link";
import { Skeleton } from "@/components/ui/skeleton";

export default function EditWorkflowPage({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  const isNew = resolvedParams.id === "new";
  
  const { data: workflow, isLoading } = useAutomationWorkflow(isNew ? "" : resolvedParams.id);

  if (isLoading && !isNew) {
    return (
      <div className="p-6 space-y-4">
        <Skeleton className="h-10 w-48" />
        <Skeleton className="h-[calc(100vh-140px)] w-full" />
      </div>
    );
  }

  return (
    <div className="flex flex-col h-full max-w-[1600px] mx-auto p-4 gap-4">
      <div className="flex items-center gap-4">
        <Link href="/dashboard/automation/workflows">
          <Button variant="ghost" size="icon">
            <ArrowLeft className="h-4 w-4" />
          </Button>
        </Link>
        <div>
          <h1 className="text-xl font-bold">{isNew ? "Create Workflow" : workflow?.name}</h1>
          <p className="text-xs text-muted-foreground">{isNew ? "Drag and drop to build your automation" : workflow?.description}</p>
        </div>
      </div>

      <WorkflowBuilder 
        workflowId={resolvedParams.id} 
        initialNodes={workflow?.nodes || []} 
        initialEdges={workflow?.edges || []} 
      />
    </div>
  );
}
