"use client";

import { Handle, Position } from "@xyflow/react";
import { PlayCircle } from "lucide-react";

export function ActionNode({ data, selected }: { data: Record<string, any>, selected: boolean }) {
  return (
    <div className={`flex flex-col bg-card rounded-xl border-2 shadow-sm min-w-[250px] ${selected ? "border-primary shadow-md" : "border-muted"}`}>
      <Handle type="target" position={Position.Top} className="w-3 h-3 bg-blue-500 border-2 border-background" />
      <div className="flex items-center gap-3 bg-blue-500/10 p-3 rounded-t-xl border-b border-muted">
        <div className="p-2 bg-blue-500 rounded-lg text-white shadow-sm">
          <PlayCircle className="h-4 w-4" />
        </div>
        <div>
          <div className="text-xs font-semibold text-blue-600 uppercase tracking-wider">Action</div>
          <div className="font-medium text-sm">{data.label || "Select Action"}</div>
        </div>
      </div>
      <div className="p-3 text-xs text-muted-foreground">
        {data.description || "Configure this action in the panel"}
      </div>
      <Handle type="source" position={Position.Bottom} className="w-3 h-3 bg-blue-500 border-2 border-background" />
    </div>
  );
}
