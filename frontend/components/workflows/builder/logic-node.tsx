"use client";

import React, { memo } from "react";
import { Handle, Position } from "@xyflow/react";
import { GitBranch, Filter, Clock, Split } from "lucide-react";

const logicIcons: Record<string, any> = {
  IfElse: GitBranch,
  AndOr: Split,
  Filter: Filter,
  Delay: Clock,
};

export const LogicNode = memo(({ data, selected }: { data: any; selected?: boolean }) => {
  const Icon = logicIcons[data.logicType] || GitBranch;
  const label = data.label || data.logicType || "Condition Logic";

  return (
    <div
      className={`relative min-w-[220px] rounded-2xl border bg-background/95 p-4 backdrop-blur-md transition-all duration-200 shadow-lg ${
        selected ? "border-purple-500 ring-2 ring-purple-500/30" : "border-purple-500/30 hover:border-purple-500/60"
      }`}
    >
      <Handle
        type="target"
        position={Position.Top}
        className="!h-3.5 !w-3.5 !border-2 !border-background !bg-purple-500 shadow-md"
      />

      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-purple-500/10 text-purple-500">
          <Icon className="h-5 w-5" />
        </div>
        <div className="flex flex-col min-w-0">
          <span className="text-[10px] font-bold uppercase tracking-wider text-purple-500">Condition</span>
          <span className="truncate text-sm font-semibold text-foreground">{label}</span>
        </div>
      </div>

      {data.conditionSummary && (
        <div className="mt-2 rounded-lg bg-purple-500/5 p-2 text-xs font-mono text-purple-600 dark:text-purple-400">
          {data.conditionSummary}
        </div>
      )}

      {/* True / False Branch Handles if IF/ELSE */}
      {data.logicType === "IfElse" ? (
        <div className="mt-3 flex items-center justify-between text-[11px] font-bold">
          <span className="text-emerald-500">YES</span>
          <span className="text-rose-500">NO</span>
        </div>
      ) : null}

      <Handle
        type="source"
        position={Position.Bottom}
        className="!h-3.5 !w-3.5 !border-2 !border-background !bg-purple-500 shadow-md"
      />
    </div>
  );
});

LogicNode.displayName = "LogicNode";
