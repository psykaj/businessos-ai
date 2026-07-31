"use client";

import { Handle, Position } from "@xyflow/react";
import { Zap } from "lucide-react";

export function TriggerNode({ data, selected }: { data: Record<string, any>, selected: boolean }) {
  return (
    <div className={`flex flex-col bg-card rounded-xl border-2 shadow-sm min-w-[250px] ${selected ? "border-primary shadow-md" : "border-muted"}`}>
      <div className="flex items-center gap-3 bg-amber-500/10 p-3 rounded-t-xl border-b border-muted">
        <div className="p-2 bg-amber-500 rounded-lg text-white shadow-sm">
          <Zap className="h-4 w-4" />
        </div>
        <div>
          <div className="text-xs font-semibold text-amber-600 uppercase tracking-wider">Trigger</div>
          <div className="font-medium text-sm">{data.label || "Select Trigger"}</div>
        </div>
      </div>
      <div className="p-3 text-xs text-muted-foreground">
        {data.description || "Configure this trigger in the panel"}
      </div>
      <Handle type="source" position={Position.Bottom} className="w-3 h-3 bg-amber-500 border-2 border-background" />
    </div>
  );
}
