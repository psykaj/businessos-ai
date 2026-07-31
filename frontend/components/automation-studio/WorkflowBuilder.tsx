"use client";

import React, { useState, useCallback, useRef } from "react";
import {
  ReactFlow,
  ReactFlowProvider,
  addEdge,
  useNodesState,
  useEdgesState,
  Controls,
  Background,
  Connection,
  Edge,
  Node,
  ReactFlowInstance
} from "@xyflow/react";
import "@xyflow/react/dist/style.css";

import { Sidebar } from "./Sidebar";
import { NodeConfigPanel } from "./NodeConfigPanel";
import { TriggerNode } from "./nodes/TriggerNode";
import { ActionNode } from "./nodes/ActionNode";
import { ConditionNode } from "./nodes/ConditionNode";
import { Save } from "lucide-react";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";
import { useSaveWorkflow } from "@/hooks/useAutomationStudio";

const nodeTypes = {
  triggerNode: TriggerNode,
  actionNode: ActionNode,
  conditionNode: ConditionNode,
};

let id = 0;
const getId = () => `dndnode_${id++}`;

function WorkflowBuilderInner({ workflowId, initialNodes = [], initialEdges = [] }: { workflowId: string, initialNodes?: Node[], initialEdges?: Edge[] }) {
  const reactFlowWrapper = useRef<HTMLDivElement>(null);
  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const [reactFlowInstance, setReactFlowInstance] = useState<ReactFlowInstance | null>(null);
  
  const [selectedNodeId, setSelectedNodeId] = useState<string | null>(null);

  const { mutate: saveWorkflow, isPending } = useSaveWorkflow();

  const onConnect = useCallback(
    (params: Connection | Edge) => setEdges((eds) => addEdge({ ...params, animated: true }, eds)),
    [setEdges]
  );

  const onDragOver = useCallback((event: React.DragEvent) => {
    event.preventDefault();
    event.dataTransfer.dropEffect = "move";
  }, []);

  const onDrop = useCallback(
    (event: React.DragEvent) => {
      event.preventDefault();

      if (!reactFlowInstance) return;

      const type = event.dataTransfer.getData("application/reactflow");
      const label = event.dataTransfer.getData("application/reactflow-label");

      if (typeof type === "undefined" || !type) return;

      const position = reactFlowInstance.screenToFlowPosition({
        x: event.clientX,
        y: event.clientY,
      });
      const newNode = {
        id: getId(),
        type,
        position,
        data: { label, type },
      };

      setNodes((nds) => nds.concat(newNode));
    },
    [reactFlowInstance, setNodes]
  );

  const onNodeClick = (event: React.MouseEvent, node: Node) => {
    setSelectedNodeId(node.id);
  };

  const onPaneClick = () => {
    setSelectedNodeId(null);
  };

  const updateNodeData = (nodeId: string, newData: Record<string, unknown>) => {
    setNodes((nds) =>
      nds.map((node) => {
        if (node.id === nodeId) {
          node.data = { ...node.data, ...newData };
        }
        return node;
      })
    );
  };

  const handleSave = () => {
    saveWorkflow({
      id: workflowId,
      payload: { nodes, edges }
    });
  };

  const selectedNode = nodes.find(n => n.id === selectedNodeId);

  return (
    <div className="flex w-full h-[calc(100vh-140px)] border rounded-xl overflow-hidden relative bg-background">
      <Sidebar />
      <div className="flex-1 relative" ref={reactFlowWrapper}>
        <div className="absolute top-4 right-4 z-10">
          <Button onClick={handleSave} disabled={isPending} className="shadow-lg gap-2">
            <Save className="h-4 w-4" />
            {isPending ? "Saving..." : "Save Workflow"}
          </Button>
        </div>
        
        <ReactFlow
          nodes={nodes}
          edges={edges}
          onNodesChange={onNodesChange}
          onEdgesChange={onEdgesChange}
          onConnect={onConnect}
          onInit={setReactFlowInstance}
          onDrop={onDrop}
          onDragOver={onDragOver}
          onNodeClick={onNodeClick}
          onPaneClick={onPaneClick}
          nodeTypes={nodeTypes}
          fitView
          className="bg-muted/10"
        >
          <Controls />
          <Background color="#ccc" gap={16} size={1} />
        </ReactFlow>

        {selectedNode && (
          <NodeConfigPanel 
            node={selectedNode} 
            onClose={() => setSelectedNodeId(null)} 
            onUpdate={updateNodeData} 
          />
        )}
      </div>
    </div>
  );
}

export function WorkflowBuilder(props: { workflowId: string, initialNodes?: Node[], initialEdges?: Edge[] }) {
  return (
    <ReactFlowProvider>
      <WorkflowBuilderInner {...props} />
    </ReactFlowProvider>
  );
}
