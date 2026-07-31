"use client";

import { Handle, Position } from "@xyflow/react";
import { Split } from "lucide-react";

export function ConditionNode({ data, selected }: { data: Record<string, any>, selected: boolean }) {
  return (
    <div className={`flex flex-col bg-card rounded-xl border-2 shadow-sm min-w-[250px] ${selected ? "border-primary shadow-md" : "border-muted"}`}>
      <Handle type="target" position={Position.Top} className="w-3 h-3 bg-indigo-500 border-2 border-background" />
      <div className="flex items-center gap-3 bg-indigo-500/10 p-3 rounded-t-xl border-b border-muted">
        <div className="p-2 bg-indigo-500 rounded-lg text-white shadow-sm">
          <Split className="h-4 w-4" />
        </div>
        <div>
          <div className="text-xs font-semibold text-indigo-600 uppercase tracking-wider">Condition</div>
          <div className="font-medium text-sm">{data.label || "Select Condition"}</div>
        </div>
      </div>
      <div className="p-3 text-xs text-muted-foreground">
        {data.description || "Configure logic in the panel"}
      </div>
      
      {/* Two source handles for True/False paths */}
      <div className="relative h-6 flex justify-between px-6 border-t border-muted bg-muted/20 rounded-b-xl items-center text-[10px] uppercase font-bold text-muted-foreground">
        <span>True</span>
        <span>False</span>
      </div>
      <Handle type="source" position={Position.Bottom} id="true" className="w-3 h-3 bg-emerald-500 border-2 border-background left-1/4" />
      <Handle type="source" position={Position.Bottom} id="false" className="w-3 h-3 bg-rose-500 border-2 border-background left-3/4" />
    </div>
  );
}
