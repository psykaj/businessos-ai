"use client";

import React, { useState, useCallback, useMemo } from "react";
import {
  ReactFlow,
  MiniMap,
  Controls,
  Background,
  useNodesState,
  useEdgesState,
  addEdge,
  Connection,
  Edge,
  Node,
  BackgroundVariant,
} from "@xyflow/react";

import "@xyflow/react/dist/style.css";

import { TriggerNode } from "./trigger-node";
import { ActionNode } from "./action-node";
import { LogicNode } from "./logic-node";
import { NodePalette } from "./node-palette";
import { NodeConfigDrawer } from "./node-config-drawer";

const nodeTypes = {
  trigger: TriggerNode,
  action: ActionNode,
  logic: LogicNode,
};

interface WorkflowCanvasProps {
  initialNodes?: Node[];
  initialEdges?: Edge[];
  onSaveGraph?: (nodes: Node[], edges: Edge[]) => void;
  selectedNode: Node | null;
  setSelectedNode: (node: Node | null) => void;
}

export function WorkflowCanvas({
  initialNodes = [],
  initialEdges = [],
  onSaveGraph,
  selectedNode,
  setSelectedNode,
}: WorkflowCanvasProps) {
  const [nodes, setNodes, onNodesChange] = useNodesState(
    initialNodes.length > 0
      ? initialNodes
      : [
          {
            id: "node-1",
            type: "trigger",
            position: { x: 250, y: 100 },
            data: {
              triggerType: "LeadCreated",
              label: "Lead Created",
              description: "When a new lead enters CRM",
            },
          },
          {
            id: "node-2",
            type: "action",
            position: { x: 250, y: 280 },
            data: {
              actionType: "SendWhatsApp",
              label: "Send WhatsApp Welcome",
              description: "Send automated message to new lead",
            },
          },
        ]
  );

  const [edges, setEdges, onEdgesChange] = useEdgesState(
    initialEdges.length > 0
      ? initialEdges
      : [
          {
            id: "edge-1-2",
            source: "node-1",
            target: "node-2",
            animated: true,
            style: { stroke: "#6366f1", strokeWidth: 2 },
          },
        ]
  );

  const onConnect = useCallback(
    (params: Connection | Edge) =>
      setEdges((eds) =>
        addEdge(
          {
            ...params,
            animated: true,
          },
          eds
        )
      ),
    [setEdges]
  );

  const handleAddNode = useCallback(
    (kind: "trigger" | "action" | "logic", type: string, label: string) => {
      const newNodeId = `node-${Date.now()}`;
      const newNode: Node = {
        id: newNodeId,
        type: kind,
        position: {
          x: 250 + (nodes.length % 3) * 40,
          y: 120 + nodes.length * 90,
        },
        data: {
          [kind === "trigger" ? "triggerType" : kind === "action" ? "actionType" : "logicType"]: type,
          label,
          description: `Configured ${label} node`,
        },
      };

      setNodes((nds) => [...nds, newNode]);
    },
    [nodes, setNodes]
  );

  const handleNodeClick = useCallback(
    (_: React.MouseEvent, node: Node) => {
      setSelectedNode(node);
    },
    [setSelectedNode]
  );

  const handleUpdateNode = useCallback(
    (nodeId: string, updatedData: any) => {
      setNodes((nds) =>
        nds.map((n) => (n.id === nodeId ? { ...n, data: { ...n.data, ...updatedData } } : n))
      );
    },
    [setNodes]
  );

  const handleDeleteNode = useCallback(
    (nodeId: string) => {
      setNodes((nds) => nds.filter((n) => n.id !== nodeId));
      setEdges((eds) => eds.filter((e) => e.source !== nodeId && e.target !== nodeId));
    },
    [setNodes, setEdges]
  );

  return (
    <div className="relative flex flex-1 overflow-hidden h-[calc(100vh-4rem)]">
      {/* Node Palette */}
      <NodePalette onAddNode={handleAddNode} />

      {/* React Flow Canvas */}
      <div className="flex-1 h-full bg-background relative">
        <ReactFlow
          nodes={nodes}
          edges={edges}
          onNodesChange={onNodesChange}
          onEdgesChange={onEdgesChange}
          onConnect={onConnect}
          onNodeClick={handleNodeClick}
          nodeTypes={nodeTypes}
          fitView
        >
          <Controls className="!bg-background !border-border !shadow-lg" />
          <MiniMap className="!bg-background/80 !border-border !shadow-lg rounded-xl overflow-hidden" />
          <Background variant={BackgroundVariant.Dots} gap={16} size={1} />
        </ReactFlow>
      </div>

      {/* Node Configuration Drawer */}
      <NodeConfigDrawer
        node={selectedNode}
        onClose={() => setSelectedNode(null)}
        onUpdateNode={handleUpdateNode}
        onDeleteNode={handleDeleteNode}
      />
    </div>
  );
}
