"use client";

import { useState } from "react";
import { CopilotNav } from "@/components/copilot/CopilotNav";
import { useCopilot } from "@/hooks/use-copilot";
import {
  Activity,
  CheckCircle2,
  XCircle,
  AlertTriangle,
  Clock,
  Wrench,
  Search,
  RefreshCw,
  Terminal,
} from "lucide-react";
import { CommandExecution } from "@/lib/copilot-service";
import { cn } from "@/lib/utils";

export default function ActionsTimelinePage() {
  const [statusFilter, setStatusFilter] = useState<string | undefined>(undefined);
  const [toolFilter, setToolFilter] = useState<string | undefined>(undefined);

  const { useExecutions, executeCommand, isExecuting } = useCopilot();
  const { data: executionsData, isLoading, refetch } = useExecutions({
    status: statusFilter,
    tool: toolFilter,
  });

  const executions: CommandExecution[] = executionsData?.items || [];

  return (
    <div className="flex flex-col space-y-6 pb-12">
      <CopilotNav />

      <div className="px-6 space-y-6">
        {/* Header & Controls */}
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h2 className="text-xl font-bold text-foreground flex items-center gap-2">
              AI Task Execution Audit & Timeline
              <Activity className="h-5 w-5 text-indigo-500" />
            </h2>
            <p className="text-xs text-muted-foreground">
              Complete audit history of natural language commands executed by the AI Business Agent.
            </p>
          </div>

          <div className="flex items-center gap-3">
            <select
              value={statusFilter || ""}
              onChange={(e) => setStatusFilter(e.target.value || undefined)}
              className="rounded-xl border border-border/60 bg-card px-3 py-2 text-xs text-foreground focus:outline-none"
            >
              <option value="">All Execution Statuses</option>
              <option value="Success">Success</option>
              <option value="Failed">Failed</option>
              <option value="RequiresConfirmation">Requires Confirmation</option>
              <option value="Running">Running</option>
            </select>

            <button
              onClick={() => refetch()}
              className="flex items-center gap-1.5 rounded-xl border border-border/60 bg-card px-3.5 py-2 text-xs font-semibold text-foreground hover:bg-muted transition-colors"
            >
              <RefreshCw className="h-3.5 w-3.5" />
              Refresh
            </button>
          </div>
        </div>

        {/* Audit Log Table / Timeline */}
        {isLoading ? (
          <div className="rounded-2xl border border-border/60 bg-card p-12 text-center text-xs text-muted-foreground animate-pulse">
            Loading task execution history...
          </div>
        ) : executions.length === 0 ? (
          <div className="rounded-2xl border border-border/60 bg-card p-12 text-center space-y-3">
            <Terminal className="h-10 w-10 text-muted-foreground mx-auto" />
            <h3 className="text-sm font-semibold text-foreground">No execution logs recorded yet</h3>
            <p className="text-xs text-muted-foreground">
              Commands executed via the Copilot workspace will appear in this timeline.
            </p>
          </div>
        ) : (
          <div className="rounded-2xl border border-border/60 bg-card shadow-sm overflow-hidden">
            <div className="overflow-x-auto">
              <table className="w-full text-left text-xs">
                <thead className="bg-muted/40 border-b border-border/50 font-semibold text-muted-foreground">
                  <tr>
                    <th className="py-3.5 px-4">Status</th>
                    <th className="py-3.5 px-4">Command</th>
                    <th className="py-3.5 px-4">Invoked Service</th>
                    <th className="py-3.5 px-4">Started At</th>
                    <th className="py-3.5 px-4">Result Summary</th>
                    <th className="py-3.5 px-4 text-right">Actions</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-border/40 font-medium">
                  {executions.map((item) => {
                    const isSuccess = item.status === "Success";
                    const isFailed = item.status === "Failed";
                    const isNeedsConf = item.status === "RequiresConfirmation";

                    return (
                      <tr key={item.id} className="hover:bg-muted/20 transition-colors">
                        <td className="py-3.5 px-4">
                          <span
                            className={cn(
                              "inline-flex items-center gap-1.5 rounded-full px-2.5 py-0.5 text-[10px] font-bold border",
                              isSuccess && "bg-emerald-500/10 text-emerald-500 border-emerald-500/20",
                              isFailed && "bg-red-500/10 text-red-500 border-red-500/20",
                              isNeedsConf && "bg-amber-500/10 text-amber-500 border-amber-500/20"
                            )}
                          >
                            {isSuccess && <CheckCircle2 className="h-3 w-3" />}
                            {isFailed && <XCircle className="h-3 w-3" />}
                            {isNeedsConf && <AlertTriangle className="h-3 w-3" />}
                            {item.status}
                          </span>
                        </td>
                        <td className="py-3.5 px-4 font-semibold text-foreground max-w-xs truncate">
                          {item.command}
                        </td>
                        <td className="py-3.5 px-4">
                          <span className="inline-flex items-center gap-1 rounded bg-indigo-500/10 px-2 py-0.5 font-mono text-[11px] font-semibold text-indigo-500">
                            <Wrench className="h-3 w-3" />
                            {item.toolInvoked}
                          </span>
                        </td>
                        <td className="py-3.5 px-4 text-muted-foreground whitespace-nowrap">
                          {new Date(item.startedAt).toLocaleString()}
                        </td>
                        <td className="py-3.5 px-4 text-muted-foreground max-w-sm truncate">
                          {item.resultSummary || item.errorMessage || "—"}
                        </td>
                        <td className="py-3.5 px-4 text-right">
                          <button
                            onClick={() => executeCommand({ command: item.command, isConfirmed: true })}
                            disabled={isExecuting}
                            className="rounded-lg border border-border px-2.5 py-1 text-[11px] font-semibold text-foreground hover:bg-muted transition-colors disabled:opacity-50"
                          >
                            Re-run
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
