"use client";

import React, { useState, useEffect, use } from "react";
import { useRouter } from "next/navigation";
import { WorkflowToolbar } from "@/components/workflows/builder/workflow-toolbar";
import { WorkflowCanvas } from "@/components/workflows/builder/workflow-canvas";
import { AiWorkflowAssistant } from "@/components/workflows/ai-assistant";
import { useWorkflow, useUpdateWorkflow } from "@/hooks/use-workflows";
import { toast } from "sonner";
import { Node, Edge } from "@xyflow/react";

export default function EditWorkflowPage({ params }: { params: Promise<{ id: string }> }) {
  const { id } = use(params);
  const router = useRouter();
  const { data: workflow, isLoading } = useWorkflow(id);
  const updateWorkflow = useUpdateWorkflow();

  const [name, setName] = useState("");
  const [status, setStatus] = useState<"Draft" | "Active" | "Paused" | "Archived">("Draft");
  const [isAiOpen, setIsAiOpen] = useState(false);
  const [selectedNode, setSelectedNode] = useState<Node | null>(null);

  useEffect(() => {
    if (workflow) {
      setName(workflow.name);
      setStatus(workflow.status);
    }
  }, [workflow]);

  if (isLoading) {
    return <div className="p-12 text-center text-sm text-muted-foreground">Loading workflow editor...</div>;
  }

  if (!workflow) {
    return <div className="p-12 text-center text-sm text-muted-foreground">Workflow not found</div>;
  }

  const handleSave = () => {
    updateWorkflow.mutate({
      id: workflow.id,
      payload: {
        name,
        description: workflow.description || "Updated via Visual Builder",
        status,
        trigger: {
          triggerType: workflow.trigger?.triggerType || "LeadCreated",
          triggerConfiguration: workflow.trigger?.triggerConfiguration || "{}",
          enabled: true,
        },
        actions: workflow.actions?.map((a) => ({
          actionType: a.actionType,
          configuration: a.configuration,
          executionOrder: a.executionOrder,
        })),
      },
    });
  };

  const handleApplyAiWorkflow = (aiName: string, aiNodes: Node[], aiEdges: Edge[]) => {
    setName(aiName);
    toast.success("AI generated workflow applied!");
  };

  return (
    <div className="flex flex-col h-screen overflow-hidden bg-background">
      <WorkflowToolbar
        name={name}
        setName={setName}
        status={status}
        setStatus={setStatus}
        onSave={handleSave}
        onTestRun={() => toast.info("Test execution started")}
        onOpenAiAssistant={() => setIsAiOpen(true)}
        isSaving={updateWorkflow.isPending}
      />

      <WorkflowCanvas
        selectedNode={selectedNode}
        setSelectedNode={setSelectedNode}
      />

      <AiWorkflowAssistant
        isOpen={isAiOpen}
        onClose={() => setIsAiOpen(false)}
        onApplyGeneratedWorkflow={handleApplyAiWorkflow}
      />
    </div>
  );
}
