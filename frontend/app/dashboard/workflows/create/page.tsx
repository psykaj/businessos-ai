"use client";

import React, { useState } from "react";
import { useRouter } from "next/navigation";
import { WorkflowToolbar } from "@/components/workflows/builder/workflow-toolbar";
import { WorkflowCanvas } from "@/components/workflows/builder/workflow-canvas";
import { AiWorkflowAssistant } from "@/components/workflows/ai-assistant";
import { useCreateWorkflow, useExecuteWorkflowManual } from "@/hooks/use-workflows";
import { toast } from "sonner";
import { Node, Edge } from "@xyflow/react";

export default function CreateWorkflowPage() {
  const router = useRouter();
  const [name, setName] = useState("New Automated Workflow");
  const [status, setStatus] = useState<"Draft" | "Active" | "Paused" | "Archived">("Draft");
  const [isAiOpen, setIsAiOpen] = useState(false);
  const [selectedNode, setSelectedNode] = useState<Node | null>(null);

  const createWorkflow = useCreateWorkflow();
  const executeManual = useExecuteWorkflowManual();

  const handleSave = () => {
    createWorkflow.mutate(
      {
        name,
        description: "Created via BusinessOS AI Visual Builder",
        trigger: {
          triggerType: "LeadCreated",
          triggerConfiguration: "{}",
          enabled: true,
        },
        actions: [
          {
            actionType: "SendWhatsApp",
            configuration: JSON.stringify({ Message: "Welcome to BusinessOS AI!" }),
            executionOrder: 1,
          },
        ],
      },
      {
        onSuccess: (data) => {
          router.push(`/dashboard/workflows/${data.id}`);
        },
      }
    );
  };

  const handleApplyAiWorkflow = (aiName: string, aiNodes: Node[], aiEdges: Edge[]) => {
    setName(aiName);
    toast.success("AI generated workflow applied to canvas!");
  };

  return (
    <div className="flex flex-col h-screen overflow-hidden bg-background">
      {/* Toolbar */}
      <WorkflowToolbar
        name={name}
        setName={setName}
        status={status}
        setStatus={setStatus}
        onSave={handleSave}
        onTestRun={() => toast.info("Run a test trigger from the dashboard once saved.")}
        onOpenAiAssistant={() => setIsAiOpen(true)}
        isSaving={createWorkflow.isPending}
      />

      {/* Visual Canvas */}
      <WorkflowCanvas
        selectedNode={selectedNode}
        setSelectedNode={setSelectedNode}
      />

      {/* AI Assistant Panel */}
      <AiWorkflowAssistant
        isOpen={isAiOpen}
        onClose={() => setIsAiOpen(false)}
        onApplyGeneratedWorkflow={handleApplyAiWorkflow}
      />
    </div>
  );
}
